using System.Numerics;
using Hematite.Resources;
using Silk.NET.OpenGL;
using Vortice.Mathematics;

namespace Hematite.Rendering;

public sealed class ImmediateRenderer(GL gl, Effect effect, ushort bufferQuadCount = 16383) : IDisposable
{
    public static readonly Lazy<ImmediateRenderer> Instance =
        new(() => new ImmediateRenderer(Hem.Gl, Effect.Simple.Value));

    public readonly BufferBuilder<SimpleVertex> Builder = new(gl, bufferQuadCount);
    public readonly Effect Effect = effect;

    private Vector2 _texCoord;
    private Color _color;

    public void Begin(bool quads = true)
    {
        Builder.Begin(quads);
    }

    public void End()
    {
        Builder.End();
        Builder.Render();
    }

    public void TexCoord(float x, float y) => _texCoord = new Vector2(x, y);
    public void TexCoord(in Vector2 texCoord) => _texCoord = texCoord;

    public void Color(float r, float g, float b, float a = 1) => _color = new Color(r, g, b, a);
    public void Color(byte r, byte g, byte b, byte a = 255) => _color = new Color(r, g, b, a);
    public void Color(in Color color) => _color = color;

    public void Vertex(float x, float y, float z) =>
        Builder.Add(new SimpleVertex(new Vector3(x, y, z), in _texCoord, in _color));

    public void Vertex(in Vector3 position) => Builder.Add(new SimpleVertex(in position, in _texCoord, in _color));
    public void Vertex(in SimpleVertex vertex) => Builder.Add(in vertex);
#if NET9_0_OR_GREATER
    public void Vertices(params ReadOnlySpan<SimpleVertex> vertices) => Builder.Add(vertices);
#else
    public void Vertices(params SimpleVertex[] vertices) => Builder.Add(vertices);
#endif

    public void RenderTri(Vector3 a, Vector3 b, Vector3 c, Color color)
    {
        Effect.Use();
        Texture2D.WhiteTexture.Value.Bind();
        Begin(quads: false);
        Vertices(
            new SimpleVertex(in a, Vector2.Zero, in color),
            new SimpleVertex(in b, Vector2.Zero, in color),
            new SimpleVertex(in c, Vector2.Zero, in color)
        );
        End();
    }

    public void RenderTri(Vector2 a, Vector2 b, Vector2 c, Color color)
    {
        Effect.Use();
        Texture2D.WhiteTexture.Value.Bind();
        Begin(quads: false);
        Vertices(
            new SimpleVertex(new Vector3(a, 0), Vector2.Zero, in color),
            new SimpleVertex(new Vector3(b, 0), Vector2.Zero, in color),
            new SimpleVertex(new Vector3(c, 0), Vector2.Zero, in color)
        );
        End();
    }

    public void RenderRect(Rect rect, Color color)
    {
        Effect.Use();
        Texture2D.WhiteTexture.Value.Bind();
        Begin();
        Vertices(
            new SimpleVertex(new Vector3(rect.Left, rect.Top, 0), Vector2.Zero, in color),
            new SimpleVertex(new Vector3(rect.Left, rect.Bottom, 0), Vector2.Zero, in color),
            new SimpleVertex(new Vector3(rect.Right, rect.Bottom, 0), Vector2.Zero, in color),
            new SimpleVertex(new Vector3(rect.Right, rect.Top, 0), Vector2.Zero, in color)
        );
        End();
    }

    public void RenderTexture(Texture2D texture, Rect? src, Rect dst, Color tint)
    {
        Rect uSrc = src.GetValueOrDefault(new Rect(texture.Width, texture.Height));

        float reciprocalWidth = 1.0f / texture.Width;
        float reciprocalHeight = 1.0f / texture.Height;

        float u0 = uSrc.Left * reciprocalWidth;
        float u1 = uSrc.Right * reciprocalWidth;
        float v0 = uSrc.Top * reciprocalHeight;
        float v1 = uSrc.Bottom * reciprocalHeight;

        Effect.Use();
        texture.Bind();
        Begin();
        Vertices(
            new SimpleVertex(new Vector3(dst.Left, dst.Top, 0), new Vector2(u0, v0), in tint),
            new SimpleVertex(new Vector3(dst.Left, dst.Bottom, 0), new Vector2(u0, v1), in tint),
            new SimpleVertex(new Vector3(dst.Right, dst.Bottom, 0), new Vector2(u1, v1), in tint),
            new SimpleVertex(new Vector3(dst.Right, dst.Top, 0), new Vector2(u1, v0), in tint)
        );
        End();
    }

