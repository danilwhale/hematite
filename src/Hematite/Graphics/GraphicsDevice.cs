using Hematite.Windowing;
using Vortice.Mathematics;

namespace Hematite.Graphics;

public abstract class GraphicsDevice(Window window) : IDisposable
{
    public Window Window { get; } = window;

    public abstract void Clear(Color4 color);
    public abstract void Clear(float depth);
    public abstract void Clear(Color4 color, float depth);

    public abstract void Resize(in RectI newSize);
    public abstract void MakeCurrent();
    public abstract void Update();

    public abstract void Dispose();
}