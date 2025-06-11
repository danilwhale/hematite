using Hematite.Backends;

namespace Hematite.Windowing;

public sealed class hmWindow
{
    public readonly nint Handle;
    public bool ShouldClose;
    internal hmGfxContext GfxContext;

    internal hmWindow(nint handle, hmGfxContext gfxContext)
    {
        Handle = handle;
        GfxContext = gfxContext;
    }
}