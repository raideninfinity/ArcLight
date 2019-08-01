using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class EmitterC2 : Emitter
    {
        int interval = 3;
        public EmitterC2(Enemy owner, Vector2 pos, float angle, int polarity)
        {
            this.owner = owner;
            this.x = pos.X;
            this.y = pos.Y;
            this.angle = angle;
            this.polarity = polarity;
            frame = interval;
        }

        int count = 3;
        public override void Update()
        {
            if (alive)
            {
                if (frame >= interval)
                {
                    float offset = 5f;
                    float a = angle + offset;
                    AddBullet(new EBulletVar(owner, pos, a, 1, 50.0f, 4.0f, 8.0f, polarity, 90));
                    a = angle + offset * 3;
                    AddBullet(new EBulletVar(owner, pos, a, 1, 50.0f, 4.0f, 8.0f, polarity, 90));
                    a = angle - offset;
                    AddBullet(new EBulletVar(owner, pos, a, 1, 50.0f, 4.0f, 8.0f, polarity, 90));
                    a = angle - offset * 3;
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
