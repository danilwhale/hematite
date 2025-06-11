namespace Hematite;

public readonly struct hmVertexFormatElement(hmVertexFormatElementType type, bool normalized, uint size) 
    : IEquatable<hmVertexFormatElement>
{
    public readonly hmVertexFormatElementType Type = type;
    public readonly bool Normalized = normalized;
    public readonly uint Size = size;

    public uint SizeInBytes => (uint)(Size * Type switch
    {
        hmVertexFormatElementType.Byte or hmVertexFormatElementType.UnsignedByte => 1,
        hmVertexFormatElementType.Short or hmVertexFormatElementType.UnsignedShort => 2,
        hmVertexFormatElementType.Int or hmVertexFormatElementType.UnsignedInt => 4,
        hmVertexFormatElementType.HalfFloat => 2,
        hmVertexFormatElementType.Float => 4,
        hmVertexFormatElementType.Double => 8
    });

    public bool Equals(hmVertexFormatElement other)
    {
        return Type == other.Type && Normalized == other.Normalized && Size == other.Size;
    }

    public override bool Equals(object? obj)
    {
        return obj is hmVertexFormatElement other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)Type, Normalized, Size);
    }

    public static bool operator ==(hmVertexFormatElement left, hmVertexFormatElement right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(hmVertexFormatElement left, hmVertexFormatElement right)
    {
        return !left.Equals(right);
    }
}