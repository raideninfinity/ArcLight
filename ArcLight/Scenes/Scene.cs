using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public abstract class Scene
    {
        public abstract void Start();
        public abstract void Update();
        public abstract void Draw();
        public abstract void End();
    }
}
