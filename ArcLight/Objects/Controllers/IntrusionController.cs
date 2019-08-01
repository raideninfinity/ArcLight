using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcLight
{
    public class IntrusionController
    {

        int blink_frame = 0;
        bool blink_visible = false;
        bool p1_selecting = false;
        bool p2_selecting = false;
        int p1_select_type = 0;
        int p1_select_mode = 0;
        int p2_select_type = 0;
        int p2_select_mode = 0;
        int p1_select_time_left = 0;
        int p2_select_time_left = 0;
        bool p1_continue = false;
        bool p2_continue = false;
        int p1_continue_time_left = 0;
        int p2_continue_time_left = 0;

        bool movable { get { return Core.Session.movable; } }

        public IntrusionController()
        {

        }

        public void Update()
        {
            if (blink_frame > 30)
            {
                blink_visible ^= true;
                blink_frame = 0;
            }
            blink_frame += 1;
            IntrusionControl();
            ContinueControl();
        }

        public void IntrusionControl()
        {
            //Intrusion
            if (!Core.Session.Player1Online() && movable && !p1_selecting)
            {
                if (Input.Trigger(0, 0))
                {
                    Audio.PlayStartButtonSE();
                    p1_selecting = true;
                    p1_select_mode = Core.Session.p1_last_mode;
                    p1_select_type = Core.Session.p1_last_type;
                    p1_select_time_left = 605;
                    p1_continue = false;
                    p1_continue_time_left = 0;
                }
            }
            if (!Core.Session.Player2Online() && movable && !p2_selecting)
            {
                if (Input.Trigger(0, 1))
                {
                    Audio.PlayStartButtonSE();
                    p2_selecting = true;
                    p2_select_mode = Core.Session.p2_last_mode;
                    p2_select_type = Core.Session.p2_last_type;
                    p2_select_time_left = 605;
                    p2_continue = false;
                    p2_continue_time_left = 0;
                }
            }
            //Intrusion Cancel
            if (p1_selecting && !movable)
                p1_selecting = false;
            if (p2_selecting && !movable)
                p2_selecting = false;
            //Intrusion Control
            if (p1_selecting)
            {
                if (Input.D4Trigger(0) != 0)
                    Audio.PlayCursorSE();
                switch (Input.D4Trigger(0))
                {
                    case 8: p1_select_mode -= 1; break;
                    case 6: p1_select_type += 1; break;
                    case 4: p1_select_type -= 1; break;
                    case 2: p1_select_mode += 1; break;
                }
                if (p1_select_type < 0) p1_select_type = 1;
                else if (p1_select_type > 1) p1_select_type = 0;
                if (p1_select_mode < 0) p1_select_mode = 1;
                else if (p1_select_mode > 1) p1_select_mode = 0;

                if (Input.Trigger(1, 0) || p1_select_time_left <= 0)
                {
                    if (Input.Trigger(1, 0))
                        Audio.PlayReadySE();
                    Core.Session.SetPlayer(0, p1_select_type, p1_select_mode, true);
                    p1_selecting = false;
                }
            }
            if (p2_selecting)
            {
                if (Input.D4Trigger(0) != 0)
                    Audio.PlayCursorSE();
                switch (Input.D4Trigger(1))
                {
                    case 8: p2_select_mode -= 1; break;
                    case 6: p2_select_type += 1; break;
                    case 4: p2_select_type -= 1; break;
                    case 2: p2_select_mode += 1; break;
                }
                if (p2_select_type < 0) p2_select_type = 1;
                else if (p2_select_type > 1) p2_select_type = 0;
                if (p2_select_mode < 0) p2_select_mode = 1;
                else if (p2_select_mode > 1) p2_select_mode = 0;

                if (Input.Trigger(1, 1) || p2_select_time_left <= 0)
                {
                    if (Input.Trigger(1, 1))
                        Audio.PlayReadySE();
                    Core.Session.SetPlayer(1, p2_select_type, p2_select_mode);
                    p2_selecting = false;
                }
            }
            //Step Select Time
            if (p1_select_time_left > 0) p1_select_time_left -= 1;
            if (p2_select_time_left > 0) p2_select_time_left -= 1;
        }

        public bool p1_prompt_c { get { return Core.Session.continue_prompt[0]; } set { Core.Session.continue_prompt[0] = value; } }
        public bool p2_prompt_c { get { return Core.Session.continue_prompt[1]; } set { Core.Session.continue_prompt[1] = value; } }

        public void ContinueControl()
        {
            if (p1_prompt_c)
            {
                p1_prompt_c = false;
                p1_continue = true;
                p1_continue_time_left = 605;
            }
            if (p2_prompt_c)
            {
                p2_prompt_c = false;
                p2_continue = true;
                p2_continue_time_left = 605;
            }
            if (p1_continue_time_left > 0) p1_continue_time_left -= 1;
            if (p2_continue_time_left > 0) p2_continue_time_left -= 1;
        }

        bool prev_inactive = false;
        bool faded_out = false;
        string prev_text = "";

        public void Draw()
        {
            SpriteFont font = Graphics.spr_font_main;
            string text;
            Vector2 pos;
            Vector2 origin;
            Color color = Color.White;
            float scale = 0.8f;
            Graphics.spriteBatch.Begin();
            //Draw game active

            if (!Core.Session.GameActive())
            {
                if (!prev_inactive && !faded_out)
                {
                    Audio.BGMHalfFadeOut(30);
                    faded_out = true;
                    prev_inactive = true;
                }
                Graphics.spriteBatch.Draw(Graphics.b_pixel, new Vector2(Graphics.Width / 2, Graphics.Height / 2),
                    Graphics.b_pixel.GetRect(), Color.White * 0.5f, 0, Graphics.b_pixel.GetCenter(),
                    new Vector2(Graphics.Width, Graphics.Height), SpriteEffects.None, 0);
            }
            else
            {
                if (prev_inactive && !Core.Session.game_over)
                {
                    Audio.BGMHalfFadeIn(30);
                    prev_inactive = false;
                    faded_out = false;
                }
            }
            //Draw Standby
            if (blink_visible)
            {
                text = movable ? "PRESS START" : "PLEASE WAIT";
                if (!Core.Session.Player1Online() && !p1_selecting)
                {
                    pos = new Vector2(8, 8);
                    origin = Vector2.Zero;
                    Graphics.spriteBatch.DrawString(font, text, pos, color, 0, origin, scale, SpriteEffects.None, 0);
                }
                if (!Core.Session.Player2Online() && !p2_selecting)
                {
                    pos = new Vector2(Graphics.Width - 8, 8);
                    origin = new Vector2(font.MeasureString(text).X, 0);
                    Graphics.spriteBatch.DrawString(font, text, pos, color, 0, origin, scale, SpriteEffects.None, 0);
                }
            }
            //Draw Continue
            if (p1_continue || p2_continue)
            {
                if (p1_continue_show())
                {
                    pos = new Vector2(8, 8);
                    origin = Vector2.Zero;
                    text = "CONTINUE?";
                    float x_size = font.MeasureString(text).X - 4.0f;
                    Graphics.spriteBatch.DrawString(font, text, pos + new Vector2(0, 16), color, 0, origin, scale, SpriteEffects.None, 0);
                    int i = (p1_continue_time_left / 60);
                    if (i < 2) color = Color.Red;
                    else if (i <= 5) color = Color.Yellow;
                    Graphics.spriteBatch.DrawString(font, i.ToString(), pos + new Vector2(x_size, 16), color, 0, origin, scale, SpriteEffects.None, 0);
                    color = Color.White;
                }
                if (p2_continue_show())
                {
                    pos = new Vector2(Graphics.Width - 8, 8);
                    text = "CONTINUE? ";
                    origin = new Vector2(font.MeasureString(text).X, 0);
                    int i = (p2_continue_time_left / 60);
                    float x_size = font.MeasureString("10").X;
                    Graphics.spriteBatch.DrawString(font, text, pos - new Vector2(x_size, 0) + new Vector2(0, 16), color, 0, origin, scale, SpriteEffects.None, 0);
                    if (i < 2) color = Color.Red;
                    else if (i <= 5) color = Color.Yellow;
                    text = i.ToString();
                    origin = new Vector2(font.MeasureString(text).X, 0);
                    Graphics.spriteBatch.DrawString(font, text, pos + new Vector2(0, 16), color, 0, origin, scale, SpriteEffects.None, 0);
                    color = Color.White;
                }
            }
            //Draw Selection
            if (p1_selecting)
            {
                pos = new Vector2(8, 8);
                origin = Vector2.Zero;
                text = "1P TIME";
                float x_size = font.MeasureString(text).X;
                Graphics.spriteBatch.DrawString(font, text, pos, color, 0, origin, scale, SpriteEffects.None, 0);
                int i = (p1_select_time_left / 60);
                if (i < 2) color = Color.Red;
                else if (i <= 5) color = Color.Yellow;
                Graphics.spriteBatch.DrawString(font, i.ToString(), pos + new Vector2(x_size, 0), color, 0, origin, scale, SpriteEffects.None, 0);
                color = Color.White;
                text = (p1_select_type == 0) ? "TYPE-A" : "TYPE-B";
                Graphics.spriteBatch.DrawString(font, text, pos + new Vector2(0, 16), color, 0, origin, scale, SpriteEffects.None, 0);
                text = (p1_select_mode == 0) ? "SPREAD" : "FOCUS";
                Graphics.spriteBatch.DrawString(font, text, pos + new Vector2(0, 32), color, 0, origin, scale, SpriteEffects.None, 0);
            }
            if (p2_selecting)
            {
                pos = new Vector2(Graphics.Width - 8, 8);
                text = "2P TIME ";
                origin = new Vector2(font.MeasureString(text).X, 0);
                int i = (p2_select_time_left / 60);
                float x_size = font.MeasureString("10").X;
                Graphics.spriteBatch.DrawString(font, text, pos - new Vector2(x_size, 0), color, 0, origin, scale, SpriteEffects.None, 0);
                if (i < 2) color = Color.Red;
                else if (i <= 5) color = Color.Yellow;
                text = i.ToString();
                origin = new Vector2(font.MeasureString(text).X, 0);
                Graphics.spriteBatch.DrawString(font, text, pos, color, 0, origin, scale, SpriteEffects.None, 0);
                color = Color.White;
                text = (p2_select_type == 0) ? "TYPE-A" : "TYPE-B";
                origin = new Vector2(font.MeasureString(text).X, 0);
                Graphics.spriteBatch.DrawString(font, text, pos + new Vector2(0, 16), color, 0, origin, scale, SpriteEffects.None, 0);
                text = (p2_select_mode == 0) ? "SPREAD" : "FOCUS";
                origin = new Vector2(font.MeasureString(text).X, 0);
                Graphics.spriteBatch.DrawString(font, text, pos + new Vector2(0, 32), color, 0, origin, scale, SpriteEffects.None, 0);
            }
            //Draw Main Continue
            if (!Core.Session.GameActive() && (!p1_selecting && !p2_selecting))
            {
                int continue_time_left = (p1_continue_time_left >= p2_continue_time_left) ? p1_continue_time_left : p2_continue_time_left;
                text = "CONTINUE?";
                origin = font.MeasureString(text) * 0.5f;
                pos = new Vector2(Graphics.Width / 2, Graphics.Height / 2 - origin.Y);
                scale = 2.0f;
                Graphics.spriteBatch.DrawString(font, text, pos + new Vector2(0, 0), color, 0, origin, scale, SpriteEffects.None, 0);
                int i = (continue_time_left / 60);
                text = i.ToString();
                origin = font.MeasureString(text) * 0.5f;
                if (i < 2) color = Color.Red;
                else if (i <= 5) color = Color.Yellow;
                Graphics.spriteBatch.DrawString(font, i.ToString(), pos + new Vector2(0, 40), color, 0, origin, scale, SpriteEffects.None, 0);
                if (prev_text != text)
                {
                    Audio.PlayBeepSE();
                    prev_text = text;
                }
            }
            Graphics.spriteBatch.End();
        }

        private bool p1_continue_show()
        {
            if (p1_continue_time_left <= 0) return false;
            if (!Core.Session.GameActive() && !p2_continue && !p2_selecting) return false;
            if ((p1_continue && p2_continue) && (p1_continue_time_left >= p2_continue_time_left)) return false;
            return true;
        }

        private bool p2_continue_show()
        {
            if (p2_continue_time_left <= 0) return false;
            if (!Core.Session.GameActive() && !p1_continue && !p1_selecting) return false;
            if ((p1_continue && p2_continue) && (p1_continue_time_left < p2_continue_time_left)) return false;
            return true;
        }

        public bool ContinueSelecting()
        {
            return !(!(p1_selecting || p2_selecting) && (p1_continue_time_left <= 0 && p2_continue_time_left <= 0));
        }

    }
}
