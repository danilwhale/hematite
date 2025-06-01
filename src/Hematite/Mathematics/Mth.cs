using System.Runtime.CompilerServices;

namespace Hematite.Mathematics;

// when there are too many mathhelpers
public static class Mth
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Floor(float x)
    {
        // IL_0000: ldarg.0      // x
        // IL_0001: conv.i4
        // IL_0002: ldarg.0      // x
        // IL_0003: ldc.r4       0.0
        // IL_0008: clt
        // IL_000a: sub
        // IL_000b: ret
        return (int)x - (x < 0 ? 1 : 0);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Floor(double x)
    {
        return (int)x - (x < 0 ? 1 : 0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Ceiling(float x)
    {
        // IL_0000: ldarg.0      // x
        // IL_0001: conv.i4
        // IL_0002: dup
        // IL_0003: conv.r4
        // IL_0004: ldarg.0      // x
        // IL_0005: clt
        // IL_0007: add
        // IL_0008: ret
        int ix = (int)x;
        return ix + (ix < x ? 1 : 0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Ceiling(double x)
    {
        int ix = (int)x;
        return ix + (ix < x ? 1 : 0);
    }
}