using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Hematite.Windowing;
using Silk.NET.OpenGL;
using Vortice.Mathematics;

namespace Hematite.Graphics.Backends;

internal sealed unsafe class hmGLBackend : hmIBackend
{
    public bool TryInitialize()
    {
        return true;
    }

    public void Destroy()
    {
    }

    public void WindowHandleResize(hmWindow window, int newWidth, int newHeight)
    {
        GL gl = ((hmGLContext)window.GfxContext).Gl;
        gl.Viewport(0, 0, (uint)newWidth, (uint)newHeight);
    }

    public void WindowClearColor(hmWindow window, Color color)
    {
        GL gl = ((hmGLContext)window.GfxContext).Gl;

        Vector4 c = color.ToVector4();
        gl.ClearColor(c.X, c.Y, c.Z, c.W);
        gl.Clear(ClearBufferMask.ColorBufferBit);
    }

    public void WindowClearDepth(hmWindow window, double depth)
    {
        GL gl = ((hmGLContext)window.GfxContext).Gl;
        gl.ClearDepth(depth);
        gl.Clear(ClearBufferMask.DepthBufferBit);
    }

    public void WindowClear(hmWindow window, Color color, double depth)
    {
        GL gl = ((hmGLContext)window.GfxContext).Gl;
        
        Vector4 c = color.ToVector4();
        gl.ClearColor(c.X, c.Y, c.Z, c.W);
        gl.ClearDepth(depth);
        gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }

    public hmTexture? TextureLoad(hmWindow window, ReadOnlySpan<byte> pixels, uint width, uint height,
        hmPixelFormat format)
    {
        GL gl = ((hmGLContext)window.GfxContext).Gl;

        uint texture = gl.GenTexture();
        gl.BindTexture(TextureTarget.Texture2D, texture);
        gl.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        gl.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        gl.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        gl.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        fixed (byte* pPixels = pixels)
        {
            gl.TexImage2D(
                TextureTarget.Texture2D,
                0,
                InternalFormat.Rgba8,
                width, height,
                0,
                format switch
                {
                    hmPixelFormat.Rgb8 => PixelFormat.Rgb,
                    hmPixelFormat.Rgba8 => PixelFormat.Rgba,
                },
                PixelType.UnsignedByte,
                pPixels
            );
        }

        return new hmTexture(window, texture);
    }

    public void TextureUse(hmTexture texture)
    {
        GL gl = ((hmGLContext)texture.Owner.GfxContext).Gl;
        gl.BindTexture(TextureTarget.Texture2D, texture.Handle);
    }

    public void DestroyTexture(hmTexture texture)
    {
        GL gl = ((hmGLContext)texture.Owner.GfxContext).Gl;
        gl.DeleteTexture(texture.Handle);
    }

    public hmEffect? EffectLoad(hmWindow window, string vertexSource, string fragmentSource)
    {
        GL gl = ((hmGLContext)window.GfxContext).Gl;

        uint vertexShader = gl.CreateShader(ShaderType.VertexShader);
        gl.ShaderSource(vertexShader, vertexSource);
        gl.CompileShader(vertexShader);
        if (gl.GetShader(vertexShader, ShaderParameterName.CompileStatus) == 0)
        {
            throw new Exception("Failed to compile a vertex shader:\n" + gl.GetShaderInfoLog(vertexShader));
        }

        uint fragmentShader = gl.CreateShader(ShaderType.FragmentShader);
        gl.ShaderSource(fragmentShader, fragmentSource);
        gl.CompileShader(fragmentShader);
        if (gl.GetShader(fragmentShader, ShaderParameterName.CompileStatus) == 0)
        {
            throw new Exception("Failed to compile a fragment shader:\n" + gl.GetShaderInfoLog(fragmentShader));
        }

        uint program = gl.CreateProgram();
        gl.AttachShader(program, vertexShader);
        gl.AttachShader(program, fragmentShader);
        gl.LinkProgram(program);
        if (gl.GetProgram(program, ProgramPropertyARB.LinkStatus) == 0)
        {
            throw new Exception("Failed to link a shader program:\n" + gl.GetProgramInfoLog(program));
        }

        gl.DetachShader(program, vertexShader);
        gl.DetachShader(program, fragmentShader);
        gl.DeleteShader(vertexShader);
        gl.DeleteShader(fragmentShader);

        return new hmEffect(window, program);
    }

    public void EffectUse(hmEffect effect)
    {
        GL gl = ((hmGLContext)effect.Owner.GfxContext).Gl;
        gl.UseProgram(effect.Handle);
    }

