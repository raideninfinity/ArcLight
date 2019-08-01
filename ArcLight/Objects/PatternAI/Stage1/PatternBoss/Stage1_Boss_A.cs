using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class Stage1_Boss_A : PatternAI
    {
        public Stage1_Boss_A(int param)
        {
            this.param = param;
            f_interval = 60;
            f_frame = 30;
        }

        public override void Initialize(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public override void UpdateFiring()
        {
            if (f_frame >= f_interval)
            {
                AddEmitter(new EmitterB1A(enemy, enemy.pos, 180));
                f_frame = 0;
            }
        }
    }
}