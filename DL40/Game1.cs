using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
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
        float timer;
        bool toecutter;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
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
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
