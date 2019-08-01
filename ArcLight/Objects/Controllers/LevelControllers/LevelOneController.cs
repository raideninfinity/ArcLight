using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public partial class LevelOneController : LevelController
    {
        public LevelOneController(GameController controller) : base(controller)
        {
            this.controller = controller;
            pattern_end = true;
        }

        public override void Update()
        {
            if (pattern_end)
            {
                frame = -1;
                pattern += 1;
                spawn_end = false;
                pattern_end = false;
                param1 = 0;
                param2 = 0;
                param3 = 0;
            }
            else
            {
                switch (pattern)
                {
                    case 1: PerformPattern1(); break;
                    case 2: PerformPattern2(); break;
                    case 3: PerformPattern3(); break;
                    case 4: PerformPattern4(); break;
                    case 5: PerformPattern5(); break;
                    case 6: PerformPattern6(); break;
                    case 7: PerformPattern7(); break;
                    case 8: PerformPattern8(); break;
                    case 9: PerformPattern9(); break;
                    case 10: PerformPattern10(); break;
                    case 11: PerformBossAlert(); break;
                    case 12: PerformBossPattern(); break;
                }
            }
            base.Update();
        }
    }
}
