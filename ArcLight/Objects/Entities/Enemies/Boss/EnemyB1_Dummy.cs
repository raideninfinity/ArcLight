using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class EnemyB1_Dummy : Enemy
    {
        int frames;
        int phase = 0;
        Vector2 velocity;


        public EnemyB1_Dummy(Vector2 pos, float angle)
        {
            hp = 1;
            x = 0;
            y = 486;
            phase = 1;
            angle = 60;
            velocity = (new Vector2(0, -8).TurnAngle(angle));
            Audio.PlayBoss1EntranceSE();
        }

        public override bool Active()
        {
            return false;
        }

        protected override Texture2D GetTexture()
        {
            return Cache.Texture("enemy/enemy_b1_full", 1);
        }

        float scale = 0.5f;
        float alpha = 0.5f;

        public override void Update()
        {
            frames += 1;
            if (phase == 1)
            {
                x += velocity.X;
                y += velocity.Y;
                if (frames == 90)
                {
                    phase = 2; frames = 0;
                    x = Graphics.Width / 2;
                    y = 0;
                    Core.Session.movable = true;
                    Audio.PlayBossBGM();
                }
            }
            else if (phase == 2)
            {
                if (y < 240) { y += 8; }
                if (scale < 1.0f) { scale += (0.5f / 30); }
                if (alpha < 1.0f) { alpha += (0.5f / 30); }
                if (frames == 30) { dying = true; death_frames = 2; }
            }
            base.Update();
        }

        public override void Draw()
        {
            if (!alive) return;
            Texture2D texture = GetTexture();
            Graphics.spriteBatch.Begin();
            if (phase == 1)
            {
                float alpha = 0.5f;
                float size = 0.6f;
                Graphics.spriteBatch.Draw(texture, pos, texture.GetRect(), Color.White * alpha, (180 + 60f).ToRad(), texture.GetCenter() - new Vector2(0, 8), size, SpriteEffects.None, 0);
            }
            else if (phase == 2)
            {
                Graphics.spriteBatch.Draw(texture, pos, texture.GetRect(), Color.White * alpha, 0, texture.GetCenter() - new Vector2(0, 8), scale, SpriteEffects.None, 0);
            }
            Graphics.spriteBatch.End();
        }
    }
}
