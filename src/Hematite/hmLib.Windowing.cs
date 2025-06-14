using Hematite.Windowing;
using Vortice.Mathematics;

namespace Hematite;

public static partial class hmLib
{
    public static partial hmWindow? hmMakeWindow(ref readonly hmWindowDescriptor descriptor)
    {
        return Platform.MakeWindow(in descriptor);
    }

    public static partial hmWindow? hmWindowSetCurrent(hmWindow? window)
    {
        return CurrentWindow = window;
    }

    public static partial hmWindow? hmWindowGetCurrent()
    {
        return CurrentWindow;
    }

    public static partial bool hmWindowShouldClose(hmWindow? window)
    {
        window ??= CurrentWindow;
        return window is null || window.ShouldClose;
    }
    
    public static partial void hmWindowClose(hmWindow? window)
    {
        window ??= CurrentWindow;
        if (window is null) return;
        window.ShouldClose = true;
    }
    
    public static partial bool hmWindowWasResized(hmWindow? window)
    {
        window ??= CurrentWindow;
        if (window is null) return false;
        bool wasResized = window.WasResized;
        window.WasResized = false;
        return wasResized;
    }

    public static partial bool hmWindowWasMoved(hmWindow? window)
    {
        window ??= CurrentWindow;
        if (window is null) return false;
        bool wasMoved = window.WasMoved;
        window.WasMoved = false;
        return wasMoved;
    }
    
    public static partial void hmWindowUpdate(hmWindow? window)
    {
        window ??= CurrentWindow;
        if (window is null) return;
        UpdateWindow(window);
    }

    public static partial hmWindowBorder hmWindowGetBorder(hmWindow? window)
    {
        window ??= CurrentWindow;
        return window is null ? hmWindowBorder.Fixed : Platform.WindowGetBorder(window);
    }

    public static partial void hmWindowSetBorder(hmWindow? window, hmWindowBorder border)
    {
        window ??= CurrentWindow;
        if (window is null) return;
        Platform.WindowSetBorder(window, border);
    }

    public static partial hmWindowState hmWindowGetState(hmWindow? window)
    {
        window ??= CurrentWindow;
        return window is null ? hmWindowState.Normal : Platform.WindowGetState(window);
    }

    public static partial void hmWindowSetState(hmWindow? window, hmWindowState state)
    {
        window ??= CurrentWindow;
        if (window is null) return;
        Platform.WindowSetState(window, state);
    }

    public static partial string? hmWindowGetTitle(hmWindow? window)
    {
        window ??= CurrentWindow;
        return window is null ? null : Platform.WindowGetTitle(window);
    }

    public static partial void hmWindowSetTitle(hmWindow? window, string? title)
    {
        window ??= CurrentWindow;
        if (window is null) return;
        Platform.WindowSetTitle(window, title);
    }

    public static partial SizeI hmWindowGetSize(hmWindow? window)
    {
        window ??= CurrentWindow;
        return window is null ? default : Platform.WindowGetSize(window);
    }

    public static partial void hmWindowSetSize(hmWindow? window, SizeI size)
    {
        window ??= CurrentWindow;
        if (window is null) return;
        Platform.WindowSetSize(window, size);
    }

    public static partial SizeI hmWindowGetMinSize(hmWindow? window)
    {
        window ??= CurrentWindow;
        return window is null ? default : Platform.WindowGetMinSize(window);
    }

    public static partial void hmWindowSetMinSize(hmWindow? window, SizeI minSize)
    {
        window ??= CurrentWindow;
        if (window is null) return;
        Platform.WindowSetMinSize(window, minSize);
    }

    public static partial SizeI hmWindowGetMaxSize(hmWindow? window)
    {
        window ??= CurrentWindow;
        return window is null ? default : Platform.WindowGetMaxSize(window);
    }

    public static partial void hmWindowSetMaxSize(hmWindow? window, SizeI maxSize)
    {
        window ??= CurrentWindow;
        if (window is null) return;
        Platform.WindowSetMaxSize(window, maxSize);
    }

    public static partial Int2 hmWindowGetPosition(hmWindow? window)
    {
        window ??= CurrentWindow;
        return window is null ? default : Platform.WindowGetPosition(window);
    }

    public static partial void hmWindowSetPosition(hmWindow? window, Int2 position)
    {
        window ??= CurrentWindow;
        if (window is null) return;
        Platform.WindowSetPosition(window, position);
    }
    
    public static partial float hmWindowGetOpacity(hmWindow? window)
    {
        window ??= CurrentWindow;
        return window is null ? 0 : Platform.WindowGetOpacity(window);
    }

    public static partial void hmWindowSetOpacity(hmWindow? window, float opacity)
    {
        window ??= CurrentWindow;
        if (window is null) return;
        Platform.WindowSetOpacity(window, opacity);
    }

    public static partial void hmDestroyWindow(hmWindow window)
    {
        window.GfxContext.Dispose();
        Platform.DestroyWindow(window);
    }
}