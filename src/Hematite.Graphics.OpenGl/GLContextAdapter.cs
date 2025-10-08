using System.Diagnostics.CodeAnalysis;
using Silk.NET.Core.Contexts;

namespace Hematite.Graphics.OpenGL;

internal readonly struct GLContextAdapter(IOpenGLContext value) : IGLContext
{
    public readonly IOpenGLContext Value = value;
    
    public IntPtr GetProcAddress(string proc, int? slot = null)
    {
        if (!Value.TryGetProcAddress(proc, out nint address))
        {
            throw new InvalidOperationException(Value.GetLastError());
        }

        return address;
    }

    public bool TryGetProcAddress(string proc, [UnscopedRef] out IntPtr addr, int? slot = null) => Value.TryGetProcAddress(proc, out addr);
    public void SwapInterval(int interval) => Value.SwapInterval(interval);
    public void SwapBuffers() => Value.SwapBuffers();
    public void MakeCurrent() => Value.MakeCurrent();
    public void Clear() => Value.Clear();
    public void Dispose() => Value.Clear();

    public IntPtr Handle => Value.Handle;
    public IGLContextSource? Source => null;
    public bool IsCurrent => Value.IsCurrent;
}