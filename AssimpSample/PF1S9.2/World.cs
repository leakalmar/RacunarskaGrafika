// -----------------------------------------------------------------------
// <file>World.cs</file>
// <copyright>Grupa za Grafiku, Interakciju i Multimediju 2013.</copyright>
// <author>Srđan Mihić</author>
// <author>Aleksandar Josić</author>
// <summary>Klasa koja enkapsulira OpenGL programski kod.</summary>
// -----------------------------------------------------------------------
using System;
using Assimp;
using System.IO;
using System.Reflection;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Primitives;
using SharpGL.SceneGraph.Quadrics;
using SharpGL.SceneGraph.Core;
using SharpGL;
using System.Windows.Controls;

namespace AssimpSample
{


    /// <summary>
    ///  Klasa enkapsulira OpenGL kod i omogucava njegovo iscrtavanje i azuriranje.
    /// </summary>
    public class World : IDisposable
    {
        #region Atributi

        /// <summary>
        ///	 Scena koja se prikazuje.
        /// </summary>
        private AssimpScene m_scene_board;
        /// <summary>
        ///	 Scena koja se prikazuje.
        /// </summary>
        private AssimpScene m_scene_dart;

        /// <summary>
        ///	 Ugao rotacije sveta oko X ose.
        /// </summary>
        private float m_xRotation = 0.0f;

        /// <summary>
        ///	 Ugao rotacije sveta oko Y ose.
        /// </summary>
        private float m_yRotation = 0.0f;

        /// <summary>
        ///	 Udaljenost scene od kamere.
        /// </summary>
        private float m_sceneDistance = 50.0f;
        private readonly float m_xtranslate = 0.0f;
        private readonly float m_ytranslate = 0.0f;

        /// <summary>
        ///	 Sirina OpenGL kontrole u pikselima.
        /// </summary>
        private int m_width;

        /// <summary>
        ///	 Visina OpenGL kontrole u pikselima.
        /// </summary>
        private int m_height;

        #endregion Atributi

        #region Properties

        /// <summary>
        ///	 Scena koja se prikazuje.
        /// </summary>
        public AssimpScene SceneBoard
        {
            get { return m_scene_board; }
            set { m_scene_board = value; }
        }

        /// <summary>
        ///	 Scena koja se prikazuje.
        /// </summary>
        public AssimpScene SceneDart
        {
            get { return m_scene_dart; }
            set { m_scene_dart = value; }
        }

        /// <summary>
        ///	 Ugao rotacije sveta oko X ose.
        /// </summary>
        public float RotationX
        {
            get { return m_xRotation; }
            set { m_xRotation = value; }
        }

        /// <summary>
        ///	 Ugao rotacije sveta oko Y ose.
        /// </summary>
        public float RotationY
        {
            get { return m_yRotation; }
            set { m_yRotation = value; }
        }

        /// <summary>
        ///	 Udaljenost scene od kamere.
        /// </summary>
        public float SceneDistance
        {
            get { return m_sceneDistance; }
            set { m_sceneDistance = value; }
        }

        /// <summary>
        ///	 Sirina OpenGL kontrole u pikselima.
        /// </summary>
        public int Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        /// <summary>
        ///	 Visina OpenGL kontrole u pikselima.
        /// </summary>
        public int Height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        #endregion Properties

        #region Konstruktori

        /// <summary>
        ///  Konstruktor klase World.
        /// </summary>
        public World(String scenePathBoard, String sceneFileNameBoard, String scenePathDart, String sceneFileNameDart, int width, int height, OpenGL gl)
        {
            this.m_scene_board = new AssimpScene(scenePathBoard, sceneFileNameBoard, gl);
            this.m_scene_dart = new AssimpScene(scenePathDart, sceneFileNameDart, gl);
            this.m_width = width;
            this.m_height = height;
        }

        /// <summary>
        ///  Destruktor klase World.
        /// </summary>
        ~World()
        {
            this.Dispose(false);
        }

        #endregion Konstruktori

        #region Metode

        /// <summary>
        ///  Korisnicka inicijalizacija i podesavanje OpenGL parametara.
        /// </summary>
        public void Initialize(OpenGL gl)
        {
            gl.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            gl.Color(1f, 0f, 0f);
            // Model sencenja na flat (konstantno)
            //gl.ShadeModel(OpenGL.GL_FLAT);
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.Enable(OpenGL.GL_CULL_FACE);
            m_scene_board.LoadScene();
            m_scene_board.Initialize();
            m_scene_dart.LoadScene();
            m_scene_dart.Initialize();
        }

        /// <summary>
        /// Podesava viewport i projekciju za OpenGL kontrolu.
        /// </summary>
        public void Resize(OpenGL gl, int width, int height)
        {
            m_width = width;
            m_height = height;
            gl.Viewport(0, 0, width, height);
            gl.MatrixMode(OpenGL.GL_PROJECTION);      // selektuj Projection Matrix
            gl.LoadIdentity();
            gl.Perspective(m_sceneDistance, (double)width / height, 1f, 1000f);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();                // resetuj ModelView Matrix
        }

        /// <summary>
        ///  Iscrtavanje OpenGL kontrole.
        /// </summary>
        public void Draw(OpenGL gl)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            gl.PushMatrix();
            gl.MatrixMode(OpenGL.GL_MODELVIEW);

            //za celu scenu
            //translate za uvecavanje i smanjivanje
            gl.Translate(m_xtranslate, m_ytranslate, -m_sceneDistance);
            gl.Rotate(m_xRotation, 1.0f, 0.0f, 0.0f);
            gl.Rotate(m_yRotation, 0.0f, 1.0f, 0.0f);

