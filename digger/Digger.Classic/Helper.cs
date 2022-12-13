using Digger.API;

// ReSharper disable ForCanBeConvertedToForeach
namespace DiggerClassic
{
    public static class Helper
    {
        public static byte[] GetRgb24(this int[] pixels, IColorModel model)
        {
            var bytes = new byte[pixels.Length * 3];
            var count = 0;
            for (var i = 0; i < pixels.Length; i++)
            {
                var (r, g, b) = model.GetColor(pixels[i]);
                bytes[count++] = (byte)r;
                bytes[count++] = (byte)g;
                bytes[count++] = (byte)b;
            }
            return bytes;
        }
    }
}