using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    class PBurstBulletB1 : PlayerBullet
    {

        public PBurstBulletB1(Fighter owner, Vector2 pos, float angle) : base(owner, pos, angle)
        {
            this.owner = owner;
            this.angle = angle;
            x = pos.X;
            y = pos.Y;
            speed = 8.0f;
            on_hit = true;
            Vector2 v = new Vector2(0, -speed);
            velocity = Vector2.Transform(v, Matrix.CreateRotationZ(MathHelper.ToRadians(angle)));
            priority = 100;
        }

        protected override Texture2D GetTexture()
        {
            return Cache.Texture("player_bullet/type_b_burst/circle", 1);
        }

        protected override Hitbox GetHitbox()
        {
            return new CircleHitbox(19);
        }

        public override void Draw()
        {
            if (!alive) return;
            Texture2D texture = GetTexture();
            Vector2 origin = texture.GetCenter();
            Graphics.spriteBatch.Begin();
            Graphics.spriteBatch.Draw(texture, pos, texture.GetRect(), Color.White, angle, origin, 1.2f, SpriteEffects.None, 0);
            Graphics.spriteBatch.End();
        }

        public override void Update()
        {
            base.Update();
            x += velocity.X;
            y += velocity.Y;
            CheckOutOfBounds();
        }

        void CheckOutOfBounds()
        {
            if (x < -64 || x > 480 + 64 || y < -64 || y > 640 + 64)
                alive = false;
        }

        public override void OnHit()
        {
            Core.Controller.AddPlayerBullet(new PBurstEffectB1(owner, new Vector2(x, y), 0));
        }
    }
}