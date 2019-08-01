using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class PBurstFieldA1 : PlayerBullet
    {

        int time;

        public PBurstFieldA1(Fighter owner, Vector2 pos, float angle) : base(owner, pos, angle)
        {
            Audio.PlayBurstFieldSE();
            this.owner = owner;
            this.angle = angle;
            x = pos.X;
            y = pos.Y;
            damage = Status.PBurstFieldA1_Damage / 60.0f;
            Vector2 v = new Vector2(0, -speed);
            is_burst = true;
            velocity = Vector2.Transform(v, Matrix.CreateRotationZ(MathHelper.ToRadians(angle)));
            pierce = true;
            priority = 80;
            time = Status.PBurstFieldA1Time;
            burst_active = true;
        }

        public override void Update()
        {
            base.Update();
            if (time > 0) { time -= 1; }
            if (time <= 0) { Erase(); }
        }

        public override void Draw()
        {
            float scale = 2.0f;
            Texture2D texture = GetTexture();
            Vector2 origin = texture.GetCenter();
            Graphics.spriteBatch.Begin();
            float a = (time * 3.0f).ToRad();
            float alpha = 1.0f;
            if (time >= Status.PBurstFieldA1Time - 30) { alpha = 1.0f * (30 - (time - (Status.PBurstFieldA1Time - 30))) / 30f; }
            else if (time <= 30) { alpha = 1.0f * time / 30f; }
            alpha *= 0.8f;
            Graphics.spriteBatch.Draw(texture, pos, texture.GetRect(), Color.White * alpha, a, origin, scale, SpriteEffects.None, 0);
            Graphics.spriteBatch.End();
        }

        protected override Texture2D GetTexture()
        {
            return Cache.Texture("player_bullet/type_a_burst/laser_barrier", 1);
        }

        protected override Hitbox GetHitbox()
        {
            return new CircleHitbox(GetTexture().Width * 0.5f * 2f);
        }

    }
}
