using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class Stage1_Boss_B : PatternAI
    {
        public Stage1_Boss_B(int param)
        {
            this.param = param;
            f_interval = 30;
        }

        public override void Initialize(Enemy enemy)
        {
            this.enemy = enemy;
        }

        int phase = 0;
        int f_count = 0;
        public override void UpdateFiring()
        {
            if (f_frame >= f_interval)
            {
                int count = Core.Controller.GetFighterCount();
                if (count > 0)
                    AddEmitter(new EmitterS1(enemy, enemy.pos, FindClosestPlayerAngle(), phase));
                if (count > 1)
                    AddEmitter(new EmitterS1(enemy, enemy.pos, FindFurthestPlayerAngle(), phase));
                f_frame = 0;
                f_count += 1;
                if (f_count >= ((enemy.GetHPRatio() <= 0.5f) ? 1 : 3))
                {
                    phase += 1;
                    if (phase > 1) phase = 0;
                    f_count = 0;
                }
            }
        }
    }
}