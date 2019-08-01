using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class PLSwordB1 : PlayerBullet
    {

        public PLSwordB1(Fighter owner, Vector2 pos, float angle) : base(owner, pos, angle)
        {
            this.owner = owner;
            this.angle = angle;
            x = owner.pos.X;
            y = owner.pos.Y;
            damage = Status.PLSwordB1_Damage[BurstChain(1)] / 60.0f;
            Vector2 v = new Vector2(0, -speed);
            velocity = Vector2.Transform(v, Matrix.CreateRotationZ(MathHelper.ToRadians(angle)));
            is_setup = true;
            is_focus = true;
            priority = 110;
        }

        public override void Update()
        {
            base.Update();
            x = owner.pos.X;
            y = owner.pos.Y;
            foreach (Enemy e in Core.Controller.enemies)
            {
                if (!e.Active()) continue;
                if (this.Intersect(e))
                {
                    DamageEnemy(e);
                }
            }
            UpdateDamageCooldown();
        }

        public void DamageEnemy(Enemy e)
        {
            Audio.PlayLaserHit(owner.player.index);
            if (!damage_cooldown.ContainsKey(e) || damage_cooldown[e] <= 0)
            {
                e.Damage(this, Status.PLSwordB1_AddDamage[BurstChain(1)]);
                damage_cooldown[e] = 60;
            }
            else
            {
                e.Damage(this);
            }
        }

        public static Dictionary<Enemy, int> damage_cooldown = new Dictionary<Enemy, int>();
        List<Enemy> toRemove = new List<Enemy>();
        List<Enemy> toUpdate = new List<Enemy>();

        public void UpdateDamageCooldown()
        {
            toRemove.Clear();
            toUpdate.Clear();
            foreach (KeyValuePair<Enemy, int> pair in damage_cooldown)
            {
                if (!pair.Key.alive && !(pair.Value > 0))
                {
                    toRemove.Add(pair.Key);
                }
                else
                {
                    toUpdate.Add(pair.Key);
                }
            }
            foreach (var key in toRemove)
            {
                damage_cooldown.Remove(key);
            }
            foreach (var key in toUpdate)
            {
                damage_cooldown[key] -= 1;
            }
        }

        public override void Draw()
        {
            Graphics.spriteBatch.Begin();
            DrawSword();
            Graphics.spriteBatch.End();
        }

        public override bool Intersect(Entity e)
        {
            float scale = owner.BurstChain(1) * 0.25f;
            RectangleHitbox hitbox = new RectangleHitbox(22 * (1 + scale), 83 * (1 + scale));
            //first
            Vector2 pos1 = new Vector2(0, 64 + (83 * 0.5f * scale));
            bool intersect = e.hitbox.Intersect(e.pos, e.angle, hitbox, pos - pos1, 0);
            //second
            Vector2 pos2 = Vector2.Transform(pos1, Matrix.CreateRotationZ(MathHelper.ToRadians(120)));
            intersect |= e.hitbox.Intersect(e.pos, e.angle, hitbox, pos - pos2, 120);
            //third
            pos2 = Vector2.Transform(pos1, Matrix.CreateRotationZ(MathHelper.ToRadians(240)));
            intersect |= e.hitbox.Intersect(e.pos, e.angle, hitbox, pos - pos2, 240);
            //finalize
            return base.Intersect(e) || intersect;
        }

        protected override Hitbox GetHitbox()
        {
            return new CircleHitbox(38);
        }

        public void DrawSword()
        {
            Texture2D texture = Cache.Texture("player_bullet/type_b/sword", 1);
            float scale = owner.BurstChain(1) * 0.25f;
            Vector2 pos1 = new Vector2(0, 64 + (texture.Height * 0.5f * scale)); //text height = 83
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
        }
    }
}
