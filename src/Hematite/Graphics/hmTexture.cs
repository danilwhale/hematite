using Hematite.Windowing;

namespace Hematite.Graphics;

public sealed class hmTexture : hmGfxResource<hmWindow>
{
    internal hmTexture(hmWindow owner, uint handle) : base(owner, handle)
    {
    }
}