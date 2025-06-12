using System.Diagnostics.CodeAnalysis;
using Hematite.Graphics;
using Hematite.Windowing;
using Vortice.Mathematics;

namespace Hematite;

public static partial class hmLib
{
    // core >>>
    public static partial bool hmTryInitialize();
    public static partial void hmUpdate();
    public static partial void hmDestroy();
    // <<<
    
    // windowing >>>
    public static partial hmWindow? hmMakeWindow(ref readonly hmWindowDescriptor descriptor);
    [return: NotNullIfNotNull(nameof(window))] 
    public static partial hmWindow? hmWindowSetCurrent(hmWindow? window);
    public static partial hmWindow? hmWindowGetCurrent();
    public static partial bool hmWindowShouldClose(hmWindow? window);
    public static partial void hmWindowClose(hmWindow? window);
    public static partial bool hmWindowWasResized(hmWindow? window);
    public static partial bool hmWindowWasMoved(hmWindow? window);
    public static partial void hmWindowUpdate(hmWindow? window);
    public static partial hmWindowBorder hmWindowGetBorder(hmWindow? window);
    public static partial void hmWindowSetBorder(hmWindow? window, hmWindowBorder border);
    public static partial hmWindowState hmWindowGetState(hmWindow? window);
    public static partial void hmWindowSetState(hmWindow? window, hmWindowState state);
    public static partial string? hmWindowGetTitle(hmWindow? window);
    public static partial void hmWindowSetTitle(hmWindow? window, string? title);
    public static partial SizeI hmWindowGetSize(hmWindow? window);
    public static partial void hmWindowSetSize(hmWindow? window, SizeI size);
    public static partial SizeI hmWindowGetMinSize(hmWindow? window);
    public static partial void hmWindowSetMinSize(hmWindow? window, SizeI minSize);
    public static partial SizeI hmWindowGetMaxSize(hmWindow? window);
    public static partial void hmWindowSetMaxSize(hmWindow? window, SizeI maxSize);
    public static partial Int2 hmWindowGetPosition(hmWindow? window);
    public static partial void hmWindowSetPosition(hmWindow? window, Int2 position);
    public static partial float hmWindowGetOpacity(hmWindow? window);
    public static partial void hmWindowSetOpacity(hmWindow? window, float opacity);
    public static partial void hmDestroyWindow(hmWindow window);
    // <<<
    
    // gfx >>>
    // core >>
    public static partial void hmWindowClearColor(hmWindow? window, Color color);
    public static partial void hmWindowClearDepth(hmWindow? window, double depth);
    public static partial void hmWindowClear(hmWindow? window, Color color, double depth);
    // <<
    
    // textures >>
    public static partial hmTexture? hmTextureLoad(hmWindow? window, ReadOnlySpan<byte> pixels, uint width, uint height, hmPixelFormat format);
    public static partial hmTexture? hmTextureLoadFile(hmWindow? window, string path);
    public static partial void hmTextureUse(hmTexture texture);
    public static partial void hmDestroyTexture(hmTexture texture);
    // <<
    
    // effects >>
    public static partial hmEffect? hmEffectLoad(hmWindow? window, string vertexSource, string fragmentSource);
    public static partial hmEffect? hmEffectLoadFile(hmWindow? window, string vertexPath, string fragmentPath);
    public static partial void hmEffectUse(hmEffect effect);
    public static partial void hmDestroyEffect(hmEffect effect);
    // <<
    
    // buffers >>
    public static partial hmBuffer? hmMakeBuffer(hmWindow? window, uint sizeInBytes);
    public static partial hmBuffer? hmMakeBuffer<T>(hmWindow? window, uint size) where T : unmanaged;
    public static partial bool hmBufferTryWrite<T>(hmBuffer buffer, ReadOnlySpan<T> data, uint offsetInBytes) where T : unmanaged;
    public static partial bool hmBufferTryRead<T>(hmBuffer buffer, Span<T> data, uint offsetInBytes) where T : unmanaged;
    public static partial uint hmBufferGetSizeInBytes(hmBuffer buffer);
    public static partial uint hmBufferGetSize<T>(hmBuffer buffer) where T : unmanaged;
    public static partial bool hmBufferTryLock(hmBuffer buffer, hmBufferAccess access, out nint data);
    public static partial bool hmBufferTryLock(hmBuffer buffer, uint offsetInBytes, uint sizeInBytes, hmBufferAccess access, out hmBufferData data);
    public static partial bool hmBufferTryLock<T>(hmBuffer buffer, uint offsetInBytes, uint size, hmBufferAccess access, out hmBufferData data) where T : unmanaged;
    public static partial bool hmBufferTryUnlock(hmBuffer buffer);
    public static partial void hmDestroyBuffer(hmBuffer buffer);
    // <<
    
    // meshes >>
    public static partial hmMesh? hmMakeMesh(hmWindow? window);
    public static partial void hmMeshAttachVertexBuffer(hmMesh mesh, hmBuffer vertexBuffer);
    public static partial void hmMeshAttachElementBuffer(hmMesh mesh, hmBuffer elementBuffer, hmMeshElementType elementType);
    public static partial void hmMeshAttachVertexFormat(hmMesh mesh, hmVertexFormat vertexFormat);
    public static partial void hmMeshRender(hmMesh mesh, hmPrimitiveTopology topology, uint baseVertex, uint elementCount);
    public static partial void hmDestroyMesh(hmMesh mesh);
    // <<
    // <<<
}