            gl.PushMatrix();
            DrawDart(gl, 0);
            gl.PopMatrix();

            gl.PushMatrix();
            DrawDart(gl, 1);
            gl.PopMatrix();


            gl.PushMatrix();
            DrawDart(gl, 2);
            gl.PopMatrix();

            gl.PushMatrix();
            DrawBoard(gl);
            gl.PopMatrix();

            gl.PushMatrix();
            DrawFloor(gl);
            gl.PopMatrix();

            gl.PushMatrix();
            DrawWalls(gl, "LEFT");
            gl.PopMatrix();

            gl.PushMatrix();
            DrawWalls(gl, "RIGHT");
            gl.PopMatrix();

            gl.PushMatrix();
            DrawWalls(gl, "BACK");
            gl.PopMatrix();

            gl.PushMatrix();
            DrawWalls(gl, "FRONT");
            gl.PopMatrix();

            gl.PushMatrix();
            DrawPedestal(gl);
            gl.PopMatrix();

            gl.Viewport(m_width -230, 0, 230, m_height / 2);
            gl.PushMatrix();
            DrawInfo(gl);
            gl.PopMatrix();
            gl.Viewport(0, 0, m_width, m_height);

            gl.PopMatrix();
            gl.Flush();
        }

        public void DrawDart(OpenGL gl, int next)
        {
            gl.Translate(5f + next, 0f, 20f);
            gl.Rotate(90f, 1.0f, 0.0f, 0.0f);
            gl.Scale(0.2f, 0.2f, 0.2f);
            m_scene_dart.Draw();
        }

        public void DrawBoard(OpenGL gl)
        {
            gl.Translate(0f, 0f, -14f);
            gl.Scale(0.1f, 0.1f, 0.1f);
            m_scene_board.Draw();
        }

        public void DrawFloor(OpenGL gl)
        {
            gl.Color(0.91f, 0.76f, 0.65f);
            gl.Translate(0f, -10f, 0f);
            gl.Rotate(90f, 0f, 0f);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Vertex(10.0f,-20.0f);
            gl.Vertex(-10.0f, -20.0f);
            gl.Vertex(-10.0f, 20.0f);
            gl.Vertex(10.0f, 20.0f);
            gl.End();
        }

        public void DrawWalls(OpenGL gl, string wall)
        {
            switch (wall)
            {
                case "LEFT":
                    gl.Color(0.137f, 0.419f, 0.556f);
                    gl.Translate(-10f, 0f, 0f);
                    gl.Rotate(0f, -90f, 0f);
                    gl.Begin(OpenGL.GL_QUADS);
                    gl.Vertex(20.0f, -10.0f);
                    gl.Vertex(-20.0f, -10.0f);
                    gl.Vertex(-20.0f, 10.0f);
                    gl.Vertex(20.0f, 10.0f);
                    gl.End();
                    break;
                case "RIGHT":
                    gl.Color(0.137f, 0.419f, 0.556f);
                    gl.Translate(10f, 0f, 0f);
                    gl.Rotate(0f, 90f, 0f);
                    gl.Begin(OpenGL.GL_QUADS);
                    gl.Vertex(20.0f, -10.0f);
                    gl.Vertex(-20.0f, -10.0f);
                    gl.Vertex(-20.0f, 10.0f);
                    gl.Vertex(20.0f, 10.0f);
                    gl.End();
                    break;
                case "BACK":
                    gl.Color(0.137f, 0.419f, 0.556f);
                    gl.Translate(0f, 0f, -20f);
                    gl.Rotate(180f, 0f, -90f);
                    gl.Begin(OpenGL.GL_QUADS);
                    gl.Vertex(-10.0f, 10.0f);
                    gl.Vertex(10.0f, 10.0f);
                    gl.Vertex(10.0f, -10.0f);
                    gl.Vertex(-10.0f, -10.0f);
                    gl.End();
                    break;
                default:
                    gl.Color(0.137f, 0.419f, 0.556f);
                    gl.Translate(0f, 0f, 20f);
                    gl.Rotate(0f, 0f, -90f);
                    gl.Begin(OpenGL.GL_QUADS);
                    gl.Vertex(-10.0f, 10.0f);
                    gl.Vertex(10.0f, 10.0f);
                    gl.Vertex(10.0f, -10.0f);
                    gl.Vertex(-10.0f, -10.0f);
                    gl.End();
                    break;
            }
               

        }

        public void DrawPedestal(OpenGL gl)
        {
            gl.Color(0f, 0f, 0f);
            gl.Translate(0, -4f, -15f);
            gl.Scale(2f, 6f, 1f);
            Cube cube = new Cube();
            cube.Render(gl, RenderMode.Render);
        }

        public void DrawInfo(OpenGL gl)
        {
            gl.DrawText(0, 140, 1.0f, 0.0f, 0.0f, "Arial Bold", 14, "Predmet: Racunarska grafika");
            gl.DrawText(0, 110, 1.0f, 0.0f, 0.0f, "Arial Bold", 14, "Sk.god: 2021/22");
            gl.DrawText(0, 80, 1.0f, 0.0f, 0.0f, "Arial Bold", 14, "Ime: Lea");
            gl.DrawText(0, 50, 1.0f, 0.0f, 0.0f, "Arial Bold", 14, "Prezime: Kalmar");
            gl.DrawText(0, 20, 1.0f, 0.0f, 0.0f, "Arial Bold", 14, "Sifra zad.: 9.2");
        }

        /// <summary>
        ///  Implementacija IDisposable interfejsa.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_scene_board.Dispose();
                m_scene_dart.Dispose();
            }
        }

        #endregion Metode

        #region IDisposable metode

        /// <summary>
        ///  Dispose metoda.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable metode
    }
}
