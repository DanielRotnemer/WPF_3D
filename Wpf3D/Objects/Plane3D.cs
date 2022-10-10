using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using Wpf3D.Utilities;

namespace Wpf3D.Objects
{
    public class Plane3D : Object3D
    {
        #region Private variables

        private Point3D[] points;

        private double xCenterAngle = 0;
        private double yCenterAngle = 0;
        private double zCenterAngle = 0;

        private CoordinateSystem coordinateSystem = null;

        private SolidColorBrush background = new SolidColorBrush(Color.FromArgb(255, 85, 255, 85));

        #endregion

        #region Constructors

        public Plane3D(Point3D a, Point3D b, Point3D c) { Initialize(a, b, c); }

        public Plane3D(Point3D center, double size)
        {
            Point3D a = new Point3D(center.X - size / 2, center.Y - size / 2, center.Z);
            Point3D b = new Point3D(center.X - size / 2, center.Y + size / 2, center.Z);
            Point3D c = new Point3D(center.X + size / 2, center.Y + size / 2, center.Z);
            Initialize(a, b, c);
        }

        #endregion

        #region Initializers

        private void Initialize(Point3D a, Point3D b, Point3D c)
        {
            Point3D diffAB = b - a;
            Point3D d = c - diffAB;

            points = new Point3D[] { a, b, c, d };

            if (!AB.ToVector3D().IsParallel(CD.ToVector3D(), false))
                return;

            Point3D origin = new Point3D((b.X + d.X) / 2, (b.Y + d.Y) / 2, (b.Z + d.Z) / 2);
            coordinateSystem = new CoordinateSystem
            (
                new Line3D(new Point3D(origin.X - 50d, origin.Y, origin.Z), new Point3D(origin.X + 50d, origin.Y, origin.Z)), // x
                new Line3D(new Point3D(origin.X, origin.Y - 50d, origin.Z), new Point3D(origin.X, origin.Y + 50d, origin.Z)), // y
                new Line3D(new Point3D(origin.X, origin.Y, origin.Z - 50d), new Point3D(origin.X, origin.Y, origin.Z + 50d)), // z
                origin
            );

            double angleSign = DA.ToVector3D().Normalize().Head.Z < 0 ? -1d : 1d;
            // here don't send the rotation axis to the rotate function because i want the values to remain the same
            coordinateSystem.Z.RotateAroundCenterX(angleSign * DA.ToVector3D().Angle(new Vector3D(DA.ToVector3D().Head.X, DA.ToVector3D().Head.Y, 0d)));
            coordinateSystem.Y.RotateAroundCenterX(angleSign * DA.ToVector3D().Angle(new Vector3D(DA.ToVector3D().Head.X, DA.ToVector3D().Head.Y, 0d)));

            angleSign = 1d;//PlaneLines[1].ToVector().Normalize().X <= 0d || PlaneLines[1].ToVector().Normalize().Z <= 0d ? 1d : -1d;
            coordinateSystem.Z.RotateAroundCenterY(angleSign * AB.ToVector3D().Angle(new Vector3D(AB.ToVector3D().Head.X, AB.ToVector3D().Head.Y, 0d)));
            coordinateSystem.X.RotateAroundCenterY(angleSign * AB.ToVector3D().Angle(new Vector3D(AB.ToVector3D().Head.X, AB.ToVector3D().Head.Y, 0d)));

            angleSign = DA.ToVector3D().Normalize().Head.X <= 0d ? 1d : -1d;
            Vector3D yAxis = new Vector3D(0d, 1d, 0d);
            coordinateSystem.Z.RotateAroundCenterZ(angleSign * new Vector3D(DA.ToVector3D().Head.X, DA.ToVector3D().Head.Y, 0d).Angle(yAxis));
            coordinateSystem.X.RotateAroundCenterZ(angleSign * new Vector3D(DA.ToVector3D().Head.X, DA.ToVector3D().Head.Y, 0d).Angle(yAxis));
            coordinateSystem.Y.RotateAroundCenterZ(angleSign * new Vector3D(DA.ToVector3D().Head.X, DA.ToVector3D().Head.Y, 0d).Angle(yAxis));
        }

        #endregion

        #region Properties

        public Line3D AB
        {
            get { return new Line3D(points[0], points[1]); }
        }

        public Line3D BC
        {
            get { return new Line3D(points[1], points[2]); }
        }

        public Line3D CD
        {
            get { return new Line3D(points[2], points[3]); }
        }

        public Line3D DA
        {
            get { return new Line3D(points[3], points[0]); }
        }

        public Point3D[] Points
        {
            get { return points; }
        }

        public CoordinateSystem CoordinateSystem
        {
            get { return coordinateSystem; }
        }

        public Point3D Center
        {
            get 
            {
                Point3D a = AB.Start;
                Point3D c = CD.Start;
                return new Point3D((a.X + c.X) / 2, (a.Y + c.Y) / 2, (a.Z + c.Z) / 2);
            }
        }

        public SolidColorBrush Background
        {
            get { return background; }
            set { background = value; }
        }

        #endregion

        #region Rotations

