using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    class PBulletB3 : PlayerBullet
    {

        Enemy target;
        int startup_frame = 8;

        public PBulletB3(Fighter owner, Vector2 pos, float angle, Enemy target) : base(owner, pos, angle)
        {
            this.owner = owner;
            this.target = target;
            this.angle = angle;
            x = pos.X;
            y = pos.Y;
            speed = 12.0f;
            damage = Status.PBulletB3_Damage[BurstChain(0)];
            Vector2 v = new Vector2(0, -speed);
            velocity = Vector2.Transform(v, Matrix.CreateRotationZ(MathHelper.ToRadians(angle)));
            priority = 120;
        }

        void TurnAngle(float angle)
        {
            velocity = Vector2.Transform(velocity, Matrix.CreateRotationZ(MathHelper.ToRadians(angle)));
        }

        protected override Texture2D GetTexture()
        {
            return Cache.Texture("player_bullet/type_b/bullet_2", 1);
        }

        protected override Hitbox GetHitbox()
        {
            return new CircleHitbox(4);
        }

        public override void Update()
        {
            if (startup_frame > 0) startup_frame -= 1;
            base.Update();
            if (target != null && target.Active() && startup_frame <= 0)
            {
                float deltax = pos.X - target.pos.X;
                float deltay = pos.Y - target.pos.Y;
                float diff = -(float)Math.Atan2(deltax, deltay);
                //calc angle 
                if (deltax > 0 && deltay > 0)      //s1
                    diff = (float)Math.PI / 2 + diff;
                else if (deltax < 0 && deltay > 0) //s2
                    diff = -3 * (float)Math.PI / 2 + diff;
                else if (deltax < 0 && deltay < 0) //s3
                    diff = diff + (float)Math.PI / 2;
                else                               //s4
                    diff = (float)Math.PI / 2 + diff;
                diff = diff.ToDeg();
                diff = diff + 90;
                //finalize
                //if (angle > 360) { angle -= 360; }
                //if (angle > diff) { angle += Math.Abs(diff - angle).Clamp(-1f, 1f); }
                //else { angle -= Math.Abs(diff - angle).Clamp(-1f, 1f); }
                //if (angle != diff)
                //    Console.WriteLine(angle.ToString() + " === " + diff.ToString());
                angle = diff - 180;
                Vector2 v = new Vector2(0, -speed);
                velocity = Vector2.Transform(v, Matrix.CreateRotationZ(MathHelper.ToRadians(angle)));
            }
            x += velocity.X;
            y += velocity.Y;
            CheckOutOfBounds();
        }

        public override void Draw()
        {
            if (!alive) return;
            Texture2D texture = GetTexture();
            Vector2 origin = texture.GetCenter();
            Graphics.spriteBatch.Begin();
            Graphics.spriteBatch.Draw(texture, pos, texture.GetRect(), Color.White, angle, origin, 1, SpriteEffects.None, 0);
            Graphics.spriteBatch.End();
        }

        void CheckOutOfBounds()
        {
            if (x < -64 || x > 480 + 64 || y < -64 || y > 640 + 64)
                alive = false;
        }
    }
}