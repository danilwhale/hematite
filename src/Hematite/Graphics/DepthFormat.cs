namespace Hematite.Graphics;

public enum DepthFormat
{
    D0, // no depth buffer

    // D (depth bits) S (stencil bits)
    // unsigned integers
    D16,
    D24,
    D24S8,
    
    // floating point (+ unsigned byte)
    DSingle,
    DSingleS8
}