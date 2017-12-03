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
        Tileset set;
        TextureDrawer fullheart, emptyheart;
        float lastInter;
        string currentStr;

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

            graphics.PreferredBackBufferHeight = 320;//(int)(GraphicsDevice.DisplayMode.Height / 1);
            graphics.PreferredBackBufferWidth = 640;//(int)(GraphicsDevice.DisplayMode.Width / 1);
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
            zoom = 1;
            InitGraphics();
            soundManager = new SoundManager();
            fd = new FontDrawer();

            KeyManager[] kms = new KeyManager[]{
                new KeyManager(Keys.Left,"left"),
                new KeyManager(Keys.Right,"right"),
                new KeyManager(Keys.Up,"up"),
                new KeyManager(Keys.Down,"down"),
                new KeyManager(Keys.Space,"space"),
                new KeyManager(Keys.R,"restart")
            };
            ipp = new InputProfile(kms);
            base.Initialize();

            set = getTileset(XDocument.Load("Content/tiletest.tsx"));
        }
        protected override void LoadContent()
        {
            soundManager.AddEffect(Content.Load<SoundEffect>("hurty"), "hurty");
            soundManager.AddEffect(Content.Load<SoundEffect>("jump"), "jump");

            menu = new TextureDrawer(Content.Load<Texture2D>("Menu"));
            
            spriteBatch = new SpriteBatch(GraphicsDevice);

            emptyheart = new TextureDrawer(Content.Load<Texture2D>("emptyheart"));
            fullheart = new TextureDrawer(Content.Load<Texture2D>("fullheart2"));

            Texture2D src = Content.Load<Texture2D>("Original");
            Font f = new Font(new TextureDrawer[] {
                new TextureDrawer(src,new Rectangle(0,0,16,16),Point.Zero,"a"),
                new TextureDrawer(src,new Rectangle(16,0,16,16),Point.Zero,"b"),
                new TextureDrawer(src,new Rectangle(32,0,16,16),Point.Zero,"c"),
                new TextureDrawer(src,new Rectangle(48,0,16,16),Point.Zero,"d"),
                new TextureDrawer(src,new Rectangle(64,0,16,16),Point.Zero,"e"),
                new TextureDrawer(src,new Rectangle(80,0,16,16),Point.Zero,"f"),
                new TextureDrawer(src,new Rectangle(96,0,16,16),Point.Zero,"g"),
                new TextureDrawer(src,new Rectangle(112,0,16,16),Point.Zero,"h"),
                new TextureDrawer(src,new Rectangle(128,0,16,16),Point.Zero,"i"),
                new TextureDrawer(src,new Rectangle(144,0,16,16),Point.Zero,"j"),
                new TextureDrawer(src,new Rectangle(160,0,16,16),Point.Zero,"k"),
                new TextureDrawer(src,new Rectangle(176,0,16,16),Point.Zero,"l"),
                new TextureDrawer(src,new Rectangle(192,0,16,16),Point.Zero,"m"),
                new TextureDrawer(src,new Rectangle(208,0,16,16),Point.Zero,"n"),
                new TextureDrawer(src,new Rectangle(224,0,16,16),Point.Zero,"o"),
                new TextureDrawer(src,new Rectangle(240,0,16,16),Point.Zero,"p"),
                new TextureDrawer(src,new Rectangle(256,0,16,16),Point.Zero,"q"),
                new TextureDrawer(src,new Rectangle(272,0,16,16),Point.Zero,"r"),
                new TextureDrawer(src,new Rectangle(288,0,16,16),Point.Zero,"s"),
                new TextureDrawer(src,new Rectangle(304,0,16,16),Point.Zero,"t"),

                new TextureDrawer(src,new Rectangle(0,16,16,16),Point.Zero,"u"),
                new TextureDrawer(src,new Rectangle(16,16,16,16),Point.Zero,"v"),
                new TextureDrawer(src,new Rectangle(32,16,16,16),Point.Zero,"w"),
                new TextureDrawer(src,new Rectangle(48,16,16,16),Point.Zero,"x"),
                new TextureDrawer(src,new Rectangle(64,16,16,16),Point.Zero,"y"),
                new TextureDrawer(src,new Rectangle(80,16,16,16),Point.Zero,"z"),

                new TextureDrawer(src,new Rectangle(0,32,16,16),Point.Zero,"1"),
                new TextureDrawer(src,new Rectangle(16,32,16,16),Point.Zero,"2"),
                new TextureDrawer(src,new Rectangle(32,32,16,16),Point.Zero,"3"),
                new TextureDrawer(src,new Rectangle(48,32,16,16),Point.Zero,"4"),
                new TextureDrawer(src,new Rectangle(64,32,16,16),Point.Zero,"5"),
                new TextureDrawer(src,new Rectangle(80,32,16,16),Point.Zero,"6"),
                new TextureDrawer(src,new Rectangle(96,32,16,16),Point.Zero,"7"),
                new TextureDrawer(src,new Rectangle(112,32,16,16),Point.Zero,"8"),
                new TextureDrawer(src,new Rectangle(128,32,16,16),Point.Zero,"9"),
                new TextureDrawer(src,new Rectangle(144,32,16,16),Point.Zero,"0"),

                new TextureDrawer(src,new Rectangle(160,32,8,16),Point.Zero,","),
                new TextureDrawer(src,new Rectangle(168,32,8,16),Point.Zero,"-"),
                new TextureDrawer(src,new Rectangle(176,32,8,16),Point.Zero,"!"),
                new TextureDrawer(src,new Rectangle(184,32,8,16),Point.Zero,"?"),

                new TextureDrawer(src,new Rectangle(160,48,8,16),Point.Zero," "),

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
            maps.Add(getTilemap(XDocument.Load("Content/Tilemap2.tmx"), new Point(-2, 0)));
            maps.Add(getTilemap(XDocument.Load("Content/Tilemap2.tmx"), new Point(-3, 0)));
            maps.Add(getTilemap(XDocument.Load("Content/Tilemap2.tmx"), new Point(-4, 0)));
            maps.Add(getTilemap(XDocument.Load("Content/BigMap.tmx"), new Point(-6, -1)));
            maps.Add(getTilemap(XDocument.Load("Content/TopMap.tmx"), new Point(-1, -2)));
            map = maps[0];

            TextureDrawer walk = new TextureDrawer(Content.Load<Texture2D>("walk"),
                new Rectangle[] {
                    new Rectangle(0,0,32,32),
                    new Rectangle(32,0,32,32),
                    new Rectangle(64,0,32,32),
                    new Rectangle(96,0,32,32),},
                new Point[] { new Point(16, 16), new Point(16, 16), new Point(16, 16), new Point(16, 16), },
                0.1f, 4, true, "walk");
            TextureDrawer jump = new TextureDrawer(Content.Load<Texture2D>("walk"),
                new Rectangle[] {
                    new Rectangle(0,0,32,32),
                    new Rectangle(32,0,32,32),
                    new Rectangle(64,0,32,32),
                    new Rectangle(96,0,32,32),},
                new Point[] { new Point(16, 16), new Point(16, 16), new Point(16, 16), new Point(16, 16), },
                0.3f, 4, true, "jump");
            TextureDrawer fall = new TextureDrawer(Content.Load<Texture2D>("walk"),
                new Rectangle[] {
                    new Rectangle(0,0,32,32),
                    new Rectangle(32,0,32,32),
                    new Rectangle(64,0,32,32),
                    new Rectangle(96,0,32,32),},
                new Point[] { new Point(16, 16), new Point(16, 16), new Point(16, 16), new Point(16, 16), },
                0.3f, 4, true, "fall");
            TextureDrawer wallclimb = new TextureDrawer(Content.Load<Texture2D>("walk"),
                new Rectangle[] {
                    new Rectangle(0,0,32,32),
                    new Rectangle(32,0,32,32),
                    new Rectangle(64,0,32,32),
                    new Rectangle(96,0,32,32),},
                new Point[] { new Point(16, 16), new Point(16, 16), new Point(16, 16), new Point(16, 16), },
                0.3f, 4, true, "wallclimb");
            TextureDrawer ground = new TextureDrawer(Content.Load<Texture2D>("walk"),
                new Rectangle[] {
                    new Rectangle(0,0,32,32),
                    new Rectangle(32,0,32,32),
                    new Rectangle(64,0,32,32),
                    new Rectangle(96,0,32,32),},
                new Point[] { new Point(16, 16), new Point(16, 16), new Point(16, 16), new Point(16, 16), },
                0.3f, 4, true, "ground");
            TextureDrawer dead = new TextureDrawer(Content.Load<Texture2D>("walk"),
                new Rectangle[] {
                    new Rectangle(0,0,32,32),
                    new Rectangle(32,0,32,32),
                    new Rectangle(64,0,32,32),
                    new Rectangle(96,0,32,32),},
                new Point[] { new Point(16, 16), new Point(16, 16), new Point(16, 16), new Point(16, 16), },
                0.3f, 4, true, "dead");

            player = new Player(new TextureDrawer[] { walk,dead,wallclimb,ground,jump,fall }, new Vector2(100, 150),soundManager);
            mapPos = map.vpos;
        }
        //UTILS
        Tilemap getTilemap(XDocument doc_, Point vpos_)
        {
            Point dims = new Point(int.Parse(doc_.Element("map").Attribute("width").Value), int.Parse(doc_.Element("map").Attribute("height").Value));
            Point tdims = new Point(int.Parse(doc_.Element("map").Attribute("tilewidth").Value), int.Parse(doc_.Element("map").Attribute("tileheight").Value));
            int count = dims.X * dims.Y;
            List<Tile> tiles = new List<Tile>();
            List<Entity> ents = new List<Entity>();
            Tileset ts = set;

            foreach(XElement layer in doc_.Element("map").Elements("layer"))
            {
                string raw = layer.Element("data").Value;


                string[] split = raw.Split(',');

                for (int x = 0; x < dims.X; x++)
                {
                    for (int y = 0; y < dims.Y; y++)
                    {
                        if (int.Parse(split[x + dims.X * y]) != 0)                    
                        {
                            int id = int.Parse(split[x + dims.X * y]) - 1;
                            if (ts.isEntity(id))
                                ents.Add(ts.GetEntity(id, new Vector2(x * tdims.X, y * tdims.Y)));
                            else
                                tiles.Add(ts.getTile(id, new Vector2(x * tdims.X, y * tdims.Y)));
                        }
                    }
                }
            }
            


            return new Tilemap(tiles,ents, dims,vpos_);
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
            string[] actived = new string[count];
            bool[] slimeball = new bool[count];
            string[] facing = new string[count];
            bool[] arrow = new bool[count];
            bool[] ladder = new bool[count];
            List<TextureDrawer>[] texes = new List<TextureDrawer>[count];

            for (int i = 0; i < count; i++)
            {
                texes[i] = new List<TextureDrawer>();
            }
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
                    if (prop.Attribute("name").Value == "debuff")
                    {
                        actived[int.Parse(tile.Attribute("id").Value)] = (prop.Attribute("value").Value);
                    }
                    if (prop.Attribute("name").Value == "slimeball")
                    {
                        slimeball[int.Parse(tile.Attribute("id").Value)] = bool.Parse(prop.Attribute("value").Value);
                    }
                    if (prop.Attribute("name").Value == "facing")
                    {
                        facing[int.Parse(tile.Attribute("id").Value)] = (prop.Attribute("value").Value);
                    }
                    if (prop.Attribute("name").Value == "arrow")
                    {
                        arrow[int.Parse(tile.Attribute("id").Value)] = bool.Parse(prop.Attribute("value").Value);
                    }
                    if (prop.Attribute("name").Value == "texture")
                    {
                        texes[int.Parse(tile.Attribute("id").Value)].Add(getTDXML(prop.Attribute("value").Value));
                    }
                    if (prop.Attribute("name").Value == "ladder")
                    {
                        ladder[int.Parse(tile.Attribute("id").Value)] = bool.Parse(prop.Attribute("value").Value);
                    }
                }
            }
            return new Tileset(dims, src, columns, count, solid, hurtsmyass,slips,door,pool,actived,slimeball,texes,facing,arrow,ladder);
        }
        //UPDATE
        TextureDrawer getTDXML(string name)
        {
            XDocument texes = XDocument.Load("Content/Textures.xml");
            foreach(XElement tex in texes.Element("Textures").Elements("Texture"))
            {
                if (tex.Attribute("name").Value == name)
                {
                    if (tex.Element("Center") == null)
                    {
                        return new TextureDrawer(Content.Load<Texture2D>(tex.Attribute("source").Value),tex.Attribute("truename").Value);
                    }
                    else if (tex.Element("Anim") == null)
                    {
                        int fc = int.Parse(tex.Attribute("fc").Value);
                        List<Rectangle> rs = new List<Rectangle>();
                        foreach (XElement r in tex.Elements("Dim"))
                        {
                            rs.Add(new Rectangle(
                                int.Parse(r.Attribute("x").Value),
                                int.Parse(r.Attribute("y").Value),
                                int.Parse(r.Attribute("w").Value),
                                int.Parse(r.Attribute("h").Value)
                                ));
                        }

                        List<Point> cs = new List<Point>();
                        foreach (XElement r in tex.Elements("Center"))
                        {
                            cs.Add(new Point(
                                int.Parse(r.Attribute("x").Value),
                                int.Parse(r.Attribute("y").Value)
                                ));
                        }
                        return new TextureDrawer(Content.Load<Texture2D>(tex.Attribute("source").Value),
                            rs.ToArray(),
                            cs.ToArray(),
                            float.Parse(tex.Attribute("ft").Value),
                            fc,
                            true,
                            tex.Attribute("truename").Value
                            );
                    }
                    else
                    {
                        
                    }
                }
            }
            return null;
        }
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
            zoom = map.GetBounds().Width / 640;
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
            foreach (Entity e in map.bouncies) { e.Move(); }
            //PRE-UPDATE
            player.PreUpdate(es_);
            map.PreUpdate(es_);
            //COLLISIONS
            DoCollisions();
            //UPDATE
            player.Update(es_);
            map.Update(es_);

            if (!map.GetBounds().Contains(player.pos))
            {
                //switch maps
                Point currentpos = map.vpos;
                Point searchPos = Point.Zero;
                Vector2 copy = player.pos;
                while(copy.X > 640)
                {
                    searchPos.X += 1;
                    copy.X -= 640;
                }
                while (copy.X < 0)
                {
                    searchPos.X -= 1;
                    copy.X += 640;
                }
                while (copy.Y > 320)
                {
                    searchPos.Y += 1;
                    copy.Y -= 320;
                }
                while (copy.Y < 0)
                {
                    searchPos.Y -= 1;
                    copy.Y += 320;
                }

                if(player.GetHBAfterMov().X < map.GetBounds().X)
                {
                    //searchPos.X -= map.GetBounds().Width/640;
                    player.pos.Y -= 3;
                }
                else if (player.GetHBAfterMov().X > map.GetBounds().X+map.GetBounds().Width)
                {
                    //searchPos.X += map.GetBounds().Width / 640;
                    player.pos.Y -= 3;
                }
                else if (player.GetHBAfterMov().Y < map.GetBounds().Y)
                {
                    //searchPos.Y -= map.GetBounds().Height / 320;                   
                }
                else if (player.GetHBAfterMov().Y > map.GetBounds().Y+map.GetBounds().Height)
                {
                    //searchPos.Y += map.GetBounds().Height / 320;                    
                }

                foreach(Tilemap tm in maps)
                {
                    for(int x = 0; x < tm.GetBounds().Width / 640; x++)
                    {
                        for (int y = 0; y < tm.GetBounds().Height / 320; y++)
                        {
                            if (tm.vpos + new Point(x,y) == searchPos)
                            { map = tm; mapPos = tm.vpos; }
                        }
                    }                   
                }
                zoom = map.GetBounds().Width / 640;
            }
            if (ipp.JustPressed("restart")) { GoToNewGame(); }
        }
        void Collide(Entity ent, Tile t)
        {
            
            Rectangle r = ent.GetHBafterY();
            r.Y += 1;
            if (r.Intersects(t.GetHB()))
            {
                if (t.isSolid && ent.pos.Y < t.pos.Y)
                {
                    ent.onground = true;
                }
                if (t.isHurty)
                    ent.TakeDamage(1);

                if (t.isSlippery)
                    ent.slipping = true;
                else if (t.isSolid)
                    ent.slipping = false;
            }
            if (ent.GetHBAfterMov().Intersects(t.GetHB()))
            {
                if (t.isSolid)
                {
                    Vector2 inter = Vector2.Zero;
                    if (ent.pos.X < t.pos.X) { inter.X = ent.GetHBAfterMov().Width + ent.GetHBAfterMov().X - t.pos.X; }
                    else { inter.X = t.GetHB().Width + t.GetHB().X - ent.GetHBAfterMov().X; }
                    if (ent.pos.Y < t.pos.Y) { inter.Y = ent.GetHBAfterMov().Height + ent.GetHBAfterMov().Y - t.pos.Y; }
                    else { inter.Y = t.GetHB().Height + t.GetHB().Y - ent.GetHBAfterMov().Y; }
                    //calc best         
                    if (ent.GetHBafterX().Intersects(t.GetHB()))
                    {
                        if (ent.pos.X < t.pos.X)
                        {
                            ent.mov.X -= inter.X;
                        }
                        else
                        {
                            ent.mov.X += inter.X;
                        }
                        ent.isOnWall = true;
                    }
                    else if (ent.GetHBafterY().Intersects(t.GetHB()))
                    {

                        if (ent.pos.Y < t.pos.Y)
                        {
                            ent.mov.Y -= inter.Y;
                        }
                        else
                        {
                            ent.mov.Y += inter.Y;
                        }
                        ent.Yvel = 0f;
                    }                                         
                }
            }
        }
        void EvaluateInter()
        {

        }
        void DoCollisions()
        {
            player.onground = false;
            player.isOnWall = false;
            player.isOnLadder = false;
            foreach (Entity b in map.bouncies)
            {
                b.onground = false;
                b.isOnWall = false;
            }
            foreach (Tile t in map.tiles)
            {
                foreach (Entity e in map.bouncies)
                {
                    Collide(e, t);
                    if (e.GetHBAfterMov().Intersects(player.GetHBAfterMov())) { player.TakeDamage(1); }
                }
                Collide(player, t);

                //ARROW
                if (t.arrow)
                {
                    //if (t.activated)
                    //{
                        if (t.facing == "left")
                        {
                            if (player.pos.X < t.pos.X)
                            {
                                if (player.pos.Y - 20 < t.pos.Y && player.pos.Y + 20 > t.pos.Y)
                                {
                                    if (t.activated) {
                                    map.bouncies.Add(set.GetEntity(83, t.pos - new Vector2(32, 0)));
                                    t.activated = false;
                                    }                                   
                                }
                                else { t.activated = true; }
                            }
                        }
                    //}
                }
            }
            currentStr = "";
            
            foreach (Tile e in map.tiles)
            {
                if (e.isLaddery)
                {
                    if (player.GetHBAfterMov().Intersects(e.GetHB()))
                    {
                        player.isOnLadder = true;
                    }
                }
                if (e.actived != null && e.GetHB().Intersects(player.GetHBAfterMov()) && e.isDead == false)
                {
                    if (e.actived == "doublejump")
                    {
                        currentStr = "wow, shiny! pretty sure carrying this would prevent me from mid-air jumping though";
                    }
                    if (e.actived == "ladder")
                    {
                        currentStr = "wow, shiny! pretty sure carrying this would prevent me from using ladders";
                    }
                    if (e.actived == "dash")
                    {
                        currentStr = "wow, shiny! pretty sure carrying this would prevent me from dashing";
                    }
                    if (e.actived == "walljump")
                    {
                        currentStr = "wow, shiny! pretty sure carrying this would prevent me from jumping from walls";
                    }

                    if (ipp.JustPressed("space"))
                    {
                        //debuff
                        int id = e.activID;
                        e.isDead = true;
                        if (e.actived == "doublejump")
                        {
                            player.isDJumpDeactived = true;
                        }
                        if (e.actived == "ladder")
                        {
                            player.canClimbLadders = true;
                        }
                        if (e.actived == "dash")
                        {
                            player.isDashDeactived = true;
                        }
                        if (e.actived == "walljump")
                        {
                            player.isWJumpDeactived = true;
                        }
                        foreach (Tilemap mapp in maps)
                        {
                            foreach (Tile t in mapp.tiles)
                            {
                                if (!t.isDead && t.activID == id) { t.Activate(); }
                            }
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
            else { fd.DrawText("font", currentStr, new Rectangle(0, 272, 640, 320), spriteBatch);
                fd.DrawText("font", player.isOnLadder.ToString(), new Rectangle(0, 272, 640, 320), spriteBatch);
                for (int x = 0; x < 5; x++)
                {
                    if (x < player.hp) { fullheart.Draw(spriteBatch, new Vector2(16 * x, 0)); }
                    if (x >= player.hp) { emptyheart.Draw(spriteBatch, new Vector2(16 * x, 0)); }
                }
            }
            spriteBatch.End();

            //GAME DRAW
            Matrix translation = Matrix.CreateTranslation(new Vector3(-mapPos.X * 640,- mapPos.Y * 320, 0))*Matrix.CreateScale(1f/zoom);
            GraphicsDevice.SetRenderTarget(gameTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(transformMatrix:translation, samplerState: SamplerState.PointWrap, sortMode: SpriteSortMode.Immediate);
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