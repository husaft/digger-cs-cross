using Digger.API;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Spectre.Console;

// ReSharper disable InconsistentNaming
namespace Digger.Spectre
{
    internal sealed class ConDigger : AppletCompat, IFactory
    {
        internal IDigger _digger;

        private readonly CanvasImage _bitmap;

        public ConDigger(IDigger digger)
        {
            _digger = digger;
            _bitmap = Helper.CreateNewCanvas();
        }

        public CanvasImage OnDraw()
        {
            var pc = _digger.GetPc();

            var w = pc.GetWidth();
            var h = pc.GetHeight();
            var model = pc.GetCurrentSource().Model;
            var data = pc.GetPixels().GetRgba32(model);

            var image = Image.LoadPixelData<Rgba32>(data, w, h);
            using var old = _bitmap.Exchange(image);

            return _bitmap;
        }

        protected override bool KeyUp(int key) => _digger.KeyUp(key);
        protected override bool KeyDown(int key) => _digger.KeyDown(key);

        public IRefresher CreateRefresher(IDigger _, IColorModel model)
            => new ConRefresher(model);
    }
}