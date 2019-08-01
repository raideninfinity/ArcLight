using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class Stage1_P5B : PatternAI
    {

        int phase = 0;

        public Stage1_P5B(int param)
        {
            this.param = param;
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
                        if (enemy.x > 60)
                            enemy.x -= 1.0f;
                        else
                        {
                            enemy.x = 60;
                            phase = 1;
                        }
                    }
                    else
                    {
                        if (enemy.x < Graphics.Width - 60)
                            enemy.x += 1.0f;
                        else
                        {
                            enemy.x = Graphics.Width - 60;
                            phase = 1;
                        }
                    }
                }
                else
                {
                    if (param == 1)
                    {
                        if (enemy.x < 180)
                            enemy.x += 1.0f;
                        else
                        {
                            enemy.x = 180;
                            phase = 0;
                        }
                    }
                    else
                    {
                        if (enemy.x > Graphics.Width - 180)
                            enemy.x -= 1.0f;
                        else
                        {
                            enemy.x = Graphics.Width - 180;
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