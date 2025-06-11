using System.Runtime.InteropServices;

namespace Hematite.Graphics;

public readonly ref struct hmBufferData
{
    public readonly hmBuffer Owner;
    
    public readonly Span<byte> Data;
    public readonly hmBufferAccess Access;

    internal hmBufferData(hmBuffer owner, Span<byte> data, hmBufferAccess access)
    {
        Owner = owner;
        Data = data;
        Access = access;
    }

    public Span<T> AsSpan<T>() where T : unmanaged
    {
        return MemoryMarshal.Cast<byte, T>(Data);
    }
}