using Hematite.Windowing;
using Silk.NET.Core.Contexts;
using Silk.NET.OpenGL;
using Vortice.Mathematics;

namespace Hematite.Graphics.OpenGL;

public sealed class OpenGLDevice : GraphicsDevice
{
    public GL Gl { get; }
    public IOpenGLContext Context { get; }
    public IGLContext SilkContext { get; }

    internal OpenGLDevice(Window window, GL gl, GLContextAdapter adapter) : base(window)
    {
        Gl = gl;
        Context = adapter.Value;
        SilkContext = adapter;
    }

    public override void Clear(Color4 color)
    {
        Gl.ClearColor(color.R, color.G, color.B, color.A);
        Gl.Clear(ClearBufferMask.ColorBufferBit);
    }

    public override void Clear(float depth)
    {
        Gl.ClearDepth(depth);
        Gl.Clear(ClearBufferMask.DepthBufferBit);
    }

    public override void Clear(Color4 color, float depth)
    {
        Gl.ClearColor(color.R, color.G, color.B, color.A);
        Gl.ClearDepth(depth);
        Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }

    public override void Resize(in RectI newSize)
    {
        Gl.Viewport(newSize.X, newSize.Y, (uint)newSize.Width, (uint)newSize.Height);
    }

    public override void MakeCurrent()
    {
        Context.MakeCurrent();
    }

    public override void Update()
    {
        Context.SwapBuffers();
    }

    public override void Dispose()
    {
        Gl.Dispose();
    }
}