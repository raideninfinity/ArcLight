using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArcLight
{
    public static class Utility
    {

        public static float tan15 = (float)Math.Tan((90.0f - 15.0f).ToRad());
        public static float tan20 = (float)Math.Tan((90.0f - 20.0f).ToRad());
        public static float tan25 = (float)Math.Tan((90.0f - 25.0f).ToRad());
        public static float tan30 = (float)Math.Tan((90.0f - 30.0f).ToRad());
        public static float tan45 = (float)Math.Tan((90.0f - 45.0f).ToRad());
        public static float tan60 = (float)Math.Tan((90.0f - 60.0f).ToRad());
        public static float tan75 = (float)Math.Tan((90.0f - 75.0f).ToRad());

        public static Rectangle GetRect(this Texture2D texture)
        {
            return new Rectangle(0, 0, texture.Width, texture.Height);
        }

        public static Vector2 GetCenter(this Texture2D texture)
        {
            return new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
        }

        public static Vector2 GetCenter(this Vector2 vector)
        {
            return new Vector2(vector.X / 2.0f, vector.Y / 2.0f);
        }

        public static float ToRad(this float angle)
        {
            return MathHelper.ToRadians(angle);
        }

        public static float ToRad(this int angle)
        {
            return MathHelper.ToRadians(angle);
        }

        public static float ToDeg(this float angle)
        {
            return MathHelper.ToDegrees(angle);
        }

        public static float FixAngle(this float angle)
        {
            angle = 180 - angle;
            if (angle > 180) angle = -(360 - angle);
            return angle;
        }

        public static float AngleBetweenPoints(this Vector2 vector1, Vector2 vector2)
        {
            float a = vector2.X - vector1.X;
            float b = vector2.Y - vector1.Y;

            return ((float)Math.Atan2(a, b)).ToDeg();
        }

        public static float GetDistance(this Vector2 v1, Vector2 v2)
        {
            return Vector2.Distance(v1, v2);
        }

        public static float GetDistanceSquared(this Vector2 v1, Vector2 v2)
        {
            return Vector2.DistanceSquared(v1, v2);
        }

        public static float Clamp(this float f, float min, float max)
        {
            if (f >= max) { return max; }
            else if (f <= min) { return min; }
            else return f;
        }

        public static int Clamp(this int f, int min, int max)
        {
            if (f >= max) { return max; }
            else if (f <= min) { return min; }
            else return f;
        }

        public static float getDistance(float fromX, float fromY, float toX, float toY)
        {
            return (new Vector2(fromX, fromY)).GetDistance(new Vector2(toX, toY));
        }

        public static Vector2 TurnAngle(this Vector2 v, float angle)
        {
            return Vector2.Transform(v, Matrix.CreateRotationZ(MathHelper.ToRadians(angle)));
        }

        public static Vector2 TurnAngleRad(this Vector2 v, float angle)
        {
            return Vector2.Transform(v, Matrix.CreateRotationZ(angle));
        }

        public static Random Rand = new System.Random();

        public static int Random(this int min, int max)
        {
            return (Rand.Next(min, max));
        }


        public static bool CheckCircleOutOfBounds(float x, float y, float radius)
        {
            return (x + radius < 0 || x - radius > 480 || y + radius < 0 || y - radius > 640);
        }

        public static bool PointIntersect(RectangleHitbox rect, Vector2 pos, Vector2 point)
        {
            float x1 = pos.X - rect.size.X * 0.5f;
            float x2 = pos.X + rect.size.X * 0.5f;
            float y1 = pos.Y - rect.size.Y * 0.5f;
            float y2 = pos.Y + rect.size.Y * 0.5f;
            float x = point.X;
            float y = point.Y;
            if (x > x1 && x < x2 && y > y1 && y < y2) return true;
            return false;
        }

        public static bool PointInCircle(this CircleHitbox circle, Vector2 pos, Vector2 point)
        {
            float d2 = pos.GetDistanceSquared(point);
            float r = circle.radius;
            return (d2 < (r * r));
        }

        #region HitboxCollision

        public static bool CheckCollision(this CircleHitbox circle, Vector2 pos1, RectangleHitbox rect, Vector2 pos2, float angle)
        {
            //preliminary
            if (CheckCircleOutOfBounds(pos1.X, pos1.Y, circle.radius)) return false;
            float d2 = pos1.GetDistanceSquared(pos2);
            float r = (circle.radius + rect.radius);
            if (d2 > (r * r)) return false;
            float ir = (circle.radius + rect.inner_radius);
            if (d2 < (ir * ir)) return true;
            if (angle < 180) { angle = 180 - angle; }
            else if (angle > 180) { angle = 180 - (-(360 - angle)); }
            return collideCircleWithRotatedRectangle(pos1, circle.radius, pos2, rect.size, angle.ToRad());
        }

        public static bool collideCircleWithRotatedRectangle(Vector2 circle_pos, float circle_radius, Vector2 rect_pos, Vector2 rect_size, float angle)
        {
            if (CheckCircleOutOfBounds(circle_pos.X, circle_pos.Y, circle_radius)) return false;
            float rectCenterX = rect_pos.X;
            float rectCenterY = rect_pos.Y;

            float rectX = rectCenterX - rect_size.X / 2;
            float rectY = rectCenterY - rect_size.Y / 2;

            float rectReferenceX = rectX;
            float rectReferenceY = rectY;

            // Rotate circle's center point back
            float unrotatedCircleX = (float)(Math.Cos(angle) * (circle_pos.X - rectCenterX) - Math.Sin(angle) * (circle_pos.Y - rectCenterY) + rectCenterX);
            float unrotatedCircleY = (float)(Math.Sin(angle) * (circle_pos.X - rectCenterX) + Math.Cos(angle) * (circle_pos.Y - rectCenterY) + rectCenterY);

            // Closest point in the rectangle to the center of circle rotated backwards(unrotated)
            float closestX, closestY;

            // Find the unrotated closest x point from center of unrotated circle
            if (unrotatedCircleX < rectReferenceX)
            {
                closestX = rectReferenceX;
            }
            else if (unrotatedCircleX > rectReferenceX + rect_size.X)
            {
                closestX = rectReferenceX + rect_size.X;
            }
            else
            {
                closestX = unrotatedCircleX;
            }

            // Find the unrotated closest y point from center of unrotated circle
            if (unrotatedCircleY < rectReferenceY)
            {
                closestY = rectReferenceY;
            }
            else if (unrotatedCircleY > rectReferenceY + rect_size.Y)
            {
                closestY = rectReferenceY + rect_size.Y;
            }
            else
            {
                closestY = unrotatedCircleY;
            }

            // Determine collision
            var collision = false;
            var distance = getDistance(unrotatedCircleX, unrotatedCircleY, closestX, closestY);

            if (distance < circle_radius)
            {
                collision = true;
            }
            else
            {
                collision = false;
            }

            return collision;
        }

        public static bool CheckCollision(this CircleHitbox circle1, Vector2 pos1, CircleHitbox circle2, Vector2 pos2)
        {
            if (CheckCircleOutOfBounds(pos1.X, pos1.Y, circle1.radius)) return false;
            if (CheckCircleOutOfBounds(pos2.X, pos2.Y, circle2.radius)) return false;
            float x = pos1.X - pos2.X;
            float y = pos1.Y - pos2.Y;
            float r = (circle1.radius + circle2.radius);
            return (x * x) + (y * y) <= (r * r);
        }

        #endregion

    }
}
