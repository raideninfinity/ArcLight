using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class EmitterS4 : Emitter
    {
        int interval = 10;
        public EmitterS4(Enemy owner, Vector2 pos, float angle)
        {
            this.owner = owner;
            this.x = pos.X;
            this.y = pos.Y;
            frame = interval;
            this.angle = angle;
        }

        int count = 3;
        public override void Update()
        {
            if (!owner.alive || Utility.CheckCircleOutOfBounds(pos.X, pos.Y, owner.hitbox.radius) || !Core.Controller.AnyPlayerAlive())
            {
                Erase();
                return;
            }
            else
            {
                this.x = owner.x;
                this.y = owner.y;
            }
            if (alive)
            {
                if (frame >= interval)
                {
                    float offset = 5f;
                    float offset2 = 30f;

                    float a = angle;
                    a = angle + offset + offset2;
                    AddBullet(new EBulletVar(owner, pos, a, 2, 200f, 4.0f, 12.0f, owner.polarity));
                    a = angle + offset2;
                    AddBullet(new EBulletVar(owner, pos, a, 2, 200f, 4.0f, 12.0f, owner.polarity));
                    a = angle - offset + offset2;
                    AddBullet(new EBulletVar(owner, pos, a, 2, 200f, 4.0f, 12.0f, owner.polarity));
                    a = angle + offset;
                    AddBullet(new EBulletVar(owner, pos, a, 2, 200f, 4.0f, 12.0f, owner.polarity));
                    a = angle;
                    AddBullet(new EBulletVar(owner, pos, a, 2, 200f, 4.0f, 12.0f, owner.polarity));
                    a = angle - offset;
                    AddBullet(new EBulletVar(owner, pos, a, 2, 200f, 4.0f, 12.0f, owner.polarity));
                    a = angle + offset - offset2;
                    AddBullet(new EBulletVar(owner, pos, a, 2, 200f, 4.0f, 12.0f, owner.polarity));
                    a = angle - offset2;
                    AddBullet(new EBulletVar(owner, pos, a, 2, 200f, 4.0f, 12.0f, owner.polarity));
                    a = angle - offset - offset2;
                    AddBullet(new EBulletVar(owner, pos, a, 2, 200f, 4.0f, 12.0f, owner.polarity));

                    count -= 1;
                    frame = 0;
                }
                if (count == 0) Erase();
            }
            base.Update();
        }
    }
}
