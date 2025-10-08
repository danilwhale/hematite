using Hematite.Windowing;
using Silk.NET.OpenGL;

namespace Hematite.Graphics.OpenGl;

public sealed class OpenGLDriver : Driver
{
    public const string Identifier = "hematite.opengl";

    public static void Register()
    {
        TryRegister(new OpenGLDriver());
    }

    public override string Name => "OpenGL";
    public override string UniqueName => Identifier;
    public override DriverApi Api => DriverApi.OpenGl;
    
    public override GraphicsDevice CreateDevice(Window window, IGraphicsContext context)
    {
        if (context is not IOpenGLContext glContext)
        {
            throw new ArgumentException("Invalid graphics context type", nameof(context));
        }

        GLContextAdapter adapter = new(glContext);
        return new OpenGLDevice(window, GL.GetApi(adapter), adapter);
    }
}