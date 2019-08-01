using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class Stage1_P4 : PatternAI
    {
        int phase = 0;
        float drop_speed = 2.0f;
        float speed = 2.0f;
        float ori_x;
        float target_x;

        public Stage1_P4(int param)
        {
            this.param = param;
        }

        public override void Initialize(Enemy enemy)
        {
            this.enemy = enemy;
            ori_x = enemy.x;
            if (param == 1)
                target_x = ori_x + Graphics.Width / 6.0f;
            else
                target_x = ori_x - Graphics.Width / 6.0f;
            phase = 1;
        }

        public override void UpdateMovement()
        {
            if (!enemy.dying)
            {
                enemy.y += drop_speed;
            }
            if (param == 1)
            {
                if (phase == 1)
                {
                    if (frame >= 20) { phase = 2; }
                }
                else if (phase == 2)
                {
                    if (enemy.x < target_x)
                    {
                        if (!enemy.dying)
                        {
                            enemy.x += speed;
                        }
                    }
                    else { phase = 3; frame = 0; }
                }
                else if (phase == 3)
                {
                    if (frame >= 20) { phase = 4; }
                }
                else if (phase == 4)
                {
                    if (enemy.x > ori_x)
                    {
                        if (!enemy.dying)
                        {
                            enemy.x -= speed;
                        }
                    }
                    else { phase = 1; frame = 0; }
                }
            }
            else
            {
                if (phase == 1)
                {
                    if (frame >= 20) { phase = 2; }
                }
                else if (phase == 2)
                {
                    if (enemy.x > target_x)
                    {
                        if (!enemy.dying)
                        {
                            enemy.x -= speed;
                        }
                    }
                    else { phase = 3; frame = 0; }
                }
                else if (phase == 3)
                {
                    if (frame >= 20) { phase = 4; }
                }
                else if (phase == 4)
                {
                    if (enemy.x < ori_x)
                    {
                        if (!enemy.dying)
                        {
                            enemy.x += speed;
                        }
                    }
                    else { phase = 1; frame = 0; }
                }
            }
            CheckOutOfBounds();
        }

        public override void OnDeath()
        {
            AddEmitter(new EmitterS1(enemy, enemy.pos, FindClosestPlayerAngle(), enemy.polarity));
        }

        void CheckOutOfBounds()
        {
            int offset = 72;
            if (enemy.y > 640 + offset)
                enemy.Erase();
        }
    }
}
