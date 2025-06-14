namespace Hematite.Graphics;

public abstract class hmGfxResource<TOwner> where TOwner : class
{
    public readonly TOwner Owner;
    public readonly uint Handle;

    protected internal hmGfxResource(TOwner owner, uint handle)
    {
        Owner = owner;
        Handle = handle;
    }
}