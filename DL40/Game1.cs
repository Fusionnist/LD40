using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
namespace DL40
{
    enum GamePhase { Menu, Game }
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        TextureDrawer td;
        SoundManager soundManager;
        FontDrawer fd;
        RenderTarget2D gameTarget, textTarget, overlayTarget;
        float zoom;
        float scale;
        Point virtualDims, targetPos;
        float timer;
        Tilemap map;
        Point mapPos;
        GamePhase gp;
        InputProfile ipp;
        Player player;
        TextureDrawer menu;
        List<Tilemap> maps;

        public Game1()
        {
            mapPos = new Point(0, 0);
            gp = GamePhase.Menu;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";  
        }
        //LOAD+INIT
        void InitGraphics()
        {
            virtualDims = new Point(640, 320);

            graphics.PreferredBackBufferHeight = (int)(GraphicsDevice.DisplayMode.Height / 1.5);
            graphics.PreferredBackBufferWidth = (int)(GraphicsDevice.DisplayMode.Width / 1.5);
            Window.IsBorderless = true;
            graphics.ApplyChanges();

            float xscale = (float)GraphicsDevice.Viewport.Width / virtualDims.X;
            float yscale = (float)GraphicsDevice.Viewport.Height / virtualDims.Y;
            scale = (float)Math.Round(Math.Min(xscale, yscale), 1);
            if (scale < 1) { scale = 1; }
            targetPos = new Point(
                (GraphicsDevice.Viewport.Width - (int)(virtualDims.X * scale)) / 2,
                (GraphicsDevice.Viewport.Height - (int)(virtualDims.Y * scale)) / 2);

            gameTarget = new RenderTarget2D(GraphicsDevice, virtualDims.X, virtualDims.Y);
            textTarget = new RenderTarget2D(GraphicsDevice, virtualDims.X, virtualDims.Y);
            overlayTarget = new RenderTarget2D(GraphicsDevice, virtualDims.X, virtualDims.Y);
        }
        protected override void Initialize()
        {
            InitGraphics();
            soundManager = new SoundManager();
            fd = new FontDrawer();

            KeyManager[] kms = new KeyManager[]{
                new KeyManager(Keys.Left,"left"),
                new KeyManager(Keys.Right,"right"),
                new KeyManager(Keys.Up,"up"),
                new KeyManager(Keys.Down,"down"),
                new KeyManager(Keys.Space,"space")
            };
            ipp = new InputProfile(kms);
            base.Initialize();

        }
        protected override void LoadContent()
        {
            menu = new TextureDrawer(Content.Load<Texture2D>("Menu"));
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            td = new TextureDrawer(Content.Load<Texture2D>("sheet"),
                new Rectangle[] {
                    new Rectangle(0,0,64,64),
                    new Rectangle(72, 8, 48, 48),
                    new Rectangle(144, 16, 32, 32),
                    new Rectangle(216, 24, 16, 16), },
                new Point[] { new Point(32, 32), new Point(24, 24), new Point(16, 16), new Point(8, 8), },
                1f, 4, true, "test");

            Texture2D src = Content.Load<Texture2D>("Original");
            Font f = new Font(new TextureDrawer[] {
                new TextureDrawer(src,new Rectangle(0,27,9,9),Point.Zero,"a"),
                new TextureDrawer(src,new Rectangle(9,27,9,9),Point.Zero,"b"),
                new TextureDrawer(src,new Rectangle(18,27,9,9),Point.Zero,"c"),
                new TextureDrawer(src,new Rectangle(27,27,9,9),Point.Zero,"d"),
                new TextureDrawer(src,new Rectangle(36,27,9,9),Point.Zero,"e"),
                new TextureDrawer(src,new Rectangle(45,27,9,9),Point.Zero,"f"),
                new TextureDrawer(src,new Rectangle(54,27,9,9),Point.Zero,"g"),
                new TextureDrawer(src,new Rectangle(63,27,9,9),Point.Zero,"h"),
                new TextureDrawer(src,new Rectangle(72,27,9,9),Point.Zero,"i"),
                new TextureDrawer(src,new Rectangle(81,27,9,9),Point.Zero,"j"),
                new TextureDrawer(src,new Rectangle(90,27,9,9),Point.Zero,"k"),
                new TextureDrawer(src,new Rectangle(99,27,9,9),Point.Zero,"l"),
                new TextureDrawer(src,new Rectangle(108,27,9,9),Point.Zero,"m"),

                new TextureDrawer(src,new Rectangle(0,36,9,9),Point.Zero,"n"),
                new TextureDrawer(src,new Rectangle(9,36,9,9),Point.Zero,"o"),
                new TextureDrawer(src,new Rectangle(18,36,9,9),Point.Zero,"p"),
                new TextureDrawer(src,new Rectangle(27,36,9,9),Point.Zero,"q"),
                new TextureDrawer(src,new Rectangle(36,36,9,9),Point.Zero,"r"),
                new TextureDrawer(src,new Rectangle(45,36,9,9),Point.Zero,"s"),
                new TextureDrawer(src,new Rectangle(54,36,9,9),Point.Zero,"t"),
                new TextureDrawer(src,new Rectangle(63,36,9,9),Point.Zero,"u"),
                new TextureDrawer(src,new Rectangle(72,36,9,9),Point.Zero,"v"),
                new TextureDrawer(src,new Rectangle(81,36,9,9),Point.Zero,"w"),
                new TextureDrawer(src,new Rectangle(90,36,9,9),Point.Zero,"x"),
                new TextureDrawer(src,new Rectangle(99,36,9,9),Point.Zero,"y"),
                new TextureDrawer(src,new Rectangle(108,36,9,9),Point.Zero,"z"),

                new TextureDrawer(src,new Rectangle(0,9,8,9),Point.Zero,"0"),
                new TextureDrawer(src,new Rectangle(8,9,8,9),Point.Zero,"1"),
                new TextureDrawer(src,new Rectangle(16,9,8,9),Point.Zero,"2"),
                new TextureDrawer(src,new Rectangle(24,9,8,9),Point.Zero,"3"),
                new TextureDrawer(src,new Rectangle(32,9,8,9),Point.Zero,"4"),
                new TextureDrawer(src,new Rectangle(40,9,8,9),Point.Zero,"5"),
                new TextureDrawer(src,new Rectangle(48,9,8,9),Point.Zero,"6"),
                new TextureDrawer(src,new Rectangle(56,9,8,9),Point.Zero,"7"),
                new TextureDrawer(src,new Rectangle(64,9,8,9),Point.Zero,"8"),
                new TextureDrawer(src,new Rectangle(72,9,8,9),Point.Zero,"9"),
                new TextureDrawer(src,new Rectangle(80,9,8,9),Point.Zero," "),
                },
                "font");
            fd.fonts.Add(f);

           
        }
        protected override void UnloadContent()
        {
            Content.Unload();
        }
        void GoToNewGame()
        {
            gp = GamePhase.Game;
            maps = new List<Tilemap>();
            maps.Add(getTilemap(XDocument.Load("Content/TestTilemap.tmx"), Point.Zero));
            maps.Add(getTilemap(XDocument.Load("Content/Tilemap2.tmx"), new Point(-1,0)));
            map = maps[0];
            player = new Player(new TextureDrawer[] { td }, new Vector2(100, 150));
            mapPos = map.vpos;
        }
        //UTILS
        Tilemap getTilemap(XDocument doc_, Point vpos_)
        {
            Point dims = new Point(int.Parse(doc_.Element("map").Attribute("width").Value), int.Parse(doc_.Element("map").Attribute("height").Value));
            Point tdims = new Point(int.Parse(doc_.Element("map").Attribute("tilewidth").Value), int.Parse(doc_.Element("map").Attribute("tileheight").Value));
            int count = dims.X * dims.Y;
            string raw = doc_.Element("map").Element("layer").Element("data").Value;
            List<Tile> tiles = new List<Tile>();
            Tileset ts = getTileset(XDocument.Load("Content/" + doc_.Element("map").Element("tileset").Attribute("source").Value));
            string[] split = raw.Split(',');

            for (int x = 0; x < dims.X; x++)
            {
                for (int y = 0; y < dims.Y; y++)
                {
                    tiles.Add(ts.getTile(int.Parse(split[x + dims.X * y]) - 1, new Vector2(x * tdims.X, y * tdims.Y)));
                }
            }


            return new Tilemap(tiles, dims,vpos_);
        }
        Tileset getTileset(XDocument doc_)
        {

            Point dims = new Point(int.Parse(doc_.Element("tileset").Attribute("tilewidth").Value), int.Parse(doc_.Element("tileset").Attribute("tileheight").Value));
            string path = doc_.Element("tileset").Element("image").Attribute("source").Value;
            path = path.Remove(path.Length - 4, 4);
            Texture2D src = Content.Load<Texture2D>(path);
            int columns = int.Parse(doc_.Element("tileset").Attribute("columns").Value);
            int count = int.Parse(doc_.Element("tileset").Attribute("tilecount").Value);
            bool[] solid = new bool[count];
            bool[] hurtsmyass = new bool[count];
            bool[] slips = new bool[count];
            bool[] door = new bool[count];
            int[] pool = new int[count];
            foreach (XElement tile in doc_.Element("tileset").Elements("tile"))//PROPERTIES!
            {
                foreach (XElement prop in tile.Element("objectgroup").Element("properties").Elements("property"))//PROPERTIES!
                {
                    if (prop.Attribute("name").Value == "solid")
                    {
                        solid[int.Parse(tile.Attribute("id").Value)] = bool.Parse(prop.Attribute("value").Value);
                    }
                    if (prop.Attribute("name").Value == "hurty")
                    {
                        hurtsmyass[int.Parse(tile.Attribute("id").Value)] = bool.Parse(prop.Attribute("value").Value);
                    }
                    if (prop.Attribute("name").Value == "slippery")
                    {
                        slips[int.Parse(tile.Attribute("id").Value)] = bool.Parse(prop.Attribute("value").Value);
                    }
                    if (prop.Attribute("name").Value == "door")
                    {
                        door[int.Parse(tile.Attribute("id").Value)] = bool.Parse(prop.Attribute("value").Value);
                    }
                    if (prop.Attribute("name").Value == "pool")
                    {
                        pool[int.Parse(tile.Attribute("id").Value)] = int.Parse(prop.Attribute("value").Value);
                    }
                }
            }
            return new Tileset(dims, src, columns, count, solid, hurtsmyass,slips,door,pool);
        }
        //UPDATE
        protected override void Update(GameTime gameTime)
        {
            //LOGIC
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float es = (float)gameTime.ElapsedGameTime.TotalSeconds;
            ipp.Update(Keyboard.GetState(), GamePad.GetState(0));
            if(gp == GamePhase.Menu)
            {
                UpdateMenu(es);
            }
            if (gp == GamePhase.Game)
            {
                UpdateGame(es);
            }
            base.Update(gameTime);
        }
        void UpdateMenu(float es_)
        {
            if (ipp.Pressed("space"))
            {
                GoToNewGame();
            }
        }
        void UpdateGame(float es_)
        {
            Vector2 mover = Vector2.Zero;
            Vector2 input = Vector2.Zero;
            if (ipp.Pressed("left"))
            { input.X -= 1; }
            if (ipp.Pressed("right"))
            { input.X += 1; }
            if (ipp.Pressed("up"))
            { input.Y -= 1; }
            if (ipp.Pressed("down"))
            { input.Y += 1; }
            player.Move(input: input, extmov: mover);
            //PRE-UPDATE
            player.PreUpdate(es_);
            //COLLISIONS
            DoCollisions();
            //UPDATE
            player.Update(es_);

            if (ipp.Pressed("space"))
            {
                GoToNewGame();
            }

            if (!player.GetHBAfterMov().Intersects(map.GetBounds()))
            {
                //switch maps
                Point currentpos = map.vpos;
                Point searchPos = map.vpos;
                if(player.GetHBAfterMov().X < map.GetBounds().X)
                {
                    searchPos.X -= 1;                   
                }
                else if (player.GetHBAfterMov().X > map.GetBounds().X+map.GetBounds().Width)
                {
                    searchPos.X += 1;                    
                }
                else if (player.GetHBAfterMov().Y < map.GetBounds().Y)
                {
                    searchPos.Y -= 1;                   
                }
                else if (player.GetHBAfterMov().Y > map.GetBounds().Y+map.GetBounds().Height)
                {
                    searchPos.Y += 1;                    
                }

                foreach(Tilemap tm in maps)
                {
                    if(tm.vpos == searchPos)
                    { map = tm; mapPos = tm.vpos; }
                }
            }
        }
        void DoCollisions()
        {
            player.onground = false;
            
            foreach (Tile e in map.tiles)
            {
                Rectangle r = player.GetHBafterY();
                r.Y += 1;
                if (r.Intersects(e.GetHB()))
                {
                    if (e.isSolid && player.pos.Y < e.pos.Y)
                    {
                        player.onground = true;
                    }
                    if (e.isHurty)
                        player.TakeDamage(1);

                    if (e.isSlippery)
                        player.slipping = true;
                    else if(e.isSolid)
                        player.slipping = false;
                }
                if (player.GetHBAfterMov().Intersects(e.GetHB()))
                {                 
                    if (e.isSolid)
                    {
                        Vector2 inter = Vector2.Zero;
                        if (player.pos.X < e.pos.X) { inter.X = player.GetHBAfterMov().Width + player.GetHBAfterMov().X - e.pos.X; }
                        else { inter.X = e.GetHB().Width + e.GetHB().X - player.GetHBAfterMov().X; }
                        if (player.pos.Y < e.pos.Y) { inter.Y = player.GetHBAfterMov().Height + player.GetHBAfterMov().Y - e.pos.Y; }
                        else { inter.Y = e.GetHB().Height + e.GetHB().Y - player.GetHBAfterMov().Y; }
                        //calc best option
                        if (inter.X > inter.Y)
                        {
                            if (player.pos.Y < e.pos.Y)
                            {
                                player.mov.Y -= inter.Y;                              
                            }
                            else
                            {
                                player.mov.Y += inter.Y;
                            }
                            player.Yvel = 0f;
                        }
                        else
                        {
                            if (player.pos.X < e.pos.X)
                            {
                                player.mov.X -= inter.X;
                            }
                            else
                            {
                                player.mov.X += inter.X;
                            }      
                            if (!player.onground) { player.canWJump = true; }
                        }
                    }            
                }
            }
        }
        //DRAW
        protected override void Draw(GameTime gameTime)
        {
            //TEXT DRAW
            GraphicsDevice.SetRenderTarget(textTarget);
            GraphicsDevice.Clear(Color.TransparentBlack);
            spriteBatch.Begin();

            spriteBatch.End();

            //OVERLAY DRAW
            GraphicsDevice.SetRenderTarget(overlayTarget);
            GraphicsDevice.Clear(Color.TransparentBlack);
            spriteBatch.Begin();
            
            if (gp == GamePhase.Menu) { DrawMenuElements(); }
            else { fd.DrawText("font", player.onground.ToString(), new Rectangle(0, 0, 640, 320), spriteBatch); }
            spriteBatch.End();

            //GAME DRAW
            Matrix translation = Matrix.CreateTranslation(new Vector3(-mapPos.X * 640,- mapPos.Y * 320, 0));
            GraphicsDevice.SetRenderTarget(gameTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(transformMatrix:translation);
            if (gp == GamePhase.Game) { DrawGameElements(); }
            spriteBatch.End();

            //DEFINITIVE DRAW
            Matrix scaleMatrix = Matrix.CreateScale(scale);
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(samplerState: SamplerState.PointWrap, sortMode: SpriteSortMode.Immediate);
            Rectangle dest = new Rectangle(targetPos.X, targetPos.Y, (int)(virtualDims.X * scale), (int)(virtualDims.Y * scale));
            spriteBatch.Draw(gameTarget, destinationRectangle: dest);
            spriteBatch.Draw(textTarget, destinationRectangle: dest);
            spriteBatch.Draw(overlayTarget, destinationRectangle: dest);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
        void DrawGameElements()
        {
            map.Draw(spriteBatch);
            player.Draw(spriteBatch);
        }
        void DrawMenuElements()
        {
            menu.Draw(spriteBatch,Vector2.Zero);
        }
    }
}