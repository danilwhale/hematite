using Hematite.Windowing;

namespace Hematite.Graphics;

public sealed class hmTexture : hmGfxResource
{
    internal hmTexture(hmWindow owner, uint handle) : base(owner, handle)
    {
    }
}