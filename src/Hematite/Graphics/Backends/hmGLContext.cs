using Silk.NET.OpenGL;

namespace Hematite.Graphics.Backends;

internal sealed class hmGLContext(GL gl) : hmGfxContext
{
    public readonly GL Gl = gl;
    
    public override void Dispose()
    {
        Gl.Dispose();
    }
}