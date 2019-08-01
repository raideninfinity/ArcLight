using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class Stage1_P9D : PatternAI
    {
        Vector2 center1 = new Vector2(Graphics.Width / 2, 80);
        Vector2 center2 = new Vector2(Graphics.Width / 2, Graphics.Height - 80);
        int phase;
        float angle;
        Vector2 distance = new Vector2(0, -(Graphics.Width / 2));
        Vector2 pos;

        public Stage1_P9D(int param)
        {
            this.param = param;
        }

        public override void Initialize(Enemy enemy)
        {
            this.enemy = enemy;
            angle = (param == 1) ? 270 : 90;
        }

        public override void UpdateMovement()
        {
            if (param == 1)
            {
                if (phase == 0)
                {
                    pos = center1 + distance.TurnAngle(angle);
                    angle -= 1.0f;
                    if (angle <= 180)
                    {
                        phase = 1;
                        angle = 0;
                    }
                }
                else
                {
                    pos = center2 + distance.TurnAngle(angle);
                    angle += 1.0f;
                }
            }
            else
            {
                if (phase == 0)
                {
                    pos = center1 + distance.TurnAngle(angle);
                    angle += 1.0f;
                    if (angle >= 180.0f)
                    {
                        phase = 1;
                        angle = 0;
                    }
                }
                else
                {
                    pos = center2 + distance.TurnAngle(angle);
                    angle -= 1.0f;
                }
            }
            enemy.pos = pos;
            CheckOutOfBounds(72);
        }

        public override void OnDeath()
        {
            AddEmitter(new EmitterS1(enemy, enemy.pos, FindClosestPlayerAngle(), enemy.polarity));
            AddEmitter(new EmitterS1(enemy, enemy.pos, FindFurthestPlayerAngle(), enemy.polarity));
        }

    }
}