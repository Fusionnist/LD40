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
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
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
        bool toecutter;
        Tilemap map;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            
        }

        void InitGraphics()
        {
            virtualDims = new Point(640, 320);

            graphics.PreferredBackBufferHeight = (int)(GraphicsDevice.DisplayMode.Height / 1.5);
            graphics.PreferredBackBufferWidth = (int)(GraphicsDevice.DisplayMode.Width / 1.5);
            Window.IsBorderless = true;
            graphics.ApplyChanges();

            float xscale = (float)GraphicsDevice.Viewport.Width / virtualDims.X;
            float yscale = (float)GraphicsDevice.Viewport.Height / virtualDims.Y;
            scale = (float)Math.Round(Math.Min(xscale, yscale),1);
            if (scale < 1) { scale = 1; }
            targetPos = new Point(
                (GraphicsDevice.Viewport.Width - (int)(virtualDims.X * scale)) / 2,
                (GraphicsDevice.Viewport.Height - (int)(virtualDims.Y * scale)) / 2);

            gameTarget = new RenderTarget2D(GraphicsDevice, virtualDims.X, virtualDims.Y);
            textTarget = new RenderTarget2D(GraphicsDevice, virtualDims.X, virtualDims.Y);
            overlayTarget = new RenderTarget2D(GraphicsDevice, virtualDims.X, virtualDims.Y);
        }
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            InitGraphics();
            // TODO: Add your initialization logic here
            soundManager = new SoundManager();
            fd = new FontDrawer();

            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            map = getTilemap(XDocument.Load("Content/TestTilemap.tmx"));
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            td = new TextureDrawer(Content.Load<Texture2D>("sheet"),
                new Rectangle[] {
                    new Rectangle(0,0,64,64),
                    new Rectangle(72, 8, 48, 48),
                    new Rectangle(144, 16, 32, 32),
                    new Rectangle(216, 24, 16, 16), },
                new Point[] { new Point(32,32), new Point(24, 24), new Point(16, 16), new Point(8, 8), },
                1f, 4, true,"test");

            Texture2D src = Content.Load<Texture2D>("Original");
            Font f = new Font(new TextureDrawer[] {
                new TextureDrawer(src,new Rectangle(0,0,5,9),Point.Zero,"!"),
                new TextureDrawer(src,new Rectangle(5,0,8,9),Point.Zero,"o"),
                new TextureDrawer(src,new Rectangle(13,0,10,9),Point.Zero," ")
                },
                "font");
            fd.fonts.Add(f);
            // TODO: use this.Content to load your game content here
        }
        Tilemap getTilemap(XDocument doc_)
        {
            Point dims = new Point(int.Parse(doc_.Element("map").Attribute("width").Value), int.Parse(doc_.Element("map").Attribute("height").Value));
            Point tdims = new Point(int.Parse(doc_.Element("map").Attribute("tilewidth").Value), int.Parse(doc_.Element("map").Attribute("tileheight").Value));
            int count = dims.X * dims.Y;
            string raw = doc_.Element("map").Element("layer").Element("data").Value;
            List<Entity> tiles = new List<Entity>();
            Tileset ts = getTileset(XDocument.Load("Content/"+doc_.Element("map").Element("tileset").Attribute("source").Value));
            string[] split = raw.Split(',');
            
            for(int x=0; x < dims.X; x++)
            {
                for (int y = 0; y < dims.Y; y++)
                {
                    tiles.Add(ts.getTile(int.Parse(split[x + dims.X * y])-1,new Vector2(x*tdims.X,y*tdims.Y)));
                }
            }

           
            return new Tilemap(tiles, dims);
        }
        Tileset getTileset(XDocument doc_)
        {
            
            Point dims = new Point(int.Parse(doc_.Element("tileset").Attribute("tilewidth").Value), int.Parse(doc_.Element("tileset").Attribute("tileheight").Value));
            string path = doc_.Element("tileset").Element("image").Attribute("source").Value;
            path=path.Remove(path.Length - 4, 4);
            Texture2D src = Content.Load<Texture2D>(path);
            int columns= int.Parse(doc_.Element("tileset").Attribute("columns").Value);
            int count= int.Parse(doc_.Element("tileset").Attribute("tilecount").Value);
            bool[] solid = new bool[count];
            foreach(XElement tile in doc_.Element("tileset").Elements("tile"))//PROPERTIES!
            {
                foreach (XElement prop in tile.Element("objectgroup").Element("properties").Elements("property"))//PROPERTIES!
                {
                    if(prop.Attribute("name").Value == "solid")
                    {
                        solid[int.Parse(tile.Attribute("id").Value)] = bool.Parse(prop.Attribute("value").Value);
                    }
                }
            }
            
            return new Tileset(dims, src, columns, count, solid);
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float es = (float)gameTime.ElapsedGameTime.TotalSeconds;
            // TODO: Add your update logic here
            timer -= es;
            td.Update(es);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
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
            
            spriteBatch.End();

            //GAME DRAW
            GraphicsDevice.SetRenderTarget(gameTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            map.Draw(spriteBatch);
            td.Draw(spriteBatch, new Vector2(100, 100));
           
            spriteBatch.End();

            //DEFINITIVE DRAW
            Matrix scaleMatrix = Matrix.CreateScale(scale);
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(samplerState:SamplerState.PointWrap,sortMode:SpriteSortMode.Immediate);
            Rectangle dest = new Rectangle (targetPos.X, targetPos.Y, (int)(virtualDims.X * scale), (int)(virtualDims.Y * scale));
            spriteBatch.Draw(gameTarget, destinationRectangle: dest);
            spriteBatch.Draw(textTarget, destinationRectangle: dest);
            spriteBatch.Draw(overlayTarget, destinationRectangle: dest);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
