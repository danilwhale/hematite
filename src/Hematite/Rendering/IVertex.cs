using Silk.NET.OpenGL;

namespace Hematite.Rendering;

public interface IVertex<TSelf> 
    where TSelf : unmanaged, IVertex<TSelf>
{
    static abstract void FormatVertexArray(GL gl, uint vao, uint bindingIndex);
}