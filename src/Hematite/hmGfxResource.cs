namespace Hematite;

public abstract class hmGfxResource
{
    public readonly hmWindow Owner;
    public readonly uint Handle;

    protected internal hmGfxResource(hmWindow owner, uint handle)
    {
        Owner = owner;
        Handle = handle;
    }
}