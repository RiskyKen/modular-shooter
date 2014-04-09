#region "License"
//Modular Shooter - Modular 2D shooter game.
//Copyright (C) 2014 RiskyKen

//This program is free software; you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation; either version 2 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License along
//with this program; if not, write to the Free Software Foundation, Inc.,
//51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace RiskyKen.ModularShooter.Game
{
    static class Trig
    {
        public static float GetAngle(float x1, float y1, float x2, float y2)
        {
            float pX = 0;
            float pY = 0;
            float AngleRad = 0;
            float Angle = 0;

            pX = x2 - x1;
            //Adjacent
            pY = y2 - y1;
            //Opposite

            AngleRad = Convert.ToSingle(Math.Atan2(pY, pX));
            //Radians

            Angle = Convert.ToSingle(AngleRad * (180 / Math.PI));
            // Convert Radians to Degrees

            return Angle + 180;
        }

        public static float GetAngle(PointF pos1, PointF pos2)
        {
            return GetAngle(pos1.X, pos1.Y, pos2.X, pos2.Y);
        }

        public static float DistanceBetween(float X1, float Y1, float X2, float Y2)
        {
            float Horizontal = 0;
            float Vertical = 0;
            Horizontal = Math.Abs(X2 - X1);
            Vertical = Math.Abs(Y2 - Y1);

            return Convert.ToSingle(Math.Sqrt((Horizontal * Horizontal) + (Vertical * Vertical)));
        }

        public static float DistanceBetween(PointF Pos1, PointF pos2)
        {
            return DistanceBetween(Pos1.X, Pos1.Y, pos2.X, pos2.Y);
        }

        public static PointF MoveTo(PointF _point, float Speed, float Angle)
        {
            PointF NewPoint = default(PointF);
            NewPoint.X = Convert.ToSingle(_point.X - Speed * Math.Cos(DegreesToRadians(Angle)));
            NewPoint.Y = Convert.ToSingle(_point.Y - Speed * Math.Sin(DegreesToRadians(Angle)));
            return NewPoint;
        }

        public static double DegreesToRadians(double degrees)
        {
            return 2 * Math.PI * degrees / 360.0;
        }
    }
}
