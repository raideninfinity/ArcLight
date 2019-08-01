using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class GamePlayer
    {
        public GamePlayer(int index, int type, int mode)
        {
            this.index = index;
            this.type = type;
            this.mode = mode;
            energy = 10000;
        }

        public int index = 0;
        public int type = 0;
        public int mode = 0;
        public int color = 0;
        public int lives = 3;
        public int chain = 0;
        public int chain_time = 0;
        public int max_chain = 0;
        public float energy = 0;
        public int b_wpn = 0;
        public float score = 0;
        public bool intruded = false;
        public int miss = 0;

    }
}