    public void DestroyEffect(hmEffect effect)
    {
        GL gl = ((hmGLContext)effect.Owner.GfxContext).Gl;
        gl.DeleteProgram(effect.Handle);
    }

    public hmBuffer? MakeBuffer(hmWindow window, uint sizeInBytes)
    {
        GL gl = ((hmGLContext)window.GfxContext).Gl;

        uint buffer = gl.GenBuffer();
        gl.BindBuffer(BufferTargetARB.ArrayBuffer, buffer);
        gl.BufferData(BufferTargetARB.ArrayBuffer, sizeInBytes, null, BufferUsageARB.StaticDraw);

        return new hmBuffer(window, buffer, sizeInBytes);
    }

    public bool BufferTryWrite<T>(hmBuffer buffer, ReadOnlySpan<T> data, uint offset) where T : unmanaged
    {
        GL gl = ((hmGLContext)buffer.Owner.GfxContext).Gl;

        gl.BindBuffer(BufferTargetARB.ArrayBuffer, buffer.Handle);
        fixed (T* pData = data)
        {
            gl.BufferSubData(BufferTargetARB.ArrayBuffer, (nint)offset, (nuint)(data.Length * sizeof(T)), pData);
        }
        return true;
    }

    public bool BufferTryRead<T>(hmBuffer buffer, Span<T> data, uint offset) where T : unmanaged
    {
        GL gl = ((hmGLContext)buffer.Owner.GfxContext).Gl;

        gl.BindBuffer(BufferTargetARB.ArrayBuffer, buffer.Handle);
        fixed (T* pData = data)
        {
            gl.GetBufferSubData(BufferTargetARB.ArrayBuffer, (nint)offset, (nuint)(data.Length * sizeof(T)), pData);
        }
        return true;
    }

    public uint BufferGetSizeInBytes(hmBuffer buffer)
    {
        GL gl = ((hmGLContext)buffer.Owner.GfxContext).Gl;

        int size;
        gl.BindBuffer(BufferTargetARB.ArrayBuffer, buffer.Handle);
        gl.GetBufferParameter(BufferTargetARB.ArrayBuffer, BufferPNameARB.SizeArb, &size);

        return (uint)size;
    }

    public bool BufferTryLock(hmBuffer buffer, hmBufferAccess access, [NotNullWhen(true)] out hmBufferData? data)
    {
        GL gl = ((hmGLContext)buffer.Owner.GfxContext).Gl;

        gl.BindBuffer(BufferTargetARB.ArrayBuffer, buffer.Handle);
        void* pBuffer = gl.MapBuffer(BufferTargetARB.ArrayBuffer, access switch
        {
            hmBufferAccess.ReadOnly => BufferAccessARB.ReadOnly,
            hmBufferAccess.WriteOnly => BufferAccessARB.WriteOnly,
            hmBufferAccess.ReadWrite => BufferAccessARB.ReadWrite
        });
        if (pBuffer == null)
        {
            data = null;
            return false;
        }

        data = new hmBufferData(buffer, pBuffer, buffer.SizeInBytes, access);
        return true;
    }

    public bool BufferTryLock(hmBuffer buffer, uint offset, uint sizeInBytes, hmBufferAccess access,
        [NotNullWhen(true)] out hmBufferData? data)
    {
        GL gl = ((hmGLContext)buffer.Owner.GfxContext).Gl;

        gl.BindBuffer(BufferTargetARB.ArrayBuffer, buffer.Handle);
        void* pBuffer = gl.MapBufferRange(BufferTargetARB.ArrayBuffer, (nint)offset, sizeInBytes, access switch
        {
            hmBufferAccess.ReadOnly => MapBufferAccessMask.ReadBit,
            hmBufferAccess.WriteOnly => MapBufferAccessMask.WriteBit,
            hmBufferAccess.ReadWrite => MapBufferAccessMask.ReadBit | MapBufferAccessMask.WriteBit
        });

        if (pBuffer == null)
        {
            data = null;
            return false;
        }

        data = new hmBufferData(buffer, pBuffer, sizeInBytes, access);
        return true;
    }

    public bool BufferTryUnlock(hmBuffer buffer)
    {
        GL gl = ((hmGLContext)buffer.Owner.GfxContext).Gl;

        gl.BindBuffer(BufferTargetARB.ArrayBuffer, buffer.Handle);
        gl.UnmapBuffer(BufferTargetARB.ArrayBuffer);

        return true;
    }

    public void DestroyBuffer(hmBuffer buffer)
    {
        GL gl = ((hmGLContext)buffer.Owner.GfxContext).Gl;
        gl.DeleteBuffer(buffer.Handle);
    }

