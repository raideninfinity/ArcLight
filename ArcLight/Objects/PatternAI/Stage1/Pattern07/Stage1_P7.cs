using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class Stage1_P7 : PatternAI
    {
        int f_interval2 = 3;
        int f_frame2;
        int start_interval = 30;
        int start_frame;

        public Stage1_P7(int param)
        {
            this.param = param;
        }

        public override void Initialize(Enemy enemy)
        {
            this.enemy = enemy;
            enemy.prepare_frames = 60;
        }

        public override void UpdateFiring()
        {
            if (start_frame >= start_interval)
            {
                //Barrier
                if (f_frame2 >= f_interval2)
                {
                    AddEmitter(new EmitterG(enemy, new EBulletVar(enemy, enemy.pos, enemy.angle - 120, 0, 2f, 8.0f, 12.0f, enemy.polarity)));
                    AddEmitter(new EmitterG(enemy, new EBulletVar(enemy, enemy.pos, enemy.angle + 120, 0, 2f, 8.0f, 12.0f, enemy.polarity)));
                    f_frame2 = 0;
                }
                f_frame2 += 1;
                //Shot
                bool can_fire = (param == 1) ? (f_frame % 180 == 0) : ((f_frame + 90) % 180 == 0);
                if (can_fire)
                {
                    AddEmitter(new EmitterS3(enemy, enemy.pos, FindClosestPlayerAngle(), enemy.polarity, 8));
                    AddEmitter(new EmitterS3(enemy, enemy.pos, enemy.angle + 180, enemy.polarity, 8));
                }
            }
            start_frame += 1;
        }

        public override void OnDeath()
        {
            AddEmitter(new EmitterC3(enemy, enemy.pos, 0, enemy.polarity));
        }

    }
}