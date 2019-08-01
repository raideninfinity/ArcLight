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
    public class LogoScene : Scene
    {

        Texture2D logo1, logo2;
        float opacity1 = 0.0f, opacity2 = 0.0f;
        int phase = 0;
        int frame = 0;
        int interval = 60;
        int pause = 60;
        int fade = 60;

        public override void Start()
        {
            Graphics.FadeIn(15);
            logo1 = Cache.Texture("system/title/logo1", 1);
            logo2 = Cache.Texture("system/title/logo2", 1);
            phase = 1;
        }

        public override void Update()
        {
            if (phase == 1)
            {
                opacity1 = ((frame - 15) / (1.0f * interval));
                if (frame == interval + 15)
                {
                    phase = 2;
                    frame = 0;
                }
            }
            else if (phase == 2)
            {
                if (frame == pause)
                {
                    phase = 3;
                    frame = 0;
                }
            }
            else if (phase == 3)
            {
                opacity1 = 1.0f - (frame / (1.0f * interval));
                if (frame == interval)
                {
                    phase = 4;
                    frame = 0;
                }
            }
            else if (phase == 4)
            {
                opacity2 = ((frame - 15) / (1.0f * interval));
                if (frame == interval + 15)
                {
                    phase = 5;
                    frame = 0;
                }
            }
            else if (phase == 5)
            {
                if (frame == pause)
                {
                    phase = 6;
                    Graphics.FadeOut(fade);
                    frame = 0;
                }
            }
            else if (phase == 6)
            {
                if (frame == fade)
                {
                    phase = 7;
                    frame = 0;
                }
            }
            else if (phase == 7)
            {
                Core.StartScene(new TitleScene());
            }
            if (phase != 6 && (Input.Trigger(0, 0) || Input.Trigger(0, 1)))
            {
                phase = 6;
                Audio.PlayReadySE();
                Graphics.FadeOut(fade);
                frame = 0;
            }
            frame += 1;
        }

        public override void Draw()
        {
            Graphics.graphicsDevice.Clear(Color.White);
            Graphics.spriteBatch.Begin();
            if (opacity1 > 0.0f)
                Graphics.spriteBatch.Draw(logo1, new Vector2(0, 0), Color.White * opacity1);
            if (opacity2 > 0.0f)
                Graphics.spriteBatch.Draw(logo2, new Vector2(0, 0), Color.White * opacity2);
            Graphics.spriteBatch.End();
        }

        public override void End()
        {
            Cache.UnloadSegment(1);
        }
    }
}
