using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class Stage1_P8 : PatternAI
    {
        int phase = 0;
        float speed = 6.0f;
        float i_x = 0;

        public Stage1_P8(int param)
        {
            this.param = param;
        }

        public override void Initialize(Enemy enemy)
        {
            this.enemy = enemy;
            i_x = enemy.x;
            enemy.y += ((param == 1) ? -10 : 10) * speed;
            phase = 1;
        }

        public override void UpdateMovement()
        {
            if (phase == 1)
            {
                if (!enemy.dying)
                {
                    enemy.y += ((param == 1) ? 1 : -1) * speed;
                }
                if (param == 1)
                {
                    if (enemy.y >= Graphics.Height - 54) { phase = 2; }
                }
                else
                {
                    if (enemy.y <= 54) { phase = 2; }
                }
            }
            else if (phase == 2)
            {
                if (!enemy.dying)
                {
                    enemy.x += ((param == 1) ? 1 : -1) * (speed * 0.8f);
                }
                if (param == 1)
                {
                    if (enemy.x >= 212) { phase = 3; }
                }
                else
                {
                    if (enemy.x <= Graphics.Width - 212) { phase = 3; }
                }
            }
            else if (phase == 3)
            {
                if (!enemy.dying)
                {
                    enemy.y += ((param == 1) ? -1 : 1) * speed;
                }
                CheckOutOfBounds(72);
            }
        }

        public override void OnDeath()
        {
            AddEmitter(new EmitterS1(enemy, enemy.pos, FindClosestPlayerAngle(), enemy.polarity));
            AddEmitter(new EmitterS1(enemy, enemy.pos, FindClosestPlayerAngle(), enemy.polarity));
            AddEmitter(new EmitterS1(enemy, enemy.pos, FindClosestPlayerAngle(), enemy.polarity));
        }

    }
}
