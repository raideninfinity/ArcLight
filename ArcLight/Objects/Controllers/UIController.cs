using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcLight
{
    public class UIController
    {

        public UIController()
        {

        }

        public void Update()
        {

        }

        public Color GetRatioColor(float ratio)
        {
            if (ratio < 0.25) { return Color.Red; }
            else if (ratio < 0.5) { return Color.Orange; }
            else if (ratio < 0.75) { return Color.Yellow; }
            else return Color.LimeGreen;
        }

        public void Draw()
        {
            Graphics.spriteBatch.Begin();
            //Draw Main UI
            string text;
            Vector2 pos, origin;
            SpriteFont font = Graphics.spr_font_main;
            Texture2D p = Graphics.w_pixel;
            Color color = Color.White;
            float scale = 0.8f;
            if (Core.Session.Player1Online())
            {
                GamePlayer player = Core.Session.player1;
                //Draw Score
                text = String.Format("1P {0:D10}", (int)player.score);
                pos = new Vector2(8, 8);
                origin = Vector2.Zero;
                Graphics.spriteBatch.DrawString(font, text, pos, color, 0, origin, scale, SpriteEffects.None, 0);
                //Draw Decoration
                pos = new Vector2(6, 25);
                Vector2 size = new Vector2(178, 2);
                Graphics.spriteBatch.Draw(p, pos + size * 0.5f, p.GetRect(), Color.White, 0, p.GetCenter(), size, SpriteEffects.None, 0);
                size = new Vector2(2, 32);
                Graphics.spriteBatch.Draw(p, pos + size * 0.5f, p.GetRect(), Color.White, 0, p.GetCenter(), size, SpriteEffects.None, 0);
                //Draw Burst Chain, Time
                if (player.chain > 0)
                {
                    text = "00";
                    float text_scale = Graphics.GetMainFontRatio(24.0f);
                    Vector2 text_size00 = font.MeasureString(text) * text_scale;
                    pos = new Vector2(12, 31);
                    text = String.Format("{0:D2}", player.chain);
                    Vector2 text_size = font.MeasureString(text) * text_scale;
                    Graphics.spriteBatch.DrawString(font, text, pos + new Vector2(text_size00.X - text_size.X, 0), Color.White, 0, Vector2.Zero, text_scale, SpriteEffects.None, 0);
                    text = "BURST";
                    text_scale = 0.8f;
                    Graphics.spriteBatch.DrawString(font, text, pos + new Vector2(text_size00.X + 8, 0), Color.White, 0, Vector2.Zero, text_scale, SpriteEffects.None, 0);
                    Vector2 bar_size = new Vector2(69, 7);
                    float ratio = player.chain_time / (Status.PlayerChainTime * 60.0f);
                    Vector2 bar_size_top = new Vector2(69 * ratio, 7);
                    pos = new Vector2(75, 48);
                    Graphics.spriteBatch.Draw(p, pos + bar_size * 0.5f, p.GetRect(), Color.White * 0.5f, 0, p.GetCenter(), bar_size, SpriteEffects.None, 0);
                    Graphics.spriteBatch.Draw(p, pos + bar_size_top * 0.5f, p.GetRect(), GetRatioColor(ratio), 0, p.GetCenter(), bar_size_top, SpriteEffects.None, 0);
                }
                //-------------------------------------------------------------------------------------------------------------------------
                Texture2D emblem = Cache.Texture(String.Format("system/emblem/emblem_s_{0}_{1}", player.type, player.mode), 1);
                pos = new Vector2(26.5f, 613.5f);
                Graphics.spriteBatch.Draw(emblem, pos, emblem.GetRect(), Color.White, 0, emblem.GetCenter(), 1, SpriteEffects.None, 0);
                //-------------------------------------------------------------------------------------------------------------------------
                Texture2D bar_bg = Cache.Texture(String.Format("system/game/ui_bg_left", player.type), 1);
                Texture2D bar_outline = Cache.Texture("system/game/bar_outline_left", 1);
                Texture2D bar_fill = Cache.Texture("system/game/bar_fill_left", 1);
                Texture2D charge = Cache.Texture("system/game/burst_charge_left", 1);
                pos = new Vector2(88, 617);
                Graphics.spriteBatch.Draw(bar_bg, pos, bar_bg.GetRect(), Color.White, 0, bar_bg.GetCenter(), 1, SpriteEffects.None, 0);
                //start draw fill
                pos = new Vector2(48, 615);
                float e_ratio = player.energy / 30000;
                Rectangle rect = new Rectangle(0, 0, (int)(e_ratio * bar_fill.Width), bar_fill.Height);
                Graphics.spriteBatch.Draw(bar_fill, pos, rect, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                //end
                pos = new Vector2(88, 624);
                Graphics.spriteBatch.Draw(bar_outline, pos, bar_outline.GetRect(), Color.White, 0, bar_outline.GetCenter(), 1, SpriteEffects.None, 0);
                pos = new Vector2(70.5f, 629);
                if (player.energy >= 10000)
                    Graphics.spriteBatch.Draw(charge, pos, charge.GetRect(), Color.White, 0, charge.GetCenter(), 1, SpriteEffects.None, 0);
                pos.X += 16;
                if (player.energy >= 20000)
                    Graphics.spriteBatch.Draw(charge, pos, charge.GetRect(), Color.White, 0, charge.GetCenter(), 1, SpriteEffects.None, 0);
                pos.X += 16;
                if (player.energy >= 30000)
                    Graphics.spriteBatch.Draw(charge, pos, charge.GetRect(), Color.White, 0, charge.GetCenter(), 1, SpriteEffects.None, 0);
                //-------------------------------------------------------------------------------------------------------------------------
                if (Data.ControlMode(0) > 0)
                {
                    Texture2D burst_indic = Cache.Texture(String.Format("system/game/burst_mode_{0}", player.b_wpn), 1);
                    pos = new Vector2(117.5f, 629);
                    Graphics.spriteBatch.Draw(burst_indic, pos, burst_indic.GetRect(), Color.White, 0, burst_indic.GetCenter(), 1, SpriteEffects.None, 0);
                }
                //-------------------------------------------------------------------------------------------------------------------------
                text = String.Format(" x {0}", player.lives);
                pos = new Vector2(74, 601);
                scale = Graphics.GetMainFontRatio(10.0f);
                Graphics.spriteBatch.DrawString(font, text, pos, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            }
            if (Core.Session.Player2Online())
            {
                GamePlayer player = Core.Session.player2;
                //Draw Score
                text = String.Format("2P {0:D10}", (int)player.score);
                pos = new Vector2(Graphics.Width - 8 - 177, 8);
                Graphics.spriteBatch.DrawString(font, text, pos, color, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0);
                //Draw Decoration
                Vector2 size = new Vector2(178, 2);
                pos = new Vector2(Graphics.Width - 6 - size.X, 26);
                Graphics.spriteBatch.Draw(p, pos + size * 0.5f, p.GetRect(), Color.White, 0, p.GetCenter(), size, SpriteEffects.None, 0);
                pos = new Vector2(Graphics.Width - 6 - 2, 26);
                size = new Vector2(2, 32);
                Graphics.spriteBatch.Draw(p, pos + size * 0.5f, p.GetRect(), Color.White, 0, p.GetCenter(), size, SpriteEffects.None, 0);
                //Draw Burst Chain, Time
                if (player.chain > 0)
                {
                    float offset_x = Graphics.Width - 160.0f;
                    text = "00";
                    float text_scale = Graphics.GetMainFontRatio(24.0f);
                    Vector2 text_size00 = font.MeasureString(text) * text_scale;
                    pos = new Vector2(12, 31);
                    text = String.Format("{0:D2}", player.chain);
                    Vector2 text_size = font.MeasureString(text) * text_scale;
                    Graphics.spriteBatch.DrawString(font, text, pos + new Vector2(offset_x + text_size00.X - text_size.X, 0), Color.White, 0, Vector2.Zero, text_scale, SpriteEffects.None, 0);
                    text = "BURST";
                    text_scale = 0.8f;
                    Graphics.spriteBatch.DrawString(font, text, pos + new Vector2(offset_x + text_size00.X + 8, 0), Color.White, 0, Vector2.Zero, text_scale, SpriteEffects.None, 0);
                    Vector2 bar_size = new Vector2(69, 7);
                    float ratio = player.chain_time / (Status.PlayerChainTime * 60.0f);
                    Vector2 bar_size_top = new Vector2(69 * ratio, 7);
                    pos = new Vector2(offset_x + 75, 48);
                    Graphics.spriteBatch.Draw(p, pos + bar_size * 0.5f, p.GetRect(), Color.White * 0.5f, 0, p.GetCenter(), bar_size, SpriteEffects.None, 0);
                    Graphics.spriteBatch.Draw(p, pos + bar_size_top * 0.5f, p.GetRect(), GetRatioColor(ratio), 0, p.GetCenter(), bar_size_top, SpriteEffects.None, 0);
                }
                //-------------------------------------------------------------------------------------------------------------------------
                Texture2D emblem = Cache.Texture(String.Format("system/emblem/emblem_s_{0}_{1}", player.type, player.mode), 1);
                pos = new Vector2(Graphics.Width - 26.5f, 613.5f);
                Graphics.spriteBatch.Draw(emblem, pos, emblem.GetRect(), Color.White, 0, emblem.GetCenter(), 1, SpriteEffects.None, 0);
                //-------------------------------------------------------------------------------------------------------------------------
                Texture2D bar_bg = Cache.Texture(String.Format("system/game/ui_bg_right", player.type), 1);
                Texture2D bar_outline = Cache.Texture("system/game/bar_outline_right", 1);
                Texture2D bar_fill = Cache.Texture("system/game/bar_fill_right", 1);
                Texture2D charge = Cache.Texture("system/game/burst_charge_right", 1);
                pos = new Vector2(Graphics.Width - 88, 617);
                Graphics.spriteBatch.Draw(bar_bg, pos, bar_bg.GetRect(), Color.White, 0, bar_bg.GetCenter(), 1, SpriteEffects.None, 0);
                //start draw fill
                float e_ratio = player.energy / 30000;
                Rectangle rect = new Rectangle(bar_fill.Width - (int)(e_ratio * bar_fill.Width), 0, (int)(e_ratio * bar_fill.Width), bar_fill.Height);
                pos = new Vector2(Graphics.Width - 48 - bar_fill.Width + rect.X, 615);
                Graphics.spriteBatch.Draw(bar_fill, pos, rect, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                //end
                pos = new Vector2(Graphics.Width - 88, 624);
                Graphics.spriteBatch.Draw(bar_outline, pos, bar_outline.GetRect(), Color.White, 0, bar_outline.GetCenter(), 1, SpriteEffects.None, 0);
                pos = new Vector2(Graphics.Width - 70.5f, 629);
                if (player.energy >= 10000)
                    Graphics.spriteBatch.Draw(charge, pos, charge.GetRect(), Color.White, 0, charge.GetCenter(), 1, SpriteEffects.None, 0);
                pos.X -= 16;
                if (player.energy >= 20000)
                    Graphics.spriteBatch.Draw(charge, pos, charge.GetRect(), Color.White, 0, charge.GetCenter(), 1, SpriteEffects.None, 0);
                pos.X -= 16;
                if (player.energy >= 30000)
                    Graphics.spriteBatch.Draw(charge, pos, charge.GetRect(), Color.White, 0, charge.GetCenter(), 1, SpriteEffects.None, 0);
                //-------------------------------------------------------------------------------------------------------------------------
                if (Data.ControlMode(1) > 0)
                {
                    Texture2D burst_indic = Cache.Texture(String.Format("system/game/burst_mode_{0}", player.b_wpn), 1);
                    pos = new Vector2(Graphics.Width - 117.5f, 629);
                    Graphics.spriteBatch.Draw(burst_indic, pos, burst_indic.GetRect(), Color.White, 0, burst_indic.GetCenter(), 1, SpriteEffects.None, 0);
                }
                //-------------------------------------------------------------------------------------------------------------------------
                text = String.Format(" x {0}", player.lives);
                pos = new Vector2(Graphics.Width - 86, 601);
                scale = Graphics.GetMainFontRatio(10.0f);
                Graphics.spriteBatch.DrawString(font, text, pos, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            }
            Graphics.spriteBatch.End();
        }

    }
}
