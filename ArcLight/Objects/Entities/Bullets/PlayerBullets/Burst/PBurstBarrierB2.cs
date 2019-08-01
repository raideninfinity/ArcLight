using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class PBurstBarrierB2 : PlayerBullet
    {

        int time;

        public PBurstBarrierB2(Fighter owner, Vector2 pos, float angle) : base(owner, pos, angle)
        {
            Audio.PlayBurstBarrierSE();
            this.owner = owner;
            this.angle = angle;
            x = owner.pos.X;
            y = owner.pos.Y;
            damage = 120 / 60.0f;
            Vector2 v = new Vector2(0, -speed);
            velocity = Vector2.Transform(v, Matrix.CreateRotationZ(MathHelper.ToRadians(angle)));
            is_setup = true;
            //is_burst = true;
            time = time = Status.PBurstBarrierB2_Time; ;
            priority = 91;
        }

        public override void Update()
        {
            base.Update();
            x = owner.pos.X;
            y = owner.pos.Y;
            if (time > 0) { time -= 1; }
            if (time <= 0) { Audio.PlayBurstBarrierExpireSE(); Erase(); Core.Session.GainEnergy(owner.player, 3000); }
        }

        protected override Texture2D GetTexture()
        {
            return Cache.Texture("player_bullet/type_b_burst/mine_barrier", 1);
        }

        public override void Draw()
        {
            Graphics.spriteBatch.Begin();
            Texture2D texture = GetTexture();
            float angle = (time * 3.0f).ToRad();
            Graphics.spriteBatch.Draw(texture, pos, texture.GetRect(), Color.White, angle, texture.GetCenter(), 1.0f, SpriteEffects.None, 0);
            Graphics.spriteBatch.End();
        }

        protected override Hitbox GetHitbox()
        {
            return new CircleHitbox(38);
        }

    }
}
