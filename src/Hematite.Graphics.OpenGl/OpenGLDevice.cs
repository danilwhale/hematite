using Hematite.Windowing;
using Silk.NET.Core.Contexts;
using Silk.NET.OpenGL;
using Vortice.Mathematics;

namespace Hematite.Graphics.OpenGl;

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

    public override void Clear(ClearDeviceMask mask, Color4? color = null, float? depth = null)
    {
        if (color is { } c) Gl.ClearColor(c.R, c.G, c.B, c.A);
        if (depth is { } d) Gl.ClearDepth(d);
        
        ClearBufferMask glMask = 0;
        if ((mask & ClearDeviceMask.Color) != 0) glMask |= ClearBufferMask.ColorBufferBit;
        if ((mask & ClearDeviceMask.Depth) != 0) glMask |= ClearBufferMask.DepthBufferBit;
        if ((mask & ClearDeviceMask.Stencil) != 0) glMask |= ClearBufferMask.StencilBufferBit;
        Gl.Clear(glMask);
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