    public hmMesh MakeMesh(hmWindow window)
    {
        GL gl = ((hmGLContext)window.GfxContext).Gl;
        return new hmMesh(window, gl.GenVertexArray());
    }

    public void MeshAttachVertexBuffer(hmMesh mesh, hmBuffer vertexBuffer)
    {
        GL gl = ((hmGLContext)mesh.Owner.GfxContext).Gl;

        gl.BindVertexArray(mesh.Handle);
        gl.BindBuffer(BufferTargetARB.ArrayBuffer, vertexBuffer.Handle);
    }

    public void MeshAttachElementBuffer(hmMesh mesh, hmBuffer elementBuffer, hmMeshElementType elementType)
    {
        GL gl = ((hmGLContext)mesh.Owner.GfxContext).Gl;

        gl.BindVertexArray(mesh.Handle);
        gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, elementBuffer.Handle);
    }

    public void MeshAttachVertexFormat(hmMesh mesh, hmVertexFormat vertexFormat)
    {
        GL gl = ((hmGLContext)mesh.Owner.GfxContext).Gl;

        gl.BindVertexArray(mesh.Handle);
        
        uint index = 0;
        nint pointer = 0;
        foreach (hmVertexFormatElement e in vertexFormat.Elements)
        {
            gl.VertexAttribPointer(
                index,
                (int)e.Size,
                e.Type switch
                {
                    hmVertexFormatElementType.Byte => VertexAttribPointerType.Byte,
                    hmVertexFormatElementType.UnsignedByte => VertexAttribPointerType.UnsignedByte,
                    hmVertexFormatElementType.Short => VertexAttribPointerType.Short,
                    hmVertexFormatElementType.UnsignedShort => VertexAttribPointerType.UnsignedShort,
                    hmVertexFormatElementType.Int => VertexAttribPointerType.Int,
                    hmVertexFormatElementType.UnsignedInt => VertexAttribPointerType.UnsignedInt,
                    hmVertexFormatElementType.HalfFloat => VertexAttribPointerType.HalfFloat,
                    hmVertexFormatElementType.Float => VertexAttribPointerType.Float,
                    hmVertexFormatElementType.Double => VertexAttribPointerType.Double
                },
                e.Normalized,
                vertexFormat.Stride,
                pointer
            );
            gl.EnableVertexAttribArray(index);
            
            index++;
            pointer += (nint)e.SizeInBytes;
        }
    }

    public void MeshRender(hmMesh mesh, hmPrimitiveTopology topology, uint baseVertex, uint elementCount)
    {
        GL gl = ((hmGLContext)mesh.Owner.GfxContext).Gl;
        
        gl.BindVertexArray(mesh.Handle);

        PrimitiveType mode = topology switch
        {
            hmPrimitiveTopology.PointList => PrimitiveType.Points,
            hmPrimitiveTopology.LineList => PrimitiveType.Lines,
            hmPrimitiveTopology.TriangleList => PrimitiveType.Triangles,
            hmPrimitiveTopology.LineStrip => PrimitiveType.LineStrip,
            hmPrimitiveTopology.LineListAdjacent => PrimitiveType.LinesAdjacency,
            hmPrimitiveTopology.LineStripAdjacent => PrimitiveType.LinesAdjacency,
            hmPrimitiveTopology.TriangleStrip => PrimitiveType.TriangleStrip,
            hmPrimitiveTopology.TriangleFan => PrimitiveType.TriangleFan,
            hmPrimitiveTopology.TriangleListAdjacent => PrimitiveType.TrianglesAdjacency,
            hmPrimitiveTopology.TriangleStripAdjacent => PrimitiveType.TriangleStripAdjacency
        };
        if (mesh.ElementBuffer != null)
        {
            gl.DrawElementsBaseVertex(mode, elementCount, mesh.ElementType switch
            {
                hmMeshElementType.UnsignedByte => DrawElementsType.UnsignedByte,
                hmMeshElementType.UnsignedShort => DrawElementsType.UnsignedShort,
                hmMeshElementType.UnsignedInt => DrawElementsType.UnsignedInt
            }, (void*)0, (int)baseVertex);
        }
        else
        {
            gl.DrawArrays(mode, (int)baseVertex, elementCount);
        }
    }

    public void DestroyMesh(hmMesh mesh)
    {
        GL gl = ((hmGLContext)mesh.Owner.GfxContext).Gl;
        gl.DeleteVertexArray(mesh.Handle);
    }
}