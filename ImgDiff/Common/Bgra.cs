using System.Runtime.InteropServices;

namespace ImgDiff.Common
{
    /// <summary>
    /// Bgra color struct. Used for fast color packing/unpacking
    /// </summary>
    /// <remarks>Be aware of endianness when using this struct</remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct Bgra
    {
        public byte B;
        public byte G;
        public byte R;
        public byte A;

        public Bgra(byte b, byte g, byte r, byte a)
        {
            B = b;
            G = g;
            R = r;
            A = a;
        }
    }
}
