# hematite

## a low-level gamedev library

> [!CAUTION]
> while hematite can sound really cool, it's still *extremely* early in development. you can check
> out the library later, or even help if you're bored enough. any kind of help, including reporting
> issues or contributing with pull requests, is appreciated.

hematite is a low-level game development library, inspired by raylib and Silk.NET, built for both simple prototypes
and games. the library is written completely in C#, although it may look like it's
a binding for a native library, it's not. the library is styled like a C library to take a rest
after passing resources like a hot potato around the project.

### features

- basic multiple window support and easy-to-implement custom platform, in case you don't like builtin SDL3 for whatever
  reason.
- todo.

### hello, world!

> [!CAUTION]
> as stated before, the library is still early in development. an API is subject to change at any moment
> and isn't guaranteed to stay stable.

```csharp
using Hematite;
using Vortice.Mathematics;
using static Hematite.hmLib;

if (!hmTryInitialize()) return 1;

hmWindowDescriptor descriptor = new()
{
    Title = "hello, world!",
    Size = new SizeI(854, 480),
    Border = hmWindowBorder.Resizable
};

hmWindow window = hmWindowSetCurrent(hmMakeWindow(in descriptor)!);

while (!hmWindowShouldClose(null))
{
    hmUpdate();
}

hmDestroyWindow(window);
hmDestroy();
return 0;
```