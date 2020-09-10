using System;
using OpenTK.Graphics.OpenGL;

namespace shaders
{
    public class Triangle
    {
        int handle;
        int vertexShader, fragmentShader;

        public Triangle(string vertexShaderSource, string fragmentShaderSource)
        {
            vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);

            fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);

            GL.CompileShader(vertexShader);
            GL.CompileShader(fragmentShader);

            handle = GL.CreateProgram();

            GL.AttachShader(handle, vertexShader);
            GL.AttachShader(handle, fragmentShader);

            GL.LinkProgram(handle);

            GL.DetachShader(handle, vertexShader);
            GL.DetachShader(handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        public void render() {
            GL.UseProgram(handle);
        }

        bool disposed = false;
        protected virtual void Dispose(bool disposing) {

            if (!disposed) {
                GL.DeleteProgram(handle);
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Triangle() {
            GL.DeleteProgram(handle);
        }
    }
}
