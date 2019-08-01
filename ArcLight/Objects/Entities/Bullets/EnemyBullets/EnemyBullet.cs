using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class EnemyBullet : Bullet
    {
        public Enemy owner;
        public float damage;
        public int priority = 100;
        public float size = 16.0f;
        public int type = 0;
        public int polarity = 2;
        public float energy = 1.0f;

        public EnemyBullet(Enemy owner, Vector2 pos, float angle, int polarity = 2)
        {

        }

        public override void Update()
        {
            x += velocity.X;
            y += velocity.Y;
            CheckOutOfBounds();
        }

        void CheckOutOfBounds()
        {
            if (x < -64 || x > 480 + 64 || y < -64 || y > 640 + 64)
                alive = false;
        }

        float GetScale()
        {
            return (size / 16.0f);
        }

        public override void Draw()
        {
            if (!alive) return;
            Texture2D texture = GetTexture();
            Vector2 origin = texture.GetCenter();
            Graphics.spriteBatch.Begin();
            Graphics.spriteBatch.Draw(texture, pos, texture.GetRect(), Color.White, angle, origin, GetScale(), SpriteEffects.None, 0);
            Graphics.spriteBatch.End();
        }

        public override void Kill()
        {

        }

        public override void Erase()
        {
            alive = false;
        }

        public override bool Intersect(Entity e)
        {
            return hitbox.Intersect(pos, angle, e.hitbox, e.pos, e.angle);
        }

        public override bool Intersect(Hitbox h, Vector2 hpos, float hangle = 0)
        {
            return hitbox.Intersect(pos, angle, h, hpos, hangle);
        }

        protected override Hitbox GetHitbox()
        {
            return new CircleHitbox(GetScale() * 8.0f);
        }

        protected virtual Texture2D GetTexture()
        {
            return Cache.Texture(String.Format("enemy_bullet/bullet_{0}_{1}", type, polarity), 1);
        }

        public virtual void OnHit()
        {

        }
    }
}
