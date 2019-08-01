using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class EnemyS3 : Enemy
    {
        int frame;
        public EnemyS3(Vector2 pos, float angle, int polarity = 2)
        {
            hp = GetMaxHP();
            x = pos.X;
            y = pos.Y;
            this.angle = 180;
            this.polarity = polarity;
            kill_score = Status.EnemyS3_KillScore;
        }

        public override float GetMaxHP()
        {
            return Status.EnemyS3_MaxHP;
        }

        protected override Hitbox GetHitbox()
        {
            return new CircleHitbox(12);
        }

        protected override Texture2D GetTexture()
        {
            return Cache.Texture("enemy/enemy_s3", 1);
        }

        public override void Update()
        {
            frame += 1;
            if (frame == 60) { frame = 0; }
            base.Update();
        }

        public override void OnDeath()
        {
            Audio.PlayEnemyExplodeSSE();
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
                Graphics.spriteBatch.Draw(texture, pos, texture.GetRect(), Color.White * alpha, (angle + 180).ToRad(), texture.GetCenter() - new Vector2(0, 6), size, SpriteEffects.None, 0);
                if (Active())
                    Graphics.spriteBatch.Draw(barrier, pos, barrier.GetRect(), Color.White * 0.5f, angle, barrier.GetCenter(), b_scale, SpriteEffects.None, 0);
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
