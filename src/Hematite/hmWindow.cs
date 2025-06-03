namespace Hematite;

public sealed class hmWindow
{
    public readonly nint Handle;
    public bool ShouldClose;

    internal hmWindow(nint handle)
    {
        Handle = handle;
    }
}