using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class EnemyB1A : Enemy
    {
        Enemy host;
        int index;

        bool activated = true;

        public EnemyB1A(Enemy host, int index)
        {
            hp = GetMaxHP();
            this.index = index;
            this.host = host;
            x = host.x;
            y = host.y;
            ((EnemyB1)host).sub_cores[0] = this;
            kill_score = Status.EnemyB1_SubCore_KillScore;
        }

        public override bool Active()
        {
            if (activated)
                return base.Active();
            else
                return false;
        }

        public override float GetMaxHP()
        {
            return Status.EnemyB1_SubCoreA_MaxHP;
        }

        protected override Hitbox GetHitbox()
        {
            return new CircleHitbox(32);
        }

        protected override Texture2D GetTexture()
        {
            return Cache.Texture("enemy/enemy_b1_sub_core", 1);
        }

        public override void OnDeath()
        {
            Audio.PlayBoss1CoreExplodeSE();
            Core.Controller.ClearEnemyBullets(this);
        }

        public override void Update()
        {
            x = host.x;
            y = host.y + 77;
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
            if (!dying)
            {
                if (Active())
                    Graphics.spriteBatch.Draw(barrier, pos, barrier.GetRect(), Color.White * 0.5f, 0, barrier.GetCenter(), b_scale, SpriteEffects.None, 0);
                Graphics.spriteBatch.Draw(texture, pos, texture.GetRect(), Color.White, 0, texture.GetCenter(), 1, SpriteEffects.None, 0);
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
