using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class Emitter
    {

        protected int frame;
        public Enemy owner;
        public int polarity;
        public bool alive = true;
        protected float x, y;
        protected float angle;

        protected Vector2 pos { get { return new Vector2(x, y); } set { x = value.X; y = value.Y; } }

        public virtual void Update()
        {
            frame += 1;
        }

        public void Erase()
        {
            alive = false;
        }

        public void AddBullet(EnemyBullet bullet)
        {
            Core.Controller.AddEnemyBullet(bullet);
        }
    }
}
