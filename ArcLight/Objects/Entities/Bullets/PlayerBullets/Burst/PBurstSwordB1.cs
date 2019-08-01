using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class PBurstSwordB1 : PlayerBullet
    {

        int time;

        public PBurstSwordB1(Fighter owner, Vector2 pos, float angle) : base(owner, pos, angle)
        {
            Audio.PlayBurstSwordSE();
            this.owner = owner;
            this.angle = angle;
            x = owner.pos.X;
            y = owner.pos.Y;
            damage = Status.PBurstSwordB1_Damage / 60.0f;
            is_burst = true;
            Vector2 v = new Vector2(0, -speed);
            velocity = Vector2.Transform(v, Matrix.CreateRotationZ(MathHelper.ToRadians(angle)));
            is_setup = true;
            priority = 110;
            time = Status.PBurstSwordB1Time;
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
                e.Damage(this, Status.PBurstSwordB1_AddDamage);
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
            DrawCentralBarrier();
            DrawSword();
            Graphics.spriteBatch.End();
        }

        public void DrawCentralBarrier()
        {
            Texture2D f_barrier;
            f_barrier = Cache.Texture("player_bullet/type_b_burst/sword_barrier", 1);
            float angle = (time * 3.0f).ToRad();
            Graphics.spriteBatch.Draw(f_barrier, pos, f_barrier.GetRect(), Color.White, angle, f_barrier.GetCenter(), 1, SpriteEffects.None, 0);
        }

        public override bool Intersect(Entity e)
        {
            float scale = 0.25f;

            RectangleHitbox hitbox = new RectangleHitbox(44 * (1 + scale), 166 * (1 + scale));
            //first
            Vector2 pos1 = new Vector2(0, 120 + (166 * 0.5f * scale));
            bool intersect = e.hitbox.Intersect(e.pos, e.angle, hitbox, pos - pos1, 0);
            //second
            Vector2 pos2 = Vector2.Transform(pos1, Matrix.CreateRotationZ(MathHelper.ToRadians(120)));
            intersect |= e.hitbox.Intersect(e.pos, e.angle, hitbox, pos - pos2, 120);
            //third
            pos2 = Vector2.Transform(pos1, Matrix.CreateRotationZ(MathHelper.ToRadians(240)));
            intersect |= e.hitbox.Intersect(e.pos, e.angle, hitbox, pos - pos2, 240);

            scale = 0.75f;
            hitbox = new RectangleHitbox(22 * (1 + scale), 83 * (1 + scale));
            //fourth
            pos1 = new Vector2(0, 64 + (83 * 0.5f * scale));
            pos2 = Vector2.Transform(pos1, Matrix.CreateRotationZ(MathHelper.ToRadians(60)));
            intersect |= e.hitbox.Intersect(e.pos, e.angle, hitbox, pos - pos2, 60);
            //fifth
            pos2 = Vector2.Transform(pos1, Matrix.CreateRotationZ(MathHelper.ToRadians(180)));
            intersect |= e.hitbox.Intersect(e.pos, e.angle, hitbox, pos - pos2, 180);
            //sixth
            pos2 = Vector2.Transform(pos1, Matrix.CreateRotationZ(MathHelper.ToRadians(300)));
            intersect |= e.hitbox.Intersect(e.pos, e.angle, hitbox, pos - pos2, 300);


            //finalize
            return base.Intersect(e) || intersect;
        }

        public override bool Intersect(Hitbox h, Vector2 hpos, float hangle)
        {
            float scale = 0.25f;

            RectangleHitbox hitbox = new RectangleHitbox(44 * (1 + scale), 166 * (1 + scale));
            //first
            Vector2 pos1 = new Vector2(0, 120 + (166 * 0.5f * scale));
            bool intersect = h.Intersect(hpos, hangle, hitbox, pos - pos1, 0);
            //second
            Vector2 pos2 = Vector2.Transform(pos1, Matrix.CreateRotationZ(MathHelper.ToRadians(120)));
            intersect |= h.Intersect(hpos, hangle, hitbox, pos - pos2, 120);
            //third
            pos2 = Vector2.Transform(pos1, Matrix.CreateRotationZ(MathHelper.ToRadians(240)));
            intersect |= h.Intersect(hpos, hangle, hitbox, pos - pos2, 240);

            scale = 0.75f;
            hitbox = new RectangleHitbox(22 * (1 + scale), 83 * (1 + scale));
            //fourth
            pos1 = new Vector2(0, 64 + (83 * 0.5f * scale));
            pos2 = Vector2.Transform(pos1, Matrix.CreateRotationZ(MathHelper.ToRadians(60)));
            intersect |= h.Intersect(hpos, hangle, hitbox, pos - pos2, 60);
            //fifth
            pos2 = Vector2.Transform(pos1, Matrix.CreateRotationZ(MathHelper.ToRadians(180)));
            intersect |= h.Intersect(hpos, hangle, hitbox, pos - pos2, 180);
            //sixth
            pos2 = Vector2.Transform(pos1, Matrix.CreateRotationZ(MathHelper.ToRadians(300)));
            intersect |= h.Intersect(hpos, hangle, hitbox, pos - pos2, 300);


            //finalize
            return base.Intersect(h, hpos, hangle) || intersect;
        }

        protected override Hitbox GetHitbox()
        {
            return new CircleHitbox(131 * 0.5f);
        }

        public void DrawSword()
        {
            Texture2D texture = Cache.Texture("player_bullet/type_b_burst/sword1", 1);
            float scale = 0.25f;
            Vector2 pos1 = new Vector2(0, 120 + (texture.Height * 0.5f * scale));
            scale += 1;
            float alpha = Math.Abs((float)Math.Sin(((float)Graphics.totalMs * 0.2f).ToRad()));
            Graphics.spriteBatch.Draw(texture, pos - pos1, texture.GetRect(),
            Color.White * (0.5f + 0.5f * alpha), 0, texture.GetCenter(), scale, SpriteEffects.None, 0);
            Vector2 pos2 = Vector2.Transform(pos1, Matrix.CreateRotationZ(MathHelper.ToRadians(120)));
            Graphics.spriteBatch.Draw(texture, pos - pos2, texture.GetRect(),
                Color.White * (0.5f + 0.5f * alpha), MathHelper.ToRadians(120), texture.GetCenter(), scale, SpriteEffects.None, 0);
            pos2 = Vector2.Transform(pos1, Matrix.CreateRotationZ(MathHelper.ToRadians(240)));
            Graphics.spriteBatch.Draw(texture, pos - pos2, texture.GetRect(),
                Color.White * (0.5f + 0.5f * alpha), MathHelper.ToRadians(240), texture.GetCenter(), scale, SpriteEffects.None, 0);
            //REVERSE
            texture = Cache.Texture("player_bullet/type_b_burst/sword", 1);
            scale = 0.75f;
            pos1 = new Vector2(0, 64 + (texture.Height * 0.5f * scale));
            scale += 1;
            pos2 = Vector2.Transform(pos1, Matrix.CreateRotationZ(MathHelper.ToRadians(60)));
            Graphics.spriteBatch.Draw(texture, pos - pos2, texture.GetRect(),
                Color.White * (0.5f + 0.5f * alpha), MathHelper.ToRadians(60), texture.GetCenter(), scale, SpriteEffects.None, 0);
            pos2 = Vector2.Transform(pos1, Matrix.CreateRotationZ(MathHelper.ToRadians(180)));
            Graphics.spriteBatch.Draw(texture, pos - pos2, texture.GetRect(),
                Color.White * (0.5f + 0.5f * alpha), MathHelper.ToRadians(180), texture.GetCenter(), scale, SpriteEffects.None, 0);
            pos2 = Vector2.Transform(pos1, Matrix.CreateRotationZ(MathHelper.ToRadians(300)));
            Graphics.spriteBatch.Draw(texture, pos - pos2, texture.GetRect(),
                Color.White * (0.5f + 0.5f * alpha), MathHelper.ToRadians(300), texture.GetCenter(), scale, SpriteEffects.None, 0);
        }
    }
}
