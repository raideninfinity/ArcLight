using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class EnemyM2 : Enemy
    {
        int frame;
        public EnemyM2(Vector2 pos, float angle, int polarity = 2)
        {
            hp = GetMaxHP();
            x = pos.X;
            y = pos.Y;
            this.polarity = polarity;
            this.angle = angle;
            kill_score = Status.EnemyM2_KillScore;
        }

        public override float GetMaxHP()
        {
            return Status.EnemyM2_MaxHP;
        }

        protected override Hitbox GetHitbox()
        {
            return new CircleHitbox(36);
        }

        public override void OnDeath()
        {
            Audio.PlayEnemyExplodeMSE();
            base.OnDeath();
        }

        public override void Update()
        {
            frame += 1;
            if (frame == 240) { frame = 0; }
            base.Update();
        }

        protected override Texture2D GetTexture()
        {
            return Cache.Texture("enemy/enemy_m2_btm", 1);
        }

        protected Texture2D GetTexture2()
        {
            return Cache.Texture("enemy/enemy_m2_top", 1);
        }

        public override void Draw()
        {
            if (!alive) return;
            Texture2D texture = GetTexture();
            Texture2D texture2 = GetTexture2();
            Texture2D barrier = GetBarrier();
            Texture2D d_ring = GetDeathRing();
            float b_scale = (hitbox.radius * 2) / 76;
            Graphics.spriteBatch.Begin();
            float a = (frame * 1.5f).ToRad();
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
                Graphics.spriteBatch.Draw(texture, pos, texture.GetRect(), Color.White * alpha, a, texture.GetCenter() + new Vector2(0, 2.5f), size, SpriteEffects.None, 0);
                Graphics.spriteBatch.Draw(texture2, pos, texture2.GetRect(), Color.White * alpha, 0, texture2.GetCenter() + new Vector2(0, 4), size, SpriteEffects.None, 0);
                if (Active())
                    Graphics.spriteBatch.Draw(barrier, pos, barrier.GetRect(), Color.White * 0.25f, angle, barrier.GetCenter(), b_scale, SpriteEffects.None, 0);
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
