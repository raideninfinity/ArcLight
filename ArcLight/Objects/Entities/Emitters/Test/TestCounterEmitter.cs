using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class TestCounterEmitter : Emitter
    {

        int interval = 5;

        public TestCounterEmitter(Enemy owner, Vector2 pos, float angle, int polarity = 2)
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
                        AddBullet(new TestCounterBullet(owner, pos, angle + 30 * i + offset * 15, polarity));
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
