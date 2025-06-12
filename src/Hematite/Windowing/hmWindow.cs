using Hematite.Graphics.Backends;

namespace Hematite.Windowing;

public sealed class hmWindow
{
    public readonly nint Handle;

    internal hmGfxContext GfxContext;

    internal bool ShouldClose;
    internal bool WasResized;
    internal bool WasMoved;

    internal hmWindow(nint handle, hmGfxContext gfxContext)
    {
        Handle = handle;
        GfxContext = gfxContext;
    }
}