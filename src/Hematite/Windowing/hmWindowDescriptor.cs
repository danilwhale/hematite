using Vortice.Mathematics;

namespace Hematite.Windowing;

public readonly struct hmWindowDescriptor()
{
    public string Title { get; init; } = "";
    public SizeI Size { get; init; } = new(1024, 768);
    public Int2? Position { get; init; }
    public bool AlwaysOnTop { get; init; }
    public bool Transparent { get; init; }
    public bool NotFocusable { get; init; }
    public hmWindowBorder Border { get; init; }
    public hmWindowState State { get; init; }
}