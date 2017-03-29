using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TemplatePrinter
{
    public struct SizeM
    {
        public static readonly SizeM Empty;

        private Measure width;

        private Measure height;

        public Measure Width
        {
            get { return width; }
            set { width = value; }
        }

        public Measure Height
        {
            get { return height; }
            set { height = value; }
        }

        public SizeM(Measure width, Measure height)
        {
            this.width = width;
            this.height = height;
        }

        public static SizeM FromSize(SizeF size, UnitOfMeasure unit)
        {
            return new SizeM(Measure.FromUnitMeasure(unit, size.Width), Measure.FromUnitMeasure(unit, size.Height));
        }

        public SizeF ToDisplay(double dpi)
        {
            return new SizeF((float)Width.Pixels(dpi), (float)Height.Pixels(dpi));
        }
    }
}
