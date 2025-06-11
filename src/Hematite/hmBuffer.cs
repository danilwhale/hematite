namespace Hematite;

public sealed class hmBuffer : hmGfxResource
{
    public readonly uint SizeInBytes;
    internal bool Locked;
    
    internal hmBuffer(hmWindow owner, uint handle, uint sizeInBytes) : base(owner, handle)
    {
        SizeInBytes = sizeInBytes;
    }
}