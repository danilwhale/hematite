using System.Diagnostics.CodeAnalysis;
using Vortice.Mathematics;

namespace Hematite;

public static partial class hmLib
{
    public static partial bool hmTryInitialize();
    public static partial void hmUpdate();
    public static partial void hmDestroy();
    
    public static partial hmWindow? hmMakeWindow(ref readonly hmWindowDescriptor descriptor);
    [return: NotNullIfNotNull(nameof(window))] 
    public static partial hmWindow? hmWindowSetCurrent(hmWindow? window);
    public static partial hmWindow? hmWindowGetCurrent();
    public static partial bool hmWindowShouldClose(hmWindow? window);
    public static partial hmWindowBorder hmWindowGetBorder(hmWindow? window);
    public static partial void hmWindowSetBorder(hmWindow? window, hmWindowBorder border);
    public static partial hmWindowState hmWindowGetState(hmWindow? window);
    public static partial void hmWindowSetState(hmWindow? window, hmWindowState state);
    public static partial string? hmWindowGetTitle(hmWindow? window);
    public static partial void hmWindowSetTitle(hmWindow? window, string? title);
    public static partial SizeI hmWindowGetSize(hmWindow? window);
    public static partial void hmWindowSetSize(hmWindow? window, SizeI size);
    public static partial SizeI hmWindowGetMinSize(hmWindow? window);
    public static partial void hmWindowSetMinSize(hmWindow? window, SizeI minSize);
    public static partial SizeI hmWindowGetMaxSize(hmWindow? window);
    public static partial void hmWindowSetMaxSize(hmWindow? window, SizeI maxSize);
    public static partial Int2 hmWindowGetPosition(hmWindow? window);
    public static partial void hmWindowSetPosition(hmWindow? window, Int2 position);
    public static partial float hmWindowGetOpacity(hmWindow? window);
    public static partial void hmWindowSetOpacity(hmWindow? window, float opacity);
    public static partial void hmDestroyWindow(hmWindow window);
}