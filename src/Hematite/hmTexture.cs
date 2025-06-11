namespace Hematite;

public sealed class hmTexture
{
    public readonly hmWindow Owner;
    public readonly uint Handle;

    internal hmTexture(hmWindow owner, uint handle)
    {
        Owner = owner;
        Handle = handle;
    }
}