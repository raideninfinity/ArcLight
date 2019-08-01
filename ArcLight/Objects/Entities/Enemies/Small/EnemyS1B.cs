using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class EnemyS1B : Enemy
    {
        int frame;
        public EnemyS1B(Vector2 pos, float angle, int polarity = 2)
        {
            hp = GetMaxHP();
            x = pos.X;
            y = pos.Y;
            this.angle = angle;
            this.polarity = polarity;
            kill_score = Status.EnemyS1B_KillScore;
        }

        public override float GetMaxHP()
        {
            return Status.EnemyS1B_MaxHP;
        }

        protected override Hitbox GetHitbox()
        {
            return new CircleHitbox(24);
        }

        public override void OnDeath()
        {
            Audio.PlayEnemyExplodeSSE();
            base.OnDeath();
        }

        protected override Texture2D GetTexture()
        {
            return Cache.Texture("enemy/enemy_s1_b", 1);
        }


        public override void Update()
        {
            frame += 1;
            if (frame == 60) { frame = 0; }
            base.Update();
        }

        public override void Draw()
        {
            if (!alive) return;
            Texture2D texture = GetTexture();
            Texture2D barrier = GetBarrier();
            Texture2D d_ring = GetDeathRing();
            float b_scale = (hitbox.radius * 2) / 76;
            Graphics.spriteBatch.Begin();
            float a = (frame * 3.0f).ToRad();
            float size = 1.0f;
            size *= 1 - (prepare_frames / 60.0f);
            float alpha = (1 - (prepare_frames / 60.0f));
            if (leaving)
            {
                size *= leave_frames / 60.0f;
                alpha *= leave_frames / 60.0f;
            }
            if (!dying)
            {
                if (Active())
                    Graphics.spriteBatch.Draw(barrier, pos, barrier.GetRect(), Color.White * 0.5f, angle, barrier.GetCenter(), b_scale, SpriteEffects.None, 0);
                Graphics.spriteBatch.Draw(texture, pos, texture.GetRect(), Color.White * alpha, a, texture.GetCenter(), size, SpriteEffects.None, 0);
            }
            else
            {
                if (death_frames >= 30)
                {
                    int df = death_frames - 30;
                    float scale = (30 - df) * (2.0f / 30);
                    float opacity = 0.2f + df * (0.8f / 30);
                    Graphics.spriteBatch.Draw(d_ring, pos, d_ring.GetRect(), Color.White * opacity, 0, d_ring.GetCenter(), scale, SpriteEffects.None, 0);
                }
            }
            Graphics.spriteBatch.End();
        }
    }
}
