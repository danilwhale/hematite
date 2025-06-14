namespace Hematite;

public static partial class hmLib
{
    public static partial bool hmTryInitialize()
    {
        return Platform.TryInitialize() && Backend.TryInitialize();
    }

    public static partial void hmUpdate()
    {
        if (CurrentWindow is not null) UpdateWindow(CurrentWindow);
        Platform.Update();
    }

    public static partial void hmDestroy()
    {
        Backend.Destroy();
        Platform.Destroy();
    }
}