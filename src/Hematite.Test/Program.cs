using Hematite.Windowing;
using Hematite.Windowing.SDL;
using Vortice.Mathematics;

SdlPlatform.Register();

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