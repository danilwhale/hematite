using System.Collections.Frozen;

namespace Hematite.Graphics;

public sealed class hmVertexFormat
{
    public readonly FrozenSet<hmVertexFormatElement> Elements;
    public readonly uint Stride;

    public hmVertexFormat(FrozenSet<hmVertexFormatElement> elements)
    {
        Elements = elements;
        foreach (hmVertexFormatElement e in elements)
        {
            Stride += e.SizeInBytes;
        }
    }
    
    public hmVertexFormat(params hmVertexFormatElement[] elements)
        : this(elements.ToFrozenSet())
    {
    }
}