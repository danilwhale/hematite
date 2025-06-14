using System.Numerics;
using Hematite.Graphics.Backends;
using Hematite.Input;

namespace Hematite.Windowing;

public sealed class hmWindow
{
    public readonly nint Handle;

    internal readonly hmGfxContext GfxContext;
    internal readonly Dictionary<hmKeyCode, bool> LastKeyboardState = [], KeyboardState = [];
    internal readonly Dictionary<hmMouseButton, bool> LastMouseState = [], MouseState = [];
    internal Vector2 LastMousePosition, MousePosition;
    internal Vector2 MouseWheelVelocity;
    
    internal bool ShouldClose;
    internal bool WasResized;
    internal bool WasMoved;

    internal hmWindow(nint handle, hmGfxContext gfxContext)
    {
        Handle = handle;
        GfxContext = gfxContext;
    }
}