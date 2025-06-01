using System.Diagnostics;
using System.Runtime.InteropServices;
using Silk.NET.OpenGL;

namespace Hematite.Rendering;

// optimized for constructing quads 
public sealed unsafe class BufferBuilder<TVertex> : IDisposable
    where TVertex : unmanaged, IVertex<TVertex>
{
    public readonly GL Gl;
    private readonly uint _vao;
    private readonly uint _vebo; // vertex + element buffer object
    private readonly bool _ownsVao;

    private readonly TVertex[] _vertices;
    private int _pointer;
    private bool _buildQuads;
    
    private readonly uint _indexOffset;
    private uint _renderCount;
    private bool _renderQuads;

    public BufferBuilder(GL gl, uint vao, TVertex[] vertices, ushort[] indices, bool ownsVao = false,
        bool cookIndices = true)
    {
#if DEBUG
        // validate buffers
        Debug.Assert((vertices.Length & 3) == 0);
        Debug.Assert((indices.Length & 5) == 0);
#endif

        Gl = gl;

        _vao = vao;
        _vebo = gl.CreateBuffer();
        _ownsVao = ownsVao;

        gl.NamedBufferStorage(
            _vebo,
            (nuint)(vertices.Length * sizeof(TVertex) + indices.Length * sizeof(ushort)),
            null,
            BufferStorageMask.MapWriteBit
        );

        // you 100% won't use different vertex type if you use shared vao for the builders
        // if you do then stand up, walk away from your computer and rethink what you did
        TVertex.FormatVertexArray(gl, vao, 0);

        // we don't bind buffers if you don't own the vao here btw
        // case when you don't own the vao is level chunk rendering
        // go to Render method
        if (_ownsVao)
        {
            gl.VertexArrayVertexBuffer(vao, 0, _vebo, 0, (uint)sizeof(TVertex));
            gl.VertexArrayElementBuffer(vao, _vebo);
        }

        _vertices = vertices;
        _indexOffset = (uint)(vertices.Length * sizeof(TVertex));

        if (cookIndices) BufferBuilder.CookIndices(indices);

        // now let's just copy indices into the buffer
        void* dataBuffer = Gl.MapNamedBuffer(_vebo, BufferAccessARB.WriteOnly);
        fixed (ushort* pIndices = indices)
        {
            NativeMemory.Copy(
                pIndices,
                (void*)((nint)dataBuffer + _indexOffset),
                (nuint)indices.Length * sizeof(ushort)
            );
        }

        Gl.UnmapNamedBuffer(_vebo);
    }

    public BufferBuilder(GL gl, ushort quadCount)
        : this(gl, gl.CreateVertexArray(), new TVertex[quadCount * 4], new ushort[quadCount * 6], true)
    {
    }

    public void Begin(bool quads = true)
    {
        _pointer = 0;
        _buildQuads = quads;
    }

    public void End()
    {
        int quadCount = _pointer >> 2;

        _renderCount = (uint)(_buildQuads ? quadCount * 6 : _pointer);
        if (_renderCount == 0) return;

        void* dataBuffer = Gl.MapNamedBuffer(_vebo, BufferAccessARB.WriteOnly);
        fixed (TVertex* pVertices = _vertices)
        {
            NativeMemory.Copy(
                pVertices,
                dataBuffer,
                (nuint)((_buildQuads ? quadCount << 2 : _pointer) * sizeof(TVertex))
            );
        }

        Gl.UnmapNamedBuffer(_vebo);
        _renderQuads = _buildQuads;
    }

    public void Render()
    {
        if (_renderCount == 0) return;

        if (!_ownsVao)
        {
            Gl.VertexArrayVertexBuffer(_vao, 0, _vebo, 0, (uint)sizeof(TVertex));
            if (_renderQuads) Gl.VertexArrayElementBuffer(_vao, _vebo);
        }
        else
        {
            Gl.BindVertexArray(_vao);
        }

        if (_renderQuads)
        {
            Gl.DrawElements(PrimitiveType.Triangles, _renderCount, DrawElementsType.UnsignedShort, (void*)_indexOffset);
        }
        else
        {
            Gl.DrawArrays(PrimitiveType.Triangles, 0, _renderCount);
        }
    }

    public void Add(in TVertex vertex)
    {
        _vertices[_pointer++] = vertex;
    }

    #if NET9_0_OR_GREATER
    public void Add(params ReadOnlySpan<TVertex> vertices)
    #else
    public void Add(params TVertex[] vertices)
    #endif
    {
        vertices.CopyTo(_vertices.AsSpan(_pointer));
        // apparently this is faster
        // fixed (TVertex* pInputVertices = vertices)
        // fixed (TVertex* pResultVertices = _vertices)
        // {
        //     NativeMemory.Copy(
        //         pInputVertices,
        //         pResultVertices,
        //         (nuint)(vertices.Length * sizeof(TVertex))
        //     );
        // }

        _pointer += vertices.Length;
    }

    public void Dispose()
    {
        Gl.DeleteBuffer(_vebo);
        if (_ownsVao) Gl.DeleteVertexArray(_vao);
    }
}