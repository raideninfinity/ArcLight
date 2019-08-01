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
    public class TitleScene : Scene
    {

        Texture2D title, prompt;
        int phase = 0;
        int frame = 0;
        int fade_in = 30;
        bool visible = false;

        public override void Start()
        {
            title = Cache.Texture("system/title/game_title", 1);
            prompt = Cache.Texture("system/title/1p_or_2p_start", 1);
            Graphics.FadeIn(fade_in);
            Audio.PlayTitleBGM();
            phase = 1;
        }

        public override void Update()
        {
            if (phase == 1)
            {
                if (frame == fade_in)
                {
                    phase = 2;
                    frame = 0;
                    visible = true;
                }
            }
            else if (phase == 2)
            {
                if (frame > 30)
                {
                    visible ^= true;
                    frame = 0;
                }
                if (Input.Trigger(0, 0) || Input.Trigger(0, 1))
                {
                    Audio.PlayNextSE();
                    frame = 0;
                    phase = 3;
                    visible = false;
                    Graphics.FadeOut(30);
                }
            }
            else if (phase == 3)
            {
                if (frame > 30)
                {
                    Core.StartScene(new SelectScene());
                }
            }
            frame += 1;
        }

        public override void Draw()
        {
            Graphics.spriteBatch.Begin();
            Graphics.spriteBatch.Draw(title, new Vector2(0, 0), Color.White);
            if (visible)
                Graphics.spriteBatch.Draw(prompt, new Vector2(0, 0), Color.White);
            Graphics.spriteBatch.End();
        }

        public override void End()
        {
            Cache.UnloadSegment(1);
        }
    }
}
