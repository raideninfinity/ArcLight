using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class Stage1_P2A : PatternAI
    {
        Vector2 center1 = new Vector2(Graphics.Width / 4 + 24, 24);
        Vector2 distance1 = new Vector2(0, -Graphics.Width / 4);
        Vector2 center2 = new Vector2(Graphics.Width / 4 + 24 + 24, (Graphics.Width / 4) * 2 + 24);
        Vector2 distance2 = new Vector2(0, -36);
        Vector2 cur_pos;
        Vector2 center_pos;
        float displace;
        float angle;
        int phase;

        public Stage1_P2A(int param)
        {
            this.param = param;
            f_frame = 30;
            f_interval = 60;
        }

        public override void Initialize(Enemy enemy)
        {
            this.enemy = enemy;
            angle = 270;
        }

        public override void UpdateMovement()
        {
            if (!enemy.dying)
            {
                if (phase == 0)
                {
                    center_pos = center1 + distance1.TurnAngle(angle);
                    angle -= 2.0f;
                    if (angle <= 180)
                    {
                        angle = 0;
                        phase = 1;
                        cur_pos = center_pos;
                    }
                }
                else if (phase == 1)
                {
                    if (displace < 24.0f)
                    {
                        displace += 0.2f;
                        center_pos.X = cur_pos.X + displace;
                    }
                    else
                    {
                        phase = 2;
                    }
                }
                else if (phase == 2)
                {
                    center_pos = center2 + distance1.TurnAngle(angle);
                    angle += 3.0f;
                    if (angle >= 180)
                    {
                        phase = 3;
                        displace = 0;
                        cur_pos = center_pos;
                    }
                }
                else if (phase == 3)
                {
                    displace -= 6.0f;
                    center_pos.X = cur_pos.X + displace;
                }
            }
            enemy.pos = center_pos + distance2.TurnAngle(30 + (param - 1) * 120);
            CheckOutOfBounds(72);
        }

        public override void UpdateFiring()
        {
            if (f_frame >= f_interval)
            {
                AddEmitter(new EmitterC1(enemy, enemy.pos, FindClosestPlayerAngle(), enemy.polarity, 8, 30));
                f_frame = 0;
            }
        }

        public override void OnDeath()
        {
            for (int i = 0; i < 5; i++)
            {
                AddEmitter(new EmitterS1(enemy, enemy.pos, FindClosestPlayerAngle(), enemy.polarity));
            }
        }

    }
}
