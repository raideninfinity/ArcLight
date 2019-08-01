using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class Stage1_Main_Boss : PatternAI
    {
        Vector2 original_pos;
        public Stage1_Main_Boss(int param)
        {
            this.param = param;
        }

        public override void Initialize(Enemy enemy)
        {
            this.enemy = enemy;
            original_pos = enemy.pos;
        }

        int x_phase = 1;
        int y_phase = 1;

        public override void UpdateMovement()
        {
            //x
            if (x_phase == 1)
            {
                if (enemy.x > original_pos.X - 90) { enemy.x -= 0.5f * (float)Utility.Rand.NextDouble(); }
                else x_phase = 2;
            }
            else if (x_phase == 2)
            {
                if (enemy.x < original_pos.X + 90) { enemy.x += 0.5f * (float)Utility.Rand.NextDouble(); }
                else x_phase = 1;
            }
            //y
            if (y_phase == 1)
            {
                if (enemy.y > original_pos.Y - 60) { enemy.y -= 0.5f * (float)Utility.Rand.NextDouble(); }
                else y_phase = 2;
            }
            else if (y_phase == 2)
            {
                if (enemy.y < original_pos.Y + 30) { enemy.y += 0.5f * (float)Utility.Rand.NextDouble(); }
                else y_phase = 1;
            }
        }

    }
}
