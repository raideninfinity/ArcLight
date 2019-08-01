using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class EmitterS3 : Emitter
    {
        int interval = 3;
        public EmitterS3(Enemy owner, Vector2 pos, float angle, int polarity, int count)
        {
            this.owner = owner;
            this.x = pos.X;
            this.y = pos.Y;
            this.angle = angle;
            this.polarity = polarity;
            frame = interval;
            this.count = count;
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
                    float a = angle + ((float)(Utility.Rand.NextDouble() - 0.5f) * 15);
                    AddBullet(new EBulletVar(owner, pos, a, 1, 50.0f, 6.0f, 10.0f, polarity, 90));
                    count -= 1;
                    frame = 0;
                }
                if (count == 0) Erase();
            }
            base.Update();
        }
    }
}
