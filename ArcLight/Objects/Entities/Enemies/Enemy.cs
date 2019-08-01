using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class Enemy : Entity
    {
        public float hp = 1;
        public bool dying = false;
        public bool leaving = false;
        public int death_frames = 0;
        public int prepare_frames = 0;
        public int leave_frames = 0;
        public int polarity = 2;
        public PatternAI pattern_ai;
        public float kill_score;

        public Enemy()
        {
        }

        public virtual float GetHP()
        {
            return 0;
        }

        public virtual float GetMaxHP()
        {
            return 1;
        }

        public virtual float GetHPRatio()
        {
            return hp / GetMaxHP();
        }

        public override void Update()
        {
            if (death_frames > 0) { death_frames -= 1; }
            if (prepare_frames > 0) { prepare_frames -= 1; }
            if (leave_frames > 0) { leave_frames -= 1; }
            if (dying && death_frames <= 0) { alive = false; }
            if (leaving && leave_frames <= 0) { alive = false; }
            if (pattern_ai != null) pattern_ai.Update();
        }

        public void SetPatternAI(PatternAI ai)
        {
            pattern_ai = ai;
        }

        public virtual bool Active()
        {
            return (alive && !dying && prepare_frames <= 0 && !leaving);
        }

        public override void Draw()
        {
            
        }

        public override void Kill()
        {
            if (!dying)
            {
                dying = true;
                death_frames = 60;
                OnDeath();
            }
        }

        public virtual void OnDeath()
        {
            if (pattern_ai != null) pattern_ai.OnDeath();
        }

        public void Leave()
        {
            if (!leaving)
            {
                leaving = true;
                leave_frames = 60;
            }
        }

        public override void Erase()
        {
            alive = false;
        }

        public override bool Intersect(Entity e)
        {
            return hitbox.Intersect(pos, angle, e.hitbox, e.pos, e.angle);
        }

        public override bool Intersect(Hitbox h, Vector2 hpos, float hangle)
        {
            return hitbox.Intersect(pos, angle, h, hpos, hangle);
        }

        protected override Hitbox GetHitbox()
        {
            return null;
        }

        protected virtual Texture2D GetTexture()
        {
            return null;
        }

        protected Texture2D GetBarrier()
        {
           if (polarity <= 2)
                return Cache.Texture("enemy/component/hitbox_" + polarity.ToString(), 1);
           else
                return Cache.Texture("enemy/component/hitbox_2", 1);
        }

        protected Texture2D GetDeathRing()
        {
            if (polarity <= 2)
                return Cache.Texture("enemy/component/death_ring_" + polarity.ToString(), 1);
            else
                return Cache.Texture("enemy/component/hitbox_2", 1);
        }

        public void Damage(PlayerBullet bullet, float add = 0)
        {
            if (bullet != null)
            {
                float real_damage = bullet.damage + add;
                if (!bullet.is_burst && polarity < 2 && bullet.polarity == polarity) real_damage *= 1.5f;
                if (bullet.is_burst && polarity == 3) real_damage *= 1.5f;
                hp -= real_damage;
                if (!bullet.is_burst)
                {
                    float multiplier = multiplier = (bullet.is_focus ? Status.EnergyHitGainBonusFocus : Status.EnergyHitGainMultSpread);
                    if (bullet.owner.player.mode == 0 && !bullet.is_focus) multiplier = Status.EnergyHitGainMultSpread;
                    else if (bullet.owner.player.mode == 1 && bullet.is_focus) multiplier = Status.EnergyHitGainMultFocus;
                    if (bullet != null)
                    {
                        Core.Session.GainEnergy(bullet.owner.player, real_damage * multiplier);
                        Core.Session.GainScore(bullet.owner.player, real_damage * Status.ScoreHitGainMult);
                    }
                }
                else
                {
                    Core.Session.GainScore(bullet.owner.player, real_damage * Status.ScoreHitGainMult);
                }
            }
            else
            {
                hp -= add;
            }
            if (hp <= 0)
            {
                Kill();
                if (bullet != null && bullet.owner != null)
                {
                    Core.Session.GainScore(bullet.owner.player, kill_score);
                }
            }
        }
    }
}
