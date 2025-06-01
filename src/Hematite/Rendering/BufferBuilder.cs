using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace Hematite.Rendering;

public static class BufferBuilder
{
    private static ReadOnlySpan<ushort> QuadIndices => [0, 1, 2, 0, 2, 3];

    public static void CookIndices(Span<ushort> indices)
    {
        Vector128<ushort> quadIndices = Vector128.LoadUnsafe(ref MemoryMarshal.GetReference(QuadIndices));
        ref ushort rIndices = ref MemoryMarshal.GetReference(indices);
        for (int i = 0, j = 0; i < indices.Length; i += 6, j += 4)
        {
            // what the fuck did i just do
            Unsafe.Add(ref rIndices, i + 0) = (ushort)(j + quadIndices[0]);
            Unsafe.Add(ref rIndices, i + 1) = (ushort)(j + quadIndices[1]);
            Unsafe.Add(ref rIndices, i + 2) = (ushort)(j + quadIndices[2]);
            Unsafe.Add(ref rIndices, i + 3) = (ushort)(j + quadIndices[3]);
            Unsafe.Add(ref rIndices, i + 4) = (ushort)(j + quadIndices[4]);
            Unsafe.Add(ref rIndices, i + 5) = (ushort)(j + quadIndices[5]);
        }
    }
}