using Hematite.Platforms;
using Vortice.Mathematics;

namespace Hematite;

public static partial class hmLib
{
    private static readonly hmSDLPlatform Platform = new();

    [ThreadStatic] internal static hmContext? Context;

    private static hmWindow? GetCurrentWindowOrNull(hmWindow? window)
    {
        if (window == null)
        {
            if (Context?.Window is null) return null;
            window ??= Context.Window;
        }

        return window;
    }

    public static partial bool hmTryInitialize()
    {
        return Platform.TryInitialize();
    }

    public static partial void hmUpdate()
    {
        Platform.Update();
    }

    public static partial void hmDestroy()
    {
        Platform.Destroy();
    }

    public static partial hmContext hmMakeContext()
    {
        return new hmContext();
    }

    public static partial hmContext? hmContextSetCurrent(hmContext? context)
    {
        hmContext? oldContext = Context;
        Context = context;
        return oldContext;
    }

    public static partial hmContext? hmContextGetCurrent()
    {
        return Context;
    }

    public static partial void hmDestroyContext(hmContext context)
    {
        if (context.Window != null) hmDestroyWindow(context.Window);
    }

    public static partial hmWindow? hmMakeWindow(ref readonly hmWindowDescriptor descriptor)
    {
        return Platform.MakeWindow(in descriptor);
    }

    public static partial hmWindow? hmContextSetCurrentWindow(hmWindow? window)
    {
        if (Context == null) return null;
        hmWindow? oldWindow = Context.Window;
        Context.Window = window;
        return oldWindow;
    }

    public static partial hmWindow? hmContextGetCurrentWindow()
    {
        return Context?.Window;
    }

    public static partial bool hmWindowShouldClose(hmWindow? window)
    {
        window ??= GetCurrentWindowOrNull(window);
        return window == null || Platform.WindowShouldClose(window);
    }

    public static partial hmWindowBorder hmWindowGetBorder(hmWindow? window)
    {
        window ??= GetCurrentWindowOrNull(window);
        return window == null ? hmWindowBorder.Fixed : Platform.WindowGetBorder(window);
    }

    public static partial void hmWindowSetBorder(hmWindow? window, hmWindowBorder border)
    {
        window ??= GetCurrentWindowOrNull(window);
        if (window == null) return;
        Platform.WindowSetBorder(window, border);
    }

    public static partial hmWindowState hmWindowGetState(hmWindow? window)
    {
        window ??= GetCurrentWindowOrNull(window);
        return window == null ? hmWindowState.Normal : Platform.WindowGetState(window);
    }

    public static partial void hmWindowSetState(hmWindow? window, hmWindowState state)
    {
        window ??= GetCurrentWindowOrNull(window);
        if (window == null) return;
        Platform.WindowSetState(window, state);
    }

    public static partial string? hmWindowGetTitle(hmWindow? window)
    {
        window ??= GetCurrentWindowOrNull(window);
        return window == null ? null : Platform.WindowGetTitle(window);
    }

    public static partial void hmWindowSetTitle(hmWindow? window, string? title)
    {
        window ??= GetCurrentWindowOrNull(window);
        if (window == null) return;
        Platform.WindowSetTitle(window, title);
    }

    public static partial SizeI hmWindowGetSize(hmWindow? window)
    {
        window ??= GetCurrentWindowOrNull(window);
        return window == null ? default : Platform.WindowGetSize(window);
    }

    public static partial void hmWindowSetSize(hmWindow? window, SizeI size)
    {
        window ??= GetCurrentWindowOrNull(window);
        if (window == null) return;
        Platform.WindowSetSize(window, size);
    }

    public static partial SizeI hmWindowGetMinSize(hmWindow? window)
    {
        window ??= GetCurrentWindowOrNull(window);
        return window == null ? default : Platform.WindowGetMinSize(window);
    }

    public static partial void hmWindowSetMinSize(hmWindow? window, SizeI minSize)
    {
        window ??= GetCurrentWindowOrNull(window);
        if (window == null) return;
        Platform.WindowSetMinSize(window, minSize);
    }

    public static partial SizeI hmWindowGetMaxSize(hmWindow? window)
    {
        window ??= GetCurrentWindowOrNull(window);
        return window == null ? default : Platform.WindowGetMaxSize(window);
    }

    public static partial void hmWindowSetMaxSize(hmWindow? window, SizeI maxSize)
    {
        window ??= GetCurrentWindowOrNull(window);
        if (window == null) return;
        Platform.WindowSetMaxSize(window, maxSize);
    }

    public static partial Int2 hmWindowGetPosition(hmWindow? window)
    {
        window ??= GetCurrentWindowOrNull(window);
        return window == null ? default : Platform.WindowGetPosition(window);
    }

    public static partial void hmWindowSetPosition(hmWindow? window, Int2 position)
    {
        window ??= GetCurrentWindowOrNull(window);
        if (window == null) return;
        Platform.WindowSetPosition(window, position);
    }
    
    public static partial float hmWindowGetOpacity(hmWindow? window)
    {
        window ??= GetCurrentWindowOrNull(window);
        return window == null ? 0 : Platform.WindowGetOpacity(window);
    }

    public static partial void hmWindowSetOpacity(hmWindow? window, float opacity)
    {
        window ??= GetCurrentWindowOrNull(window);
        if (window == null) return;
        Platform.WindowSetOpacity(window, opacity);
    }

    public static partial void hmDestroyWindow(hmWindow window)
    {
        Platform.DestroyWindow(window);
    }
}