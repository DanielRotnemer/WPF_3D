using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using Wpf3D.Objects;

namespace Wpf3D.Utilities
{
    public class CoordinateSystem
    {
        #region Constructor

        public CoordinateSystem(Line3D x, Line3D y, Line3D z, Point3D origin)
        {
            X = x;
            Y = y;
            Z = z;

            X.Background = Brushes.Green;
            Y.Background = Brushes.Red;
            Z.Background = Brushes.Black;

            Origin = origin;
        }

        #endregion

        #region Properties

        public Point3D Origin { get; set; }
        public Line3D X { get; set; }
        public Line3D Y { get; set; }
        public Line3D Z { get; set; }

        #endregion

        #region Overriden methods

        public override string ToString()
        {
            return X.Start.X + ", " + X.Start.Y + ", " + X.Start.Z;
        }

        #endregion
    }
}
