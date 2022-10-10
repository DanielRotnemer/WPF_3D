using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Wpf3D.Objects;

namespace Wpf3D.Utilities
{
    class Ray3D
    {
        #region Private variables

        private Point3D origin;
        private Vector3D direction;

        #endregion

        #region Constructor

        public Ray3D(Vector3D direction, Point3D origin)
        {
            this.direction = direction;
            this.origin = origin;
        }

        #endregion

        #region Properties

        public Vector3D Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public Point3D Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        #endregion

        #region Methods

        public Point3D Intersects(Plane3D plane) // send the plane's normal vector as a parameter to preven calculating it inside the function
        {
            double denom = plane.GetNormalVector().DotProduct(Direction);
            double epsilon = 0.0001d;
            if (Math.Abs(denom) > epsilon)
            {
                double t = new Vector3D(plane.Center - Origin).DotProduct(plane.GetNormalVector()) / denom;
                if (t >= epsilon)
                {
                    Point3D hit = new Point3D(origin.X + t * direction.Head.X, origin.Y + t * direction.Head.Y,
                        origin.Z + t * direction.Head.Z);
                    return hit;
                }
            }
            return null;
        }

        #endregion
    }
}
