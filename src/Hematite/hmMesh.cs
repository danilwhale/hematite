namespace Hematite;

public sealed class hmMesh
{
    public readonly hmWindow Owner;
    public readonly uint Handle;
    
    public hmBuffer? VertexBuffer;
    public hmBuffer? ElementBuffer;
    public hmMeshElementType ElementType;
    public hmVertexFormat? VertexFormat;

    internal hmMesh(hmWindow owner, uint handle)
    {
        Owner = owner;
        Handle = handle;
    }
}