        public void RotateAroundCenterX(double angle, Vector3D axis = null, bool resetAngle = true)
        {
            /*da.RotateAroundCenterX(angle, coordinateSystem.X.ToVector3D());
            bc.RotateAroundCenterX(angle, coordinateSystem.X.ToVector3D());*/

            double tempAngle = resetAngle ? angle - xCenterAngle : angle;
            bool rotated = false;
            if (CoordinateSystem != null)
            {
                Point3D centerA = new Point3D((DA.Start.X + DA.End.X) / 2, (DA.Start.Y + DA.End.Y) / 2, (DA.Start.Z + DA.End.Z) / 2);
                Point3D centerC = new Point3D((BC.Start.X + BC.End.X) / 2, (BC.Start.Y + BC.End.Y) / 2, (BC.Start.Z + BC.End.Z) / 2);
                Vector3D n = /*axis*/CoordinateSystem.X.ToVector3D().Normalize();

                // rotate (end - center) of line a
                Vector3D vEnd = new Vector3D(DA.End.X - centerA.X, DA.End.Y - centerA.Y, DA.End.Z - centerA.Z);
                Vector3D crossEnd = n.CrossProduct(vEnd);
                crossEnd = crossEnd.Scale(Math.Sin(angle * (Math.PI / 180.0)));
                Vector3D cosV = vEnd.Scale(Math.Cos(angle * (Math.PI / 180.0)));
                points[0] = new Point3D(centerA.X + cosV.Head.X + crossEnd.Head.X, centerA.Y + cosV.Head.Y + crossEnd.Head.Y, centerA.Z + cosV.Head.Z + crossEnd.Head.Z);

                // rotate (start - center) of line a
                Vector3D vStart = new Vector3D(DA.Start.X - centerA.X, DA.Start.Y - centerA.Y, DA.Start.Z - centerA.Z);
                Vector3D crossStart = n.CrossProduct(vStart);
                crossStart = crossStart.Scale(Math.Sin(angle * (Math.PI / 180.0)));
                cosV = vStart.Scale(Math.Cos(angle * (Math.PI / 180.0)));
                points[3] = new Point3D(centerA.X + cosV.Head.X + crossStart.Head.X, centerA.Y + cosV.Head.Y + crossStart.Head.Y, centerA.Z + cosV.Head.Z + crossStart.Head.Z);

                // rotate (end - center) of line c
                vEnd = new Vector3D(BC.End.X - centerC.X, BC.End.Y - centerC.Y, BC.End.Z - centerC.Z);
                crossEnd = n.CrossProduct(vEnd);
                crossEnd = crossEnd.Scale(Math.Sin(-angle * (Math.PI / 180.0)));
                cosV = vEnd.Scale(Math.Cos(-angle * (Math.PI / 180.0)));
                points[2] = new Point3D(centerC.X + cosV.Head.X + crossEnd.Head.X, centerC.Y + cosV.Head.Y + crossEnd.Head.Y, centerC.Z + cosV.Head.Z + crossEnd.Head.Z);

                // rotate (start - center) of line c
                vStart = new Vector3D(BC.Start.X - centerC.X, BC.Start.Y - centerC.Y, BC.Start.Z - centerC.Z);
                crossStart = n.CrossProduct(vStart);
                crossStart = crossStart.Scale(Math.Sin(-angle * (Math.PI / 180.0)));
                cosV = vStart.Scale(Math.Cos(-angle * (Math.PI / 180.0)));
                points[1] = new Point3D(centerC.X + cosV.Head.X + crossStart.Head.X, centerC.Y + cosV.Head.Y + crossStart.Head.Y, centerC.Z + cosV.Head.Z + crossStart.Head.Z);

                // rotating the coordinate system
                CoordinateSystem.Z.RotateAroundCenterX(angle, /*axis*/CoordinateSystem.X.ToVector3D(), false);
                CoordinateSystem.Y.RotateAroundCenterX(angle, /*axis*/CoordinateSystem.X.ToVector3D(), false);

                xCenterAngle = angle;
            }
        }

        public void RotateAroundCenterY(double angle, Vector3D axis = null, bool resetAngle = true)
        {
            
        }

        public void RotateAroundCenterZ(double angle, Vector3D axis = null, bool resetAngle = true)
        {
            
        }

        #endregion

        #region Interface implementation

        public Vector3D GetNormalVector()
        {
            Vector3D abVector = AB.ToVector3D();
            Vector3D bcVector = BC.ToVector3D();
            return abVector.CrossProduct(bcVector);
        }

        public bool ContainsPoint(Point3D p)
        {
            Point3D a = AB.Start;
            Point3D c = BC.End;

            double highX_Bound = a.X > c.X ? a.X : c.X;
            double lowX_Bound = a.X > c.X ? c.X : a.X;
            double highY_Bound = a.Y > c.Y ? a.Y : c.Y;
            double lowY_Bound = a.Y > c.Y ? c.Y : a.Y;
            double highZ_Bound = a.Z > c.Z ? a.Z : c.Z;
            double lowZ_Bound = a.Z > c.Z ? c.Z : a.Z;

            return (p.X >= lowX_Bound && p.X <= highX_Bound &&
                p.Y >= lowY_Bound && p.Y <= highY_Bound &&
                p.Z >= lowZ_Bound && p.Z <= highZ_Bound);
        }

        #endregion
    }
}