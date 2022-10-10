using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf3D.Objects;
using Wpf3D.Utilities;

namespace Wpf3D.UserControls
{
    public partial class Scene : UserControl
    {
        #region Constructor

        public Scene(double width = 800, double height = 400)
        {
            InitializeComponent();
            Width = width;
            Height = height;
            sceneWidth = 600;
            sceneLength = 600;
            sceneLocation = new Point3D(0, 0, 0);
            pointOfView = new Point3D(0, -300, 0);
            sceneZoom = 2d;
            lines = new List<Line3D>();
            planes = new List<Plane3D>();
            Invalidate();
        }

        #endregion

        #region Variables

        private double sceneZoom;
        private double sceneLength;
        private double sceneWidth;

        private Point3D sceneLocation;
        private Point3D pointOfView;

        private List<Line3D> gridLines;
        private List<Line3D> lines;
        private List<Plane3D> planes;

        private double sceneX_RotationAngle = 0;
        private double sceneY_RotationAngle = 0;
        private double sceneZ_RotationAngle = 0;

        private Plane3D gridPlane;

        #endregion

        #region Properties

        public double SceneZoom
        {
            get { return sceneZoom; }
            set { sceneZoom = value; Invalidate(); }
        }

        public double SceneWidth
        {
            get { return sceneWidth; }
            set { sceneWidth = value; Invalidate(); }
        }

        public double SceneLength
        {
            get { return sceneLength; }
            set { sceneLength = value; Invalidate(); }
        }

        public Point3D SceneLocation
        {
            get { return sceneLocation; }
            set { sceneLocation = value; Invalidate(); }
        }

        public Point3D PointOfView
        {
            get { return pointOfView; }
            set { pointOfView = value; Invalidate(); }
        }

        public List<Line3D> Lines
        {
            get { return lines; }
            set { lines = value; Invalidate(); }
        }

        public List<Plane3D> Planes
        {
            get { return planes; }
            set { planes = value; Invalidate(); }
        }

        #endregion

        #region Essential methods

        public PointF To2D(Point3D p)
        {
            Point3D p3d = new Point3D(p.X, p.Y, p.Z);
            p3d.Y += SceneLength / 2;
            double sign = p3d.Z < 0 ? 1 : -1;
            if (p3d.Y == 0d)
                p3d.Y++;
            if (p3d.Y + SceneLocation.Y == 0)
                SceneLocation.Y++;

            return new PointF
            (
                (float)(Width / 2 + SceneLocation.X + SceneZoom * p3d.X / (Math.Abs(p3d.Y + SceneLocation.Y + 200) / 220)),
                (float)(Height / 2 + sign * Math.Abs(SceneZoom * p3d.Z / ((p3d.Y + SceneLocation.Y + 200) / 220)))
            );
        }

        #endregion

        #region Scene rotations

        Point3D RotateAroundX(Point3D point)
        {
            Point3D p = new Point3D
            (
                point.X,
                point.Y * Math.Cos(sceneX_RotationAngle * (Math.PI / 180.0)) - point.Z * Math.Sin(sceneX_RotationAngle * (Math.PI / 180.0)),
                point.Y * Math.Sin(sceneX_RotationAngle * (Math.PI / 180.0)) + point.Z * Math.Cos(sceneX_RotationAngle * (Math.PI / 180.0))
            );
            return p;
        }

        Point3D RotateAroundY(Point3D point)
        {
            Point3D p = new Point3D
            (
                point.X * Math.Cos(sceneY_RotationAngle * (Math.PI / 180.0)) + point.Z * Math.Sin(sceneY_RotationAngle * (Math.PI / 180.0)),
                point.Y,
                -point.X * Math.Sin(sceneY_RotationAngle * (Math.PI / 180.0)) + point.Z * Math.Cos(sceneY_RotationAngle * (Math.PI / 180.0))
            );
            return p;
        }

