using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TemplatePrinter
{
    public struct RectangleM
    {
        public static readonly RectangleM Empty;

        private Measure _X;
        private Measure _Y;
        private Measure _Width;
        private Measure _Height;

        public Measure X
        {
            get { return _X; }
            set { _X = value; }
        }

        public Measure Y
        {
            get { return _Y; }
            set { _Y = value; }
        }

        public Measure Width
        {
            get { return _Width; }
            set { _Width = value; }
        }

        public Measure Height
        {
            get { return _Height; }
            set { _Height = value; }
        }

        public PointM Location
        {
            get { return new PointM(X, Y); }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public SizeM Size
        {
            get { return new SizeM(Width, Height); }
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        public RectangleM(Measure x, Measure y, Measure width, Measure height)
        {
            _X = x;
            _Y = y;
            _Width = width;
            _Height = height;
        }
        public RectangleM(PointM pos, SizeM size)
        {
            _X = pos.X;
            _Y = pos.Y;
            _Width = size.Width;
            _Height = size.Height;
        }
        public RectangleF ToDisplay(double dpi)
        {
            return new RectangleF((float)X.Pixels(dpi), (float)Y.Pixels(dpi), (float)Width.Pixels(dpi), (float)Height.Pixels(dpi));
        }
    }
}
