using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class Stage1_P6 : PatternAI
    {
        int phase = 0;
        float angle = 0;
        int type = 0;
        Vector2 center;

        public Stage1_P6(float angle, int param, int type)
        {
            this.angle = angle;
            this.param = param;
            this.type = type;
        }

        public override void Initialize(Enemy enemy)
        {
            this.enemy = enemy;
            enemy.prepare_frames = 60;
            center = enemy.pos;
            phase = 1;
        }

        public override void UpdateMovement()
        {
            if (phase == 1)
            {
                if (frame == 600) phase = 2;
            }
            else if (phase == 2)
            {
                enemy.Leave();
                phase = 3;
            }
            if (enemy.alive)
            {
                float distance = (param == 1) ? 120 : 240;
                Vector2 pos = center + new Vector2(0, distance).TurnAngle(angle);
                if (!enemy.dying)
                {
                    enemy.x = pos.X;
                    enemy.y = pos.Y;
                }
            }
            angle += 1.0f * param;
            if (angle >= 360) { angle -= 360; }
        }

        public override void OnDeath()
        {
            if (type == 0)
                AddEmitter(new EmitterC1(enemy, enemy.pos, FindClosestPlayerAngle(), enemy.polarity, 4.Random(8)));
            else
            {
                AddEmitter(new EmitterS1(enemy, enemy.pos, FindClosestPlayerAngle(), enemy.polarity));
                AddEmitter(new EmitterS1(enemy, enemy.pos, FindFurthestPlayerAngle(), enemy.polarity));
            }
        }

    }
}
