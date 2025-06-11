using Hematite.Backends;
using Hematite.Platforms;

namespace Hematite;

public static partial class hmLib
{
    internal static readonly hmIPlatform Platform = new hmSDLPlatform();
    internal static readonly hmIGfxBackend GfxBackend = new hmGLBackend();
    
    public static partial bool hmTryInitialize()
    {
        return Platform.TryInitialize() && GfxBackend.TryInitialize();
    }

    public static partial void hmUpdate()
    {
        Platform.Update();
        if (Window != null) Platform.WindowUpdate(Window);
    }

    public static partial void hmDestroy()
    {
        GfxBackend.Destroy();
        Platform.Destroy();
    }
}