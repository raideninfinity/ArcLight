using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class PLaserB1 : PlayerBullet
    {

        public float laser_angle;

        public PLaserB1(Fighter owner, Vector2 pos, float angle) : base(owner, pos, angle)
        {
            this.owner = owner;
            this.angle = angle;
            x = owner.pos.X;
            y = owner.pos.Y;
            damage = Status.PLaserB1_Damage[BurstChain(1)] / 60.0f;
            Vector2 v = new Vector2(0, -speed);
            velocity = Vector2.Transform(v, Matrix.CreateRotationZ(MathHelper.ToRadians(angle)));
            is_setup = true;
            is_focus = true;
            priority = 110;
        }

        public void MoveTypeBLaserAngle(float angle)
        {
            float offset = 0.5f;
            if (angle >= 180) angle = -(360 - angle);
            if (laser_angle > angle)
            {
                laser_angle -= offset;
                if (laser_angle < angle)
                    laser_angle = angle;
            }
            else if (laser_angle < angle)
            {
                laser_angle += offset;
                if (laser_angle > angle)
                    laser_angle = angle;
            }
        }

        public override void Update()
        {
            base.Update();
            x = owner.pos.X;
            y = owner.pos.Y;
            Enemy enemy = Core.Controller.GetNearestEnemyForBF1(pos);
            if (enemy == null)
                MoveTypeBLaserAngle(0);
            else
                MoveTypeBLaserAngle(180 - pos.AngleBetweenPoints(enemy.pos));
            foreach (Enemy e in Core.Controller.enemies)
            {
                if (!e.Active()) continue;
                if (this.Intersect(e))
                {
                    DamageEnemy(e);
                }
            }
            UpdateDamageCooldown();
            UpdateDamageMult();
        }

        public override void Draw()
        {
            Graphics.spriteBatch.Begin();
            DrawLaser();
            Graphics.spriteBatch.End();
        }

        List<Enemy> hitEnemyM = new List<Enemy>();

        public void DamageEnemy(Enemy e)
        {
            Audio.PlayLaserHit(owner.player.index);
            hitEnemyM.Add(e);
            if (!damage_mult.ContainsKey(e))
            {
                damage_mult[e] = 0;
            }
            if (!damage_cooldown.ContainsKey(e) || damage_cooldown[e] <= 0)
            {
                float add = Status.PLaserB1_AddDamage[BurstChain(1)];
                add *= (1.0f + damage_mult[e]);
                e.Damage(this, add + damage * damage_mult[e]);
                damage_cooldown[e] = 60;
            }
            else
            {
                e.Damage(this, damage * damage_mult[e]);
            }
            damage_mult[e] += Status.PLaserB1_MultGainRate / 60.0f;
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

        public static Dictionary<Enemy, float> damage_mult = new Dictionary<Enemy, float>();
        List<Enemy> toRemoveM = new List<Enemy>();
        List<Enemy> toUpdateM = new List<Enemy>();

        public void UpdateDamageMult()
        {
            toRemoveM.Clear();
            toUpdateM.Clear();
            foreach (KeyValuePair<Enemy, float> pair in damage_mult)
            {
                if (!pair.Key.alive && !(pair.Value > 0))
                {
                    toRemoveM.Add(pair.Key);
                }
                else
                {
                    if (hitEnemyM.Contains(pair.Key))
                        toUpdateM.Add(pair.Key);
                }
            }
            foreach (var key in toRemoveM)
            {
                damage_mult.Remove(key);
            }
            foreach (var key in toUpdateM)
            {
                damage_mult[key] -= Status.PLaserB1_MultDecRate / 60.0f;
                if (damage_mult[key] < 0) { damage_mult[key] = 0; }
            }
            hitEnemyM.Clear();
        }

        public override bool Intersect(Entity e)
        {
            RectangleHitbox hitbox = new RectangleHitbox(16, pos.Y * 1.2f);
            Vector2 pos1 = new Vector2(0, pos.Y * 1.2f * 0.5f);
            pos1 = Vector2.Transform(pos1, Matrix.CreateRotationZ(MathHelper.ToRadians(laser_angle)));
            pos1 = pos - pos1;
            bool intersect = e.hitbox.Intersect(e.pos, e.angle, hitbox, pos1, laser_angle);
            return base.Intersect(e) || intersect;
        }

        protected override Hitbox GetHitbox()
        {
            return new CircleHitbox(38);
        }

        public void DrawLaser()
        {
            int chain = owner.BurstChain(1);
            Texture2D texture = Cache.Texture("player_bullet/type_b/laser", 1);
            Vector2 pos1 = new Vector2(0, pos.Y * 1.2f * 0.5f);
            float alpha_base = 0.7f - (chain * 0.1f);
            Vector2 pos2 = Vector2.Transform(pos1, Matrix.CreateRotationZ(MathHelper.ToRadians(laser_angle)));
            Vector2 scale = new Vector2(1, pos.Y * 1.2f);
            float alpha = Math.Abs((float)Math.Sin(((float)Graphics.totalMs * (0.2f + chain * 0.1f)).ToRad()));
            Graphics.spriteBatch.Draw(texture, pos - pos2, texture.GetRect(),
               Color.White * (alpha_base + (1 - alpha_base) * alpha), MathHelper.ToRadians(laser_angle), texture.GetCenter(), scale, SpriteEffects.None, 0);
        }
    }
}
