namespace Hematite.Graphics;

public enum PixelFormat : long
{
    // RGB (unsigned, normalized byte)
    A8,
    R8,
    Rg8,
    Rgb4,
    Rgba8,
    R16,
    Rg16,
    Rgba16,
    Rgb10A2,
    B5G6R5,
    B5G5R5A1,
    Bgra4,
    Bgra8,
    
    // sRGB (unsigned, normalized byte)
    SRgba8,
    SBgra8,
    
    // RGB (unsigned, 32-bit integers)
    R32,
    Rg32,
    Rgba32,
    
    // RGB (signed, floating point)
    RHalf,
    RgHalf,
    RgbaHalf,
    RSingle,
    RgSingle,
    RgbaSingle,
    
}