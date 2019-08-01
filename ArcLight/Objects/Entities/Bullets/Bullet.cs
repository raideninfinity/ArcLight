using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public abstract class Bullet : Entity
    {
        public float speed;
        public Vector2 velocity;
        public bool pierce = false;
        public bool on_hit = false;
        public bool is_setup = false;
        public int bullet_code = 0;
    }
}
