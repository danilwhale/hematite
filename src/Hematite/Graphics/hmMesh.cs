using Hematite.Windowing;

namespace Hematite.Graphics;

public sealed class hmMesh : hmGfxResource
{
    public hmBuffer? VertexBuffer;
    public hmBuffer? ElementBuffer;
    public hmMeshElementType ElementType;
    public hmVertexFormat? VertexFormat;

    internal hmMesh(hmWindow owner, uint handle) : base(owner, handle)
    {
    }
}