        Point3D RotateAroundZ(Point3D point)
        {
            Point3D p = new Point3D
            (
                point.X * Math.Cos(sceneZ_RotationAngle * (Math.PI / 180.0)) - point.Y * Math.Sin(sceneZ_RotationAngle * (Math.PI / 180.0)),
                point.X * Math.Sin(sceneZ_RotationAngle * (Math.PI / 180.0)) + point.Y * Math.Cos(sceneZ_RotationAngle * (Math.PI / 180.0)),
                point.Z
            );
            return p;
        }

        public void RotateAroundX(double angle)
        {
            sceneX_RotationAngle = angle;
            Invalidate();
        }

        public void RotateAroundY(double angle)
        {
            sceneY_RotationAngle = angle;
            Invalidate();
        }

        public void RotateAroundZ(double angle)
        {
            sceneZ_RotationAngle = angle;
            Invalidate();
        }

        #endregion
         
        #region Line rotations

        public void RotateLineCenterX(double angle, Line3D line)
        {
            line.RotateAroundCenterX(angle, line.CoordinateSystem.X.ToVector3D());
            Invalidate();
        }

        public void RotateLineCenterY(double angle, Line3D line)
        {
            line.RotateAroundCenterY(angle, line.CoordinateSystem.Y.ToVector3D());
            Invalidate();
        }

        public void RotateLineCenterZ(double angle, Line3D line)
        {
            line.RotateAroundCenterZ(angle, line.CoordinateSystem.Z.ToVector3D());
            Invalidate();
        }

        public void RotateLineStartX(double angle, Line3D line)
        {
            line.RotateAroundStartX(angle, line.CoordinateSystem.X.ToVector3D());
            Invalidate();
        }

        public void RotateLineStartY(double angle, Line3D line)
        {
            line.RotateAroundStartY(angle, line.CoordinateSystem.Y.ToVector3D());
            Invalidate();
        }

        public void RotateLineStartZ(double angle, Line3D line)
        {
            line.RotateAroundStartZ(angle, line.CoordinateSystem.Z.ToVector3D());
            Invalidate();
        }

        #endregion

        #region Plane rotations

        public void RotatePlaneCenterX(double angle, Plane3D plane)
        {
            plane.RotateAroundCenterX(angle, plane.CoordinateSystem.X.ToVector3D());
            Invalidate();
        }

        public void RotatePlaneCenterY(double angle, Plane3D plane)
        {
            plane.RotateAroundCenterY(angle, plane.CoordinateSystem.Y.ToVector3D());
            Invalidate();
        }
         
        public void RotatePlaneCenterZ(double angle, Plane3D plane)
        {
            plane.RotateAroundCenterZ(angle, plane.CoordinateSystem.Z.ToVector3D());
            Invalidate();
        }

        #endregion

        #region Methods

        public void MoveSceneLeft()
        {
            SceneLocation.X -= 50;
            Invalidate();
        }

        public void MoveSceneRight()
        {
            SceneLocation.X += 50;
            Invalidate();
        }

        public void MoveSceneFar()
        {
            SceneLocation.Y += 50;
            Invalidate();
        }

        public void MoveSceneNear()
        {
            if (SceneLocation.Y > -200)
            {
                SceneLocation.Y -= 50;
                Invalidate();
            }
        }

        void AddGridLines()
        {
            Point3D a = null, b = null, c = null;
            gridLines = new List<Line3D>();
            for (double i = -SceneWidth / 2; i <= SceneWidth / 2; i += 50)
            {
                Point3D start = RotateAroundZ(new Point3D(i, -SceneLength / 2, SceneLocation.Z));
                start = RotateAroundY(start);
                start = RotateAroundX(start);
                Point3D end = RotateAroundZ(new Point3D(i, SceneLength / 2, SceneLocation.Z));
                end = RotateAroundY(end);
                end = RotateAroundX(end);
                gridLines.Add(new Line3D(start, end));

                if (i == -SceneWidth / 2) {
                    a = new Point3D(start.X, start.Y, start.Z);
                    b = new Point3D(end.X, end.Y, end.Z);
                }
                if (i == SceneWidth / 2)
                    c = new Point3D(end.X, end.Y, end.Z);
            }
            for (double i = -SceneLength / 2; i <= SceneLength / 2; i += 50)
            {
                Point3D start = RotateAroundZ(new Point3D(-SceneWidth / 2, i, SceneLocation.Z));
                start = RotateAroundY(start);
                start = RotateAroundX(start);
                Point3D end = RotateAroundZ(new Point3D(SceneWidth / 2, i, SceneLocation.Z));
                end = RotateAroundY(end);
                end = RotateAroundX(end);
                gridLines.Add(new Line3D(start, end));
            }

            gridPlane = new Plane3D(a, b, c);
        }

