using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class Stage1_P5A : PatternAI
    {
        int phase = 0;

        public Stage1_P5A(int param)
        {
            this.param = param;
            f_interval = 4;
        }

        public override void Initialize(Enemy enemy)
        {
            this.enemy = enemy;
            enemy.prepare_frames = 60;
        }

        public override void UpdateMovement()
        {
            if (enemy.Active())
            {
                if (phase == 0)
                {
                    if (param == 1)
                    {
                        if (enemy.angle < 195)
                            enemy.angle += 0.5f;
                        else
                        {
                            enemy.angle = 195;
                            phase = 1;
                        }
                    }
                    else
                    {
                        if (enemy.angle > 165)
                            enemy.angle -= 0.5f;
                        else
                        {
                            enemy.angle = 165;
                            phase = 1;
                        }
                    }
                }
                else
                {
                    if (param == 1)
                    {
                        if (enemy.angle > 120)
                            enemy.angle -= 0.5f;
                        else
                        {
                            enemy.angle = 120;
                            phase = 0;
                        }
                    }
                    else
                    {
                        if (enemy.angle < 240)
                            enemy.angle += 0.5f;
                        else
                        {
                            enemy.angle = 240;
                            phase = 0;
                        }
                    }
                }
            }
        }

        public override void UpdateFiring()
        {
            if (f_frame >= f_interval)
            {
                float a = enemy.angle + ((float)(Utility.Rand.NextDouble() - 0.5f) * 5f);
                AddEmitter(new EmitterG(enemy, new EBulletVar(enemy, enemy.pos, a, 1, 25.0f, 6.0f, 10.0f, enemy.polarity, 90)));
                f_interval = 0;
            }
        }

        public override void OnDeath()
        {
            float a = enemy.angle;
            Vector2 center = enemy.pos;
            Vector2 distance = new Vector2(0, 48);
            float offset = 60;
            Vector2 pos = center + distance.TurnAngle(a);
            int polarity = (enemy.polarity == 0) ? 1 : 0;
            Core.Controller.AddEnemy(new EnemyS3(pos, 0, polarity), new Stage1_P5C(1));
            pos = center + distance.TurnAngle(a - offset);
            Core.Controller.AddEnemy(new EnemyS3(pos, 0, polarity), new Stage1_P5C(1));
            pos = center + distance.TurnAngle(a + offset);
            Core.Controller.AddEnemy(new EnemyS3(pos, 0, polarity), new Stage1_P5C(1));
        }

    }
}