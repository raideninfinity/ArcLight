using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class TestAI : PatternAI
    {

        public override void Initialize(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public override void Update()
        {
           /* if ((frame + 30) % 60 == 0)
                AddEmitter(new TestEmitter(enemy, enemy.pos, FindClosestPlayerAngle(), enemy.polarity));
            base.Update();*/
        }

        public override void OnDeath()
        {
            //AddEmitter(new TestCounterEmitter(enemy, enemy.pos, 0, enemy.polarity));
        }
    }
}