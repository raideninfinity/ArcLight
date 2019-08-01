using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class Stage1_P1B : PatternAI
    {
        Vector2 center;
        float angle;

        public Stage1_P1B(int param)
        {
            this.param = param;
        }

        public override void Initialize(Enemy enemy)
        {
            this.enemy = enemy;
            center = (param == 1) ? new Vector2(Graphics.Width, Graphics.Height + 48) : new Vector2(0, Graphics.Height + 48);
            angle = (param == 1) ? 90 : 270;
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
            angle += (param == 1) ? +0.5f : -0.5f;
            CheckOutOfBounds(72);
        }

        public override void UpdateFiring()
        {

        }

        public override void OnDeath()
        {
            AddEmitter(new EmitterC1(enemy, enemy.pos, FindClosestPlayerAngle(), enemy.polarity, 4.Random(8)));
        }

    }
}
