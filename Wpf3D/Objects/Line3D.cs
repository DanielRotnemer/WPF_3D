using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Wpf3D.Utilities;

namespace Wpf3D.Objects
{
    public class Line3D : Object3D
    {
        #region Private variables

        private Point3D start, end;

        private double xCenterAngle = 0;
        private double yCenterAngle = 0;
        private double zCenterAngle = 0;

        private double xStartAngle = 0;
        private double yStartAngle = 0;
        private double zStartAngle = 0;

        private SolidColorBrush background = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));

        private CoordinateSystem coordinateSystem = null;

        #endregion

        #region Cnstructors

        public Line3D(Point3D end)
        {
            start = new Point3D(0, 0, 0);
            this.end = end;
        }

        public Line3D(Point3D start, Point3D end, bool cs = false)
        {
            Start = start;
            End = end;  

            if (cs)
            {
                Point3D origin = start + (end - start) / 2;
                coordinateSystem = new CoordinateSystem
                (
                    new Line3D(new Point3D(origin.X - 50d, origin.Y, origin.Z), new Point3D(origin.X + 50d, origin.Y, origin.Z)), // x
                    new Line3D(new Point3D(origin.X, origin.Y - 50d, origin.Z), new Point3D(origin.X, origin.Y + 50d, origin.Z)), // y
                    new Line3D(new Point3D(origin.X, origin.Y, origin.Z - 50d), new Point3D(origin.X, origin.Y, origin.Z + 50d)), // z
                    origin
                );

                double angleSign = 1;
                // here don't send the rotation axis to the rotate function because i want the values to remain the same
                coordinateSystem.Z.RotateAroundCenterX(angleSign * ToVector3D().Angle(new Vector3D(ToVector3D().Head.X, ToVector3D().Head.Y, 0d)));
                coordinateSystem.Y.RotateAroundCenterX(angleSign * ToVector3D().Angle(new Vector3D(ToVector3D().Head.X, ToVector3D().Head.Y, 0d)));
                //Vector3D yAxis = new Vector3D(0d, Math.Sqrt(End.X * End.X + End.Y * End.Y), End.Z);
                Vector3D yAxis = new Vector3D(0d, 1d, 0d);
                angleSign = ToVector3D().Normalize().Head.X <= 0d ? 1d : -1d;
                coordinateSystem.Z.RotateAroundCenterZ(angleSign * new Vector3D(ToVector3D().Head.X, ToVector3D().Head.Y, 0d).Angle(yAxis));
                coordinateSystem.Y.RotateAroundCenterZ(angleSign * new Vector3D(ToVector3D().Head.X, ToVector3D().Head.Y, 0d).Angle(yAxis)); // don't send the rotation axis to the rotate function so z values remains the same, different in start and end
                coordinateSystem.X.RotateAroundCenterZ(angleSign * new Vector3D(ToVector3D().Head.X, ToVector3D().Head.Y, 0d).Angle(yAxis)); // don't send the rotation axis to the rotate function so z values remains the same, different in start and end
            }
        }

        #endregion

        #region Properties

        public Point3D Start
        {
            get { return start; }
            set { start = value; }
        }

        public Point3D End
        {
            get { return end; }
            set { end = value; }
        }

        public CoordinateSystem CoordinateSystem
        {
            get { return coordinateSystem; }
        }

        public Point3D Center
        {
            get
            {
                return new Point3D((Start.X + End.X) / 2, (Start.Y + End.Y) / 2, (Start.Z + End.Z) / 2);
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
            double tempAngle = resetAngle ? angle - xCenterAngle : angle;
            bool rotated = false;
            Point3D center = new Point3D((Start.X + End.X) / 2, (Start.Y + End.Y) / 2, (Start.Z + End.Z) / 2);
            if (axis != null)
            {
                if (!ToVector3D().IsParallel(axis))
                {
                    Vector3D n = axis.Normalize();
                    Vector3D vEnd = new Vector3D(End.X - center.X, End.Y - center.Y, End.Z - center.Z);
                    Vector3D crossEnd = n.CrossProduct(vEnd);
                    crossEnd = crossEnd.Scale(Math.Sin(tempAngle * (Math.PI / 180.0)));
                    Vector3D cosV = vEnd.Scale(Math.Cos(tempAngle * (Math.PI / 180.0)));
                    End = new Point3D(center.X + cosV.Head.X + crossEnd.Head.X, center.Y + cosV.Head.Y + crossEnd.Head.Y, center.Z + cosV.Head.Z + crossEnd.Head.Z);

                    Vector3D vStart = new Vector3D(Start.X - center.X, Start.Y - center.Y, Start.Z - center.Z);
                    Vector3D crossStart = n.CrossProduct(vStart);
                    crossStart = crossStart.Scale(Math.Sin(tempAngle * (Math.PI / 180.0)));
                    cosV = vStart.Scale(Math.Cos(tempAngle * (Math.PI / 180.0)));
                    Start = new Point3D(center.X + cosV.Head.X + crossStart.Head.X, center.Y + cosV.Head.Y + crossStart.Head.Y, center.Z + cosV.Head.Z + crossStart.Head.Z);

                    rotated = true;
                }
                else
                {

                }

                // rotate the coordinate system around a normal, send the normal to the function as paramter because the coordinate system don't have a cs.
                if (coordinateSystem != null)
                {
                    coordinateSystem.Y.RotateAroundCenterX(tempAngle, axis, false);
                    coordinateSystem.Z.RotateAroundCenterX(tempAngle, axis, false);
                    rotated = true;
                }
            }
            else
            {
                Start = new Point3D
                (
                    Start.X,
                    center.Y + (Start.Y - center.Y) * Math.Cos(tempAngle * (Math.PI / 180.0)) - (Start.Z - center.Z) * Math.Sin(tempAngle * (Math.PI / 180.0)),
                    center.Z + (Start.Y - center.Y) * Math.Sin(tempAngle * (Math.PI / 180.0)) + (Start.Z - center.Z) * Math.Cos(tempAngle * (Math.PI / 180.0))
                );
                End = new Point3D
                (
                    End.X,
                    center.Y + (End.Y - center.Y) * Math.Cos(tempAngle * (Math.PI / 180.0)) - (End.Z - center.Z) * Math.Sin(tempAngle * (Math.PI / 180.0)),
                    center.Z + (End.Y - center.Y) * Math.Sin(tempAngle * (Math.PI / 180.0)) + (End.Z - center.Z) * Math.Cos(tempAngle * (Math.PI / 180.0))
                );
                rotated = true;
            }
            if (rotated) { xCenterAngle = angle; }
        }

        public void RotateAroundCenterY(double angle, Vector3D axis = null, bool resetAngle = true)
        {
            double tempAngle = resetAngle ? angle - yCenterAngle : angle;
            bool rotated = false;
            Point3D center = new Point3D((Start.X + End.X) / 2, (Start.Y + End.Y) / 2, (Start.Z + End.Z) / 2);
            if (axis != null)
            {
                if (!ToVector3D().IsParallel(axis))
                {
                    Vector3D n = axis.Normalize();
                    Vector3D vEnd = new Vector3D(End.X - center.X, End.Y - center.Y, End.Z - center.Z);
                    Vector3D crossEnd = n.CrossProduct(vEnd);
                    crossEnd = crossEnd.Scale(Math.Sin(tempAngle * (Math.PI / 180.0)));
                    Vector3D cosV = vEnd.Scale(Math.Cos(tempAngle * (Math.PI / 180.0)));
                    End = new Point3D(center.X + cosV.Head.X + crossEnd.Head.X, center.Y + cosV.Head.Y + crossEnd.Head.Y, center.Z + cosV.Head.Z + crossEnd.Head.Z);

                    Vector3D vStart = new Vector3D(Start.X - center.X, Start.Y - center.Y, Start.Z - center.Z);
                    Vector3D crossStart = n.CrossProduct(vStart);
                    crossStart = crossStart.Scale(Math.Sin(tempAngle * (Math.PI / 180.0)));
                    cosV = vStart.Scale(Math.Cos(tempAngle * (Math.PI / 180.0)));
                    Start = new Point3D(center.X + cosV.Head.X + crossStart.Head.X, center.Y + cosV.Head.Y + crossStart.Head.Y, center.Z + cosV.Head.Z + crossStart.Head.Z);

                    rotated = true;
                }

                // rotate the coordinate system around a normal, send the normal to the function as paramter because the coordinate system don't have a cs.
                if (coordinateSystem != null)
                {
                    coordinateSystem.X.RotateAroundCenterY(tempAngle, axis, false);
                    coordinateSystem.Z.RotateAroundCenterY(tempAngle, axis, false);
                    rotated = true;
                }
            }
            else
            {
                Start = new Point3D
                (
                    center.X + (Start.X - center.X) * Math.Cos(tempAngle * (Math.PI / 180.0)) + (Start.Z - center.Z) * Math.Sin(tempAngle * (Math.PI / 180.0)),
                    Start.Y,
                    center.Z + (Start.X - center.X) * -1 * Math.Sin(tempAngle * (Math.PI / 180.0)) + (Start.Z - center.Z) * Math.Cos(tempAngle * (Math.PI / 180.0))
                );
                End = new Point3D
                (
                    center.X + (End.X - center.X) * Math.Cos(tempAngle * (Math.PI / 180.0)) + (End.Z - center.Z) * Math.Sin(tempAngle * (Math.PI / 180.0)),
                    End.Y,
                    center.Z + (End.X - center.X) * -1 * Math.Sin(tempAngle * (Math.PI / 180.0)) + (End.Z - center.Z) * Math.Cos(tempAngle * (Math.PI / 180.0))
                );
                rotated = true;
            }
            if (rotated) { yCenterAngle = angle; }
        }

        public void RotateAroundCenterZ(double angle, Vector3D axis = null, bool resetAngle = true)
        {
            double tempAngle = resetAngle ? angle - zCenterAngle : angle;
            bool rotated = false;
            Point3D center = new Point3D((Start.X + End.X) / 2, (Start.Y + End.Y) / 2, (Start.Z + End.Z) / 2);
            if (axis != null)
            {
                if (!ToVector3D().IsParallel(axis))
                {
                    Vector3D n = axis.Normalize();
                    Vector3D vEnd = new Vector3D(End.X - center.X, End.Y - center.Y, End.Z - center.Z);
                    Vector3D crossEnd = n.CrossProduct(vEnd);
                    crossEnd = crossEnd.Scale(Math.Sin(tempAngle * (Math.PI / 180.0)));
                    Vector3D cosV = vEnd.Scale(Math.Cos(tempAngle * (Math.PI / 180.0)));
                    End = new Point3D(center.X + cosV.Head.X + crossEnd.Head.X, center.Y + cosV.Head.Y + crossEnd.Head.Y, center.Z + cosV.Head.Z + crossEnd.Head.Z);

                    Vector3D vStart = new Vector3D(Start.X - center.X, Start.Y - center.Y, Start.Z - center.Z);
                    Vector3D crossStart = n.CrossProduct(vStart);
                    crossStart = crossStart.Scale(Math.Sin(tempAngle * (Math.PI / 180.0)));
                    cosV = vStart.Scale(Math.Cos(tempAngle * (Math.PI / 180.0)));
                    Start = new Point3D(center.X + cosV.Head.X + crossStart.Head.X, center.Y + cosV.Head.Y + crossStart.Head.Y, center.Z + cosV.Head.Z + crossStart.Head.Z);

                    rotated = true;
                }

                // rotate the coordinate system around a normal, send the normal to the function as paramter because the coordinate system don't have a cs.
                if (coordinateSystem != null)
                {
                    coordinateSystem.X.RotateAroundCenterZ(tempAngle, axis, false);
                    coordinateSystem.Y.RotateAroundCenterZ(tempAngle, axis, false);
                    rotated = true;
                }
            }
            else
            {
                Start = new Point3D
                (
                    center.X + (Start.X - center.X) * Math.Cos(tempAngle * (Math.PI / 180.0)) - (Start.Y - center.Y) * Math.Sin(tempAngle * (Math.PI / 180.0)),
                    center.Y + (Start.X - center.X) * Math.Sin(tempAngle * (Math.PI / 180.0)) + (Start.Y - center.Y) * Math.Cos(tempAngle * (Math.PI / 180.0)),
                    Start.Z
                );
                End = new Point3D
                (
                    center.X + (End.X - center.X) * Math.Cos(tempAngle * (Math.PI / 180.0)) - (End.Y - center.Y) * Math.Sin(tempAngle * (Math.PI / 180.0)),
                    center.Y + (End.X - center.X) * Math.Sin(tempAngle * (Math.PI / 180.0)) + (End.Y - center.Y) * Math.Cos(tempAngle * (Math.PI / 180.0)),
                    End.Z
                );
                rotated = true;
            }
            if (rotated) { zCenterAngle = angle; }
        }

        public void RotateAroundStartX(double angle, Vector3D axis = null, bool resetAngle = true)
        {
            double tempAngle = resetAngle ? angle - xStartAngle : angle;
            bool rotated = false;
            if (axis != null && !ToVector3D().IsParallel(axis))
            {                
                Vector3D n = axis.Normalize();
                Vector3D vEnd = new Vector3D(End.X - Start.X, End.Y - Start.Y, End.Z - Start.Z);
                Vector3D crossEnd = n.CrossProduct(vEnd);
                crossEnd = crossEnd.Scale(Math.Sin(tempAngle * (Math.PI / 180.0)));
                Vector3D cosV = vEnd.Scale(Math.Cos(tempAngle * (Math.PI / 180.0)));
                End = new Point3D(Start.X + cosV.Head.X + crossEnd.Head.X, Start.Y + cosV.Head.Y + crossEnd.Head.Y, Start.Z + cosV.Head.Z + crossEnd.Head.Z);

                // rotate the coordinate system around a normal, send the normal to the function as paramter because the coordinate system don't have a cs.
                if (coordinateSystem != null)
                {
                    vEnd = new Vector3D(coordinateSystem.Y.Start.X - Start.X, coordinateSystem.Y.Start.Y - Start.Y, coordinateSystem.Y.Start.Z - Start.Z);
                    crossEnd = n.CrossProduct(vEnd);
                    crossEnd = crossEnd.Scale(Math.Sin(tempAngle * (Math.PI / 180.0)));
                    cosV = vEnd.Scale(Math.Cos(tempAngle * (Math.PI / 180.0)));
                    coordinateSystem.Y.Start = new Point3D(Start.X + cosV.Head.X + crossEnd.Head.X, Start.Y + cosV.Head.Y + crossEnd.Head.Y, Start.Z + cosV.Head.Z + crossEnd.Head.Z);

                    vEnd = new Vector3D(coordinateSystem.Y.End.X - Start.X, coordinateSystem.Y.End.Y - Start.Y, coordinateSystem.Y.End.Z - Start.Z);
                    crossEnd = n.CrossProduct(vEnd);
                    crossEnd = crossEnd.Scale(Math.Sin(tempAngle * (Math.PI / 180.0)));
                    cosV = vEnd.Scale(Math.Cos(tempAngle * (Math.PI / 180.0)));
                    coordinateSystem.Y.End = new Point3D(Start.X + cosV.Head.X + crossEnd.Head.X, Start.Y + cosV.Head.Y + crossEnd.Head.Y, Start.Z + cosV.Head.Z + crossEnd.Head.Z);

                    // the point around which the x axis of the cs will be rotated
                    Point3D center = coordinateSystem.X.Start.Translate(coordinateSystem.Origin.X - Start.X, coordinateSystem.Origin.Y - Start.Y, coordinateSystem.Origin.Z - Start.Z, false);
                    vEnd = new Vector3D(coordinateSystem.X.Start.X - center.X, coordinateSystem.X.Start.Y - center.Y, coordinateSystem.X.Start.Z - center.Z);
                    crossEnd = n.CrossProduct(vEnd);
                    crossEnd = crossEnd.Scale(Math.Sin(tempAngle * (Math.PI / 180.0)));
                    cosV = vEnd.Scale(Math.Cos(tempAngle * (Math.PI / 180.0)));
                    coordinateSystem.X.Start = new Point3D(center.X + cosV.Head.X + crossEnd.Head.X, center.Y + cosV.Head.Y + crossEnd.Head.Y, center.Z + cosV.Head.Z + crossEnd.Head.Z);

                    center = coordinateSystem.X.End.Translate(coordinateSystem.Origin.X - Start.X, coordinateSystem.Origin.Y - Start.Y, coordinateSystem.Origin.Z - Start.Z, false);
                    vEnd = new Vector3D(coordinateSystem.X.End.X - center.X, coordinateSystem.X.End.Y - center.Y, coordinateSystem.X.End.Z - center.Z);
                    crossEnd = n.CrossProduct(vEnd);
                    crossEnd = crossEnd.Scale(Math.Sin(tempAngle * (Math.PI / 180.0)));
                    cosV = vEnd.Scale(Math.Cos(tempAngle * (Math.PI / 180.0)));
                    coordinateSystem.X.End = new Point3D(center.X + cosV.Head.X + crossEnd.Head.X, center.Y + cosV.Head.Y + crossEnd.Head.Y, center.Z + cosV.Head.Z + crossEnd.Head.Z);

                    vEnd = new Vector3D(coordinateSystem.Z.Start.X - Start.X, coordinateSystem.Z.Start.Y - Start.Y, coordinateSystem.Z.Start.Z - Start.Z);
                    crossEnd = n.CrossProduct(vEnd);
                    crossEnd = crossEnd.Scale(Math.Sin(tempAngle * (Math.PI / 180.0)));
                    cosV = vEnd.Scale(Math.Cos(tempAngle * (Math.PI / 180.0)));
                    coordinateSystem.Z.Start = new Point3D(Start.X + cosV.Head.X + crossEnd.Head.X, Start.Y + cosV.Head.Y + crossEnd.Head.Y, Start.Z + cosV.Head.Z + crossEnd.Head.Z);

                    vEnd = new Vector3D(coordinateSystem.Z.End.X - Start.X, coordinateSystem.Z.End.Y - Start.Y, coordinateSystem.Z.End.Z - Start.Z);
                    crossEnd = n.CrossProduct(vEnd);
                    crossEnd = crossEnd.Scale(Math.Sin(tempAngle * (Math.PI / 180.0)));
                    cosV = vEnd.Scale(Math.Cos(tempAngle * (Math.PI / 180.0)));
                    coordinateSystem.Z.End = new Point3D(Start.X + cosV.Head.X + crossEnd.Head.X, Start.Y + cosV.Head.Y + crossEnd.Head.Y, Start.Z + cosV.Head.Z + crossEnd.Head.Z);

                    coordinateSystem.Origin = new Point3D((Start.X + End.X) / 2, (Start.Y + End.Y) / 2, (Start.Z + End.Z) / 2);
                }
                rotated = true;
            }
            else if (axis == null)
            {
                End = new Point3D
                (
                    End.X,
                    Start.Y + (End.Y - Start.Y) * Math.Cos(tempAngle * (Math.PI / 180.0)) - (End.Z - Start.Z) * Math.Sin(tempAngle * (Math.PI / 180.0)),
                    Start.Z + (End.Y - Start.Y) * Math.Sin(tempAngle * (Math.PI / 180.0)) + (End.Z - Start.Z) * Math.Cos(tempAngle * (Math.PI / 180.0))
                );
                rotated = true;                
            }           
            if (rotated) { xStartAngle = angle; }
        }

        public void RotateAroundStartY(double angle, Vector3D axis = null, bool resetAngle = true)
        {
            double tempAngle = resetAngle ? angle - yStartAngle : angle;
            bool rotated = false;
            if (axis != null && !ToVector3D().IsParallel(axis))
            {
                Vector3D n = axis.Normalize();
                Vector3D vEnd = new Vector3D(End.X - Start.X, End.Y - Start.Y, End.Z - Start.Z);
                Vector3D crossEnd = n.CrossProduct(vEnd);
                crossEnd = crossEnd.Scale(Math.Sin(tempAngle * (Math.PI / 180.0)));
                Vector3D cosV = vEnd.Scale(Math.Cos(tempAngle * (Math.PI / 180.0)));
                End = new Point3D(Start.X + cosV.Head.X + crossEnd.Head.X, Start.Y + cosV.Head.Y + crossEnd.Head.Y, Start.Z + cosV.Head.Z + crossEnd.Head.Z);

                // rotate the coordinate system around a normal, send the normal to the function as paramter because the coordinate system don't have a cs.
                if (coordinateSystem != null)
                {
                    vEnd = new Vector3D(coordinateSystem.Y.Start.X - Start.X, coordinateSystem.Y.Start.Y - Start.Y, coordinateSystem.Y.Start.Z - Start.Z);
                    crossEnd = n.CrossProduct(vEnd);
                    crossEnd = crossEnd.Scale(Math.Sin(tempAngle * (Math.PI / 180.0)));
                    cosV = vEnd.Scale(Math.Cos(tempAngle * (Math.PI / 180.0)));
                    coordinateSystem.Y.Start = new Point3D(Start.X + cosV.Head.X + crossEnd.Head.X, Start.Y + cosV.Head.Y + crossEnd.Head.Y, Start.Z + cosV.Head.Z + crossEnd.Head.Z);

                    vEnd = new Vector3D(coordinateSystem.Y.End.X - Start.X, coordinateSystem.Y.End.Y - Start.Y, coordinateSystem.Y.End.Z - Start.Z);
                    crossEnd = n.CrossProduct(vEnd);
                    crossEnd = crossEnd.Scale(Math.Sin(tempAngle * (Math.PI / 180.0)));
                    cosV = vEnd.Scale(Math.Cos(tempAngle * (Math.PI / 180.0)));
                    coordinateSystem.Y.End = new Point3D(Start.X + cosV.Head.X + crossEnd.Head.X, Start.Y + cosV.Head.Y + crossEnd.Head.Y, Start.Z + cosV.Head.Z + crossEnd.Head.Z);

                    // the point around which the x axis of the sc will be rotated
                    Point3D center = coordinateSystem.X.Start.Translate(coordinateSystem.Origin.X - Start.X, coordinateSystem.Origin.Y - Start.Y, coordinateSystem.Origin.Z - Start.Z, false);
                    vEnd = new Vector3D(coordinateSystem.X.Start.X - center.X, coordinateSystem.X.Start.Y - center.Y, coordinateSystem.X.Start.Z - center.Z);
                    crossEnd = n.CrossProduct(vEnd);
                    crossEnd = crossEnd.Scale(Math.Sin(tempAngle * (Math.PI / 180.0)));
                    cosV = vEnd.Scale(Math.Cos(tempAngle * (Math.PI / 180.0)));
                    coordinateSystem.X.Start = new Point3D(center.X + cosV.Head.X + crossEnd.Head.X, center.Y + cosV.Head.Y + crossEnd.Head.Y, center.Z + cosV.Head.Z + crossEnd.Head.Z);

                    center = coordinateSystem.X.End.Translate(coordinateSystem.Origin.X - Start.X, coordinateSystem.Origin.Y - Start.Y, coordinateSystem.Origin.Z - Start.Z, false);
                    vEnd = new Vector3D(coordinateSystem.X.End.X - center.X, coordinateSystem.X.End.Y - center.Y, coordinateSystem.X.End.Z - center.Z);
                    crossEnd = n.CrossProduct(vEnd);
                    crossEnd = crossEnd.Scale(Math.Sin(tempAngle * (Math.PI / 180.0)));
                    cosV = vEnd.Scale(Math.Cos(tempAngle * (Math.PI / 180.0)));
                    coordinateSystem.X.End = new Point3D(center.X + cosV.Head.X + crossEnd.Head.X, center.Y + cosV.Head.Y + crossEnd.Head.Y, center.Z + cosV.Head.Z + crossEnd.Head.Z);

                    vEnd = new Vector3D(coordinateSystem.Z.Start.X - Start.X, coordinateSystem.Z.Start.Y - Start.Y, coordinateSystem.Z.Start.Z - Start.Z);
                    crossEnd = n.CrossProduct(vEnd);
                    crossEnd = crossEnd.Scale(Math.Sin(tempAngle * (Math.PI / 180.0)));
                    cosV = vEnd.Scale(Math.Cos(tempAngle * (Math.PI / 180.0)));
                    coordinateSystem.Z.Start = new Point3D(Start.X + cosV.Head.X + crossEnd.Head.X, Start.Y + cosV.Head.Y + crossEnd.Head.Y, Start.Z + cosV.Head.Z + crossEnd.Head.Z);

                    vEnd = new Vector3D(coordinateSystem.Z.End.X - Start.X, coordinateSystem.Z.End.Y - Start.Y, coordinateSystem.Z.End.Z - Start.Z);
                    crossEnd = n.CrossProduct(vEnd);
                    crossEnd = crossEnd.Scale(Math.Sin(tempAngle * (Math.PI / 180.0)));
                    cosV = vEnd.Scale(Math.Cos(tempAngle * (Math.PI / 180.0)));
                    coordinateSystem.Z.End = new Point3D(Start.X + cosV.Head.X + crossEnd.Head.X, Start.Y + cosV.Head.Y + crossEnd.Head.Y, Start.Z + cosV.Head.Z + crossEnd.Head.Z);

                    coordinateSystem.Origin = new Point3D((Start.X + End.X) / 2, (Start.Y + End.Y) / 2, (Start.Z + End.Z) / 2);
                }
                rotated = true;
            }
            else if (axis == null)
            {
                End = new Point3D
                (
                    Start.X + (End.X - Start.X) * Math.Cos(tempAngle * (Math.PI / 180.0)) + (End.Z - Start.Z) * Math.Sin(tempAngle * (Math.PI / 180.0)),
                    End.Y,
                    Start.Z + (End.X - Start.X) * -1 * Math.Sin(tempAngle * (Math.PI / 180.0)) + (End.Z - Start.Z) * Math.Cos(tempAngle * (Math.PI / 180.0))
                );
                rotated = true;
            }
            if (rotated) { yStartAngle = angle; }
        }

        public void RotateAroundStartZ(double angle, Vector3D axis = null, bool resetAngle = true)
        {
            double tempAngle = resetAngle ? angle - zStartAngle : angle;
            bool rotated = false;
            if (axis != null && !ToVector3D().IsParallel(axis))
            {
                Vector3D n = axis.Normalize();
                Vector3D vEnd = new Vector3D(End.X - Start.X, End.Y - Start.Y, End.Z - Start.Z);
                Vector3D crossEnd = n.CrossProduct(vEnd);
                crossEnd = crossEnd.Scale(Math.Sin(tempAngle * (Math.PI / 180.0)));
                Vector3D cosV = vEnd.Scale(Math.Cos(tempAngle * (Math.PI / 180.0)));
                End = new Point3D(Start.X + cosV.Head.X + crossEnd.Head.X, Start.Y + cosV.Head.Y + crossEnd.Head.Y, Start.Z + cosV.Head.Z + crossEnd.Head.Z);

                if (coordinateSystem != null)
                {
                    vEnd = new Vector3D(coordinateSystem.Y.Start.X - Start.X, coordinateSystem.Y.Start.Y - Start.Y, coordinateSystem.Y.Start.Z - Start.Z);
                    crossEnd = n.CrossProduct(vEnd);
                    crossEnd = crossEnd.Scale(Math.Sin(tempAngle * (Math.PI / 180.0)));
                    cosV = vEnd.Scale(Math.Cos(tempAngle * (Math.PI / 180.0)));
                    coordinateSystem.Y.Start = new Point3D(Start.X + cosV.Head.X + crossEnd.Head.X, Start.Y + cosV.Head.Y + crossEnd.Head.Y, Start.Z + cosV.Head.Z + crossEnd.Head.Z);

                    vEnd = new Vector3D(coordinateSystem.Y.End.X - Start.X, coordinateSystem.Y.End.Y - Start.Y, coordinateSystem.Y.End.Z - Start.Z);
                    crossEnd = n.CrossProduct(vEnd);
                    crossEnd = crossEnd.Scale(Math.Sin(tempAngle * (Math.PI / 180.0)));
                    cosV = vEnd.Scale(Math.Cos(tempAngle * (Math.PI / 180.0)));
                    coordinateSystem.Y.End = new Point3D(Start.X + cosV.Head.X + crossEnd.Head.X, Start.Y + cosV.Head.Y + crossEnd.Head.Y, Start.Z + cosV.Head.Z + crossEnd.Head.Z);

                    vEnd = new Vector3D(coordinateSystem.X.Start.X - Start.X, coordinateSystem.X.Start.Y - Start.Y, coordinateSystem.X.Start.Z - Start.Z);
                    crossEnd = n.CrossProduct(vEnd);
                    crossEnd = crossEnd.Scale(Math.Sin(tempAngle * (Math.PI / 180.0)));
                    cosV = vEnd.Scale(Math.Cos(tempAngle * (Math.PI / 180.0)));
                    coordinateSystem.X.Start = new Point3D(Start.X + cosV.Head.X + crossEnd.Head.X, Start.Y + cosV.Head.Y + crossEnd.Head.Y, Start.Z + cosV.Head.Z + crossEnd.Head.Z);

                    vEnd = new Vector3D(coordinateSystem.X.End.X - Start.X, coordinateSystem.X.End.Y - Start.Y, coordinateSystem.X.End.Z - Start.Z);
                    crossEnd = n.CrossProduct(vEnd);
                    crossEnd = crossEnd.Scale(Math.Sin(tempAngle * (Math.PI / 180.0)));
                    cosV = vEnd.Scale(Math.Cos(tempAngle * (Math.PI / 180.0)));
                    coordinateSystem.X.End = new Point3D(Start.X + cosV.Head.X + crossEnd.Head.X, Start.Y + cosV.Head.Y + crossEnd.Head.Y, Start.Z + cosV.Head.Z + crossEnd.Head.Z);

                    // the point around which the x axis of the sc will be rotated
                    Point3D center = coordinateSystem.Z.Start.Translate(coordinateSystem.Origin.X - Start.X, coordinateSystem.Origin.Y - Start.Y, coordinateSystem.Origin.Z - Start.Z, false);
                    vEnd = new Vector3D(coordinateSystem.Z.Start.X - center.X, coordinateSystem.Z.Start.Y - center.Y, coordinateSystem.Z.Start.Z - center.Z);
                    crossEnd = n.CrossProduct(vEnd);
                    crossEnd = crossEnd.Scale(Math.Sin(tempAngle * (Math.PI / 180.0)));
                    cosV = vEnd.Scale(Math.Cos(tempAngle * (Math.PI / 180.0)));
                    coordinateSystem.Z.Start = new Point3D(center.X + cosV.Head.X + crossEnd.Head.X, center.Y + cosV.Head.Y + crossEnd.Head.Y, center.Z + cosV.Head.Z + crossEnd.Head.Z);

                    center = coordinateSystem.Z.End.Translate(coordinateSystem.Origin.X - Start.X, coordinateSystem.Origin.Y - Start.Y, coordinateSystem.Origin.Z - Start.Z, false);
                    vEnd = new Vector3D(coordinateSystem.Z.End.X - center.X, coordinateSystem.Z.End.Y - center.Y, coordinateSystem.Z.End.Z - center.Z);
                    crossEnd = n.CrossProduct(vEnd);
                    crossEnd = crossEnd.Scale(Math.Sin(tempAngle * (Math.PI / 180.0)));
                    cosV = vEnd.Scale(Math.Cos(tempAngle * (Math.PI / 180.0)));
                    coordinateSystem.Z.End = new Point3D(center.X + cosV.Head.X + crossEnd.Head.X, center.Y + cosV.Head.Y + crossEnd.Head.Y, center.Z + cosV.Head.Z + crossEnd.Head.Z);

                    coordinateSystem.Origin = new Point3D((Start.X + End.X) / 2, (Start.Y + End.Y) / 2, (Start.Z + End.Z) / 2);
                }
                rotated = true;
            }
            else if (axis == null)
            {
                End = new Point3D
                (
                    Start.X + (End.X - Start.X) * Math.Cos(tempAngle * (Math.PI / 180.0)) - (End.Y - Start.Y) * Math.Sin(tempAngle * (Math.PI / 180.0)),
                    Start.Y + (End.X - Start.X) * Math.Sin(tempAngle * (Math.PI / 180.0)) + (End.Y - Start.Y) * Math.Cos(tempAngle * (Math.PI / 180.0)),
                    End.Z
                );
                rotated = true;
            }
            if (rotated) { zStartAngle = angle; }
        }

        #endregion

        #region Methods

        public Vector3D ToVector3D()
        {
            return new Vector3D(end.X - start.X, end.Y - start.Y, end.Z - start.Z);
        }

        #endregion

        #region Interface implementation

        public Vector3D GetNormalVector()
        {
            return null;
        }

        public bool ContainsPoint(Point3D p)
        {
            bool onLine = true;
            if (ToVector3D().UnitVector == new Vector3D(p).UnitVector)
            {
                if (start.X > end.X)
                    if (p.X > start.X || p.X < end.X) onLine = false;
                if (start.X < end.X)
                    if (p.X < start.X || p.X > end.X) onLine = false;

                if (start.Y > end.Y)
                    if (p.Y > start.Y || p.Y < end.Y) onLine = false;
                if (start.Y < end.Y)
                    if (p.Y < start.Y || p.Y > end.Y) onLine = false;

                if (start.Z > end.Z)
                    if (p.Z > start.Z || p.Z < end.Z) onLine = false;
                if (start.Z < end.Z)
                    if (p.Z < start.Z || p.Z > end.Z) onLine = false;
            }
            else
                onLine = false;
            return onLine;
        }

        #endregion
    }
}
