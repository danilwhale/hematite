using Hematite;
using Hematite.Windowing;
using Hematite.Windowing.SDL;
using Vortice.Mathematics;

SdlPlatform.Register();

// try to force SDL3 platform (in case you're registering several platforms)
Platform.SetTarget(SdlPlatform.Identifier); 

WindowDescriptor descriptor = new(
    Title: "test", 
    Size: new SizeI(640, 480),
    Border: WindowBorder.Resizable);

using Window window = Window.Make(in descriptor);
window.MakeCurrent();
while (!window.ShouldClose)
{
    window.Update();
}