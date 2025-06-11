namespace Hematite;

public sealed class hmBuffer
{
    public readonly hmWindow Owner;
    public readonly uint Handle;
    public readonly uint SizeInBytes;
    internal bool Locked;
    
    internal hmBuffer(hmWindow owner, uint handle, uint sizeInBytes)
    {
        Owner = owner;
        Handle = handle;
        SizeInBytes = sizeInBytes;
    }
}