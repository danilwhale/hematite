using Hematite.Graphics.Backends;
using Hematite.Windowing;
using Hematite.Windowing.Platforms;

namespace Hematite;

public static partial class hmLib
{
    internal static readonly hmIPlatform Platform = new hmSDLPlatform();
    internal static readonly hmIBackend Backend = new hmGLBackend();
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