using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class EmitterS1 : Emitter
    {

        public EmitterS1(Enemy owner, Vector2 pos, float angle, int polarity = 2)
        {
            this.owner = owner;
            this.pos = pos;
            this.angle = angle;
            this.polarity = polarity;
        }

        public override void Update()
        {
            if (!Core.Controller.AnyPlayerAlive())
            {
                Erase();
                return;
            }
            if (alive)
            {
                angle = angle + ((float)(Utility.Rand.NextDouble() - 0.5f) * 30);
                AddBullet(new EBulletVar(owner, pos, angle, 3, 100.0f, 4.0f, 12.0f, polarity, 100));
                Erase();
            }
        }
    }
}