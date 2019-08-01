using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class PlayerBullet : Bullet
    {
        public Fighter owner;
        public float damage;
        public bool burst_active = false;
        public bool is_burst = false;
        public bool is_focus = false;
        public bool is_counter = false;
        public int priority = 100;

        public PlayerBullet(Fighter player, Vector2 pos, float angle)
        {

        }

        public override void Update()
        {

        }

        public override void Draw()
        {

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

        public override bool Intersect(Hitbox h, Vector2 hpos, float hangle)
        {
            return hitbox.Intersect(pos, angle, h, hpos, hangle);
        }

        protected override Hitbox GetHitbox()
        {
            return null;
        }

        protected virtual Texture2D GetTexture() { return null; }

        public virtual void OnHit()
        {

        }

        public int polarity
        {
            get
            {
                if (is_focus) return 1;
                else return 0;
            }
        }

        public int BurstChain(int mode)
        {
            if (mode == owner.player.mode)
                return owner.player.chain.Clamp(0, 3);
            else
                return (owner.player.chain - 3).Clamp(0, 3);
        }
    }
}
