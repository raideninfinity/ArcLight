using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class PLaserA2_Sub
    {
        PLaserA2 host;
        int index;

        public PLaserA2_Sub(PLaserA2 host, int index)
        {
            this.host = host;
            this.index = index;
            pos = host.pos;
            pos.X += ((index == 0) ? -48 : 48);
            pos.Y = Graphics.Height - (BaseTexture.Height * 0.5f);
            Update();
        }

        public Vector2 pos;
        public float target_x;

        public void Update()
        {
            TargetX();
            SeekX();
        }

        public void TargetX()
        {
            float pos_x = host.owner.pos.X;
            Enemy enemy = null;
            List<Enemy> enemies = Core.Controller.enemies.FindAll(a => a.Active());

            if (index == 0) //Left - priority first
            {
                List<Enemy> left = enemies.FindAll(a => a.pos.X <= pos_x);
                left = left.OrderBy(a => Math.Abs(a.pos.X - host.owner.pos.X)).ToList();
                if (left.Count > 0) enemy = left[0];
                if (enemy != null)
                {
                    target_x = enemy.pos.X;
                    return;
                }
                List<Enemy> right = enemies.FindAll(a => a.pos.X > pos_x);
                right = right.OrderBy(a => Math.Abs(a.pos.X - host.owner.pos.X)).ToList();
                if (right.Count > 0) enemy = right[0];
                if (enemy != null)
                {
                    target_x = enemy.pos.X;
                    return;
                }
                else
                {
                    target_x = host.pos.X + ((index == 0) ? -48 : 48);
                }
            }
            else //Right
            {
                List<Enemy> right = enemies.FindAll(a => a.pos.X > pos_x);
                right = right.OrderBy(a => Math.Abs(a.pos.X - host.owner.pos.X)).ToList();
                if (right.Count > 0) enemy = right[0];
                if (enemy != null)
                {
                    target_x = enemy.pos.X;
                    return;
                }
                List<Enemy> left = enemies.FindAll(a => a.pos.X <= pos_x);
                left = left.OrderBy(a => Math.Abs(a.pos.X - host.owner.pos.X)).ToList();
                if (left.Count > 0) enemy = left[0];
                if (enemy != null)
                {
                    target_x = enemy.pos.X;
                    return;
                }
                else
                {
                    target_x = host.pos.X + ((index == 0) ? -48 : 48);
                }
            }
        }

        public void SeekX()
        {
            float move_rate = 4.0f;
            if (pos.X > target_x)
            {
                pos.X -= move_rate;
                if (pos.X < target_x)
                    pos.X = target_x;
            }
            else if (pos.X < target_x)
            {
                pos.X += move_rate;
                if (pos.X > target_x)
                    pos.X = target_x;
            }
            pos.X.Clamp(0 + ((x_size * 16) / 2), 480 - ((x_size * 16) / 2));
        }

        public Texture2D BaseTexture { get { return Cache.Texture("player_bullet/type_a/laser_2_base", 1); } }
        public Texture2D BodyTexture { get { return Cache.Texture("player_bullet/type_a/laser_2_body", 1); } }

        public float x_size { get { return 1.0f + host.owner.BurstChain(1) * (1 / 3.0f); } }

        public Hitbox hitbox { get { return new RectangleHitbox(x_size * 16, Graphics.Height); } }

        public void Draw()
        {
            float alpha = Math.Abs((float)Math.Sin(((float)Graphics.totalMs * 0.2f).ToRad()));
            alpha = 0.7f + 0.3f * alpha;
            Texture2D base_ = BaseTexture;
            Vector2 size = new Vector2(x_size, 1);
            Graphics.spriteBatch.Draw(base_, pos, base_.GetRect(), Color.White * alpha, 0, base_.GetCenter(), size, SpriteEffects.None, 0);
            Texture2D body = BodyTexture;
            size = new Vector2(x_size, Graphics.Height - BaseTexture.Height);
            Vector2 position = new Vector2(pos.X, size.Y * 0.5f);
            Graphics.spriteBatch.Draw(body, position, body.GetRect(), Color.White * alpha, 0, body.GetCenter(), size, SpriteEffects.None, 0);
        }
    }
}
