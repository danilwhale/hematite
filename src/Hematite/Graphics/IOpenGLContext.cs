using Hematite.Windowing;

namespace Hematite.Graphics;

public interface IOpenGLContext : IGraphicsContext
{
    bool IsCurrent { get; }
    
    bool TryGetProcAddress(string name, out nint address);
    void Clear();
    void MakeCurrent();
    void SwapBuffers();
    void SwapInterval(int interval);
}