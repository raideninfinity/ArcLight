using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class TestEmitter : Emitter
    {

        public TestEmitter(Enemy owner, Vector2 pos, float angle, int polarity = 2)
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
                AddBullet(new TestBullet(owner, pos, angle + ((float)(Utility.Rand.NextDouble() - 0.5f) * 30), polarity));
                Erase();
            }
        }
    }
}
