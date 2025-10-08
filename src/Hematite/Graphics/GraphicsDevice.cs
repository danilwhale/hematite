using Hematite.Windowing;
using Vortice.Mathematics;

namespace Hematite.Graphics;

public abstract class GraphicsDevice(Window window) : IDisposable
{
    public Window Window { get; } = window;

    public abstract void Clear(ClearDeviceMask mask, Color4? color = null, float? depth = null);

    public abstract void Resize(in RectI newSize);
    public abstract void MakeCurrent();
    public abstract void Update();

    public abstract void Dispose();
}