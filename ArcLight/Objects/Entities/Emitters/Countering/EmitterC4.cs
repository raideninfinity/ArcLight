using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class EmitterC4 : Emitter
    {

        int interval = 5;

        public EmitterC4(Enemy owner, Vector2 pos, float angle)
        {
            this.owner = owner;
            this.pos = pos;
            this.angle = angle;
            frame = interval;
        }

        int count = 8;
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
                    float offset = count % 4;
                    int polarity;
                    if (count >= 0 && count <= 2) { polarity = 0; }
                    else if (count > 2 && count <= 4) { polarity = 1; }
                    else if (count > 4 && count <= 6) { polarity = 0; }
                    else { polarity = 1; }
                    for (int i = 0; i < 24; i++)
                    {
                        AddBullet(new EBulletVar(owner, pos, angle + 30 * i + offset * 7.5f, 2, 250f, 3.0f, 12.0f, polarity));
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
