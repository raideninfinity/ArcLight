using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class Stage1_P3A : PatternAI
    {

        float drop_speed = 0.2f;

        public Stage1_P3A(int param)
        {
            this.param = param;
            f_frame = 60;
            f_interval = 60;
        }

        public override void Initialize(Enemy enemy)
        {
            this.enemy = enemy;
            enemy.prepare_frames = 60;
        }

        public override void UpdateMovement()
        {
            if (!enemy.dying)
                enemy.y += drop_speed;
            CheckOutOfBounds(96);
        }

        public override void UpdateFiring()
        {
            if (f_frame >= f_interval)
            {
                AddEmitter(new EmitterS4(enemy, enemy.pos, FindClosestPlayerAngle()));
                f_frame = 0;
            }
        }

        public override void OnDeath()
        {
            AddEmitter(new EmitterC3(enemy, enemy.pos, 0, enemy.polarity));
        }

    }
}