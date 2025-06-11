using Hematite.Graphics;
using Hematite.Windowing;
using StbImageSharp;
using Vortice.Mathematics;

namespace Hematite;

public static unsafe partial class hmLib
{
    public static partial void hmWindowClearColor(hmWindow? window, Color color)
    {
        window ??= GetCurrentWindowOrNull(window);
        if (window is not null) GfxBackend.WindowClearColor(window, color);
    }

    public static partial void hmWindowClearDepth(hmWindow? window, double depth)
    {
        window ??= GetCurrentWindowOrNull(window);
        if (window is not null) GfxBackend.WindowClearDepth(window, depth);
    }

    public static partial void hmWindowClear(hmWindow? window, Color color, double depth)
    {
        window ??= GetCurrentWindowOrNull(window);
        if (window is not null) GfxBackend.WindowClear(window, color, depth);
    }
    
    public static partial hmTexture? hmTextureLoad(hmWindow? window, ReadOnlySpan<byte> pixels, uint width, uint height, hmPixelFormat format)
    {
        window ??= GetCurrentWindowOrNull(window);
        return window is null ? null : GfxBackend.TextureLoad(window, pixels, width, height, format);
    }

    public static partial hmTexture? hmTextureLoadFile(hmWindow? window, string path)
    {
        window ??= GetCurrentWindowOrNull(window);
        if (window is null) return null;

        using Stream stream = File.OpenRead(path);
        ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
        return GfxBackend.TextureLoad(window, image.Data, (uint)image.Width, (uint)image.Height, hmPixelFormat.Rgba8);
    }

    public static partial void hmTextureUse(hmTexture texture)
    {
        GfxBackend.TextureUse(texture);
    }

    public static partial void hmDestroyTexture(hmTexture texture)
    {
        GfxBackend.DestroyTexture(texture);
    }

    public static partial hmEffect? hmEffectLoad(hmWindow? window, string vertexSource, string fragmentSource)
    {
        window ??= GetCurrentWindowOrNull(window);
        return window is null ? null : GfxBackend.EffectLoad(window, vertexSource, fragmentSource);
    }

    public static partial hmEffect? hmEffectLoadFile(hmWindow? window, string vertexPath, string fragmentPath)
    {
        window ??= GetCurrentWindowOrNull(window);
        if (window is null) return null;

        string vertexSource = File.ReadAllText(vertexPath);
        string fragmentSource = File.ReadAllText(fragmentPath);
        return GfxBackend.EffectLoad(window, vertexSource, fragmentSource);
    }
    
    public static partial void hmEffectUse(hmEffect effect)
    {
        GfxBackend.EffectUse(effect);
    }

    public static partial void hmDestroyEffect(hmEffect effect)
    {
        GfxBackend.DestroyEffect(effect);
    }

    public static partial hmBuffer? hmMakeBuffer(hmWindow? window, uint sizeInBytes)
    {
        window ??= GetCurrentWindowOrNull(window);
        return window is null ? null : GfxBackend.MakeBuffer(window, sizeInBytes);
    }
    
    public static partial hmBuffer? hmMakeBuffer<T>(hmWindow? window, uint size) where T : unmanaged
    {
        window ??= GetCurrentWindowOrNull(window);
        return window is null ? null : GfxBackend.MakeBuffer(window, (uint)(size * sizeof(T)));
    }

    public static partial bool hmBufferTryWrite<T>(hmBuffer buffer, ReadOnlySpan<T> data, uint offsetInBytes) where T : unmanaged
    {
        return !buffer.Locked && GfxBackend.BufferTryWrite(buffer, data, offsetInBytes);
    }

    public static partial bool hmBufferTryRead<T>(hmBuffer buffer, Span<T> data, uint offsetInBytes) where T : unmanaged
    {
        return !buffer.Locked && GfxBackend.BufferTryRead(buffer, data, offsetInBytes);
    }

    public static partial uint hmBufferGetSizeInBytes(hmBuffer buffer)
    {
        return GfxBackend.BufferGetSizeInBytes(buffer);
    }

    public static partial uint hmBufferGetSize<T>(hmBuffer buffer) where T : unmanaged
    {
        uint sizeInBytes = GfxBackend.BufferGetSizeInBytes(buffer);

        return sizeof(T) switch
        {
            // we can't trust compiler, it's stupid
            1 => sizeInBytes,
            2 => sizeInBytes >> 1,
            4 => sizeInBytes >> 2,
            8 => sizeInBytes >> 3,
            
            // unfortunate
            _ => (uint)(sizeInBytes / sizeof(T))
        };
    }

    public static partial void hmDestroyBuffer(hmBuffer buffer)
    {
        GfxBackend.DestroyBuffer(buffer);
    }
    
    public static partial bool hmBufferTryLock(hmBuffer buffer, hmBufferAccess access, out nint data)
    {
        if (buffer.Locked)
        {
            data = 0;
            return false;
        }

        buffer.Locked = true;
        return GfxBackend.BufferTryLock(buffer, access, out data);
    }

    public static partial bool hmBufferTryLock(hmBuffer buffer, uint offsetInBytes, uint sizeInBytes, hmBufferAccess access, out hmBufferData data)
    {
        if (buffer.Locked)
        {
            data = default;
            return false;
        }

        buffer.Locked = true;
        return GfxBackend.BufferTryLock(buffer, offsetInBytes, sizeInBytes, access, out data);
    }
    
    public static partial bool hmBufferTryLock<T>(hmBuffer buffer, uint offsetInBytes, uint size, hmBufferAccess access, out hmBufferData data) where T : unmanaged
    {
        if (buffer.Locked)
        {
            data = default;
            return false;
        }

        buffer.Locked = true;
        return GfxBackend.BufferTryLock(buffer, offsetInBytes, (uint)(size * sizeof(T)), access, out data);
    }

    public static partial bool hmBufferTryUnlock(hmBuffer buffer)
    {
        if (!buffer.Locked) return false;
        buffer.Locked = false;
        return GfxBackend.BufferTryUnlock(buffer);
    }

    public static partial hmMesh? hmMakeMesh(hmWindow? window)
    {
        window ??= GetCurrentWindowOrNull(window);
        return window is null ? null : GfxBackend.MakeMesh(window);
    }

    public static partial void hmMeshAttachVertexBuffer(hmMesh mesh, hmBuffer vertexBuffer)
    {
        GfxBackend.MeshAttachVertexBuffer(mesh, vertexBuffer);
        mesh.VertexBuffer = vertexBuffer;
    }

    public static partial void hmMeshAttachElementBuffer(hmMesh mesh, hmBuffer elementBuffer, hmMeshElementType elementType)
    {
        GfxBackend.MeshAttachElementBuffer(mesh, elementBuffer, elementType);
        mesh.ElementBuffer = elementBuffer;
        mesh.ElementType = elementType;
    }

    public static partial void hmMeshAttachVertexFormat(hmMesh mesh, hmVertexFormat vertexFormat)
    {
        GfxBackend.MeshAttachVertexFormat(mesh, vertexFormat);
        mesh.VertexFormat = vertexFormat;
    }

    public static partial void hmMeshRender(hmMesh mesh, hmPrimitiveTopology topology, uint baseVertex, uint elementCount)
    {
        GfxBackend.MeshRender(mesh, topology, baseVertex, elementCount);
    }

    public static partial void hmDestroyMesh(hmMesh mesh)
    {
        GfxBackend.DestroyMesh(mesh);
    }
}