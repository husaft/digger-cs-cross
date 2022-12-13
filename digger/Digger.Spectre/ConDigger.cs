using System;
using Digger.API;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
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
            var play = pc.GetPlayer();
            const int boxSize = 20;
            const int moxSizeX = 8 * boxSize;
            const int moxSizeY = 5 * boxSize;
            if (play.vis)
                image.Mutate(c =>
                {
                    var ax = play.x - boxSize;
                    if (ax < 0)
                        ax = Math.Max(0, ax);
                    var ay = play.y - boxSize;
                    if (ay < 0)
                        ay = Math.Max(0, ay);

                    var max = ax + moxSizeX;
                    if (max > w)
                        ax -= max - w;
                    var may = ay + moxSizeY;
                    if (may > h)
                        ay -= may - h;

                    c.Crop(new Rectangle(ax, ay, moxSizeX, moxSizeY));
                });
            using var old = _bitmap.Exchange(image);

            return _bitmap;
        }

        protected override bool KeyUp(int key) => _digger.KeyUp(key);
        protected override bool KeyDown(int key) => _digger.KeyDown(key);

        public IRefresher CreateRefresher(IDigger _, IColorModel model)
            => new ConRefresher(model);
    }
}