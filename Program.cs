using System;
using System.IO;
using System.Text;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace shaders
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            new Game(1000,720);
        }
    }

    class Game : GameWindow
    {
        int VertexBufferObject;
        int VertexArrayObject;

        float[] vertices = {
            -0.5f, -0.5f, 0.0f, //Bottom-left vertex
             0.5f, -0.5f, 0.0f, //Bottom-right vertex
             0.0f,  0.5f, 0.0f  //Top vertex
        };

        Triangle triangle;

        string vertexShaderSource;
        string fragmentShaderSource;
        string main_path = Directory.GetCurrentDirectory();

        public Game(int width, int height) : base(width,height,GraphicsMode.Default,"SHADER TEST")
        {
            fragmentShaderSource = getShaderCode("FragmentShader.glsl");
            vertexShaderSource = getShaderCode("VertexShader.glsl");

            Console.WriteLine(fragmentShaderSource);
            start();
        }

        void start() {
            Run(60);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            VertexArrayObject = GL.GenVertexArray();

            GL.VertexAttribPointer(0,3,VertexAttribPointerType.Float,false,3 * sizeof(float),0);
            GL.EnableVertexAttribArray(0);

            GL.BindVertexArray(VertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            try { 
                triangle.render();
            } catch(Exception e1) {}
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0, 0, 0, 0);
            triangle = new Triangle(vertexShaderSource, fragmentShaderSource);
            VertexBufferObject = GL.GenBuffer();
            base.OnLoad(e);
        }

        protected override void OnUnload(EventArgs e) { 

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VertexBufferObject);

            triangle.Dispose();
            base.OnUnload(e);
        }


        string getShaderCode(string fileName)
        {
            // GLSL (OR SHADER) FILES MUST BE STORED IN THE bin/Debug FOLDER

            string code = "";
            using (StreamReader reader = new StreamReader(main_path+"/"+fileName, Encoding.UTF8)) {
                code = reader.ReadToEnd();
            }

            return code;
        }
    }
}
