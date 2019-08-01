using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class PBurstLaserA1 : PlayerBullet
    {

        int time;

        public PBurstLaserA1(Fighter owner, Vector2 pos, float angle) : base(owner, pos, angle)
        {
            Audio.PlayBurstLaserSE();
            this.owner = owner;
            this.angle = angle;
            x = owner.pos.X;
            y = owner.pos.Y;
            damage = Status.PBurstLaserA1_Damage / 60.0f;
            Vector2 v = new Vector2(0, -speed);
            is_burst = true;
            velocity = Vector2.Transform(v, Matrix.CreateRotationZ(MathHelper.ToRadians(angle)));
            is_setup = true;
            priority = 110;
            time = Status.PBurstLaserA1Time;
            burst_active = true;
        }

        public override void Update()
        {
            base.Update();
            x = owner.pos.X;
            y = owner.pos.Y;
            if (time > 0) { time -= 1; }
            if (time <= 0) { Erase(); }
            foreach (Enemy e in Core.Controller.enemies)
            {
                if (!e.Active()) continue;
                if (this.Intersect(e))
                {
                    DamageEnemy(e);
                }
            }
        }

        public Dictionary<Enemy, int> damage_cooldown = new Dictionary<Enemy, int>();
        public void DamageEnemy(Enemy e)
        {
            if (!damage_cooldown.ContainsKey(e) || damage_cooldown[e] <= 0)
            {
                e.Damage(this, Status.PBurstLaserA1_AddDamage);
                damage_cooldown[e] = 60;
            }
            else
            {
                e.Damage(this);
            }
        }

        public override void Draw()
        {
            Graphics.spriteBatch.Begin();
            DrawLaser();
            Graphics.spriteBatch.End();
        }

        public override bool Intersect(Entity e)
        {
            RectangleHitbox hitbox = new RectangleHitbox(131, pos.Y);
            Vector2 pos1 = new Vector2(pos.X, pos.Y - (hitbox.size.Y * 0.5f));
            bool intersect = e.hitbox.Intersect(e.pos, e.angle, hitbox, pos1, 0);
            //finalize
            return base.Intersect(e) || intersect;
        }

        public override bool Intersect(Hitbox h, Vector2 hpos, float hangle)
        {
            RectangleHitbox hitbox = new RectangleHitbox(131, pos.Y);
            Vector2 pos1 = new Vector2(pos.X, pos.Y - (hitbox.size.Y * 0.5f));
            bool intersect = h.Intersect(hpos, hangle, hitbox, pos1, 0);
            //finalize
            return base.Intersect(h, pos, hangle) || intersect;
        }

        protected override Hitbox GetHitbox()
        {
            return new CircleHitbox(131 * 0.5f);
        }

        public void DrawLaser()
        {
            Texture2D barrier = Cache.Texture("player_bullet/type_a_burst/laser_barrier", 1);
            Texture2D end = Cache.Texture("player_bullet/type_a_burst/laser_end", 1);
            Texture2D body = Cache.Texture("player_bullet/type_a_burst/laser_body", 1);
            float alpha = Math.Abs((float)Math.Sin(((float)Graphics.totalMs * 0.2f).ToRad()));
            alpha = 0.8f + 0.2f * alpha;
            Graphics.spriteBatch.Draw(end, pos - new Vector2(0, 50f), end.GetRect(), Color.White * alpha, 0, end.GetCenter(), 1.0f, SpriteEffects.None, 0);
            Vector2 scale = new Vector2(1, pos.Y);
            Vector2 pos1 = pos - new Vector2(0, pos.Y * 0.5f + 50 + 48 + 5);
            Graphics.spriteBatch.Draw(body, pos1, body.GetRect(), Color.White * alpha, 0, body.GetCenter(), scale, SpriteEffects.None, 0);
            float angle = (time * 3.0f).ToRad();
            Graphics.spriteBatch.Draw(barrier, pos, barrier.GetRect(), Color.White, angle, barrier.GetCenter(), 1.0f, SpriteEffects.None, 0);
        }
    }
}