    // port of raylib's DrawTextureNPatch:
    // https://github.com/raysan5/raylib/blob/924c87db33bded97e244ebd289650896418032ad/src/rtextures.c#L4622
    public void RenderTexture9Grid(
        Texture2D texture, Rect? src, Rect dst,
        float leftBorder, float rightBorder,
        float topBorder, float bottomBorder,
        Color tint)
    {
        Rect uSrc = src.GetValueOrDefault(new Rect(texture.Width, texture.Height));

        float reciprocalWidth = 1.0f / texture.Width;
        float reciprocalHeight = 1.0f / texture.Height;

        float patchWidth = (int)dst.Width <= 0 ? 0 : dst.Width;
        float patchHeight = (int)dst.Height <= 0 ? 0 : dst.Height;

        if (uSrc.Width < 0) uSrc.X -= uSrc.Width;
        if (uSrc.Height < 0) uSrc.Y -= uSrc.Height;

        bool drawCenter = true;
        bool drawMiddle = true;

        // Adjust the lateral (left and right) border widths in case patchWidth < texture.width
        if (patchWidth <= leftBorder + rightBorder)
        {
            drawCenter = false;
            leftBorder = leftBorder / (leftBorder + rightBorder) * patchWidth;
            rightBorder = patchWidth - leftBorder;
        }

        // Adjust the lateral (top and bottom) border heights in case patchHeight < texture.height
        if (patchHeight <= topBorder + bottomBorder)
        {
            drawMiddle = false;
            topBorder = topBorder / (topBorder + bottomBorder) * patchHeight;
            bottomBorder = patchHeight - topBorder;
        }

        Vector2
            vertA = dst.Position,
            vertB = dst.Position + new Vector2(leftBorder, topBorder),
            vertC = dst.Position + new Vector2(patchWidth - rightBorder, patchHeight - bottomBorder),
            vertD = dst.Position + new Vector2(patchWidth, patchHeight);

        Vector2
            coordA = new(uSrc.Left * reciprocalWidth, uSrc.Top * reciprocalHeight),
            coordB = new((uSrc.Left + leftBorder) * reciprocalWidth, (uSrc.Top + topBorder) * reciprocalHeight),
            coordC = new((uSrc.Right - rightBorder) * reciprocalWidth, (uSrc.Bottom - bottomBorder) * reciprocalHeight),
            coordD = new(uSrc.Right * reciprocalWidth, uSrc.Bottom * reciprocalHeight);

        Effect.Use();
        texture.Bind();
        Begin();

        // ------------------------------------------------------------
        // TOP-LEFT QUAD
        Vertices(
            new SimpleVertex(new Vector3(vertA.X, vertB.Y, 0), new Vector2(coordA.X, coordB.Y), in tint),
            new SimpleVertex(new Vector3(vertB.X, vertB.Y, 0), new Vector2(coordB.X, coordB.Y), in tint),
            new SimpleVertex(new Vector3(vertB.X, vertA.Y, 0), new Vector2(coordB.X, coordA.Y), in tint),
            new SimpleVertex(new Vector3(vertA.X, vertA.Y, 0), new Vector2(coordA.X, coordA.Y), in tint)
        );

        if (drawCenter)
        {
            // TOP-CENTER QUAD
            Vertices(
                new SimpleVertex(new Vector3(vertB.X, vertB.Y, 0), new Vector2(coordB.X, coordB.Y), in tint),
                new SimpleVertex(new Vector3(vertC.X, vertB.Y, 0), new Vector2(coordC.X, coordB.Y), in tint),
                new SimpleVertex(new Vector3(vertC.X, vertA.Y, 0), new Vector2(coordC.X, coordA.Y), in tint),
                new SimpleVertex(new Vector3(vertB.X, vertA.Y, 0), new Vector2(coordB.X, coordA.Y), in tint)
            );
        }

        // TOP-RIGHT QUAD
        Vertices(
            new SimpleVertex(new Vector3(vertC.X, vertB.Y, 0), new Vector2(coordC.X, coordB.Y), in tint),
            new SimpleVertex(new Vector3(vertD.X, vertB.Y, 0), new Vector2(coordD.X, coordB.Y), in tint),
            new SimpleVertex(new Vector3(vertD.X, vertA.Y, 0), new Vector2(coordD.X, coordA.Y), in tint),
            new SimpleVertex(new Vector3(vertC.X, vertA.Y, 0), new Vector2(coordC.X, coordA.Y), in tint)
        );

        if (drawMiddle)
        {
            // ------------------------------------------------------------
            // MIDDLE-LEFT QUAD
            Vertices(
                new SimpleVertex(new Vector3(vertA.X, vertC.Y, 0), new Vector2(coordA.X, coordC.Y), in tint),
                new SimpleVertex(new Vector3(vertB.X, vertC.Y, 0), new Vector2(coordB.X, coordC.Y), in tint),
                new SimpleVertex(new Vector3(vertB.X, vertB.Y, 0), new Vector2(coordB.X, coordB.Y), in tint),
                new SimpleVertex(new Vector3(vertA.X, vertB.Y, 0), new Vector2(coordA.X, coordB.Y), in tint)
            );

            if (drawCenter)
            {
                // MIDDLE-CENTER QUAD
                Vertices(
                    new SimpleVertex(new Vector3(vertB.X, vertC.Y, 0), new Vector2(coordB.X, coordC.Y), in tint),
                    new SimpleVertex(new Vector3(vertC.X, vertC.Y, 0), new Vector2(coordC.X, coordC.Y), in tint),
                    new SimpleVertex(new Vector3(vertC.X, vertB.Y, 0), new Vector2(coordC.X, coordB.Y), in tint),
                    new SimpleVertex(new Vector3(vertB.X, vertB.Y, 0), new Vector2(coordB.X, coordB.Y), in tint)
                );
            }

            // MIDDLE-RIGHT QUAD
            Vertices(
                new SimpleVertex(new Vector3(vertC.X, vertC.Y, 0), new Vector2(coordC.X, coordC.Y), in tint),
                new SimpleVertex(new Vector3(vertD.X, vertC.Y, 0), new Vector2(coordD.X, coordC.Y), in tint),
                new SimpleVertex(new Vector3(vertD.X, vertB.Y, 0), new Vector2(coordD.X, coordB.Y), in tint),
                new SimpleVertex(new Vector3(vertC.X, vertB.Y, 0), new Vector2(coordC.X, coordB.Y), in tint)
            );
        }

        // ------------------------------------------------------------
        // BOTTOM-LEFT QUAD 
        Vertices(
            new SimpleVertex(new Vector3(vertA.X, vertD.Y, 0), new Vector2(coordA.X, coordD.Y), in tint),
            new SimpleVertex(new Vector3(vertB.X, vertD.Y, 0), new Vector2(coordB.X, coordD.Y), in tint),
            new SimpleVertex(new Vector3(vertB.X, vertC.Y, 0), new Vector2(coordB.X, coordC.Y), in tint),
            new SimpleVertex(new Vector3(vertA.X, vertC.Y, 0), new Vector2(coordA.X, coordC.Y), in tint)
        );

        if (drawCenter)
        {
            // BOTTOM-CENTER QUAD
            Vertices(
                new SimpleVertex(new Vector3(vertB.X, vertD.Y, 0), new Vector2(coordB.X, coordD.Y), in tint),
                new SimpleVertex(new Vector3(vertC.X, vertD.Y, 0), new Vector2(coordC.X, coordD.Y), in tint),
                new SimpleVertex(new Vector3(vertC.X, vertC.Y, 0), new Vector2(coordC.X, coordC.Y), in tint),
                new SimpleVertex(new Vector3(vertB.X, vertC.Y, 0), new Vector2(coordB.X, coordC.Y), in tint)
            );
        }

        // BOTTOM-RIGHT QUAD
        Vertices(
            new SimpleVertex(new Vector3(vertC.X, vertD.Y, 0), new Vector2(coordC.X, coordD.Y), in tint),
            new SimpleVertex(new Vector3(vertD.X, vertD.Y, 0), new Vector2(coordD.X, coordD.Y), in tint),
            new SimpleVertex(new Vector3(vertD.X, vertC.Y, 0), new Vector2(coordD.X, coordC.Y), in tint),
            new SimpleVertex(new Vector3(vertC.X, vertC.Y, 0), new Vector2(coordC.X, coordC.Y), in tint)
        );

        End();
    }

    public void Dispose()
    {
        Builder.Dispose();
    }
}