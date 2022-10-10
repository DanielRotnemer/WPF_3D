using System;
using System.Collections.Generic;
using System.Text;

namespace Wpf3D.Utilities
{
    public class Vector3D
    {
        #region Private variables

        private Point3D head;

        #endregion

        #region Constructors

        public Vector3D(Point3D head)
        {
            this.head = head;
        }

        public Vector3D(double x, double y, double z)
        {
            head = new Point3D(x, y, z);
        }

        #endregion

        #region Properties

        public Point3D Head
        {
            get { return head; }
            set { head = value; }
        }

        public Vector3D UnitVector
        {
            get { return this / Length(); }
        }

        public Vector3D DirectionVector
        {
            get 
            {
                double x = head.X, y = head.Y, z = head.Z;
                if (Math.Abs(x) > Math.Abs(y) && Math.Abs(x) > Math.Abs(z))
                    return new Vector3D(x / Math.Abs(x), y / Math.Abs(x), z / Math.Abs(x));
                else if (Math.Abs(y) > Math.Abs(x) && Math.Abs(y) > Math.Abs(z))
                    return new Vector3D(x / Math.Abs(y), y / Math.Abs(y), z / Math.Abs(y));
                else
                    return new Vector3D(x / Math.Abs(z), y / Math.Abs(z), z / Math.Abs(z));
            }
        }

        #endregion

        #region Rotations

        public Vector3D RotateAroundX(double angle)
        {
            double radians = angle * (Math.PI / 180.0);
            return new Vector3D
            (
                head.X,
                head.Y * Math.Cos((float)radians) - head.Z * Math.Sin((float)radians),
                head.Y * Math.Sin((float)radians) + head.Z * Math.Cos((float)radians)
            );
        }

        public Vector3D RotateAroundY(double angle)
        {
            double radians = angle * (Math.PI / 180.0);
            return new Vector3D
            (
                head.X * Math.Cos((float)radians) + head.Z * Math.Sin((float)radians),
                head.Y,
                head.X * -1 * Math.Sin((float)radians) + head.Z * Math.Cos((float)radians)
            );
        }

        public Vector3D RotateAroundZ(double angle)
        {
            double radians = angle * (Math.PI / 180.0);
            return new Vector3D
            (
                head.X * Math.Cos((float)radians) - head.Y * Math.Sin((float)radians),
                head.X * Math.Sin((float)radians) + head.Y * Math.Cos((float)radians),
                head.Z
            );
        }

        #endregion

        #region Methods

        public double Angle(Vector3D v)
        {
            float unit = Length() * v.Length() == 0 ? 0 : (float)(DotProduct(v) / (Length() * v.Length()));
            return Math.Acos(unit) * 180 / Math.PI;
        }

        public Vector3D DirectionInDegrees()
        {
            double xAngle = Math.Acos((float)(head.X / Length())) * 180 / Math.PI;
            double yAngle = Math.Acos((float)(head.Y / Length())) * 180 / Math.PI;
            double zAngle = Math.Acos((float)(head.Z / Length())) * 180 / Math.PI;
            return new Vector3D(xAngle, yAngle, zAngle);
        }

        public Vector3D Normalize()
        {
            head /= Length();
            return this;
        }

        public double Length()
        {
            double xComponent = head.X;
            double yComponent = head.Y;
            double zComponent = head.Z;
            return Math.Sqrt(xComponent * xComponent + yComponent * yComponent
                + zComponent * zComponent);
        }

        public bool IsOrthogonal(Vector3D v)
        {
            return DotProduct(v) == 0d;
        }

        public bool IsParallel(Vector3D v, bool sameDirection = true)  
        {
            if (sameDirection)
                return Angle(v) == 0d;
            return Angle(v) == 180d;
        }

        public Vector3D Scale(double factor)
        {
            return new Vector3D(head * factor);
        }

        public double DotProduct(double scalar)
        {
            return head.DotProduct(scalar);
        }

        public double DotProduct(Vector3D v)
        {
            return head.DotProduct(v.head);
        }

        public Vector3D CrossProduct(Vector3D v)
        {
            return new Vector3D
            (
                head.Y * v.Head.Z - head.Z * v.Head.Y,
                -1 * (head.X * v.Head.Z - head.Z * v.Head.X),
                head.X * v.Head.Y - head.Y * v.Head.X
            );
        }

        #endregion

        #region Operators

        public static Vector3D operator +(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.Head + v2.Head);
        }

        public static Vector3D operator -(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.Head - v2.Head);
        }

        public static Vector3D operator *(Vector3D v, double scalar)
        {
            return new Vector3D(v.Head * scalar);
        }

        public static Vector3D operator *(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.Head * v2.Head);
        }

        public static Vector3D operator /(Vector3D v, double denominator)
        {
            return new Vector3D(v.Head / denominator);
        }

        public static Vector3D operator /(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.Head / v2.Head);
        }

        #endregion

        #region Overriden & Static methods

        public override string ToString()
        {
            return "(" + head.X + ", " + head.Y + ", " + head.Z + ")";
        }

        public static bool Equals(Vector3D v1, Vector3D v2)
        {
            if (v1 == null && v2 == null)
                return true;
            if (v1 == null || v2 == null)
                return false;
            return v1.Head == v2.Head;
        }

        #endregion
    }
}
