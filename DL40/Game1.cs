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
        GamePhase gp;
        InputProfile ipp;
        Player player;
        TextureDrawer menu;
        List<Tilemap> maps;

        public Game1()
        {
            gp = GamePhase.Menu;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";  
        }
        //LOAD+INIT
        void InitGraphics()
        {
            virtualDims = new Point(640, 320);

            graphics.PreferredBackBufferHeight = (int)(GraphicsDevice.DisplayMode.Height / 1);
            graphics.PreferredBackBufferWidth = (int)(GraphicsDevice.DisplayMode.Width / 1);
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
                new TextureDrawer(src,new Rectangle(0,0,5,9),Point.Zero,"!"),
                new TextureDrawer(src,new Rectangle(5,0,8,9),Point.Zero,"o"),
                new TextureDrawer(src,new Rectangle(13,0,10,9),Point.Zero," ")
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
            foreach (XElement tile in doc_.Element("tileset").Elements("tile"))//PROPERTIES!
            {
                foreach (XElement prop in tile.Element("objectgroup").Element("properties").Elements("property"))//PROPERTIES!
                {
                    if (prop.Attribute("name").Value == "solid")
                    {
                        solid[int.Parse(tile.Attribute("id").Value)] = bool.Parse(prop.Attribute("value").Value);
                    }
                }
            }

            return new Tileset(dims, src, columns, count, solid);
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
            { mover.X -= 150; }
            if (ipp.Pressed("right"))
            { mover.X += 150; }
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
                if(player.GetHBAfterMov().X < map.GetBounds().X)
                {

                }
                if (player.GetHBAfterMov().X > map.GetBounds().X+map.GetBounds().Width)
                {

                }
                if (player.GetHBAfterMov().Y < map.GetBounds().Y)
                {

                }
                if (player.GetHBAfterMov().Y > map.GetBounds().Y+map.GetBounds().Height)
                {

                }
            }
        }
        void DoCollisions()
        {
            player.onground = false;
            foreach (Tile e in map.tiles)
            {
                if (e.isSolid && player.GetHBAfterMov().Intersects(e.GetHB()))
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
                            player.onground = true;
                        }
                        else
                        {
                            player.mov.Y += inter.Y;
                        }
                        player.Yvel = 0;
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
                    }
                    if (e.isHurty)
                        player.hp -= 1;
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
            spriteBatch.End();

            //GAME DRAW
            GraphicsDevice.SetRenderTarget(gameTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
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