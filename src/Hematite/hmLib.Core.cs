using Hematite.Graphics.Backends;
using Hematite.Windowing.Platforms;

namespace Hematite;

public static partial class hmLib
{
    internal static readonly hmIPlatform Platform = new hmSDLPlatform();
    internal static readonly hmIBackend Backend = new hmGLBackend();
    
    public static partial bool hmTryInitialize()
    {
        return Platform.TryInitialize() && Backend.TryInitialize();
    }

    public static partial void hmUpdate()
    {
        Platform.Update();
        if (Window != null) Platform.WindowUpdate(Window);
    }

    public static partial void hmDestroy()
    {
        Backend.Destroy();
        Platform.Destroy();
    }
}