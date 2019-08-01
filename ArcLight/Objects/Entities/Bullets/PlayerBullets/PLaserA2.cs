using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class PLaserA2 : PlayerBullet
    {
        public PLaserA2_Sub left_sub;
        public PLaserA2_Sub right_sub;

        public PLaserA2(Fighter owner, Vector2 pos, float angle) : base(owner, pos, angle)
        {
            this.owner = owner;
            this.angle = angle;
            x = owner.pos.X;
            y = owner.pos.Y;
            damage = Status.PLaserA2_Damage[BurstChain(1)] / 60.0f;
            Vector2 v = new Vector2(0, -speed);
            velocity = Vector2.Transform(v, Matrix.CreateRotationZ(MathHelper.ToRadians(angle)));
            is_setup = true;
            is_focus = true;
            priority = 95;
            left_sub = new PLaserA2_Sub(this, 0);
            right_sub = new PLaserA2_Sub(this, 1);
        }

        public override void Update()
        {
            base.Update();
            x = owner.pos.X;
            y = owner.pos.Y;
            left_sub.Update();
            right_sub.Update();
            CheckIntersect();
            UpdateDamageCooldownM();
            UpdateDamageCooldownL();
            UpdateDamageCooldownR();
        }

        public void CheckIntersect()
        {
            foreach (Enemy e in Core.Controller.enemies)
            {
                if (!e.Active()) continue;
                if (this.Intersect(e))
                    DamageEnemyM(e);
                Vector2 hitbox_pos = new Vector2(left_sub.pos.X, Graphics.Width / 2);
                if (e.Intersect(left_sub.hitbox, hitbox_pos, 0))
                    DamageEnemyL(e);
                hitbox_pos = new Vector2(right_sub.pos.X, Graphics.Width / 2);
                if (e.Intersect(right_sub.hitbox, hitbox_pos, 0))
                    DamageEnemyR(e);
            }
        }

        #region DamageEnemy

        public void DamageEnemyM(Enemy e)
        {
            Audio.PlayLaserHit(owner.player.index);
            if (!damage_cooldownM.ContainsKey(e) || damage_cooldownM[e] <= 0)
            {
                e.Damage(this, Status.PLaserA2_AddDamage[BurstChain(1)]);
                damage_cooldownM[e] = 60;
            }
            else
            {
                e.Damage(this);
            }
        }

        public void DamageEnemyL(Enemy e)
        {
            Audio.PlayLaserHit(owner.player.index);
            if (!damage_cooldownL.ContainsKey(e) || damage_cooldownL[e] <= 0)
            {
                e.Damage(this, Status.PLaserA2_AddDamage[BurstChain(1)]);
                damage_cooldownL[e] = 60;
            }
            else
            {
                e.Damage(this);
            }
        }

        public void DamageEnemyR(Enemy e)
        {
            Audio.PlayLaserHit(owner.player.index);
            if (!damage_cooldownR.ContainsKey(e) || damage_cooldownR[e] <= 0)
            {
                e.Damage(this, Status.PLaserA2_AddDamage[BurstChain(1)]);
                damage_cooldownR[e] = 60;
            }
            else
            {
                e.Damage(this);
            }
        }

        #endregion

        #region Damage Cooldown

        public static Dictionary<Enemy, int> damage_cooldownM = new Dictionary<Enemy, int>();
        List<Enemy> toRemoveM = new List<Enemy>();
        List<Enemy> toUpdateM = new List<Enemy>();

        public void UpdateDamageCooldownM()
        {
            toRemoveM.Clear();
            toUpdateM.Clear();
            foreach (KeyValuePair<Enemy, int> pair in damage_cooldownM)
            {
                if (!pair.Key.alive && !(pair.Value > 0))
                {
                    toRemoveM.Add(pair.Key);
                }
                else
                {
                    toUpdateM.Add(pair.Key);
                }
            }
            foreach (var key in toRemoveM)
            {
                damage_cooldownM.Remove(key);
            }
            foreach (var key in toUpdateM)
            {
                damage_cooldownM[key] -= 1;
            }
        }

        public static Dictionary<Enemy, int> damage_cooldownL = new Dictionary<Enemy, int>();
        List<Enemy> toRemoveL = new List<Enemy>();
        List<Enemy> toUpdateL = new List<Enemy>();

        public void UpdateDamageCooldownL()
        {
            toRemoveL.Clear();
            toUpdateL.Clear();
            foreach (KeyValuePair<Enemy, int> pair in damage_cooldownL)
            {
                if (!pair.Key.alive && !(pair.Value > 0))
                {
                    toRemoveL.Add(pair.Key);
                }
                else
                {
                    toUpdateL.Add(pair.Key);
                }
            }
            foreach (var key in toRemoveL)
            {
                damage_cooldownL.Remove(key);
            }
            foreach (var key in toUpdateL)
            {
                damage_cooldownL[key] -= 1;
            }
        }

        public static Dictionary<Enemy, int> damage_cooldownR = new Dictionary<Enemy, int>();
        List<Enemy> toRemoveR = new List<Enemy>();
        List<Enemy> toUpdateR = new List<Enemy>();

        public void UpdateDamageCooldownR()
        {
            toRemoveR.Clear();
            toUpdateR.Clear();
            foreach (KeyValuePair<Enemy, int> pair in damage_cooldownR)
            {
                if (!pair.Key.alive && !(pair.Value > 0))
                {
                    toRemoveR.Add(pair.Key);
                }
                else
                {
                    toUpdateR.Add(pair.Key);
                }
            }
            foreach (var key in toRemoveR)
            {
                damage_cooldownR.Remove(key);
            }
            foreach (var key in toUpdateR)
            {
                damage_cooldownR[key] -= 1;
            }
        }


        #endregion

        public override void Draw()
        {
            Graphics.spriteBatch.Begin();
            left_sub.Draw();
            right_sub.Draw();
            Graphics.spriteBatch.End();
        }

        protected override Hitbox GetHitbox()
        {
            return new CircleHitbox(38);
        }

    }
}
