namespace Hematite;

// todo idea: maybe add ability to attach custom shaders and then link it all together?
public sealed class hmEffect
{
    public readonly hmWindow Owner;
    public readonly uint Handle;

    internal hmEffect(hmWindow owner, uint handle)
    {
        Owner = owner;
        Handle = handle;
    }
}