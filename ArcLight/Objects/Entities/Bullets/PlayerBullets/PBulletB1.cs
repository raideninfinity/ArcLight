using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    class PBulletB1 : PlayerBullet
    {

        public PBulletB1(Fighter owner, Vector2 pos, float angle) : base(owner, pos, angle)
        {
            this.owner = owner;
            this.angle = angle;
            x = pos.X;
            y = pos.Y;
            speed = 12.0f;
            damage = Status.PBulletB1_Damage[BurstChain(0)];
            Vector2 v = new Vector2(0, -speed);
            velocity = Vector2.Transform(v, Matrix.CreateRotationZ(MathHelper.ToRadians(angle)));
            priority = 120;
        }

        protected override Texture2D GetTexture()
        {
            return Cache.Texture("player_bullet/type_b/bullet_1", 1);
        }

        protected override Hitbox GetHitbox()
        {
            return new CircleHitbox(5);
        }

        public override void Update()
        {
            base.Update();
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