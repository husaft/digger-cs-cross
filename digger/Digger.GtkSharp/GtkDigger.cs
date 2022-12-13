using Cairo;
using Digger.API;
using DiggerClassic;
using Gdk;

namespace Digger.GtkSharp
{
    internal sealed class GtkDigger : AppletCompat, IFactory
    {
        public IDigger _digger;

        public GtkDigger(IDigger digger)
        {
            _digger = digger;
        }

        protected override bool OnDrawn(Context g)
        {
            g.Scale(4, 4);

            var pc = _digger.GetPc();

            var w = pc.GetWidth();
            var h = pc.GetHeight();
            var data = pc.GetPixels().GetRgb24(pc.GetCurrentSource().Model);

            const int shift = 1;

            /*
            for (var x = 0; x < w; x++)
            {
                for (var y = 0; y < h; y++)
                {
                    var arrayIndex = y * w + x;
                    var (sr, sg, sb) = pc.GetCurrentSource().Model.GetColor(data[arrayIndex]);
                    g.SetSourceRGB(sr, sg, sb);
                    g.Rectangle(x + shift, y + shift, 1, 1);
                    g.Fill();
                }
            }
            */

            using var picture = new Pixbuf(data, Colorspace.Rgb, false, 8, w, h, w * 3, null);
            CairoHelper.SetSourcePixbuf(g, picture, shift, shift);
            g.Paint();

            return false;
        }

        protected override bool KeyUp(int key) => _digger.KeyUp(key);
        protected override bool KeyDown(int key) => _digger.KeyDown(key);

        public IRefresher CreateRefresher(IDigger digger, IColorModel model)
            => new GtkRefresher(this, model);
    }
}