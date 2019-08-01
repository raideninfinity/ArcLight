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
    public class GameScene : Scene
    {

        public GameController controller;
        public UIController uiController;
        public IntrusionController intrusionController;
        List<ParallaxPlane> backgrounds = new List<ParallaxPlane>();
        int phase = 0;
        public int frame = 0;
        Texture2D top_shutter, btm_shutter, left_shutter, right_shutter;
        Texture2D boss_alert_bg, boss_alert_caption, boss_alert_text;

        public bool boss_alert = false;
        public float displacement;
        public bool show_msg = false;
        float msg_opacity = 0;
        string msg_text = "";
        public BossEnemy boss_enemy;

        public override void Start()
        {
            backgrounds.Clear();
            backgrounds.Add(new ParallaxPlane(Cache.Texture("background/bg" + Core.Session.stage.ToString(), 1), 0, 1));
            top_shutter = Cache.Texture("system/shutter/top_shutter", 1);
            btm_shutter = Cache.Texture("system/shutter/btm_shutter", 1);
            left_shutter = Cache.Texture("system/shutter/left_shutter", 1);
            right_shutter = Cache.Texture("system/shutter/right_shutter", 1);
            boss_alert_bg = Cache.Texture("system/game/boss_alert_bg", 1);
            boss_alert_caption = Cache.Texture("system/game/boss_alert_caption", 1);
            boss_alert_text = Cache.Texture("system/game/boss_alert_text", 1);
            controller = new GameController();
            intrusionController = new IntrusionController();
            uiController = new UIController();
            //Set BGM
            SetStageBGM(Core.Session.stage);
        }

        public void SetStageBGM(int stage)
        {
            if (stage == 1)
                Audio.LoadStage1BGM();
        }

        public override void Update()
        {
            frame += 1;
            if (phase == 0)
            {
                if (frame >= 30) { phase = 1; frame = 0; Audio.PlayShutterOpenSE(); }
            }
            else if (phase == 1)
            {
                if (frame == 30)
                {
                    phase = 2;
                    frame = 0;
                    Audio.PlayStageBGM();
                }
            }
            if (Core.Session.GameActive())
            {
                backgrounds.ForEach(a => a.Update());
                uiController.Update();
            }
            if (phase > 1)
            {
                if (Core.Session.GameActive()) controller.Update();
                intrusionController.Update();
                if (phase != 5)
                    UpdateGameOver();
            }
            if (controller.StageEnd && phase != 5)
            {
                Audio.PlayShutterCloseSE();
                phase = 5; //shutter close
                frame = 0;
            }
            if (phase == 5)
            {
                if (frame >= 0 && frame < 5)
                {
                    Audio.BGMFadeOut(30);
                }
                else if (frame >= 60)
                {
                    if (Core.Session.game_over)
                        Core.StartScene(new GameOverScene());
                    else
                        Core.StartScene(new StageResultScene());
                }
            }
        }

        public void UpdateGameOver()
        {
            if (!Core.Session.GameActive() && !intrusionController.ContinueSelecting())
            {
                Audio.PlayTimeoutSE();
                Core.Session.game_over = true;
                Audio.PlayShutterCloseSE();
                phase = 5;
                frame = 0;
            }
        }

        public void ShowMessage(string text)
        {
            msg_text = text;
            show_msg = true;
            frame = 0;
        }

        public void SetBossEnemy(BossEnemy boss)
        {
            this.boss_enemy = boss;
        }

        public override void Draw()
        {
            backgrounds.ForEach(a => a.Draw());
            controller.Draw();
            uiController.Draw();
            if (boss_enemy != null && !boss_enemy.dying)
            {
                DrawBossHP();
            }
            intrusionController.Draw();
            if (phase < 2)
                DrawShutter();
            if (phase == 5)
                DrawShutterClose();
            if (show_msg)
                DrawMessage();
            if (boss_alert)
                DrawBossAlert();
        }

        public void DrawShutter()
        {
            Vector2 pos;
            Graphics.spriteBatch.Begin();
            float offset_rate = (phase < 1) ? 1 : 1 - (frame / 30f).Clamp(0, 1);
            pos = new Vector2(Graphics.Width * 0.5f * offset_rate, Graphics.Height * 0.5f);
            Graphics.spriteBatch.Draw(left_shutter, pos, left_shutter.GetRect(), Color.White, 0, left_shutter.GetCenter(), 1, SpriteEffects.None, 0);
            pos = new Vector2(Graphics.Width - Graphics.Width * 0.5f * offset_rate, Graphics.Height * 0.5f);
            Graphics.spriteBatch.Draw(right_shutter, pos, right_shutter.GetRect(), Color.White, 0, right_shutter.GetCenter(), 1, SpriteEffects.None, 0);
            offset_rate = (phase < 1) ? 1 : 1 - (frame / 30f).Clamp(0, 1);
            pos = new Vector2(Graphics.Width * 0.5f, Graphics.Height * 0.5f * offset_rate);
            Graphics.spriteBatch.Draw(top_shutter, pos, top_shutter.GetRect(), Color.White, 0, top_shutter.GetCenter(), 1, SpriteEffects.None, 0);
            pos = new Vector2(Graphics.Width * 0.5f, Graphics.Height - Graphics.Height * 0.5f * offset_rate);
            Graphics.spriteBatch.Draw(btm_shutter, pos, btm_shutter.GetRect(), Color.White, 0, btm_shutter.GetCenter(), 1, SpriteEffects.None, 0);
            Graphics.spriteBatch.End();
        }

        public void DrawShutterClose()
        {
            Vector2 pos;
            Graphics.spriteBatch.Begin();
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
            Graphics.spriteBatch.End();
        }

        public void DrawBossHP()
        {
            float width = Graphics.Width - 42 * 2;
            float height = 10;
            Texture2D p = Graphics.w_pixel;
            if (boss_enemy.GetHP() <= 0) { return; }
            Graphics.spriteBatch.Begin();
            Vector2 pos = new Vector2(42, 67) + new Vector2(width / 2f, height / 2f);
            Graphics.spriteBatch.Draw(p, pos, p.GetRect(), Color.Black * 0.5f, 0, p.GetCenter(), new Vector2(width, height), SpriteEffects.None, 0);
            width -= 2;
            height -= 2;
            pos = new Vector2(43, 68) + new Vector2(width / 2f, height / 2f);
            Graphics.spriteBatch.Draw(p, pos, p.GetRect(), Color.DarkRed, 0, p.GetCenter(), new Vector2(width, height), SpriteEffects.None, 0);
            float ratio = boss_enemy.GetHP() / boss_enemy.GetMaxHP();
            width *= ratio;
            pos = new Vector2(43, 68) + new Vector2(width / 2f, height / 2f);
            float alpha = 0.5f + 0.5f * Math.Abs((float)Math.Sin(((float)Graphics.totalMs * 0.2f).ToRad()));
            Graphics.spriteBatch.Draw(p, pos, p.GetRect(), Color.LimeGreen * alpha, 0, p.GetCenter(), new Vector2(width, height), SpriteEffects.None, 0);
            //Draw Timer
            SpriteFont font = Graphics.spr_font_main;
            if (boss_enemy.timer > 0 && !boss_enemy.dying)
            {
                string text = ((int)(Math.Ceiling(boss_enemy.timer / 60.0))).ToString();
                float size = 0.8f;
                Graphics.spriteBatch.DrawString(font, text, new Vector2(Graphics.Width - 43 - (font.MeasureString(text).X * size), 80), Color.White, 0, Vector2.Zero, size, SpriteEffects.None, 1);
                Graphics.spriteBatch.DrawString(font, "Time   ", new Vector2(Graphics.Width - 43 - 88, 80), Color.White, 0, Vector2.Zero, size, SpriteEffects.None, 1);
            }
            Graphics.spriteBatch.End();
        }

        public void BossAlert()
        {
            boss_alert = true;
            Audio.BGMFadeOut(30);
            frame = 0;
            Audio.PlayBossSirenSE();
        }

        int boss_siren_time = 300;
        public void DrawBossAlert()
        {
            float size = 0;
            Graphics.spriteBatch.Begin();
            if (frame <= 30)
            {
                size = frame / 30.0f;
            }
            else if (frame >= 30 && frame < boss_siren_time)
            {
                size = 1;
                displacement += 3;
            }
            else if (frame >= boss_siren_time && frame < boss_siren_time + 30)
            {
                size = 1 - (frame - boss_siren_time) / 30.0f;
            }
            else if (frame > boss_siren_time + 10)
            {
                boss_alert = false;
                displacement = 0;
            }
            if (boss_alert)
            {
                Vector2 scale = new Vector2(1, size);
                Graphics.spriteBatch.Draw(boss_alert_bg, new Vector2(240, 278), boss_alert_bg.GetRect(), Color.White, 0, boss_alert_bg.GetCenter(), scale, SpriteEffects.None, 0);
                float alpha = 0.2f + 0.8f * Math.Abs((float)Math.Sin(((float)Graphics.totalMs * 0.2f).ToRad()));
                Graphics.spriteBatch.Draw(boss_alert_caption, new Vector2(240, 278), boss_alert_caption.GetRect(), Color.White * alpha, 0, boss_alert_caption.GetCenter(), scale, SpriteEffects.None, 0);
                //Draw Scrolling Text
                if (frame >= 30 && frame < boss_siren_time)
                {
                    float x = displacement;
                    float y = 278 - boss_alert_bg.Height * 0.5f + 15 + boss_alert_text.GetCenter().Y;
                    Graphics.spriteBatch.Draw(boss_alert_text, new Vector2(240 + x - boss_alert_text.Width * 2 - 6, y), boss_alert_text.GetRect(), Color.White, 0, boss_alert_text.GetCenter(), 1, SpriteEffects.None, 0);
                    Graphics.spriteBatch.Draw(boss_alert_text, new Vector2(240 + x - boss_alert_text.Width - 6, y), boss_alert_text.GetRect(), Color.White, 0, boss_alert_text.GetCenter(), 1, SpriteEffects.None, 0);
                    Graphics.spriteBatch.Draw(boss_alert_text, new Vector2(240 + x, y), boss_alert_text.GetRect(), Color.White, 0, boss_alert_text.GetCenter(), 1, SpriteEffects.None, 0);
                    Graphics.spriteBatch.Draw(boss_alert_text, new Vector2(240 + x + boss_alert_text.Width + 6, y), boss_alert_text.GetRect(), Color.White, 0, boss_alert_text.GetCenter(), 1, SpriteEffects.None, 0);
                    Graphics.spriteBatch.Draw(boss_alert_text, new Vector2(240 + x + boss_alert_text.Width * 2 + 6, y), boss_alert_text.GetRect(), Color.White, 0, boss_alert_text.GetCenter(), 1, SpriteEffects.None, 0);
                    y = 278 + boss_alert_bg.Height * 0.5f - 15 - boss_alert_text.GetCenter().Y;
                    Graphics.spriteBatch.Draw(boss_alert_text, new Vector2(240 - x - boss_alert_text.Width * 2 - 6, y), boss_alert_text.GetRect(), Color.White, 0, boss_alert_text.GetCenter(), 1, SpriteEffects.None, 0);
                    Graphics.spriteBatch.Draw(boss_alert_text, new Vector2(240 - x - boss_alert_text.Width - 6, y), boss_alert_text.GetRect(), Color.White, 0, boss_alert_text.GetCenter(), 1, SpriteEffects.None, 0);
                    Graphics.spriteBatch.Draw(boss_alert_text, new Vector2(240 - x, y), boss_alert_text.GetRect(), Color.White, 0, boss_alert_text.GetCenter(), 1, SpriteEffects.None, 0);
                    Graphics.spriteBatch.Draw(boss_alert_text, new Vector2(240 - x + boss_alert_text.Width + 6, y), boss_alert_text.GetRect(), Color.White, 0, boss_alert_text.GetCenter(), 1, SpriteEffects.None, 0);
                    Graphics.spriteBatch.Draw(boss_alert_text, new Vector2(240 - x + boss_alert_text.Width * 2 + 6, y), boss_alert_text.GetRect(), Color.White, 0, boss_alert_text.GetCenter(), 1, SpriteEffects.None, 0);
                }
            }
            Graphics.spriteBatch.End();
        }


        public void DrawMessage()
        {
            Graphics.spriteBatch.Begin();
            if (frame <= 30)
            {
                msg_opacity = (frame / 30.0f);
            }
            else if (frame <= 150)
            {
                msg_opacity = 1;
            }
            else if (frame <= 180)
            {
                msg_opacity = 1 - ((frame - 150) / 30.0f);
            }
            else if (frame == 240)
            {
                show_msg = false;
            }
            if (show_msg)
            {
                float height = 84;
                Texture2D p = Graphics.w_pixel;
                SpriteFont font = Graphics.spr_font_main;
                Vector2 pos = new Vector2(Graphics.Width / 2, Graphics.Height / 2 - 40);
                Graphics.spriteBatch.Draw(p, pos, p.GetRect(), Color.Black * 0.8f * msg_opacity, 0, p.GetCenter(), new Vector2(Graphics.Width, height), SpriteEffects.None, 0);
                string text = msg_text;
                Vector2 text_size = font.MeasureString(text);
                float size = 1.2f;
                Graphics.spriteBatch.DrawString(font, text, pos, Color.White * msg_opacity, 0, text_size * 0.5f, size, SpriteEffects.None, 1);
            }
            Graphics.spriteBatch.End();
        }

        public override void End()
        {
            Cache.UnloadSegment(1);
        }
    }
}
