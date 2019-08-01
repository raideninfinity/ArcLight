using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class EmitterC1 : Emitter
    {
        int interval = 3;
        float random_angle;
        public EmitterC1(Enemy owner, Vector2 pos, float angle, int polarity, int count, float random_angle = 5)
        {
            this.owner = owner;
            this.x = pos.X;
            this.y = pos.Y;
            this.angle = angle;
            this.polarity = polarity;
            frame = interval;
            this.count = count;
            this.random_angle = random_angle;
        }

        int count;
        public override void Update()
        {
            if (!Core.Controller.AnyPlayerAlive())
            {
                Erase();
                return;
            }
            if (alive)
            {
                if (frame >= interval)
                {
                    float a = angle + ((float)(Utility.Rand.NextDouble() - 0.5f) * random_angle);
                    AddBullet(new EBulletVar(owner, pos, a, 1, 50.0f, 4.0f, 8.0f, polarity, 90));
                    count -= 1;
                    frame = 0;
                }
                if (count == 0) Erase();
            }
            base.Update();
        }
    }
}
