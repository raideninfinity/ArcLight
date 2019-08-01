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
    public class SelectScene : Scene
    {

        Texture2D background;
        Texture2D fighter_a1, fighter_a2, fighter_b1, fighter_b2;
        Texture2D bit_a1, bit_a2, bit_b1, bit_b2;
        Texture2D top_shutter, btm_shutter, left_shutter, right_shutter;
        int fade_in = 30;
        int time = 1860;
        int time_left = 0;
        int phase = 0;
        int frame = 0;
        bool p1_start = false;
        bool p2_start = false;
        bool start_visible = true;
        bool p1_ready = false;
        bool p2_ready = false;
        int p1_type = -1;
        int p1_mode = -1;
        int p2_type = -1;
        int p2_mode = -1;
        bool shutter_on = false;
        float y_displace = 0;

        public override void Start()
        {
            background = Cache.Texture("system/select/select_fighter", 1);
            fighter_a1 = Cache.Texture("player/type_a_0", 1);
            fighter_a2 = Cache.Texture("player/type_a_1", 1);
            fighter_b1 = Cache.Texture("player/type_b_0", 1);
            fighter_b2 = Cache.Texture("player/type_b_1", 1);
            bit_a1 = Cache.Texture("player/type_a_0_bit", 1);
            bit_a2 = Cache.Texture("player/type_a_1_bit", 1);
            bit_b1 = Cache.Texture("player/type_b_0_bit", 1);
            bit_b2 = Cache.Texture("player/type_b_1_bit", 1);
            top_shutter = Cache.Texture("system/shutter/top_shutter", 1);
            btm_shutter = Cache.Texture("system/shutter/btm_shutter", 1);
            left_shutter = Cache.Texture("system/shutter/left_shutter", 1);
            right_shutter = Cache.Texture("system/shutter/right_shutter", 1);
            Graphics.FadeIn(fade_in);
        }

        public override void Update()
        {
            frame += 1;
            //Fade Out
            if (phase == 3)
            {
                if (frame == 61)
                {
                    Core.StartScene(new TitleScene());
                }
                return;
            }
            //Ready/Exit
            if (phase == 1)
            {
                shutter_on = true;
                if (frame == 45)
                {
                    StartGame();
                }
                return;
            }
            //Press Start
            if (frame > 30 && phase == 0)
            {
                start_visible ^= true;
                frame = 0;
            }
            time -= 1;
            time_left = time / 60;
            if (time_left < 0) time_left = 0;
            if (time <= 0)
            {
                if (!p1_start && !p2_start)
                {
                    //Timeout - Return to title
                    phase = 3;
                    frame = 0;
                    Audio.PlayTimeoutSE();
                    Graphics.FadeOut(60);
                }
                else
                {
                    p1_ready = p1_start;
                    p2_ready = p2_start;
                }
            }
            //Controls
            if (!p1_start)
            {
                if (Input.Trigger(0, 0))
                {
                    Audio.PlayStartButtonSE();
                    p1_start = true;
                    p1_type = 0;
                    p1_mode = 0;
                }
            }
            else
            {
                if (!p1_ready)
                {
                    int dir = Input.D4Trigger(0);
                    if (dir != 0)
                    {
                        Audio.PlayCursorSE();
                    }
                    switch (dir)
                    {
                        case 8: p1_mode -= 1; break;
                        case 6: p1_type += 1; break;
                        case 4: p1_type -= 1; break;
                        case 2: p1_mode += 1; break;
                    }
                    if (p1_type < 0) p1_type = 1;
                    else if (p1_type > 1) p1_type = 0;
                    if (p1_mode < 0) p1_mode = 1;
                    else if (p1_mode > 1) p1_mode = 0;
                    if (Input.Trigger(1, 0))
                    {
                        Audio.PlayReadySE();
                        p1_ready = true;
                    }
                }
            }
            if (!p2_start)
            {
                if (Input.Trigger(0, 1))
                {
                    Audio.PlayStartButtonSE();
                    p2_start = true;
                    p2_type = 0;
                    p2_mode = 0;
                }
            }
            else
            {
                if (!p2_ready)
                {
                    int dir = Input.D4Trigger(1);
                    if (dir != 0)
                    {
                        Audio.PlayCursorSE();
                    }
                    switch (dir)
                    {
                        case 8: p2_mode -= 1; break;
                        case 6: p2_type += 1; break;
                        case 4: p2_type -= 1; break;
                        case 2: p2_mode += 1; break;
                    }
                    if (p2_type < 0) p2_type = 1;
                    else if (p2_type > 1) p2_type = 0;
                    if (p2_mode < 0) p2_mode = 1;
                    else if (p2_mode > 1) p2_mode = 0;
                    if (Input.Trigger(1, 1))
                    {
                        Audio.PlayReadySE();
                        p2_ready = true;
                    }
                }
            }
            //Ready
            if ((p1_ready && !p2_start) || (p2_ready && !p1_start) || (p1_ready && p2_ready))
            {
                Audio.PlayShutterCloseSE();
                phase = 1;
                frame = 0;
                start_visible = false;
                Audio.BGMFadeOut(60);
            }
        }

        public void StartGame()
        {
            Core.StartNewGame(p1_start, p1_type, p1_mode, p2_start, p2_type, p2_mode);
        }

        string prev_text = "";

        public override void Draw()
        {
            Graphics.spriteBatch.Begin();
            //Draw Background
            Graphics.spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            //Draw Time Left
            SpriteFont font = Graphics.spr_font_main;
            string text = time_left.ToString();
            Vector2 size = font.MeasureString(text);
            Vector2 origin = size * 0.5f;
            float scale = 1.5f;
            Vector2 pos = new Vector2(Graphics.Width / 2, Graphics.Height - size.Y - 14);
            Color color = Color.White;
            if (time_left < 5) color = Color.Red;
            else if (time_left < 10) color = Color.Yellow;
            Graphics.spriteBatch.DrawString(font, text, pos, color, 0, origin, scale, SpriteEffects.None, 0);
            if (prev_text != text)
            {
                if ((time / 60) < 10)
                    Audio.PlayBeepSE();
                prev_text = text;
            }
            //Draw Start
            text = "PRESS START";
            size = font.MeasureString(text);
            origin = size * 0.5f;
            color = Color.White;
            scale = 0.8f;
            if (!p1_start && start_visible)
            {
                pos = new Vector2(Graphics.Width / 4 + 6, 200);
                Graphics.spriteBatch.DrawString(font, text, pos, color, 0, origin, scale, SpriteEffects.None, 0);
            }
            if (!p2_start && start_visible)
            {
                pos = new Vector2(Graphics.Width / 4 * 3 - 6, 200);
                Graphics.spriteBatch.DrawString(font, text, pos, color, 0, origin, scale, SpriteEffects.None, 0);
            }
            //Draw Standby/Ready
            scale = 1.0f;
            if (p1_start)
            {
                text = !p1_ready ? "STANDBY" : "READY";
                origin = font.MeasureString(text) * 0.5f;
                color = !p1_ready ? Color.White : Color.LightGreen;
                pos = new Vector2(Graphics.Width / 4 - 24, Graphics.Height - 36);
                Graphics.spriteBatch.DrawString(font, text, pos, color, 0, origin, scale, SpriteEffects.None, 0);
            }
            if (p2_start)
            {
                text = !p2_ready ? "STANDBY" : "READY";
                origin = font.MeasureString(text) * 0.5f;
                color = !p2_ready ? Color.White : Color.LightGreen;
                pos = new Vector2(Graphics.Width / 4 * 3 + 24, Graphics.Height - 36);
                Graphics.spriteBatch.DrawString(font, text, pos, color, 0, origin, scale, SpriteEffects.None, 0);
            }
            //Draw Fighter
            if (p1_start)
            {
                //Fighter
                Texture2D fighter = fighter_a1;
                if (p1_type == 0) fighter = fighter_a1;
                else if (p1_type == 1) fighter = fighter_b1;
                origin = new Vector2(fighter.Width / 2, fighter.Height / 2);
                pos = new Vector2(Graphics.Width / 4 + 4, Graphics.Height - 220 - y_displace);
                Graphics.spriteBatch.Draw(fighter, pos, fighter.GetRect(), Color.White, 0, origin, 1, SpriteEffects.None, 0);
                //Draw Bit
                Texture2D bit = (p1_type == 0) ? bit_a1 : bit_b1;
                if (p1_type == 0)
                {
                    pos += new Vector2(28, -21);
                    Graphics.spriteBatch.Draw(bit, pos, bit.GetRect(), Color.White, 0, bit.GetCenter(), 1, SpriteEffects.None, 0);
                    pos += new Vector2(-(28 * 2), 0);
                    Graphics.spriteBatch.Draw(bit, pos, bit.GetRect(), Color.White, 0, bit.GetCenter(), 1, SpriteEffects.None, 0);
                }
                else
                {
                    pos += new Vector2(21, 17);
                    Graphics.spriteBatch.Draw(bit, pos, bit.GetRect(), Color.White, 135f.ToRad(), bit.GetCenter(), 1, SpriteEffects.None, 0);
                    pos += new Vector2(-(21 * 2), 0);
                    Graphics.spriteBatch.Draw(bit, pos, bit.GetRect(), Color.White, -135f.ToRad(), bit.GetCenter(), 1, SpriteEffects.None, 0);
                }
                //Text
                text = (p1_type == 0) ? "TYPE-A" : "TYPE-B";
                origin = font.MeasureString(text) * 0.5f;
                color = !p1_ready ? Color.White : Color.LightGreen;
                pos = new Vector2(Graphics.Width / 4 + 4, 180);
                Graphics.spriteBatch.DrawString(font, text, pos, color, 0, origin, scale, SpriteEffects.None, 0);
                text = (p1_mode == 0) ? "SPREAD" : "FOCUS";
                origin = font.MeasureString(text) * 0.5f;
                pos = new Vector2(Graphics.Width / 4 + 4, 180 + 24);
                Graphics.spriteBatch.DrawString(font, text, pos, color, 0, origin, scale, SpriteEffects.None, 0);
                //Draw Emblem
                pos = new Vector2(126, 277);
                Texture2D emblem = Cache.Texture(String.Format("system/emblem/emblem_{0}_{1}", p1_type, p1_mode), 1);
                Graphics.spriteBatch.Draw(emblem, pos, emblem.GetRect(), Color.White, 0, emblem.GetCenter(), 1, SpriteEffects.None, 0);
            }
            if (p2_start)
            {
                //Fighter
                Texture2D fighter = fighter_a1;
                if (p2_type == 0) fighter = (p1_type == 0) ? fighter_a2 : fighter_a1;
                else if (p2_type == 1) fighter = (p1_type == 1) ? fighter_b2 : fighter_b1;
                origin = new Vector2(fighter.Width / 2, fighter.Height / 2);
                pos = new Vector2(Graphics.Width - (Graphics.Width / 4 + 4), Graphics.Height - 220 - y_displace);
                Graphics.spriteBatch.Draw(fighter, pos, fighter.GetRect(), Color.White, 0, origin, 1, SpriteEffects.None, 0);
                //Draw Bit
                Texture2D bit = null;
                if (p2_type == 0) bit = (p1_type == 0) ? bit_a2 : bit_a1;
                else if (p2_type == 1) bit = (p1_type == 1) ? bit_b2 : bit_b1;
                if (p2_type == 0)
                {
                    pos += new Vector2(28, -21);
                    Graphics.spriteBatch.Draw(bit, pos, bit.GetRect(), Color.White, 0, bit.GetCenter(), 1, SpriteEffects.None, 0);
                    pos += new Vector2(-(28 * 2), 0);
                    Graphics.spriteBatch.Draw(bit, pos, bit.GetRect(), Color.White, 0, bit.GetCenter(), 1, SpriteEffects.None, 0);
                }
                else
                {
                    pos += new Vector2(21, 17);
                    Graphics.spriteBatch.Draw(bit, pos, bit.GetRect(), Color.White, 135f.ToRad(), bit.GetCenter(), 1, SpriteEffects.None, 0);
                    pos += new Vector2(-(21 * 2), 0);
                    Graphics.spriteBatch.Draw(bit, pos, bit.GetRect(), Color.White, -135f.ToRad(), bit.GetCenter(), 1, SpriteEffects.None, 0);
                }
                //Text
                text = (p2_type == 0) ? "TYPE-A" : "TYPE-B";
                origin = font.MeasureString(text) * 0.5f;
                color = !p2_ready ? Color.White : Color.LightGreen;
                pos = new Vector2(Graphics.Width - (Graphics.Width / 4 + 4), 180);
                Graphics.spriteBatch.DrawString(font, text, pos, color, 0, origin, scale, SpriteEffects.None, 0);
                text = (p2_mode == 0) ? "SPREAD" : "FOCUS";
                origin = font.MeasureString(text) * 0.5f;
                pos = new Vector2(Graphics.Width - (Graphics.Width / 4 + 4), 180 + 24);
                Graphics.spriteBatch.DrawString(font, text, pos, color, 0, origin, scale, SpriteEffects.None, 0);
                //Draw Emblem
                pos = new Vector2(Graphics.Width - 126, 277);
                Texture2D emblem = Cache.Texture(String.Format("system/emblem/emblem_{0}_{1}", p2_type, p2_mode), 1);
                Graphics.spriteBatch.Draw(emblem, pos, emblem.GetRect(), Color.White, 0, emblem.GetCenter(), 1, SpriteEffects.None, 0);
            }
            //Shutter
            if (shutter_on)
            {
                float offset_rate = (frame / 30f).Clamp(0, 1);
                pos = new Vector2(Graphics.Width * 0.5f * offset_rate, Graphics.Height * 0.5f);
                Graphics.spriteBatch.Draw(left_shutter, pos, left_shutter.GetRect(), Color.White, 0, left_shutter.GetCenter(), 1, SpriteEffects.None, 0);
                pos = new Vector2(Graphics.Width - Graphics.Width * 0.5f * offset_rate, Graphics.Height * 0.5f);
                Graphics.spriteBatch.Draw(right_shutter, pos, right_shutter.GetRect(), Color.White, 0, right_shutter.GetCenter(), 1, SpriteEffects.None, 0);
                offset_rate = (frame / 30f).Clamp(0, 1);
                pos = new Vector2(Graphics.Width * 0.5f, Graphics.Height * 0.5f * offset_rate);
                Graphics.spriteBatch.Draw(top_shutter, pos, top_shutter.GetRect(), Color.White, 0, top_shutter.GetCenter(), 1, SpriteEffects.None, 0);
                pos = new Vector2(Graphics.Width * 0.5f, Graphics.Height - Graphics.Height * 0.5f * offset_rate);
                Graphics.spriteBatch.Draw(btm_shutter, pos, btm_shutter.GetRect(), Color.White, 0, btm_shutter.GetCenter(), 1, SpriteEffects.None, 0);
            }
            //End
            Graphics.spriteBatch.End();
        }

        public override void End()
        {
            Cache.UnloadSegment(1);
        }
    }
}
