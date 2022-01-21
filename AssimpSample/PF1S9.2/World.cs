// -----------------------------------------------------------------------
// <file>World.cs</file>
// <copyright>Grupa za Grafiku, Interakciju i Multimediju 2013.</copyright>
// <author>Srđan Mihić</author>
// <author>Aleksandar Josić</author>
// <summary>Klasa koja enkapsulira OpenGL programski kod.</summary>
// -----------------------------------------------------------------------
using System;
using SharpGL.SceneGraph.Primitives;
using SharpGL.SceneGraph.Quadrics;
using SharpGL.SceneGraph.Core;
using SharpGL;
using System.Windows.Threading;
using System.Drawing;
using System.Drawing.Imaging;

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

        private float m_dart1_position_x = 5f;
        private float m_dart2_position_x = 6f;
        private float m_dart3_position_x = 7f;
        private float m_dart1_position_y = 0f;
        private float m_dart2_position_y = 0f;
        private float m_dart3_position_y = 0f;
        private float m_dart1_position_z = 20f;
        private float m_dart2_position_z = 20f;
        private float m_dart3_position_z = 20f;
        private float m_dart1_rotation_x = 90f;
        private float m_dart2_rotation_x = 90f;
        private float m_dart3_rotation_x = 90f;
        private float m_dart1_rotation_y = 0f;
        private float m_dart2_rotation_y = 0f;
        private float m_dart3_rotation_y = 0f;
        private float m_dart1_rotation_z = 0f;
        private float m_dart2_rotation_z = 0f;
        private float m_dart3_rotation_z = 0f;
        private float m_dart_scale = 0.2f;
        private float m_board_scale = 0;
        private float m_board_position = 0f;
        private float m_ambientR = 0.5f;
        private float m_ambientG = 0f;
        private float m_ambientB = 0f;

        private DispatcherTimer timer1;
        private DispatcherTimer timer2;
        private DispatcherTimer timer3;
        private DispatcherTimer timer4;
        private DispatcherTimer timer5;


        private Sphere yellowLamp;

        private Sphere redLapmp;

        private uint[] m_textures;
        private string[] m_textureFiles = { "..//..//images//brick.jpg", "..//..//images//floor.jpg" , "..//..//images//pedestal.jpg" };

        private enum TextureObjects { Brick = 0, Floor, Pedestal };
        private readonly int m_textureCount = Enum.GetNames(typeof(TextureObjects)).Length;

        /// <summary>
        ///	 Sirina OpenGL kontrole u pikselima.
        /// </summary>
        private int m_width;

        /// <summary>
        ///	 Visina OpenGL kontrole u pikselima.
        /// </summary>
        private int m_height;

        private bool animation;

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

        public bool Animation
        {
            get { return animation; }
            set { animation = value; }
        }
        public float Dart_scale
        {
            get { return m_dart_scale; }
            set { m_dart_scale = value; }
        }
        public float Board_position
        {
            get { return m_board_position; }
            set { m_board_position = value; }
        }
        public float AmbientR
        {
            get { return m_ambientR; }
            set { m_ambientR = value; }
        }
        public float AmbientG
        {
            get { return m_ambientG; }
            set { m_ambientG = value; }
        }
        public float AmbientB
        {
            get { return m_ambientB; }
            set { m_ambientB = value; }
        }
        #endregion Properties

        #region Konstruktori

        /// <summary>
        ///  Konstruktor klase World.
        /// </summary>
        public World(String scenePathBoard, String sceneFileNameBoard, String scenePathDart, String sceneFileNameDart, int width, int height, OpenGL gl)
        {
            m_textures = new uint[m_textureCount];
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
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.Enable(OpenGL.GL_CULL_FACE);
            gl.ShadeModel(OpenGL.GL_FLAT);


            gl.Enable(OpenGL.GL_COLOR_MATERIAL);
            gl.ColorMaterial(OpenGL.GL_FRONT, OpenGL.GL_AMBIENT_AND_DIFFUSE);

            SetUpLighting(gl);


            yellowLamp = new Sphere();
            yellowLamp.CreateInContext(gl);
            yellowLamp.Radius = 0.5f;
            yellowLamp.Material = new SharpGL.SceneGraph.Assets.Material();


            redLapmp = new Sphere();
            redLapmp.CreateInContext(gl);
            redLapmp.Radius = 0.5f;
            redLapmp.Material = new SharpGL.SceneGraph.Assets.Material();

            // Teksture se primenjuju sa parametrom decal
             gl.Enable(OpenGL.GL_TEXTURE_2D);
             gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_DECAL);

             //Ucitaj slike i kreiraj teksture
             gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_LINEAR_MIPMAP_LINEAR);  // Linear mipmap Filtering
             gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_LINEAR_MIPMAP_LINEAR);  // Linear mipmap Filtering
             gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, OpenGL.GL_REPEAT);
             gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, OpenGL.GL_REPEAT);

            gl.GenTextures(m_textureCount, m_textures);
            for (int i = 0; i < m_textureCount; ++i)
            {
                // Pridruzi teksturu odgovarajucem identifikatoru
                gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[i]);

                // Ucitaj sliku i podesi parametre teksture
                Bitmap image = new Bitmap(m_textureFiles[i]);
                // rotiramo sliku zbog koordinantog sistema opengl-a
                image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
                // RGBA format (dozvoljena providnost slike tj. alfa kanal)
                BitmapData imageData = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly,
                                                      System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                gl.Build2DMipmaps(OpenGL.GL_TEXTURE_2D, (int)OpenGL.GL_RGBA8, image.Width, image.Height, OpenGL.GL_BGRA, OpenGL.GL_UNSIGNED_BYTE, imageData.Scan0);

                image.UnlockBits(imageData);
                image.Dispose();
            }

            gl.Disable(OpenGL.GL_TEXTURE_2D);

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

        public void SetUpLighting(OpenGL gl)
        {
            // tackasto zuto osvetljenje
            float[] light0pos = new float[] { 0f, 0f, 0f, 1.0f };
            float[] light0ambient = new float[] { 0.8f, 0.8f, 0.5f, 1.0f };
            float[] light0diffuse = new float[] { 1f, 1f, 0.5f, 1.0f };

            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, light0pos);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, light0ambient);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, light0diffuse);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPOT_CUTOFF, 180.0f);  // za tackasto osvetljenje

            
            float[] direction = new float[] { 1.5f, 0.3f, -43.2f, 1.0f };
            float[] light1pos = new float[] { 0, 0, 0 };
            float[] light1ambient = new float[] { 0.4f, 0f, 0f, 1.0f };
            float[] light1diffuse = new float[] { 0.5f, 0f, 0f, 1.0f };

            // crveno svetlo 1
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_POSITION, light1pos);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_AMBIENT, light1ambient);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_DIFFUSE, light1diffuse);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPOT_CUTOFF, 30.0f);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPOT_EXPONENT, 5.0f);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPOT_DIRECTION, direction);

            
            gl.Enable(OpenGL.GL_LIGHTING);
            gl.Enable(OpenGL.GL_LIGHT0);
            gl.Enable(OpenGL.GL_LIGHT1);

            gl.Enable(OpenGL.GL_NORMALIZE);

        }

        /// <summary>
        ///  Iscrtavanje OpenGL kontrole.
        /// </summary>
        public void Draw(OpenGL gl)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            gl.LoadIdentity();

            gl.PushMatrix();
            float[] pos = new float[] { 0f, 10f, 10.0f, 1.0f };
            gl.Color(0.8f, 0.8f, 0.5f);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, pos);
            gl.Translate(pos[0], pos[1], pos[2]);
            gl.PopMatrix();

            gl.PushMatrix();
            float[] pos2 = new float[] { 0f, 4f, 1.0f, 1.0f };
            float[] dir = new float[] { 1f, 0f, 0f, 1f };
            float[] ambient = new float[] { m_ambientR, m_ambientG, m_ambientB, 1.0f };
            gl.Color(m_ambientR, m_ambientG, m_ambientB);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_POSITION, pos2);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPOT_DIRECTION, dir);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_AMBIENT, ambient);
            gl.Translate(pos[0], pos[1], pos[2]);
            gl.PopMatrix();

            gl.LookAt(0f, 0f, -20f, 0f, 0f, -50f, 0f, 1f, 0f);

            gl.PushMatrix();
            gl.MatrixMode(OpenGL.GL_MODELVIEW);

            //za celu scenu
            //translate za uvecavanje i smanjivanje
            gl.Translate(m_xtranslate, m_ytranslate, -m_sceneDistance);
            gl.Rotate(m_xRotation, 1.0f, 0.0f, 0.0f);
            gl.Rotate(m_yRotation, 0.0f, 1.0f, 0.0f);

            gl.PushMatrix();
            DrawYellowLight(gl);
            gl.PopMatrix();

            gl.PushMatrix();
            DrawRedLight(gl);
            gl.PopMatrix();

            gl.PushMatrix();
            DrawDart(gl, m_dart1_position_x, m_dart1_position_y, m_dart1_position_z, m_dart1_rotation_x, m_dart1_rotation_y, m_dart1_rotation_z, m_dart_scale);
            gl.PopMatrix();

            gl.PushMatrix();
            DrawDart(gl, m_dart2_position_x, m_dart2_position_y, m_dart2_position_z, m_dart2_rotation_x, m_dart2_rotation_y, m_dart2_rotation_z, m_dart_scale);
            gl.PopMatrix();


            gl.PushMatrix();
            DrawDart(gl, m_dart3_position_x, m_dart3_position_y, m_dart3_position_z, m_dart3_rotation_x, m_dart3_rotation_y, m_dart3_rotation_z, m_dart_scale);
            gl.PopMatrix();

            gl.PushMatrix();
            DrawBoard(gl, m_board_scale);
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

        #region Draw

        public void DrawYellowLight(OpenGL gl)
        {

            float[] pos = new float[] { 0f, 10f, 10.0f, 1.0f };
            float[] ambient = new float[] { 0.8f, 0.8f, 0.5f, 1.0f };
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, ambient);
            gl.Color(0.8f, 0.8f, 0.5f);
            gl.Translate(pos[0], pos[1], pos[2]);
            yellowLamp.Material.Bind(gl);
            yellowLamp.Render(gl, RenderMode.Render);
        }

        public void DrawRedLight(OpenGL gl)
        {
            float[] pos = new float[] { 0f, 4f, 1.0f, 1.0f };
            float[] dir = new float[] {-1f, 0f, 0f, 1f };
            float[] ambient = new float[] { m_ambientR, m_ambientG, m_ambientB,0.5f };
            gl.Color(m_ambientR, m_ambientG, m_ambientB);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_POSITION, pos);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPOT_DIRECTION, dir);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_AMBIENT, ambient);
            gl.Translate(pos[0], pos[1], pos[2]);
            redLapmp.Material.Bind(gl);
            redLapmp.Render(gl, RenderMode.Render);
        }

        public void DrawDart(OpenGL gl, float x, float y, float z, float rotation_x, float rotation_y, float rotation_z, float scale)
        {
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_MODULATE);
            gl.Translate(x, y, z);
            gl.Rotate(rotation_x, 1.0f, 0.0f, 0.0f);
            gl.Rotate(rotation_y, 0.0f, 1.0f, 0.0f);
            gl.Rotate(rotation_z, 0.0f, 0.0f, 1.0f);
            gl.Scale(scale, scale, scale);
            m_scene_dart.Draw();
            gl.Disable(OpenGL.GL_TEXTURE_2D);
        }

        public void DrawBoard(OpenGL gl, float scale)
        {
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_MODULATE);
            gl.Translate(0f, m_board_position, 1f);
            gl.Scale(scale*0.1f + 0.1f, scale * 0.1f + 0.1f, scale * 0.1f + 0.1f);
            m_scene_board.Draw();
            gl.Disable(OpenGL.GL_TEXTURE_2D);
        }

        public void DrawFloor(OpenGL gl)
        {
            gl.Color(0.91f, 0.76f, 0.65f);
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.MatrixMode(OpenGL.GL_TEXTURE);
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Floor]);
            gl.LoadIdentity();
            gl.Scale(5, 5, 5);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.Translate(0f, -10f, 0f);
            gl.Rotate(90f, 0f, 0f);
            gl.Begin(OpenGL.GL_QUADS);
                gl.Normal(0f, 1f, 0f);
                gl.TexCoord(1.0f, 1.0f);
                gl.Vertex(10.0f,-5.0f); //top right
                gl.TexCoord(0.0f, 0.0f);
                gl.Vertex(-10.0f, -5.0f); // top left
                gl.TexCoord(0.0f, 1.0f);
                gl.Vertex(-10.0f, 20.0f); //bottom left
                gl.TexCoord(1.0f, 0.0f);
                gl.Vertex(10.0f, 20.0f); //bottom right
            gl.End();

            gl.MatrixMode(OpenGL.GL_TEXTURE);
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_MODULATE);
            gl.LoadIdentity();
            gl.Scale(1, 1, 1);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);

            gl.Disable(OpenGL.GL_TEXTURE_2D);
        }

        public void DrawWalls(OpenGL gl, string wall)
        {
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Brick]);
            switch (wall)
            {
                case "LEFT":
                    gl.Translate(-10f, 0f, 0f);
                    gl.Rotate(0f, -90f, 0f);
                    gl.Begin(OpenGL.GL_QUADS);
                        gl.Normal(1f, 0f, 0f);
                        gl.TexCoord(0.0f, 0.0f);
                        gl.Vertex(20.0f, -10.0f); //bottom left
                        gl.TexCoord(1.0f, 0.0f);
                        gl.Vertex(-5.0f, -10.0f); //bottom right
                        gl.TexCoord(1.0f, 1.0f);
                        gl.Vertex(-5.0f, 10.0f);  //top right
                        gl.TexCoord(0.0f, 1.0f);
                        gl.Vertex(20.0f, 10.0f);  //top left
                    gl.End();
                    break;
                case "RIGHT":
                    gl.Translate(10f, 0f, 0f);
                    gl.Rotate(0f, 90f, 0f);
                    gl.Begin(OpenGL.GL_QUADS);
                        gl.Normal(1f, 0f, 0f);
                        gl.TexCoord(0.0f, 0.0f);
                        gl.Vertex(5.0f, -10.0f); //bottom right
                        gl.TexCoord(1.0f, 0.0f);
                        gl.Vertex(-20.0f, -10.0f); //top right
                        gl.TexCoord(1.0f, 1.0f);
                        gl.Vertex(-20.0f, 10.0f);//top left
                        gl.TexCoord(0.0f, 1.0f);
                        gl.Vertex(5.0f, 10.0f);//bottom left
                    gl.End();
                    break;
                case "BACK":
                    gl.Translate(0f, 0f, -5f);
                    gl.Rotate(180f, 0f, -90f);
                    gl.Begin(OpenGL.GL_QUADS);
                        gl.Normal(0f, 0f, 1f);
                        gl.TexCoord(0.0f, 0.0f);
                        gl.Vertex(-10.0f, 10.0f); //bottom left
                        gl.TexCoord(0.0f, 1.0f);
                        gl.Vertex(10.0f, 10.0f); //bottom right
                        gl.TexCoord(1.0f, 1.0f);
                        gl.Vertex(10.0f, -10.0f); //top right
                        gl.TexCoord(1.0f, 0.0f);
                        gl.Vertex(-10.0f, -10.0f); //top left
                    gl.End();
                    break;
                default:
                    gl.Translate(0f, 0f, 20f);
                    gl.Rotate(0f, 0f, -90f);
                    gl.Begin(OpenGL.GL_QUADS);
                        gl.Normal(0f, 0f, 1f);
                        gl.TexCoord(1.0f, 1.0f);
                        gl.Vertex(-10.0f, 10.0f);//top left
                        gl.TexCoord(1.0f, 0.0f);
                        gl.Vertex(10.0f, 10.0f);//bottom left
                        gl.TexCoord(0.0f, 0.0f);
                        gl.Vertex(10.0f, -10.0f);//bottom right
                        gl.TexCoord(0.0f, 1.0f);
                        gl.Vertex(-10.0f, -10.0f);//top right
                    gl.End();
                    break;
            }

            gl.Disable(OpenGL.GL_TEXTURE_2D);

        }

        public void DrawPedestal(OpenGL gl)
        {
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Pedestal]);
            gl.Translate(0, -4f, 0f);
            gl.Scale(4f, 9f, 1f);
            Cube cube = new Cube();
            cube.Render(gl, RenderMode.Render);
            gl.Disable(OpenGL.GL_TEXTURE_2D);
        }

        public void DrawInfo(OpenGL gl)
        {
            gl.DrawText3D("Arial Bold", 14, 0, 0, "");
            gl.DrawText(0, 140, 1.0f, 0.0f, 0.0f, "Arial Bold", 14, "Predmet: Racunarska grafika");
            gl.DrawText(0, 110, 1.0f, 0.0f, 0.0f, "Arial Bold", 14, "Sk.god: 2021/22");
            gl.DrawText(0, 80, 1.0f, 0.0f, 0.0f, "Arial Bold", 14, "Ime: Lea");
            gl.DrawText(0, 50, 1.0f, 0.0f, 0.0f, "Arial Bold", 14, "Prezime: Kalmar");
            gl.DrawText(0, 20, 1.0f, 0.0f, 0.0f, "Arial Bold", 14, "Sifra zad.: 9.2");
        }
        #endregion

        #region Animation
        public void AnimationStart()
        {
            m_dart1_position_x = 5f;
            m_dart2_position_x = 6f;
            m_dart3_position_x = 7f;
            m_dart1_position_y = 0f;
            m_dart2_position_y = 0f;
            m_dart3_position_y = 0f;
            m_dart1_position_z = 20f;
            m_dart2_position_z = 20f;
            m_dart3_position_z = 20f;
            m_dart1_rotation_x = 90f;
            m_dart2_rotation_x = 90f;
            m_dart3_rotation_x = 90f;
            m_dart1_rotation_y = 0f;
            m_dart2_rotation_y = 0f;
            m_dart3_rotation_y = 0f;
            m_dart1_rotation_z = 0f;
            m_dart2_rotation_z = 0f;
            m_dart3_rotation_z = 0f;
            m_dart_scale = 0.2f;
            m_board_scale = 0f;
            m_board_position = 0f;
            timer1 = new DispatcherTimer();
            timer1.Interval = TimeSpan.FromMilliseconds(10);
            timer1.Tick += new EventHandler(ThrowDart);
            timer1.Start();
        }

        public void ThrowDart(object sender, EventArgs e)
        {
            if (m_dart1_position_x > 0)
            {
                m_dart1_position_x -= 0.1f;
                if(m_dart1_rotation_x > 20)
                {
                    m_dart1_rotation_x -= 1f;
                }
            }else if (m_dart1_rotation_x > 20)
            {
                m_dart1_rotation_x -= 1f;
            }else if(m_dart1_position_z > 1f)
            {
                timer1.Interval = TimeSpan.FromMilliseconds(0.3);
                if (m_dart1_rotation_x > 0)
                {
                    m_dart1_rotation_x -= 0.2f;
                }
                m_dart1_position_z -= 0.1f;
                m_dart1_position_x -= 0.015f;
                m_dart1_rotation_x -= 0.03f;
                m_dart1_position_y -= 0.005f;
            }
            else
            {
                timer1.Stop();
                SecondDart();
            }
        }

        public void SecondDart()
        {
            timer2 = new DispatcherTimer();
            timer2.Interval = TimeSpan.FromMilliseconds(10);
            timer2.Tick += new EventHandler(ThrowDart2);
            timer2.Start();
        }

        public void ThrowDart2(object sender, EventArgs e)
        {
            if (m_dart2_position_x > 0)
            {
                m_dart2_position_x -= 0.1f;
                if (m_dart2_rotation_x > 20)
                {
                    m_dart2_rotation_x -= 1f;
                }
            }
            else if (m_dart2_rotation_x > 20)
            {
                m_dart2_rotation_x -= 1f;
            }
            else if (m_dart2_position_z > 1f)
            {
                timer2.Interval = TimeSpan.FromMilliseconds(0.3);
                if (m_dart2_rotation_x > 0)
                {
                    m_dart2_rotation_x -= 0.2f;
                }
                m_dart2_position_z -= 0.1f;
                m_dart2_position_x += 0.5f;
                m_dart2_rotation_x -= 0.03f;
                m_dart2_position_y -= 0.015f;
            }
            else
            {
                timer2.Stop();
                ScaleBoard();
            }
        }
        public void ScaleBoard()
        {
            timer3 = new DispatcherTimer();
            timer3.Interval = TimeSpan.FromMilliseconds(10);
            timer3.Tick += new EventHandler(ScaleBoardEv);
            timer3.Start();
        }

        public void ScaleBoardEv(object sender, EventArgs e)
        {
            if (m_board_scale < 2)
            {
                m_board_scale += 0.2f;
            }
            else
            {
                timer3.Stop();
                ThirdDart();
            }
        }
        public void ThirdDart()
        {
            timer4 = new DispatcherTimer();
            timer4.Interval = TimeSpan.FromMilliseconds(10);
            timer4.Tick += new EventHandler(ThrowDart3);
            timer4.Start();
        }

        public void ThrowDart3(object sender, EventArgs e)
        {
            if (m_dart3_position_x > 0)
            {
                m_dart3_position_x -= 0.1f;
                if (m_dart3_rotation_x > 20)
                {
                    m_dart3_rotation_x -= 1f;
                }
            }
            else if (m_dart3_rotation_x > 20)
            {
                m_dart3_rotation_x -= 1f;
            }
            else if (m_dart3_position_x <= 0 && m_dart3_rotation_x <= 20 && m_dart3_position_z <= 20f && m_dart3_position_z > 1f)
            {
                timer4.Interval = TimeSpan.FromMilliseconds(0.3);
                if (m_dart3_rotation_x > 0)
                {
                    m_dart3_rotation_x -= 0.11f;
                }
                m_dart3_position_z -= 0.1f;
            }
            else
            {
                timer4.Stop();
                ScaleBoardDown();
            }
        }
        public void ScaleBoardDown()
        {
            timer5 = new DispatcherTimer();
            timer5.Interval = TimeSpan.FromMilliseconds(10);
            timer5.Tick += new EventHandler(ScaleBoardDownEv);
            timer5.Start();
        }

        public void ScaleBoardDownEv(object sender, EventArgs e)
        {
            if (m_board_scale > 0.2)
            {
                m_board_scale -= 0.2f;
            }
            else
            {
                timer5.Stop();
                this.Animation = false;
            }
        }
        #endregion

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
