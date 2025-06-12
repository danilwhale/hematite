namespace Hematite;

public static partial class hmLib
{
    public static partial bool hmTryInitialize()
    {
        return Platform.TryInitialize() && Backend.TryInitialize();
    }

    public static partial void hmUpdate()
    {
        Platform.Update();
        if (Window is not null) Platform.WindowUpdate(Window);
    }

    public static partial void hmDestroy()
    {
        Backend.Destroy();
        Platform.Destroy();
    }
}