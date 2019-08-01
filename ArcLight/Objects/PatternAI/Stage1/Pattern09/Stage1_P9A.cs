using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class Stage1_P9A : PatternAI
    {
        public Stage1_P9A(int param)
        {
            this.param = param;
            f_frame = 15;
            f_interval = 30;
        }

        public override void Initialize(Enemy enemy)
        {
            this.enemy = enemy;
            enemy.prepare_frames = 60;
            enemy.hp *= 2;
        }


        public override void UpdateMovement()
        {

        }

        int phase;
        int count;
        public override void UpdateFiring()
        {
            if (f_frame >= f_interval)
            {
                AddEmitter(new EmitterS5(enemy, enemy.pos, (count == 0) ? 0 : 30, phase));
                count += 1;
                if (count > 1)
                {
                    phase += 1;
                    if (phase > 1) phase = 0;
                    count = 0;
                }
                f_frame = 0;
            }
        }

        public override void OnDeath()
        {
            AddEmitter(new EmitterC4(enemy, enemy.pos, enemy.angle));
        }

    }
}