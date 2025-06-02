# hematite
### a crappy framework to get started with the game faster
lightweight and crappy framework based on OpenGL 4.5, made to start developing the game faster
and not waste time on reimplementing the wheel over and over.

this framework is *early in development* and i just wanted to separate boilerplate rendering code 
from my game as soon as possible. so use it for your own risk, don't expect to have a full fledged and
professional framework with rich class library.

to include the framework you need to add it as git submodule, or just clone it into your solution
folder, and add `Hematite` project to your solution.

to work with lazy initialized instances of objects you need to set `Hem.Gl` to OpenGL context with
support for at least OpenGL 4.5 (soon should be 3.3, but don't expect it to happen *really* soon).

to dispose all initialized objects you can call `Hem.DisposeAll`
> [!CAUTION]
> do NOT try to access lazy initiailized objects after you call `Hem.DisposeAll`!!!
> they are NOT getting reset to default values and remain invalid *forever*.
> you should call `Hem.DisposeAll` *only* after the game lifecycle ends

### todo
- [ ] add batching to ImmediateRenderer
- [ ] add nuget package
