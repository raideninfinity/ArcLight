using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class Stage1_P10 : PatternAI
    {
        int phase = 0;
        Vector2 velocity;
        Vector2 velocity_back;

        public Stage1_P10(int param)
        {
            this.param = param;
        }

        public override void Initialize(Enemy enemy)
        {
            this.enemy = enemy;
            enemy.prepare_frames = 60;
            phase = 1;
            TurnTowardsPlayer();
            velocity_back = (new Vector2(0, 0.5f).TurnAngle(enemy.angle));
        }

        public override void UpdateMovement()
        {
            if (phase == 1)
            {
                if (frame < 60)
                {
                    if (!enemy.dying)
                    {
                        enemy.x += velocity_back.X;
                        enemy.y += velocity_back.Y;
                    }
                }
                else
                {
                    velocity = (new Vector2(0, 16).TurnAngle(enemy.angle));
                    phase = 2;
                }
            }
            else if (phase == 2)
            {
                if (!enemy.dying)
                {
                    enemy.x -= velocity.X;
                    enemy.y -= velocity.Y;
                }
            }
            CheckOutOfBounds(72);
        }

        public override void UpdateFiring()
        {

        }

        public override void OnDeath()
        {
            AddEmitter(new EmitterC2(enemy, enemy.pos, enemy.angle, enemy.polarity));
        }

        public void TurnTowardsPlayer()
        {
            Fighter fighter = Core.Controller.GetNearestFighter(enemy.pos);
            if (fighter == null)
                enemy.angle = 180;
            else
                enemy.angle = 180 - enemy.pos.AngleBetweenPoints(fighter.pos);
        }

    }
}
