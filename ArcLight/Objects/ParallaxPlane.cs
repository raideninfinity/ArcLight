using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class ParallaxPlane
    {
        public Texture2D texture;
        public float x_speed;
        public float y_speed;
        public int width;
        public int height;
        public float x_displace;
        public float y_displace;
        public float scroll_target_mult = 1.0f;
        public float scroll_mult = 1.0f;

        public ParallaxPlane(Texture2D texture, float scroll_x, float scroll_y)
        {
            this.texture = texture;
            width = texture.Width;
            height = texture.Height;
            this.x_speed = scroll_x;
            this.y_speed = scroll_y;
        }

        public void Update()
        {
            UpdateScrollMult();
            x_displace += x_speed * scroll_mult;
            y_displace += y_speed * scroll_mult;
            if (x_displace > width) x_displace -= width;
            if (y_displace > height) y_displace -= height;
            if (x_displace < -width) x_displace = 0;
            if (y_displace < -height) y_displace = 0;
        }

        public void UpdateScrollMult()
        {
            if (scroll_mult < scroll_target_mult)
            {
                scroll_mult += 2 / 60.0f;
                if (scroll_mult > scroll_target_mult)
                    scroll_mult = scroll_target_mult;
            }
            else if (scroll_mult > scroll_target_mult)
            {
                scroll_mult -= 2 / 60.0f;
                if (scroll_mult < scroll_target_mult)
                    scroll_mult = scroll_target_mult;
            }
        }

        public void Draw()
        {
            //To Be Fixed
            Graphics.spriteBatch.Begin();
            Vector2 pos = new Vector2(x_displace, y_displace);
            //Graphics.spriteBatch.Draw(texture, pos + new Vector2(-width, height), Color.White);
            Graphics.spriteBatch.Draw(texture, pos + new Vector2(0, height), Color.White);
            //Graphics.spriteBatch.Draw(texture, pos + new Vector2(width, height), Color.White);
            //Graphics.spriteBatch.Draw(texture, pos + new Vector2(-width, 0), Color.White);
            Graphics.spriteBatch.Draw(texture, pos + new Vector2(0, 0), Color.White);
            //Graphics.spriteBatch.Draw(texture, pos + new Vector2(width, 0), Color.White);
            //Graphics.spriteBatch.Draw(texture, pos + new Vector2(-width, -height), Color.White);
            Graphics.spriteBatch.Draw(texture, pos + new Vector2(0, -height), Color.White);
            //Graphics.spriteBatch.Draw(texture, pos + new Vector2(width, -height), Color.White);
            Graphics.spriteBatch.End();
        }
    }
}
