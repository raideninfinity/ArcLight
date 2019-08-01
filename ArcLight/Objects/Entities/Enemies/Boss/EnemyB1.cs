using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class EnemyB1 : BossEnemy
    {

        public Enemy main_core;
        public Enemy[] sub_cores = { null, null, null, null, null };


        public EnemyB1(Vector2 pos, float angle)
        {
            hp = 1;
            x = pos.X;
            y = pos.Y;
            this.angle = 180;
            timer = 60 * (int)Status.Boss1Timer;
        }

        public override bool Active()
        {
            return false;
        }

        protected override Texture2D GetTexture()
        {
            return Cache.Texture("enemy/enemy_b1", 1);
        }

        public override float GetHPRatio()
        {
            return GetHP() / GetMaxHP();
        }


        public bool CheckSubCoreEmpty()
        {
            bool res = true;
            for (int i = 0; i < 5; i++)
            {
                res &= sub_cores[i].dying;
            }
            return res;
        }

        int frames = 0;
        public override void Update()
        {
            timer -= 1;
            if (CheckSubCoreEmpty() && !((EnemyB1M)main_core).activated)
            {
                frames += 1;
            }
            if (frames == 60)
            {
                ((EnemyB1M)main_core).activated = true;
                frames = 0;
            }
            if (!main_core.alive)
            {
                frames += 1;
                if (frames >= 30 && !dying)
                {
                    dying = true;
                    Audio.PlayBoss1ExplodeSE();
                    death_frames = 60;
                }
            }
            if (timer <= 0 && !main_core.dying)
            {
                for (int i = 0; i < 5; i++)
                {
                    sub_cores[i].hp = 0;
                    sub_cores[i].dying = true;
                    sub_cores[i].death_frames = 60;
                }
                main_core.hp = 0;
                main_core.dying = true;
                main_core.death_frames = 60;
            }
            base.Update();
        }

        public override float GetHP()
        {
            float hp = main_core.hp.Clamp(0, 9999999);
            for (int i = 0; i < 5; i++)
            {
                hp += sub_cores[i].hp.Clamp(0, 9999999);
            }
            return hp;
        }

        public override float GetMaxHP()
        {
            float hp = main_core.GetMaxHP();
            for (int i = 0; i < 5; i++)
            {
                hp += sub_cores[i].GetMaxHP();
            }
            return hp;
        }

        public override void Draw()
        {
            if (!alive) return;
            Texture2D texture = GetTexture();
            Graphics.spriteBatch.Begin();
            float size = 1.0f;
            size *= 1 - (prepare_frames / 60.0f);
            float alpha = (1 - (prepare_frames / 60.0f));
            if (!dying)
            {
                Graphics.spriteBatch.Draw(texture, pos, texture.GetRect(), Color.White * alpha, (angle + 180).ToRad(), texture.GetCenter() - new Vector2(0, 8), size, SpriteEffects.None, 0);
            }
            else
            {
                DrawDeathRing();
            }
            Graphics.spriteBatch.End();
        }

        public void DrawDeathRing()
        {
            Texture2D d_ring = Cache.Texture("player/death_ring", 1);
            float expl_time = 15f;
            int frame = 30 - (death_frames - 30);
            if (death_frames >= 30)
            {
                float scale = 6.0f * (frame / expl_time);
                float scale2 = 4.0f * (frame / expl_time);
                float alpha = 1f - 0.5f * (frame / expl_time);
                Graphics.spriteBatch.Draw(d_ring, pos, d_ring.GetRect(), Color.White * alpha, 0, d_ring.GetCenter(), scale, SpriteEffects.None, 0);
                Graphics.spriteBatch.Draw(d_ring, pos, d_ring.GetRect(), Color.White * alpha, 0, d_ring.GetCenter(), scale2, SpriteEffects.None, 0);
            }
        }
    }
}
