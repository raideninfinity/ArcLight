using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class Stage1_Boss_M : PatternAI
    {
        public Stage1_Boss_M(int param)
        {
            this.param = param;
        }

        public override void Initialize(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public float GetRotateAngle()
        {
            float ratio = enemy.GetHPRatio();
            if (ratio >= 0.5f)
                return 1.0f;
            else
                return 2.0f;
        }

        float sp_angle;
        int f_frame2 = 0;
        int f_interval2 = 120;
        int f_frame3 = 0;
        int f_interval3 = 40;
        int phase = 1;

        public override void UpdateFiring()
        {
            if (!Core.Controller.AnyPlayerAlive() || enemy.dying || !((EnemyB1M)enemy).activated) return;
            //Spiral Shot
            if (f_frame >= 12)
            {
                float a = sp_angle;
                AddEmitter(new EmitterG(enemy, new EBulletVar(enemy, enemy.pos, a, 0, 150f, 4.0f, 12.0f, 0, 100)));
                a = sp_angle + 90;
                AddEmitter(new EmitterG(enemy, new EBulletVar(enemy, enemy.pos, a, 0, 150f, 4.0f, 12.0f, 1, 100)));
                a = sp_angle + 180;
                AddEmitter(new EmitterG(enemy, new EBulletVar(enemy, enemy.pos, a, 0, 150f, 4.0f, 12.0f, 0, 100)));
                a = sp_angle + 270;
                AddEmitter(new EmitterG(enemy, new EBulletVar(enemy, enemy.pos, a, 0, 150f, 4.0f, 12.0f, 1, 100)));
                f_frame = 0;
                sp_angle += GetRotateAngle();
            }
            //Arc Shot
            if (f_frame2 >= (f_interval2))
            {
                AddEmitter(new EmitterB1B(enemy, enemy.pos, FindClosestPlayerAngle(), phase));
                f_frame2 = 0;
                phase += 1;
                if (phase > 1) phase = 0;
            }
            //Aiming Shot
            if (f_frame3 >= f_interval3)
            {
                Vector2 left_pos = new Vector2(enemy.pos.X - 65, enemy.pos.X - 56);
                Vector2 right_pos = new Vector2(enemy.pos.X + 65, enemy.pos.X - 56);
                int count = Core.Controller.GetFighterCount();
                if (count > 0)
                {
                    AddEmitter(new EmitterS1(enemy, left_pos, FindClosestPlayerAngle(), 1));
                    AddEmitter(new EmitterS1(enemy, right_pos, FindClosestPlayerAngle(), 0));
                    AddEmitter(new EmitterS1(enemy, left_pos, FindClosestPlayerAngle(), 1));
                    AddEmitter(new EmitterS1(enemy, right_pos, FindClosestPlayerAngle(), 0));
                }
                if (count > 1)
                {
                    AddEmitter(new EmitterS1(enemy, left_pos, FindFurthestPlayerAngle(), 1));
                    AddEmitter(new EmitterS1(enemy, right_pos, FindFurthestPlayerAngle(), 0));
                    AddEmitter(new EmitterS1(enemy, left_pos, FindFurthestPlayerAngle(), 1));
                    AddEmitter(new EmitterS1(enemy, right_pos, FindFurthestPlayerAngle(), 0));
                }
                f_frame3 = 0;
            }
            f_frame2 += 1;
            f_frame3 += 1;
        }
    }
}
