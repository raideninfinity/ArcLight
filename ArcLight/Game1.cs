using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ArcLight
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Data.Initialize();
            graphics.IsFullScreen = Data.FullScreen();
            Vector2 size = Data.GetWindowSize();
            if (graphics.IsFullScreen)
            {
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
            else
            {
                graphics.PreferredBackBufferWidth = (int)size.X;
                graphics.PreferredBackBufferHeight = (int)size.Y;
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
            Cache.Initialize(Content.ServiceProvider, "Content", 4);
            Graphics.Initialize(GraphicsDevice, graphics);
            Audio.Initialize();
            Core.Initialize();
            Core.StartGame();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
            Cache.UnloadEverything();
        }

        protected override void Update(GameTime gameTime)
        {
            Graphics.gameTime = gameTime;
            Core.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Graphics.gameTime = gameTime;
            Graphics.Draw();
            base.Draw(gameTime);
        }
    }
}
