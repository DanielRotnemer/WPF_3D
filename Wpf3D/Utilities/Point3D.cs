using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Wpf3D.Utilities
{
    public class Point3D
    {
        #region Private variables

        private double x, y, z;

        #endregion

        #region Constructor

        public Point3D(double x = 0, double y = 0, double z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        #endregion

        #region Properties

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        public double Z
        {
            get { return z; }
            set { z = value; }
        }

        #endregion

        #region Methods

        public Point3D Translate(double offsetX, double offsetY, double offsetZ, bool add)
        {
            if (add)
                return new Point3D(X + offsetX, Y + offsetY, Z + offsetZ);
            else
                return new Point3D(X - offsetX, Y - offsetY, Z - offsetZ);
        }

        public Vector3D ToVector()
        {
            return new Vector3D(new Point3D(X, Y, Z));
        }

        public double DotProduct(double scalar)
        {
            return x * scalar + y * scalar + z * scalar;
        }

        public double DotProduct(Point3D p)
        {
            return x * p.X + y * p.Y + z * p.Z;
        }

        public double Distance(Point3D p)
        {
            double xComponent = Math.Pow(x - p.X, 2);
            double yComponent = Math.Pow(y - p.Y, 2);
            double zComponent = Math.Pow(z - p.Z, 2);
            return Math.Sqrt(xComponent + yComponent + zComponent);
        }

        #endregion

        #region Operators

        public static Point3D operator +(Point3D p1, Point3D p2)
        {
            return new Point3D(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        }

        public static Point3D operator -(Point3D p1, Point3D p2)
        {
            return new Point3D(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        }

        public static Point3D operator *(Point3D p, double scalar)
        {
            return new Point3D(p.X * scalar, p.Y * scalar, p.Z * scalar);
        }

        public static Point3D operator *(Point3D p1, Point3D p2)
        {
            return new Point3D(p1.X * p2.X, p1.Y * p2.Y, p1.Z * p2.Z);
        }

        public static Point3D operator /(Point3D p, double denominator)
        {
            return new Point3D(p.X / denominator, p.Y / denominator, p.Z / denominator);
        }

        public static Point3D operator /(Point3D p1, Point3D p2)
        {
            return new Point3D(p1.X / p2.X, p1.Y / p2.Y, p1.Z / p2.Z);
        }

        #endregion

        #region Overriden & Static methods

        public static bool Equals(Point3D p1, Point3D p2)
        {
            if (p1 == null && p2 == null)
                return true;
            if (p1 == null || p2 == null)
                return false;
            return p1.X == p2.X && p1.Y == p2.Y && p1.Z == p2.Z;
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ", " + Z + ")";
        }

        #endregion
    }
}
