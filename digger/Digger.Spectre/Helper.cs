using System;
using System.Linq;
using System.Reflection;
using Digger.API;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Spectre.Console;

// ReSharper disable ForCanBeConvertedToForeach
namespace Digger.Spectre
{
    internal static class Helper
    {
        public static ReadOnlySpan<byte> GetRgba32(this int[] pixels, IColorModel model)
        {
            var bytes = new byte[pixels.Length * 4];
            var count = 0;
            for (var i = 0; i < pixels.Length; i++)
            {
                var (r, g, b) = model.GetColor(pixels[i]);
                bytes[count++] = (byte)r;
                bytes[count++] = (byte)g;
                bytes[count++] = (byte)b;
                bytes[count++] = byte.MaxValue;
            }
            return bytes;
        }

        private const string EmptyBmp = "Qk06AAAAAAAAADYAAAAoAAAAAQAAAAEAAAABABgA" +
                                        "AAAAAAQAAADEDgAAxA4AAAAAAAAAAAAAAAAAAA==";

        public static CanvasImage CreateNewCanvas()
        {
            var bytes = Convert.FromBase64String(EmptyBmp);
            return new CanvasImage(bytes);
        }

        private static readonly Lazy<FieldInfo> ImageField = new(() =>
        {
            var t = typeof(CanvasImage);
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            return t.GetFields(flags).First(f => f.Name.StartsWith("<Image>"));
        });

        public static Image<Rgba32> Exchange(this CanvasImage bitmap, Image<Rgba32> image)
        {
            var field = ImageField.Value;
            var oldImage = field.GetValue(bitmap);
            field.SetValue(bitmap, image);
            return oldImage as Image<Rgba32>;
        }
    }
}