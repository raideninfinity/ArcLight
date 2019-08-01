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
    public class EndScene : Scene
    {

        int phase;
        int frame;
        int xframe;
        SpriteFont font;

        public override void Start()
        {
            phase = 1;
            frame = 0;
            font = Graphics.spr_font_main;
        }

        public override void Update()
        {
            xframe += 1;
            if (phase == 1)
            {
                if (frame == 60)
                {
                    frame = 0;
                    phase = 2;
                }
            }
            else if (phase == 2)
            {
                if (frame == 120)
                {
                    frame = 0;
                    Graphics.FadeOut(60);
                    phase = 3;
                }
            }
            else if (phase == 3)
            {
                if (frame > 60)
                {
                    Core.StartScene(new TitleScene());
                }
            }
            frame += 1;
        }

        string text1 = "THE ";
        string text2 = "END ";
        string text3 = "THANKS FOR PLAYING";

        public override void Draw()
        {
            Graphics.graphicsDevice.Clear(Color.Black);
            Vector2 pos = new Vector2(Graphics.Width / 2, Graphics.Height / 2);
            Vector2 size = new Vector2(0, 0);
            float space_x_half = font.MeasureString(" ").X * 0.5f;
            float scale = 2.0f;
            Color color = Color.White;
            Graphics.spriteBatch.Begin();
            size = font.MeasureString(text1);
            float pad_x;
            if (phase >= 2) pad_x = 0;
            else pad_x = (60 - frame) * ((Graphics.Width / 2) / 60.0f);
            Graphics.spriteBatch.DrawString(font, text1, pos - new Vector2(size.X + space_x_half + pad_x, 0), color, 0, size * 0.5f, scale, SpriteEffects.None, 0);
            Graphics.spriteBatch.DrawString(font, text2, pos + new Vector2(size.X + space_x_half + pad_x, 0), color, 0, size * 0.5f, scale, SpriteEffects.None, 0);
            scale = 1.0f;
            float opacity = ((xframe - 60) / 30.0f).Clamp(0, 1);
            size = font.MeasureString(text3);
            Graphics.spriteBatch.DrawString(font, text3, new Vector2(Graphics.Width / 2, 420), color * opacity, 0, size * 0.5f, scale, SpriteEffects.None, 0);

            Graphics.spriteBatch.End();
        }

        public override void End()
        {
            Cache.UnloadSegment(1);
        }
    }
}
