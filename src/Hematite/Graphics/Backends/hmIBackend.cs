using System.Diagnostics.CodeAnalysis;
using Hematite.Windowing;
using Vortice.Mathematics;

namespace Hematite.Graphics.Backends;

internal interface hmIBackend
{
    bool TryInitialize();
    void Destroy();
    
    // internal >>
    void WindowHandleResize(hmWindow window, int newWidth, int newHeight);
    // <<
    
    // core >>
    void WindowClearColor(hmWindow window, Color color);
    void WindowClearDepth(hmWindow window, double depth);
    void WindowClear(hmWindow window, Color color, double depth);
    // <<
    
    // textures >>
    hmTexture? TextureLoad(hmWindow window, ReadOnlySpan<byte> pixels, uint width, uint height, hmPixelFormat format);
    void TextureUse(hmTexture texture);
    void DestroyTexture(hmTexture texture);
    // <<
    
    // effects >>
    hmEffect? EffectLoad(hmWindow window, string vertexSource, string fragmentSource);
    void EffectUse(hmEffect effect);
    void DestroyEffect(hmEffect effect);
    // <<
    
    // buffers >>
    hmBuffer? MakeBuffer(hmWindow window, uint sizeInBytes);
    bool BufferTryWrite<T>(hmBuffer buffer, ReadOnlySpan<T> data, uint offset) where T : unmanaged;
    bool BufferTryRead<T>(hmBuffer buffer, Span<T> data, uint offset) where T : unmanaged;
    uint BufferGetSizeInBytes(hmBuffer buffer);
    bool BufferTryLock(hmBuffer buffer, hmBufferAccess access, [NotNullWhen(true)] out hmBufferData? data);
    bool BufferTryLock(hmBuffer buffer, uint offset, uint sizeInBytes, hmBufferAccess access, [NotNullWhen(true)] out hmBufferData? data);
    bool BufferTryUnlock(hmBuffer buffer);
    void DestroyBuffer(hmBuffer buffer);
    // <<
    
    // meshes >>
    hmMesh? MakeMesh(hmWindow window);
    void MeshAttachVertexBuffer(hmMesh mesh, hmBuffer vertexBuffer);
    void MeshAttachElementBuffer(hmMesh mesh, hmBuffer elementBuffer, hmMeshElementType elementType);
    void MeshAttachVertexFormat(hmMesh mesh, hmVertexFormat vertexFormat);
    void MeshRender(hmMesh mesh, hmPrimitiveTopology topology, uint baseVertex, uint elementCount);
    void DestroyMesh(hmMesh mesh);
    // <<
}