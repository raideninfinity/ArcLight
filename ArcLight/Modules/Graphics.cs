using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArcLight
{
    public static class Graphics
    {
        public static GameTime gameTime;
        public static int elapsedMs { get { return gameTime.ElapsedGameTime.Milliseconds; } }
        public static int totalMs { get { return (int)gameTime.TotalGameTime.TotalMilliseconds; } }

        public static Vector2 Size = new Vector2(480, 640);
        public static Vector2 FullScreenSize = new Vector2(480, 640);
        public static float ScaleFactorW { get { return Data.GetWindowSizeOriginal().X / Size.X; } }
        public static float ScaleFactorH { get { return Data.GetWindowSizeOriginal().Y / Size.Y; } }
        public static int ScaleWidth { get { return (int)Data.GetWindowSizeOriginal().X; } }
        public static int ScaleHeight { get { return (int)Data.GetWindowSizeOriginal().Y; } }
        public static int Width { get { return (int)Size.X; } }
        public static int Height { get { return (int)Size.Y; } }

        public static GraphicsDevice graphicsDevice;
        public static GraphicsDeviceManager graphicsDeviceManager;
        public static Game game;

        public static SpriteBatch spriteBatch;
        public static RenderTarget2D renderTarget;

        static bool frozen = false;
        static int fade_time;
        static int fade_counter;
        static int fade_mode;
        public static Texture2D b_pixel;
        public static Texture2D w_pixel;
        public static SpriteFont spr_font_main;

        public static bool Frozen { get { return frozen; } }
        public static int FadeMode { get { return fade_mode; } }
        public static int FadeCounter { get { return fade_counter; } }
        public static int FadeTime { get { return fade_time; } }

        public static void Initialize(GraphicsDevice gr, GraphicsDeviceManager m)
        {
            graphicsDevice = gr;
            graphicsDeviceManager = m;
            FullScreenSize = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            spriteBatch = new SpriteBatch(gr);
            renderTarget = new RenderTarget2D(gr, (int)Graphics.Size.X, (int)Graphics.Size.Y);
            b_pixel = new Texture2D(graphicsDevice, 1, 1);
            w_pixel = new Texture2D(graphicsDevice, 1, 1);
            b_pixel.SetData<Color>(new Color[] { Color.Black });
            w_pixel.SetData<Color>(new Color[] { Color.White });
            spr_font_main = Cache.Font("font/orbitron_medium", 0);
        }

        public static void Freeze()
        {
            frozen = true;
        }

        public static void FadeOut(int time)
        {
            if (fade_mode == 0)
            {
                fade_mode = 1;
                fade_time = fade_counter = time;
            }
        }

        public static void FadeIn(int time)
        {
            if (fade_mode == 3)
            {
                fade_mode = 2;
                fade_time = fade_counter = time;
            }
        }

        public static bool Fading() { return (fade_mode > 0 && fade_mode < 3); }
        public static bool FadingOut() { return (fade_mode == 1); }
        public static bool FadingIn() { return (fade_mode == 2); }
        public static bool FadedOut() { return (fade_mode == 3); }

        public static void Update()
        {
            frozen = false;

            if (fade_mode > 0 && fade_mode < 3)
            {
                if (fade_time > 0)
                {
                    fade_time -= 1;
                }
                else
                {
                    if (fade_mode == 1)
                    {
                        fade_mode = 3;
                    }
                    else
                    {
                        fade_mode = 0;
                    }
                    fade_counter = 0;
                    fade_time = 0;
                }
            }
            if (msg_duration > 0) msg_duration -= 1;
        }

        public static float GetMainFontRatio(float size)
        {
            return size / 16.0f;
        }

        public static void Draw()
        {
            if (frozen) { return; }
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.Clear(Color.Transparent);
            DrawRender();
            DrawFinisher();
            graphicsDevice.SetRenderTarget(null);
            graphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            float rotation = (Data.settings.orientation * 90f).ToRad();
            if (!Data.FullScreen())
            {
                spriteBatch.Draw(renderTarget, Data.GetWindowSize() * 0.5f, null, Color.White, rotation,
                    renderTarget.GetCenter(), new Vector2(ScaleFactorW, ScaleFactorH), SpriteEffects.None, 0);
            }
            else
            {
                Vector2 FScale;
                if ((Data.settings.orientation == 1 || Data.settings.orientation == 3))
                {
                    FScale = new Vector2(FullScreenSize.Y / Size.X);
                }
                else
                {
                    FScale = new Vector2(FullScreenSize.Y / Size.Y);
                }
                spriteBatch.Draw(renderTarget, FullScreenSize * 0.5f, null, Color.White, rotation,
                    renderTarget.GetCenter(), FScale, SpriteEffects.None, 0);
            }
            DrawMessage();
            spriteBatch.End();
        }

        private static void DrawRender()
        {
            if (Core.Scene != null)
                Core.Scene.Draw();
        }

        private static void DrawFinisher()
        {
            DrawFade();
        }

        private static float GetRotation()
        {
            return (Data.settings.orientation * 90f).ToRad();
        }

        private static void DrawFade()
        {
            if (Graphics.FadeMode > 0)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(b_pixel, Size * 0.5f, null, Color.White * GetFadeOpacity(),
                    0, b_pixel.GetCenter(), Size, SpriteEffects.None, 0);
                spriteBatch.End();
            }
        }

        public static void DrawMask(Color color)
        {
            Graphics.spriteBatch.Begin();
            spriteBatch.Draw(w_pixel, Size * 0.5f, null, color,
                0, w_pixel.GetCenter(), Size, SpriteEffects.None, 0);
            Graphics.spriteBatch.End();
        }

        private static float GetFadeOpacity()
        {
            if (Graphics.FadeMode == 1) //Fade Out
            {
                return 1.0f - ((float)Graphics.FadeTime / (float)Graphics.FadeCounter);
            }
            else if (Graphics.FadeMode == 2) //Fade In
            {
                return ((float)Graphics.FadeTime / (float)Graphics.FadeCounter);
            }
            else if (Graphics.FadeMode == 3) //Remain Black Screen
            {
                return 1.0f;
            }
            return 0.0f;
        }

        public static void ToggleFullScreen()
        {
            graphicsDeviceManager.IsFullScreen = Data.FullScreen();
            FixWindowSize();
        }

        public static void FixWindowSize()
        {
            Vector2 size = Data.GetWindowSize();
            if (graphicsDeviceManager.IsFullScreen)
            {
                graphicsDeviceManager.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphicsDeviceManager.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
            else
            {
                graphicsDeviceManager.PreferredBackBufferWidth = (int)size.X;
                graphicsDeviceManager.PreferredBackBufferHeight = (int)size.Y;
            }
            graphicsDeviceManager.ApplyChanges();
        }

        static string message = "";
        static int msg_duration = 0;

        public static void ShowMessage(string text, int duration)
        {
            message = text;
            msg_duration = duration;
        }

        public static void DrawMessage()
        {
            if (msg_duration > 0)
            {
                float text_scale = 0.6f;
                Vector2 text_size = spr_font_main.MeasureString(message);
                Graphics.spriteBatch.DrawString(spr_font_main, message, new Vector2(graphicsDeviceManager.PreferredBackBufferWidth / 2, 4),
                    Color.Magenta, 0, new Vector2(text_size.X * 0.5f, 0), text_scale, SpriteEffects.None, 0);
            }
        }
    }
}
