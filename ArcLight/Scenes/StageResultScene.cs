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
    public class StageResultScene : Scene
    {

        Texture2D top_shutter, btm_shutter, left_shutter, right_shutter, results_bg;
        Texture2D result_text_bg;
        int phase;
        int frame;
        SpriteFont font;

        public override void Start()
        {
            frame = 0;
            font = Graphics.spr_font_main;
            top_shutter = Cache.Texture("system/shutter/top_shutter", 1);
            btm_shutter = Cache.Texture("system/shutter/btm_shutter", 1);
            left_shutter = Cache.Texture("system/shutter/left_shutter", 1);
            right_shutter = Cache.Texture("system/shutter/right_shutter", 1);
            results_bg = Cache.Texture("system/result/stage_clear_results", 1);
            result_text_bg = Cache.Texture("system/result/result_text_bg", 1);
            phase = 1;
            CalculateScore();
            Audio.PlayResultBGM();
        }

        float p1_total, p2_total;

        void CalculateScore()
        {
            if (Core.Session.Player1Online())
            {
                GamePlayer player = Core.Session.player1;
                p1_total = player.score;
                p1_total += Status.StageClearScore[Core.Session.stage - 1];
                if (Core.Session.GetFlag("boss_defeat"))
                    p1_total += Status.BossDefeatScore(Core.Session.stage);
                if (Core.Session.GetFlag("midboss_defeat"))
                    p1_total += Status.MidbossDefeatScore(Core.Session.stage);
                p1_total += player.lives * Status.RemainScore;
                p1_total += player.max_chain * Status.BurstChainScore;
                if (Core.Session.Player1NoMiss())
                    p1_total += Status.NoMissScore;
                player.score = p1_total;
            }
            if (Core.Session.Player2Online())
            {
                GamePlayer player = Core.Session.player2;
                p2_total = player.score;
                p2_total += Status.StageClearScore[Core.Session.stage - 1];
                if (Core.Session.GetFlag("boss_defeat"))
                    p1_total += Status.BossDefeatScore(Core.Session.stage);
                if (Core.Session.GetFlag("midboss_defeat"))
                    p1_total += Status.MidbossDefeatScore(Core.Session.stage);
                p2_total += player.lives * Status.RemainScore;
                p2_total += player.max_chain * Status.BurstChainScore;
                if (Core.Session.Player2NoMiss())
                    p2_total += Status.NoMissScore;
                player.score = p2_total;
            }
        }

        float bg_alpha = 0;
        float text_alpha = 0;

        public override void Update()
        {
            frame += 1;
            if (phase == 1)
            {
                if (bg_alpha < 1.0f) bg_alpha += (1.0f / 30);
                else
                {
                    phase = 2;
                    frame = 0;
                }
            }
            else if (phase == 2)
            {
                if (text_alpha < 1.0f) text_alpha += (1.0f / 30);
                if (frame >= 630 || Input.TriggerAny())
                {
                    text_alpha = 1.0f;
                    phase = 3;
                }
                if (Input.TriggerAny())
                {
                    Audio.PlayReadySE();
                }
            }
            else if (phase == 3)
            {
                if (bg_alpha > 0.0f) bg_alpha -= (1.0f / 60);
                if (text_alpha > 0.0f) text_alpha -= (1.0f / 60);
                else { phase = 4; frame = 0; Audio.BGMFadeOut(60); }
            }
            else if (phase == 4)
            {
                if (frame == 30) { NextScene(); }
            }
        }

        public void NextScene()
        {
            if (Core.Session.stage == Core.Session.max_stage)
            {
                Core.StartScene(new EndScene());
            }
            else
            {
                Core.Session.stage += 1;
                Core.StartScene(new GameScene());
            }
        }

        public override void Draw()
        {
            Graphics.spriteBatch.Begin();
            //Background
            Vector2 pos = new Vector2(Graphics.Width * 0.5f, Graphics.Height * 0.5f);
            Graphics.spriteBatch.Draw(left_shutter, pos, left_shutter.GetRect(), Color.White, 0, left_shutter.GetCenter(), 1, SpriteEffects.None, 0);
            Graphics.spriteBatch.Draw(right_shutter, pos, right_shutter.GetRect(), Color.White, 0, right_shutter.GetCenter(), 1, SpriteEffects.None, 0);
            Graphics.spriteBatch.Draw(top_shutter, pos, top_shutter.GetRect(), Color.White, 0, top_shutter.GetCenter(), 1, SpriteEffects.None, 0);
            Graphics.spriteBatch.Draw(btm_shutter, pos, btm_shutter.GetRect(), Color.White, 0, btm_shutter.GetCenter(), 1, SpriteEffects.None, 0);
            Graphics.spriteBatch.Draw(results_bg, pos, results_bg.GetRect(), Color.White * bg_alpha, 0, results_bg.GetCenter(), 1, SpriteEffects.None, 0);
            Graphics.spriteBatch.Draw(result_text_bg, pos, result_text_bg.GetRect(), Color.White * text_alpha, 0, result_text_bg.GetCenter(), 1, SpriteEffects.None, 0);
            DrawP1Text();
            DrawP2Text();
            Graphics.spriteBatch.End();
        }

        public void DrawP1Text()
        {
            if (Core.Session.Player1Online())
            {
                string text = "TOTAL";
                Vector2 text_size;
                float size = Graphics.GetMainFontRatio(10.0f);
                Vector2 pos = new Vector2(92, 327);
                Graphics.spriteBatch.DrawString(font, text, pos, Color.Yellow * text_alpha, 0, Vector2.Zero, size, SpriteEffects.None, 1);
                text = String.Format("{0:D10}", (int)Core.Session.player1.score);
                size = Graphics.GetMainFontRatio(18.0f);
                pos = new Vector2(160, 319);
                Graphics.spriteBatch.DrawString(font, text, pos, Color.White * text_alpha, 0, Vector2.Zero, size, SpriteEffects.None, 1);

                float y_add = 0;
                //Clear
                size = Graphics.GetMainFontRatio(12.0f);
                text = "CLEAR";
                text_size = font.MeasureString(text);
                pos = new Vector2(92, 192);
                Graphics.spriteBatch.DrawString(font, text, pos, Color.White * text_alpha, 0, new Vector2(0, text_size.Y), size, SpriteEffects.None, 1);
                text = ((int)Status.StageClearScore[Core.Session.stage - 1]).ToString();
                text_size = font.MeasureString(text);
                size = Graphics.GetMainFontRatio(16.0f);
                pos = new Vector2(390, 192);
                Graphics.spriteBatch.DrawString(font, text, pos, Color.White * text_alpha, 0, text_size, size, SpriteEffects.None, 1);
                float y_unit = 28;
                y_add += y_unit;
                //Boss Defeat
                float boss_defeat_score = 0;
                if (Core.Session.GetFlag("boss_defeat"))
                    boss_defeat_score += Status.BossDefeatScore(Core.Session.stage);
                if (Core.Session.GetFlag("midboss_defeat"))
                    boss_defeat_score += Status.MidbossDefeatScore(Core.Session.stage);
                if (boss_defeat_score > 0)
                {
                    text = "BOSS DEFEAT";
                    size = Graphics.GetMainFontRatio(12.0f);
                    text_size = font.MeasureString(text);
                    pos = new Vector2(92, 192 + y_add);
                    Graphics.spriteBatch.DrawString(font, text, pos, Color.White * text_alpha, 0, new Vector2(0, text_size.Y), size, SpriteEffects.None, 1);
                    text = ((int)boss_defeat_score).ToString();
                    text_size = font.MeasureString(text);
                    size = Graphics.GetMainFontRatio(16.0f);
                    pos = new Vector2(390, 192 + y_add);
                    Graphics.spriteBatch.DrawString(font, text, pos, Color.White * text_alpha, 0, text_size, size, SpriteEffects.None, 1);
                    y_add += y_unit;
                }
                //Remain
                if (Core.Session.player1.lives > 0)
                {
                    text = "REMAIN";
                    size = Graphics.GetMainFontRatio(12.0f);
                    text_size = font.MeasureString(text);
                    pos = new Vector2(92, 192 + y_add);
                    Graphics.spriteBatch.DrawString(font, text, pos, Color.White * text_alpha, 0, new Vector2(0, text_size.Y), size, SpriteEffects.None, 1);
                    text = String.Format("{0} x {1}", Core.Session.player1.lives, Status.RemainScore);
                    text_size = font.MeasureString(text);
                    size = Graphics.GetMainFontRatio(16.0f);
                    pos = new Vector2(390, 192 + y_add);
                    Graphics.spriteBatch.DrawString(font, text, pos, Color.White * text_alpha, 0, text_size, size, SpriteEffects.None, 1);
                    y_add += y_unit;
                }
                //Burst Chain
                if (Core.Session.player1.max_chain > 0)
                {
                    text = "BURST CHAIN";
                    size = Graphics.GetMainFontRatio(12.0f);
                    text_size = font.MeasureString(text);
                    pos = new Vector2(92, 192 + y_add);
                    Graphics.spriteBatch.DrawString(font, text, pos, Color.White * text_alpha, 0, new Vector2(0, text_size.Y), size, SpriteEffects.None, 1);
                    text = String.Format("{0} x {1}", Core.Session.player1.max_chain, Status.BurstChainScore);
                    text_size = font.MeasureString(text);
                    size = Graphics.GetMainFontRatio(16.0f);
                    pos = new Vector2(390, 192 + y_add);
                    Graphics.spriteBatch.DrawString(font, text, pos, Color.White * text_alpha, 0, text_size, size, SpriteEffects.None, 1);
                    y_add += y_unit;
                }
                //No Miss
                if (Core.Session.Player1NoMiss())
                {
                    text = "NO MISS";
                    size = Graphics.GetMainFontRatio(12.0f);
                    text_size = font.MeasureString(text);
                    pos = new Vector2(92, 192 + y_add);
                    Graphics.spriteBatch.DrawString(font, text, pos, Color.White * text_alpha, 0, new Vector2(0, text_size.Y), size, SpriteEffects.None, 1);
                    text = String.Format("{0}", Status.NoMissScore);
                    text_size = font.MeasureString(text);
                    size = Graphics.GetMainFontRatio(16.0f);
                    pos = new Vector2(390, 192 + y_add);
                    Graphics.spriteBatch.DrawString(font, text, pos, Color.White * text_alpha, 0, text_size, size, SpriteEffects.None, 1);
                    y_add += y_unit;
                }
            }
            else
            {
                string text = "NO DATA";
                Vector2 text_size = font.MeasureString(text);
                float size = 1.2f;
                Vector2 pos = new Vector2(Graphics.Width / 2, 239);
                Graphics.spriteBatch.DrawString(font, text, pos, Color.Red * text_alpha, 0, text_size * 0.5f, size, SpriteEffects.None, 1);
            }
        }

        public void DrawP2Text()
        {
            if (Core.Session.Player2Online())
            {
                int y_offset = 215;
                GamePlayer p = Core.Session.player2;
                string text = "TOTAL";
                float size = Graphics.GetMainFontRatio(10.0f);
                Vector2 pos = new Vector2(92, y_offset + 327);
                Graphics.spriteBatch.DrawString(font, text, pos, Color.Yellow * text_alpha, 0, Vector2.Zero, size, SpriteEffects.None, 1);
                text = String.Format("{0:D10}", (int)p.score);
                size = Graphics.GetMainFontRatio(18.0f);
                pos = new Vector2(160, y_offset + 319);
                Graphics.spriteBatch.DrawString(font, text, pos, Color.White * text_alpha, 0, Vector2.Zero, size, SpriteEffects.None, 1);

                float y_add = y_offset;
                //Clear
                size = Graphics.GetMainFontRatio(12.0f);
                text = "CLEAR";
                Vector2 text_size = font.MeasureString(text);
                pos = new Vector2(92, y_offset + 192);
                Graphics.spriteBatch.DrawString(font, text, pos, Color.White * text_alpha, 0, new Vector2(0, text_size.Y), size, SpriteEffects.None, 1);
                text = ((int)Status.StageClearScore[Core.Session.stage - 1]).ToString();
                text_size = font.MeasureString(text);
                size = Graphics.GetMainFontRatio(16.0f);
                pos = new Vector2(390, y_offset + 192);
                Graphics.spriteBatch.DrawString(font, text, pos, Color.White * text_alpha, 0, text_size, size, SpriteEffects.None, 1);
                float y_unit = 28;
                y_add += y_unit;
                //Boss Defeat
                float boss_defeat_score = 0;
                if (Core.Session.GetFlag("boss_defeat"))
                    boss_defeat_score += Status.BossDefeatScore(Core.Session.stage);
                if (Core.Session.GetFlag("midboss_defeat"))
                    boss_defeat_score += Status.MidbossDefeatScore(Core.Session.stage);
                if (boss_defeat_score > 0)
                {
                    text = "BOSS DEFEAT";
                    size = Graphics.GetMainFontRatio(12.0f);
                    text_size = font.MeasureString(text);
                    pos = new Vector2(92, 192 + y_add);
                    Graphics.spriteBatch.DrawString(font, text, pos, Color.White * text_alpha, 0, new Vector2(0, text_size.Y), size, SpriteEffects.None, 1);
                    text = ((int)boss_defeat_score).ToString();
                    text_size = font.MeasureString(text);
                    size = Graphics.GetMainFontRatio(16.0f);
                    pos = new Vector2(390, 192 + y_add);
                    Graphics.spriteBatch.DrawString(font, text, pos, Color.White * text_alpha, 0, text_size, size, SpriteEffects.None, 1);
                    y_add += y_unit;
                }
                //Remain
                if (Core.Session.player2.lives > 0)
                {
                    text = "REMAIN";
                    size = Graphics.GetMainFontRatio(12.0f);
                    text_size = font.MeasureString(text);
                    pos = new Vector2(92, 192 + y_add);
                    Graphics.spriteBatch.DrawString(font, text, pos, Color.White * text_alpha, 0, new Vector2(0, text_size.Y), size, SpriteEffects.None, 1);
                    text = String.Format("{0} x {1}", Core.Session.player2.lives, Status.RemainScore);
                    text_size = font.MeasureString(text);
                    size = Graphics.GetMainFontRatio(16.0f);
                    pos = new Vector2(390, 192 + y_add);
                    Graphics.spriteBatch.DrawString(font, text, pos, Color.White * text_alpha, 0, text_size, size, SpriteEffects.None, 1);
                    y_add += y_unit;
                }
                //Burst Chain
                if (Core.Session.player2.max_chain > 0)
                {
                    text = "BURST CHAIN";
                    size = Graphics.GetMainFontRatio(12.0f);
                    text_size = font.MeasureString(text);
                    pos = new Vector2(92, 192 + y_add);
                    Graphics.spriteBatch.DrawString(font, text, pos, Color.White * text_alpha, 0, new Vector2(0, text_size.Y), size, SpriteEffects.None, 1);
                    text = String.Format("{0} x {1}", Core.Session.player2.max_chain, Status.BurstChainScore);
                    text_size = font.MeasureString(text);
                    size = Graphics.GetMainFontRatio(16.0f);
                    pos = new Vector2(390, 192 + y_add);
                    Graphics.spriteBatch.DrawString(font, text, pos, Color.White * text_alpha, 0, text_size, size, SpriteEffects.None, 1);
                    y_add += y_unit;
                }
                //No Miss
                if (Core.Session.Player2NoMiss())
                {
                    text = "NO MISS";
                    size = Graphics.GetMainFontRatio(12.0f);
                    text_size = font.MeasureString(text);
                    pos = new Vector2(92, 192 + y_add);
                    Graphics.spriteBatch.DrawString(font, text, pos, Color.White * text_alpha, 0, new Vector2(0, text_size.Y), size, SpriteEffects.None, 1);
                    text = String.Format("{0}", Status.NoMissScore);
                    text_size = font.MeasureString(text);
                    size = Graphics.GetMainFontRatio(16.0f);
                    pos = new Vector2(390, 192 + y_add);
                    Graphics.spriteBatch.DrawString(font, text, pos, Color.White * text_alpha, 0, text_size, size, SpriteEffects.None, 1);
                    y_add += y_unit;
                }
            }
            else
            {
                string text = "NO DATA";
                Vector2 text_size = font.MeasureString(text);
                float size = 1.2f;
                Vector2 pos = new Vector2(Graphics.Width / 2, 458);
                Graphics.spriteBatch.DrawString(font, text, pos, Color.Red * text_alpha, 0, text_size * 0.5f, size, SpriteEffects.None, 1);
            }
        }

        public override void End()
        {
            Cache.UnloadSegment(1);
        }
    }
}
