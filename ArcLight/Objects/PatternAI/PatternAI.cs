using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class PatternAI
    {
        public Enemy enemy;
        public int frame;
        public int param;
        public int f_interval = 0;
        public int f_frame = 0;

        public virtual void Initialize(Enemy enemy)
        {
        }

        public virtual void Update()
        {
            if (!enemy.dying)
            {
                UpdateMovement();
                if (!OutOfBounds(72) && !enemy.leaving && enemy.prepare_frames <= 0 )
                {
                    UpdateFiring();
                    f_frame += 1;
                }
                frame += 1;
            }
        }

        public virtual void UpdateMovement()
        {

        }

        public virtual void UpdateFiring()
        {

        }

        public virtual void OnDeath()
        {

        }

        public void AddEmitter(Emitter e)
        {
            Core.Controller.AddEmitter(e);
        }

        public float FindClosestPlayerAngle()
        {
            Fighter fighter = Core.Controller.GetNearestFighter(enemy.pos);
            if (fighter == null) { return 0; }
            return (180 - enemy.pos.AngleBetweenPoints(fighter.pos));
        }

        public float FindFurthestPlayerAngle()
        {
            Fighter fighter = Core.Controller.GetFurthestFighter(enemy.pos);
            if (fighter == null) { return 0; }
            return (180 - enemy.pos.AngleBetweenPoints(fighter.pos));
        }

        public virtual void CheckOutOfBounds(float range)
        {
            if (enemy.x < -range || enemy.x > 480 + range || enemy.y < -range || enemy.y > 640 + range)
                enemy.Erase();
        }

        public virtual bool OutOfBounds(float range)
        {
            return (enemy.x < -range || enemy.x > 480 + range || enemy.y < -range || enemy.y > 640 + range) ;
        }

    }
}