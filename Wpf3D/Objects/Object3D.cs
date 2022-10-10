using System;
using System.Collections.Generic;
using System.Text;
using Wpf3D.Utilities;
using System.Drawing;
using System.Windows.Media;

namespace Wpf3D.Objects
{
    interface Object3D
    {
        public bool ContainsPoint(Point3D p);
        public Vector3D GetNormalVector();
        public SolidColorBrush Background { get; set; }
    }
}