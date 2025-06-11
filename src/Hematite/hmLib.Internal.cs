using Hematite.Windowing;

namespace Hematite;

public static partial class hmLib
{
    [ThreadStatic] internal static hmWindow? Window;

    private static hmWindow? GetCurrentWindowOrNull(hmWindow? window)
    {
        if (window is null)
        {
            if (Window is null) return null;
            window ??= Window;
        }

        return window;
    }
}