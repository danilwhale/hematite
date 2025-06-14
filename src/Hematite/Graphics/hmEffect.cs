using Hematite.Windowing;

namespace Hematite.Graphics;

// todo idea: maybe add ability to attach custom shaders and then link it all together?
public sealed class hmEffect : hmGfxResource<hmWindow>
{
    internal hmEffect(hmWindow owner, uint handle) : base(owner, handle)
    {
    }
}