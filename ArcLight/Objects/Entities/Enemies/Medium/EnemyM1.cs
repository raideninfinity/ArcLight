using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class EnemyM1 : Enemy
    {
        public EnemyM1(Vector2 pos, float angle, int polarity = 2)
        {
            hp = GetMaxHP();
            x = pos.X;
            y = pos.Y;
            this.polarity = polarity;
            this.angle = 180;
            kill_score = Status.EnemyM1_KillScore;
        }

        public override float GetMaxHP()
        {
            return Status.EnemyM1_MaxHP;
        }

        protected override Hitbox GetHitbox()
        {
            return new CircleHitbox(44 * 0.8f);
        }

        protected override Texture2D GetTexture()
        {
            return Cache.Texture("enemy/enemy_m1", 1);
        }

        public override void OnDeath()
        {
            Audio.PlayEnemyExplodeMSE();
            base.OnDeath();
        }

        public override void Draw()
        {
            if (!alive) return;
            Texture2D texture = GetTexture();
            Texture2D barrier = GetBarrier();
            Texture2D d_ring = GetDeathRing();
            float b_scale = (hitbox.radius * 2) / 76;
            Graphics.spriteBatch.Begin();
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
                Graphics.spriteBatch.Draw(texture, pos, texture.GetRect(), Color.White * alpha, (angle + 180).ToRad(), texture.GetCenter() - new Vector2(0, 8), size, SpriteEffects.None, 0);
                if (Active())
                    Graphics.spriteBatch.Draw(barrier, pos, barrier.GetRect(), Color.White * 0.5f, 1, barrier.GetCenter(), b_scale, SpriteEffects.None, 0);
            }
            else
            {
                if (death_frames >= 30)
                {
                    int df = death_frames - 30;
                    float scale = (30 - df) * (2.0f / 30) * 1.5f;
                    float opacity = 0.2f + df * (0.8f / 30);
                    Graphics.spriteBatch.Draw(d_ring, pos, d_ring.GetRect(), Color.White * opacity, 0, d_ring.GetCenter(), scale, SpriteEffects.None, 0);
                }
            }
            Graphics.spriteBatch.End();
        }
    }
}
