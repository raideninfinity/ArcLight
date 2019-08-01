using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class Stage1_Boss_C : PatternAI
    {
        public Stage1_Boss_C(int param)
        {
            this.param = param;
            f_interval = 120;
            f_frame = 0;
        }

        public override void Initialize(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public float GetHPRatio()
        {
            return ((EnemyB1)((EnemyB1C)enemy).host).GetHPRatio();
        }

        int phase = 1;
        public override void UpdateFiring()
        {
            if (f_frame >= ((GetHPRatio() > 0.75f) ? f_interval: f_interval / 2))
            {
                int count = Core.Controller.GetFighterCount();
                if (count > 0)
                    AddEmitter(new EmitterS2(enemy, enemy.pos, FindClosestPlayerAngle(), phase, 6.Random(12)));
                if (count > 1)
                    AddEmitter(new EmitterS2(enemy, enemy.pos, FindFurthestPlayerAngle(), phase, 6.Random(12)));
                f_frame = 0;
                phase += 1;
                if (phase > 1) phase = 0;
            }
        }
    }
}