        public void Invalidate()
        {
            scenePanel.Children.Clear();
            DrawGrid();
            DrawLines();
            DrawPlanes();
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
        }

        private void DrawPlanes()
        {
            for (int i = 0; i < Planes.Count; i++)
            {
                Point3D p0 = RotateAroundZ(Planes[i].Points[0]);
                p0 = RotateAroundY(p0);
                p0 = RotateAroundX(p0);
                Point3D p1 = RotateAroundZ(Planes[i].Points[1]);
                p1 = RotateAroundY(p1);
                p1 = RotateAroundX(p1);
                Point3D p2 = RotateAroundZ(Planes[i].Points[2]);
                p2 = RotateAroundY(p2);
                p2 = RotateAroundX(p2);
                Point3D p3 = RotateAroundZ(Planes[i].Points[3]);
                p3 = RotateAroundY(p3);
                p3 = RotateAroundX(p3);

                PointF topLeft = To2D(p1);
                PointF topRight = To2D(p2);
                PointF bottomRight = To2D(p3);
                PointF bottomLeft = To2D(p0);

                Polygon rect = new Polygon();
                rect.Fill = Planes[i].Background;
                rect.HorizontalAlignment = HorizontalAlignment.Left;
                rect.VerticalAlignment = VerticalAlignment.Center;
                rect.Points = new PointCollection()
                {
                    new System.Windows.Point(topLeft.X, topLeft.Y),
                    new System.Windows.Point(topRight.X, topRight.Y),
                    new System.Windows.Point(bottomRight.X, bottomRight.Y),
                    new System.Windows.Point(bottomLeft.X, bottomLeft.Y)
                };
                scenePanel.Children.Add(rect);
            }
        }

