using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class PLaserA1 : PlayerBullet
    {
        float laser_length;

        public PLaserA1(Fighter owner, Vector2 pos, float angle) : base(owner, pos, angle)
        {
            this.owner = owner;
            this.angle = angle;
            x = owner.pos.X;
            y = owner.pos.Y;
            damage = Status.PLaserA1_Damage[BurstChain(1)] / 60.0f;
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
            Enemy target;
            laser_length = Core.Controller.GetTypeALaserLength(pos, 26 * (1.0f + (owner.BurstChain(1) * 0.5f)), out target);
            if (target != null)
            {
                DamageEnemy(target);
            }
            foreach (Enemy e in Core.Controller.enemies)
            {
                if (!e.Active() || target == e) continue;
                if (this.Intersect(e))
                {
                    DamageEnemy(e);
                }
            }
            UpdateDamageCooldown();
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

        public void DamageEnemy(Enemy e)
        {
            Audio.PlayLaserHit(owner.player.index);
            if (!damage_cooldown.ContainsKey(e) || damage_cooldown[e] <= 0)
            {
                e.Damage(this, Status.PLaserA1_AddDamage[BurstChain(1)]);
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

        protected override Hitbox GetHitbox()
        {
            return new CircleHitbox(38);
        }

        public void DrawLaser()
        {
            Texture2D texture = Cache.Texture("player_bullet/type_a/laser_body", 1);
            Texture2D head = Cache.Texture("player_bullet/type_a/laser_head", 1);
            if (laser_length <= 0)
            {
                Vector2 pos1 = new Vector2(0, pos.Y * 1.2f * 0.5f);
                Vector2 scale = new Vector2(1 + owner.BurstChain(1) * 0.5f, pos.Y * 1.2f);
                float alpha = Math.Abs((float)Math.Sin(((float)Graphics.totalMs * 0.2f).ToRad()));
                Graphics.spriteBatch.Draw(texture, pos - pos1, texture.GetRect(),
                    Color.White * (0.7f + 0.3f * alpha), 0, texture.GetCenter(), scale, SpriteEffects.None, 0);
            }
            else
            {
                float offset = 4;
                Vector2 pos1 = new Vector2(0, laser_length * 0.5f - offset);
                Vector2 pos2 = new Vector2(0, laser_length + head.Height * 0.5f - offset);
                Vector2 scale = new Vector2(1 + owner.BurstChain(1) * 0.5f, laser_length);
                Vector2 head_scale = new Vector2(1 + owner.BurstChain(1) * 0.5f, 1);
                float alpha = Math.Abs((float)Math.Sin(((float)Graphics.totalMs * 0.2f).ToRad()));
                Graphics.spriteBatch.Draw(texture, pos - pos1, texture.GetRect(),
                    Color.White * (0.7f + 0.3f * alpha), 0, texture.GetCenter(), scale, SpriteEffects.None, 0);
                Graphics.spriteBatch.Draw(head, pos - pos2, head.GetRect(),
                    Color.White * (0.7f + 0.3f * alpha), 0, head.GetCenter(), head_scale, SpriteEffects.None, 0);
            }
        }
    }
}
