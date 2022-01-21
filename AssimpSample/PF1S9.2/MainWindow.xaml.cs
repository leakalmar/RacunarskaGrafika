using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using SharpGL.SceneGraph;
using SharpGL;
using Microsoft.Win32;
using System.Globalization;

namespace AssimpSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Atributi

        /// <summary>
        ///	 Instanca OpenGL "sveta" - klase koja je zaduzena za iscrtavanje koriscenjem OpenGL-a.
        /// </summary>
        World m_world = null;

        #endregion Atributi

        #region Konstruktori

        public MainWindow()
        {
            // Inicijalizacija komponenti
            InitializeComponent();

            // Kreiranje OpenGL sveta
            try
            {
                m_world = new World(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "3D Models\\board"), "dartboard.obj", Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "3D Models\\dart"), "11750_throwing_dart_v1_L3.obj", (int)openGLControl.ActualWidth, (int)openGLControl.ActualHeight, openGLControl.OpenGL);
            }
            catch (Exception e)
            {
                MessageBox.Show("Neuspesno kreirana instanca OpenGL sveta. Poruka greške: " + e.Message, "Poruka", MessageBoxButton.OK);
                this.Close();
            }
        }

        #endregion Konstruktori

        /// <summary>
        /// Handles the OpenGLDraw event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        private void OpenGLControl_OpenGLDraw(object sender, OpenGLEventArgs args)
        {
            m_world.Draw(args.OpenGL);
        }

        /// <summary>
        /// Handles the OpenGLInitialized event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        private void OpenGLControl_OpenGLInitialized(object sender, OpenGLEventArgs args)
        {
            m_world.Initialize(args.OpenGL);
        }

        /// <summary>
        /// Handles the Resized event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        private void OpenGLControl_Resized(object sender, OpenGLEventArgs args)
        {
            m_world.Resize(args.OpenGL, (int)openGLControl.ActualWidth, (int)openGLControl.ActualHeight);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!m_world.Animation) { 
                switch (e.Key)
                {
                    case Key.F5: this.Close(); break;
                    case Key.T:
                        if (m_world.RotationX < 90)
                            m_world.RotationX += 5.0f; break;
                    case Key.G:
                        if (m_world.RotationX > 0)
                            m_world.RotationX -= 5.0f; break;
                    case Key.F: m_world.RotationY += 5.0f; break;
                    case Key.H: m_world.RotationY -= 5.0f; break;
                    case Key.Add: m_world.SceneDistance -= 10.0f; break;
                    case Key.Subtract: m_world.SceneDistance += 10.0f; break;
                    case Key.F2:
                        OpenFileDialog opfModel = new OpenFileDialog();
                        bool result = (bool)opfModel.ShowDialog();
                        if (result)
                        {

                            try
                            {
                                World newWorld = new World(Directory.GetParent(opfModel.FileName).ToString(), Path.GetFileName(opfModel.FileName), Directory.GetParent(opfModel.FileName).ToString(), Path.GetFileName(opfModel.FileName), (int)openGLControl.Width, (int)openGLControl.Height, openGLControl.OpenGL);
                                m_world.Dispose();
                                m_world = newWorld;
                                m_world.Initialize(openGLControl.OpenGL);
                            }
                            catch (Exception exp)
                            {
                                MessageBox.Show("Neuspesno kreirana instanca OpenGL sveta:\n" + exp.Message, "GRESKA", MessageBoxButton.OK);
                            }
                        }
                        break;
                    case Key.C:
                        m_world.Animation = true;
                        m_world.AnimationStart();
                        break;
                }
            }
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
           try
            {
                float red = float.Parse(this.red.Text, CultureInfo.InvariantCulture.NumberFormat);
                if (red < 0.0f || red > 1.0f)
                {
                    MessageBox.Show("Value RED must be a number between 0.0 and 1.0!");
                }
                else
                {
                    m_world.AmbientR = red;
                }

                float green = float.Parse(this.green.Text, CultureInfo.InvariantCulture.NumberFormat);
                if (green < 0.0f || green > 1.0f)
                {
                    MessageBox.Show("Value GREEN must be a number between 0.0 and 1.0!");
                }
                else
                {
                    m_world.AmbientG = green;
                }

                float blue = float.Parse(this.blue.Text, CultureInfo.InvariantCulture.NumberFormat);
                if (blue < 0.0f || blue > 1.0f)
                {
                    MessageBox.Show("Value BLUE must be a number between 0.0 and 1.0!");
                }
                else
                {
                    m_world.AmbientB = blue;
                }

            }
            catch
            {
                MessageBox.Show("Value of RED, GREEN and BLUE must be a number between 0.0 and 1.0!");
            }

            //Translate board
            try
            {
                float number = float.Parse(this.board_position.Text, CultureInfo.InvariantCulture.NumberFormat);
                if (number < -7.0 || number > 3.0)
                {
                    MessageBox.Show("Value must be between -7 and 3.");
                }
                else
                {
                    m_world.Board_position = float.Parse(this.board_position.Text);
                }
            }
            catch
            {
                MessageBox.Show("Translation must be a number!");
            }

            //Skale dart
            try
            {
                m_world.Dart_scale = float.Parse(this.scale_darts.Text, CultureInfo.InvariantCulture.NumberFormat);
            }
            catch
            {
                MessageBox.Show("Scaling must be a number!");
            }
        }
    }
}
