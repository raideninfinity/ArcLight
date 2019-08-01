using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class EmitterS5 : Emitter
    {
        public EmitterS5(Enemy owner, Vector2 pos, float angle, int polarity)
        {
            this.owner = owner;
            this.x = pos.X;
            this.y = pos.Y;
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
                for (int i = 0; i < 6; i++)
                {
                    AddBullet(new EBulletVar(owner, pos, angle + 60 * i, 3, 250f, 4.0f, 14.0f, polarity));
                }
                Erase();
            }
        }
    }
}
