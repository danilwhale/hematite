using Hematite.Graphics;
using Hematite.Platforms;
using Vortice.Mathematics;

namespace Hematite.Windowing;

public readonly record struct WindowDescriptor(
    string Title,
    SizeI Size,
    Int2? Position = null,
    bool AlwaysOnTop = false,
    bool Transparent = false,
    bool NotFocusable = false,
    WindowBorder Border = WindowBorder.Fixed,
    WindowState State = WindowState.Normal,
    DriverApi Api = DriverApi.Automatic)
{
    public static readonly WindowDescriptor Default = new("hematite window", new SizeI(1024, 768));
}