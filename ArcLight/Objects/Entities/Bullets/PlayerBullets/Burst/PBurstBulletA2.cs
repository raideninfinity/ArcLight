using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    class PBurstBulletA2 : PlayerBullet
    {

        int frame = -1;
        bool no_se;

        public PBurstBulletA2(Fighter owner, Vector2 pos, float angle, bool no_se = true) : base(owner, pos, angle)
        {
            this.owner = owner;
            this.angle = angle;
            x = pos.X;
            y = pos.Y;
            speed = 3f;
            pierce = true;
            this.no_se = no_se;
            Vector2 v = new Vector2(0, -speed);
            velocity = Vector2.Transform(v, Matrix.CreateRotationZ(MathHelper.ToRadians(angle)));
            priority = 100;
        }

        protected override Hitbox GetHitbox()
        {
            return new CircleHitbox(32);
        }

        public override void Draw()
        {
            if (!alive) return;
            Texture2D texture;
            Vector2 origin;
            Graphics.spriteBatch.Begin();
            texture = Cache.Texture("player_bullet/type_a_burst/circle", 1);
            origin = texture.GetCenter();
            Graphics.spriteBatch.Draw(texture, pos, texture.GetRect(), Color.White, angle, origin, 1.2f, SpriteEffects.None, 0);
            Graphics.spriteBatch.End();
        }

        public override void Update()
        {
            base.Update();
            x += velocity.X;
            y += velocity.Y;
            CheckOutOfBounds();
            frame += 1;
            if (frame % 30 == 0)
            {
                Core.Controller.AddPlayerBullet(new PBurstEffectA2(owner, new Vector2(x, y), 0, no_se));
            }
            if (frame == 120)
            {
                alive = false;
            }
        }

        void CheckOutOfBounds()
        {
            if (x < -64 || x > 480 + 64 || y < -64 || y > 640 + 64)
                alive = false;
        }
    }
}