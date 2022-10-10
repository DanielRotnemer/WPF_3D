using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using Wpf3D.UserControls;
using Wpf3D.Utilities;

namespace Wpf3D
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables

        Scene scene;

        #endregion

        #region Constrructor

        public MainWindow()
        {
            InitializeComponent();
            scene = new Scene(Width - 200, Height);
            scenePanel.Children.Add(scene);
            sidebar.Height = Height;
            scene.Margin = new Thickness(200, 0, 0, 0);
        }

        #endregion

        #region Events Execution Data

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            scene.Width = Width - 200;
            scene.Height = Height;
            sidebar.Height = Height;
            scene.Margin = new Thickness(200, 0, 0, 0);
            scene.Invalidate();
        }

        private void RotateSceneZ_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double angle = rotateSceneZ_Slider.Value;
            scene.RotateAroundZ(angle);
        }

        private void RotateSceneY_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double angle = rotateSceneY_Slider.Value;
            scene.RotateAroundY(angle);
        }

        private void RotateSceneX_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double angle = rotateSceneX_Slider.Value;
            scene.RotateAroundX(angle);
        }

        private void AddLineButton_Click(object sender, RoutedEventArgs e)
        {
            Point3D start = new Point3D
            (
                Convert.ToDouble(lineStartX.Text),
                Convert.ToDouble(lineStartY.Text),
                Convert.ToDouble(lineStartZ.Text)
            );
            Point3D end = new Point3D
            (
                Convert.ToDouble(lineEndX.Text),
                Convert.ToDouble(lineEndY.Text),
                Convert.ToDouble(lineEndZ.Text)
            );
            Line3D line = new Line3D(start, end, true);
            line.Background = Brushes.Blue;
            scene.AddLine(line);
        }

        private void RotateLineAroundCenterZ_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            scene.RotateLineCenterZ(rotateLineAroundCenterZ.Value, scene.Lines[0]);
        }

        private void rotateLineAroundCenterY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            scene.RotateLineCenterY(rotateLineAroundCenterY.Value, scene.Lines[0]);
        }

        private void RotateLineAroundCenterX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            scene.RotateLineCenterX(rotateLineAroundCenterX.Value, scene.Lines[0]);
        }

        private void AddPlane_Click(object sender, RoutedEventArgs e)
        {
            Point3D a = new Point3D(Convert.ToDouble(Ax.Text), Convert.ToDouble(Ay.Text), Convert.ToDouble(Az.Text));
            Point3D b = new Point3D(Convert.ToDouble(Bx.Text), Convert.ToDouble(By.Text), Convert.ToDouble(Bz.Text));
            Point3D c = new Point3D(Convert.ToDouble(Cx.Text), Convert.ToDouble(Cy.Text), Convert.ToDouble(Cz.Text));
            Plane3D plane = new Plane3D(a, b, c);
            plane.Background = Brushes.Maroon;
            scene.AddPlane(plane);
        }

        private void ZoomScene_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            scene.SceneZoom = zoomScene.Value + 1;
        }

        private void RotateLineAroundStartX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double angle = rotateLineAroundStartX.Value;
            scene.RotateLineStartX(angle, scene.Lines[0]);
        }

        private void rotateLineAroundStartY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double angle = rotateLineAroundStartY.Value;
            scene.RotateLineStartY(angle, scene.Lines[0]);
        }

        private void RotateLineAroundStartZ_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double angle = rotateLineAroundStartZ.Value;
            scene.RotateLineStartZ(angle, scene.Lines[0]);
        }

        private void SceneRight_Click(object sender, RoutedEventArgs e)
        {
            scene.MoveSceneRight();
        }

        private void SceneLeft_Click(object sender, RoutedEventArgs e)
        {
            int h = 0;
            DateTime t = DateTime.Now;
            for (int i = 0; i < 10000; i++)
            {
                for (int j = 0; j < 10000; j++)
                {
                    h += i + j;
                }
            }
            DateTime s = DateTime.Now;

            Title = (s - t).Milliseconds.ToString();
            //scene.MoveSceneLeft();
        }

        #endregion
    }
}
