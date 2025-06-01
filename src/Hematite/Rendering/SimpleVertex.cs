using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Silk.NET.OpenGL;
using Vortice.Mathematics;

namespace Hematite.Rendering;

[StructLayout(LayoutKind.Sequential, Size = SizeInBytes)]
public readonly struct SimpleVertex(in Vector3 position, in Vector2 texCoord, in Color color) : IVertex<SimpleVertex>
{
    public const int SizeInBytes = 3 * sizeof(float) + 2 * sizeof(float) + 4;

    public readonly Vector3 Position = position;
    public readonly Vector2 TexCoord = texCoord;
    public readonly Color Color = color;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void FormatVertexArray(GL gl, uint vao, uint bindingIndex)
    {
        gl.EnableVertexArrayAttrib(vao, 0);
        gl.VertexArrayAttribFormat(
            vao, 0,
            3, VertexAttribType.Float, false,
            0
        );
        gl.VertexArrayAttribBinding(vao, 0, bindingIndex);
        
        gl.EnableVertexArrayAttrib(vao, 1);
        gl.VertexArrayAttribFormat(
            vao, 1,
            2, VertexAttribType.Float, false,
            3 * sizeof(float)
        );
        gl.VertexArrayAttribBinding(vao, 1, bindingIndex);
        
        gl.EnableVertexArrayAttrib(vao, 2);
        gl.VertexArrayAttribFormat(
            vao, 2,
            4, VertexAttribType.UnsignedByte, true,
            5 * sizeof(float)
        );
        gl.VertexArrayAttribBinding(vao, 2, bindingIndex);
    }
}