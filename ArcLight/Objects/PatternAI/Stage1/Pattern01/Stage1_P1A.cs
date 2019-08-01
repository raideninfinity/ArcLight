using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class Stage1_P1A : PatternAI
    {
        Vector2 center;
        float angle;

        public Stage1_P1A(int param)
        {
            this.param = param;
        }

        public override void Initialize(Enemy enemy)
        {
            this.enemy = enemy;
            center = (param == 1) ? new Vector2(Graphics.Width, -48) : new Vector2(0, -48);
            angle = (param == 1) ? 90 : 270;
            f_interval = 60.Random(180);
            f_frame = 30.Random(120);
        }

        public override void UpdateMovement()
        {
            if (enemy.alive)
            {
                float distance = Graphics.Width;
                Vector2 pos = center + new Vector2(0, distance).TurnAngle(angle);
                if (!enemy.dying)
                {
                    enemy.x = pos.X;
                    enemy.y = pos.Y;
                }
            }
            angle += (param == 1) ? -0.5f : +0.5f;
            CheckOutOfBounds(72);
        }

        public override void UpdateFiring()
        {
            if (f_frame >= f_interval)
            {
                AddEmitter(new EmitterS1(enemy, enemy.pos, FindClosestPlayerAngle(), enemy.polarity));
                f_frame = 0;
            }
        }

        public override void OnDeath()
        {
            AddEmitter(new EmitterS1(enemy, enemy.pos, FindClosestPlayerAngle(), enemy.polarity));
        }

    }
}
