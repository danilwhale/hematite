namespace Hematite.Graphics;

public sealed unsafe class hmBufferData
{
    public readonly hmBuffer Owner;
    
    public readonly void* Data;
    public readonly uint SizeInBytes;
    public readonly hmBufferAccess Access;

    internal hmBufferData(hmBuffer owner, void* data, uint sizeInBytes, hmBufferAccess access)
    {
        Owner = owner;
        Data = data;
        SizeInBytes = sizeInBytes;
        Access = access;
    }

    public Span<T> AsSpan<T>() where T : unmanaged
    {
        int toSize = sizeof(T);
        return new Span<T>(Data, checked((int)(toSize == 1 ? SizeInBytes : SizeInBytes / (uint)toSize)));
    }
}