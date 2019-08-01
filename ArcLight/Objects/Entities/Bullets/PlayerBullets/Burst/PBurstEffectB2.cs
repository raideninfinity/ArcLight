using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class PBurstEffectB2 : PlayerBullet
    {

        int frame = 0;
        float expl_time = 30;

        public PBurstEffectB2(Fighter owner, Vector2 pos, float angle) : base(owner, pos, angle)
        {
            Audio.PlayBurstExplosionBigSE();
            this.owner = owner;
            this.angle = angle;
            x = pos.X;
            y = pos.Y;
            damage = Status.PBurstEffectB2_Damage;
            is_burst = true;
            Vector2 v = new Vector2(0, -speed);
            velocity = Vector2.Transform(v, Matrix.CreateRotationZ(MathHelper.ToRadians(angle)));
            is_setup = true;
            priority = 130;
        }

        protected override Texture2D GetTexture()
        {
            return Cache.Texture("player_bullet/type_b_burst/bomb_ring", 1);
        }

        protected override Hitbox GetHitbox()
        {
            float scale = 8.0f * (frame / expl_time);
            float radius = GetTexture().Width * 0.5f * scale;
            CircleHitbox hitbox = new CircleHitbox(radius);
            return hitbox;
        }

        List<Enemy> damaged_targets = new List<Enemy>();

        public override void Update()
        {
            base.Update();
            frame += 1;
            if (frame == expl_time)
            {
                alive = false;
            }
            foreach (Enemy e in Core.Controller.enemies)
            {
                if (!e.Active() || damaged_targets.Exists(a => a == e)) continue;
                if (this.Intersect(e))
                {
                    damaged_targets.Add(e);
                    e.Damage(this);
                }
            }
        }

        public override void Draw()
        {
            if (!alive) return;
            float scale = 4.0f * (frame / expl_time);
            float alpha = 1f - 0.5f * (frame / expl_time);
            Texture2D texture = GetTexture();
            Vector2 origin = texture.GetCenter();
            Graphics.spriteBatch.Begin();
            Graphics.spriteBatch.Draw(texture, pos, texture.GetRect(), Color.White * alpha, angle, origin, scale, SpriteEffects.None, 0);
            Graphics.spriteBatch.End();
        }

    }
}
