namespace Hematite.Platforms;

[Flags]
public enum DriverApi
{
    Automatic = 0, // shouldn't be used by anything other than WindowDescriptor
    OpenGl = 1 << 0,
    Vulkan = 1 << 1,
    DirectX = 1 << 2,
    Metal = 1 << 3, // def not supported for closest time being
    Unknown = 1 << 4 // in case you're feeling funny and want to add software renderer or other shit
}