        private void DrawLines()
        {
            for (int i = 0; i < Lines.Count; i++)
            {
                Point3D start = RotateAroundZ(Lines[i].Start);
                start = RotateAroundY(start);
                start = RotateAroundX(start);
                Point3D end = RotateAroundZ(Lines[i].End);
                end = RotateAroundY(end);
                end = RotateAroundX(end);

                Vector3D direction = new Line3D(start, end).ToVector3D().DirectionVector;
                int size;

                if (Math.Abs(direction.Head.X) == 1d)
                    size = Math.Abs((int)(end.X - start.X)) + 1;
                else if (Math.Abs(direction.Head.Y) == 1d)
                    size = Math.Abs((int)(end.Y - start.Y)) + 1;
                else
                    size = Math.Abs((int)(end.Z - start.Z)) + 1;

                bool drawn = false;
                for (double t = 1; t < size; t++)
                {
                    Point3D currentPoint = new Point3D(start.X + t * direction.Head.X,
                        start.Y + t * direction.Head.Y, start.Z + t * direction.Head.Z);

                    Point3D last = new Point3D(start.X + (t - 1) * direction.Head.X,
                        start.Y + (t - 1) * direction.Head.Y, start.Z + (t - 1) * direction.Head.Z);
                   
                    Vector3D rayDirection = new Vector3D(currentPoint - pointOfView).DirectionVector;
                    Ray3D ray = new Ray3D(rayDirection, pointOfView);
                    Point3D hit = ray.Intersects(gridPlane);
                    if (hit != null) 
                    {
                        double distanceToHit = pointOfView.Distance(hit);
                        double distanceToCurrentPoint = pointOfView.Distance(currentPoint);
                        if (distanceToCurrentPoint > distanceToHit)
                            continue;
                    }

                    PointF p1 = To2D(last);
                    PointF p2 = To2D(currentPoint);
                    Line l = new Line();
                    l.X1 = p1.X;
                    l.Y1 = p1.Y;
                    l.X2 = p2.X;
                    l.Y2 = p2.Y;
                    l.Stroke = Lines[i].Background;
                    l.Fill = Lines[i].Background;
                    scenePanel.Children.Add(l);
                    drawn = true;
                } 
                
                if (drawn)
                {
                    start = RotateAroundZ(Lines[i].CoordinateSystem.X.Start);
                    start = RotateAroundY(start);
                    start = RotateAroundX(start);
                    end = RotateAroundZ(Lines[i].CoordinateSystem.X.End);
                    end = RotateAroundY(end);
                    end = RotateAroundX(end);
                    PointF p1 = To2D(start);
                    PointF p2 = To2D(end);
                    Line l = new Line();
                    l.X1 = p1.X;
                    l.Y1 = p1.Y;
                    l.X2 = p2.X;
                    l.Y2 = p2.Y;
                    l.Stroke = Lines[i].CoordinateSystem.X.Background;
                    l.Fill = Lines[i].CoordinateSystem.X.Background;
                    scenePanel.Children.Add(l);

                    start = RotateAroundZ(Lines[i].CoordinateSystem.Y.Start);
                    start = RotateAroundY(start);
                    start = RotateAroundX(start);
                    end = RotateAroundZ(Lines[i].CoordinateSystem.Y.End);
                    end = RotateAroundY(end);
                    end = RotateAroundX(end);
                    p1 = To2D(start);
                    p2 = To2D(end);
                    l = new Line();
                    l.X1 = p1.X;
                    l.Y1 = p1.Y;
                    l.X2 = p2.X;
                    l.Y2 = p2.Y;
                    l.Stroke = Lines[i].CoordinateSystem.Y.Background;
                    l.Fill = Lines[i].CoordinateSystem.Y.Background;
                    scenePanel.Children.Add(l);

                    start = RotateAroundZ(Lines[i].CoordinateSystem.Z.Start);
                    start = RotateAroundY(start);
                    start = RotateAroundX(start);
                    end = RotateAroundZ(Lines[i].CoordinateSystem.Z.End);
                    end = RotateAroundY(end);
                    end = RotateAroundX(end);
                    p1 = To2D(start);
                    p2 = To2D(end);
                    l = new Line();
                    l.X1 = p1.X;
                    l.Y1 = p1.Y;
                    l.X2 = p2.X;
                    l.Y2 = p2.Y;
                    l.Stroke = Lines[i].CoordinateSystem.Z.Background;
                    l.Fill = Lines[i].CoordinateSystem.Z.Background;
                    scenePanel.Children.Add(l);
                }
            }
        }

        private void DrawGrid()
        {
            AddGridLines();
            foreach (Line3D line in gridLines)
            {
                PointF p1 = To2D(line.Start);
                PointF p2 = To2D(line.End);
                Line l = new Line();
                l.X1 = p1.X;
                l.Y1 = p1.Y;
                l.X2 = p2.X;
                l.Y2 = p2.Y;
                l.Stroke = Brushes.Black;
                l.Fill = Brushes.Black;
                scenePanel.Children.Add(l);
            }

            PointF a = To2D(gridPlane.Points[0]);
            PointF b = To2D(gridPlane.Points[1]);
            PointF c = To2D(gridPlane.Points[2]);
            PointF d = To2D(gridPlane.Points[3]);

            Polygon gridPolygon = new Polygon();
            gridPolygon.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(40, 255, 85, 85));
            gridPolygon.HorizontalAlignment = HorizontalAlignment.Left;
            gridPolygon.VerticalAlignment = VerticalAlignment.Center;
            gridPolygon.Points = new PointCollection() 
            {
                new System.Windows.Point(a.X, a.Y), new System.Windows.Point(b.X, b.Y),
                new System.Windows.Point(c.X, c.Y), new System.Windows.Point(d.X, d.Y)
            };
            scenePanel.Children.Add(gridPolygon);
        }

        #endregion

        #region Add Objects Methods

        public void AddLine(Line3D line)
        {
            Lines.Add(line);
            Invalidate();
        }

        public void AddPlane(Plane3D plane)
        {
            Planes.Add(plane); 
            Invalidate();
        }

        #endregion
    }
}
