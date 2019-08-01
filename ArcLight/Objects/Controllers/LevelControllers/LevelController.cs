using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class LevelController
    {
        public int frame;
        public int param1;
        public int param2;
        public int param3;
        public int pattern;
        public bool stage_end = false;
        public bool pattern_end = false;
        public bool spawn_end = false;
        protected GameController controller;

        public LevelController(GameController controller)
        {
            this.controller = controller;
        }

        public virtual void Update()
        {
            frame += 1;
        }

        public void EnemyAllLeave()
        {
            foreach (Enemy e in Core.Controller.enemies)
            {
                if (e.Active()) e.Leave();
            }
        }
    }
}
