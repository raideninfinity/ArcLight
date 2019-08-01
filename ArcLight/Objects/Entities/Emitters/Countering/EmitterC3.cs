using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class EmitterC3 : Emitter
    {

        int interval = 5;

        public EmitterC3(Enemy owner, Vector2 pos, float angle, int polarity = 2)
        {
            this.owner = owner;
            this.pos = pos;
            this.angle = angle;
            this.polarity = polarity;
            frame = interval;
        }

        int count = 3;
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
                    float offset = count % 2;
                    for (int i = 0; i < 12; i++)
                    {
                        AddBullet(new EBulletVar(owner, pos, angle + 30 * i + offset * 15, 2, 2.0f, 3.0f, 12.0f, polarity));
                    }
                    count -= 1;
                    frame = 0;
                }
                if (count == 0) Erase();
            }
            base.Update();
        }
    }
}
