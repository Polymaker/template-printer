using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TemplatePrinter
{
    public struct PointM
    {
        public static readonly PointM Empty;

        private Measure _X;
        private Measure _Y;

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

        public PointM(Measure x, Measure y)
        {
            _X = x;
            _Y = y;
        }

        public static PointM FromPoint(PointF point, UnitOfMeasure unit)
        {
            return new PointM(Measure.FromUnitMeasure(unit, point.X), Measure.FromUnitMeasure(unit, point.Y));
        }

        public PointF ToDisplay(double dpi)
        {
            return new PointF((float)X.Pixels(dpi), (float)Y.Pixels(dpi));
        }

        public static PointM operator +(PointM pt1, PointM pt2)
        {
            return new PointM(pt1.X + pt2.X, pt1.Y + pt2.Y);
        }

        public static PointM operator -(PointM pt1, PointM pt2)
        {
            return new PointM(pt1.X - pt2.X, pt1.Y - pt2.Y);
        }
    }
}
