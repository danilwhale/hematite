using Hematite.Graphics;
using Hematite.Windowing;
using StbImageSharp;
using Vortice.Mathematics;

namespace Hematite;

public static unsafe partial class hmLib
{
    public static partial void hmWindowClearColor(hmWindow? window, Color color)
    {
        window ??= CurrentWindow;
        if (window is not null) Backend.WindowClearColor(window, color);
    }

    public static partial void hmWindowClearDepth(hmWindow? window, double depth)
    {
        window ??= CurrentWindow;
        if (window is not null) Backend.WindowClearDepth(window, depth);
    }

    public static partial void hmWindowClear(hmWindow? window, Color color, double depth)
    {
        window ??= CurrentWindow;
        if (window is not null) Backend.WindowClear(window, color, depth);
    }
    
    public static partial hmTexture? hmTextureLoad(hmWindow? window, ReadOnlySpan<byte> pixels, uint width, uint height, hmPixelFormat format)
    {
        window ??= CurrentWindow;
        return window is null ? null : Backend.TextureLoad(window, pixels, width, height, format);
    }

    public static partial hmTexture? hmTextureLoadFile(hmWindow? window, string path)
    {
        window ??= CurrentWindow;
        if (window is null) return null;

        using Stream stream = File.OpenRead(path);
        ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
        return Backend.TextureLoad(window, image.Data, (uint)image.Width, (uint)image.Height, hmPixelFormat.Rgba8);
    }

    public static partial void hmTextureUse(hmTexture texture)
    {
        Backend.TextureUse(texture);
    }

    public static partial void hmDestroyTexture(hmTexture texture)
    {
        Backend.DestroyTexture(texture);
    }

    public static partial hmEffect? hmEffectLoad(hmWindow? window, string vertexSource, string fragmentSource)
    {
        window ??= CurrentWindow;
        return window is null ? null : Backend.EffectLoad(window, vertexSource, fragmentSource);
    }

    public static partial hmEffect? hmEffectLoadFile(hmWindow? window, string vertexPath, string fragmentPath)
    {
        window ??= CurrentWindow;
        if (window is null) return null;

        string vertexSource = File.ReadAllText(vertexPath);
        string fragmentSource = File.ReadAllText(fragmentPath);
        return Backend.EffectLoad(window, vertexSource, fragmentSource);
    }
    
    public static partial void hmEffectUse(hmEffect effect)
    {
        Backend.EffectUse(effect);
    }

    public static partial void hmDestroyEffect(hmEffect effect)
    {
        Backend.DestroyEffect(effect);
    }

    public static partial hmBuffer? hmMakeBuffer(hmWindow? window, uint sizeInBytes)
    {
        window ??= CurrentWindow;
        return window is null ? null : Backend.MakeBuffer(window, sizeInBytes);
    }
    
    public static partial hmBuffer? hmMakeBuffer<T>(hmWindow? window, uint size) where T : unmanaged
    {
        window ??= CurrentWindow;
        return window is null ? null : Backend.MakeBuffer(window, (uint)(size * sizeof(T)));
    }

    public static partial bool hmBufferTryWrite<T>(hmBuffer buffer, ReadOnlySpan<T> data, uint offsetInBytes) where T : unmanaged
    {
        return !buffer.Locked && Backend.BufferTryWrite(buffer, data, offsetInBytes);
    }

    public static partial bool hmBufferTryRead<T>(hmBuffer buffer, Span<T> data, uint offsetInBytes) where T : unmanaged
    {
        return !buffer.Locked && Backend.BufferTryRead(buffer, data, offsetInBytes);
    }

    public static partial uint hmBufferGetSizeInBytes(hmBuffer buffer)
    {
        return Backend.BufferGetSizeInBytes(buffer);
    }

    public static partial uint hmBufferGetSize<T>(hmBuffer buffer) where T : unmanaged
    {
        uint sizeInBytes = Backend.BufferGetSizeInBytes(buffer);

        int toSize = sizeof(T);
        return toSize == 1 ? sizeInBytes : sizeInBytes / (uint)toSize;
    }

    public static partial void hmDestroyBuffer(hmBuffer buffer)
    {
        Backend.DestroyBuffer(buffer);
    }
    
    public static partial bool hmBufferTryLock(hmBuffer buffer, hmBufferAccess access, out hmBufferData? data)
    {
        if (buffer.Locked)
        {
            data = null;
            return false;
        }

        buffer.Locked = true;
        return Backend.BufferTryLock(buffer, access, out data);
    }

    public static partial bool hmBufferTryLock(hmBuffer buffer, uint offsetInBytes, uint sizeInBytes, hmBufferAccess access, out hmBufferData? data)
    {
        if (buffer.Locked)
        {
            data = null;
            return false;
        }

        buffer.Locked = true;
        return Backend.BufferTryLock(buffer, offsetInBytes, sizeInBytes, access, out data);
    }
    
    public static partial bool hmBufferTryLock<T>(hmBuffer buffer, uint offsetInBytes, uint size, hmBufferAccess access, out hmBufferData? data) where T : unmanaged
    {
        if (buffer.Locked)
        {
            data = null;
            return false;
        }

        buffer.Locked = true;
        return Backend.BufferTryLock(buffer, offsetInBytes, (uint)(size * sizeof(T)), access, out data);
    }

    public static partial bool hmBufferTryUnlock(hmBuffer buffer)
    {
        if (!buffer.Locked) return false;
        buffer.Locked = false;
        return Backend.BufferTryUnlock(buffer);
    }

    public static partial hmMesh? hmMakeMesh(hmWindow? window)
    {
        window ??= CurrentWindow;
        return window is null ? null : Backend.MakeMesh(window);
    }

    public static partial void hmMeshAttachVertexBuffer(hmMesh mesh, hmBuffer vertexBuffer)
    {
        Backend.MeshAttachVertexBuffer(mesh, vertexBuffer);
        mesh.VertexBuffer = vertexBuffer;
    }

    public static partial void hmMeshAttachElementBuffer(hmMesh mesh, hmBuffer elementBuffer, hmMeshElementType elementType)
    {
        Backend.MeshAttachElementBuffer(mesh, elementBuffer, elementType);
        mesh.ElementBuffer = elementBuffer;
        mesh.ElementType = elementType;
    }

    public static partial void hmMeshAttachVertexFormat(hmMesh mesh, hmVertexFormat vertexFormat)
    {
        Backend.MeshAttachVertexFormat(mesh, vertexFormat);
        mesh.VertexFormat = vertexFormat;
    }

    public static partial void hmMeshRender(hmMesh mesh, hmPrimitiveTopology topology, uint baseVertex, uint elementCount)
    {
        Backend.MeshRender(mesh, topology, baseVertex, elementCount);
    }

    public static partial void hmDestroyMesh(hmMesh mesh)
    {
        Backend.DestroyMesh(mesh);
    }
}