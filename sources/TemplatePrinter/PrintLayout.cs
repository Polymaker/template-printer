using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;

namespace TemplatePrinter
{
    public class PrintLayout
    {
        private int _TotalPageX;
        private int _TotalPageY;
        private SizeM _PaperSize;
        private RectangleM _PrintableArea;
        private SizeM _TotalImageSize;
        private SizeM _TotalPrintableSize;
        private SizeM _TotalPaperSize;
        private PointM _AlignmentOffset;
        private bool _IsLandscape;

        public int TotalPageX { get { return _TotalPageX; } }
        public int TotalPageY { get { return _TotalPageY; } }
        public RectangleM PrintableArea { get { return _PrintableArea; } }
        public SizeM PaperSize { get { return _PaperSize; } }
        public SizeM TotalImageSize { get { return _TotalImageSize; } }
        public SizeM TotalPrintableSize { get { return _TotalPrintableSize; } }
        public SizeM TotalPaperSize { get { return _TotalPaperSize; } }
        public PointM AlignmentOffset { get { return _AlignmentOffset; } }
        public bool IsLandscape { get { return _IsLandscape; } }

        public static PrintLayout CalculateLayout(PrintParameters config)
        {
            var printLayout = new PrintLayout();
            var pageSettings = (PageSettings)config.Printer.DefaultPageSettings.Clone();
            pageSettings.PaperSize = config.PaperSize;
            printLayout._IsLandscape = pageSettings.Landscape = config.Landscape;

            printLayout._PaperSize = new SizeM(
                Measure.FromUnitMeasure(UnitOfMeasure.HundredInch, 
                    config.Landscape ? config.PaperSize.Height : config.PaperSize.Width),
                Measure.FromUnitMeasure(UnitOfMeasure.HundredInch, 
                    config.Landscape ? config.PaperSize.Width : config.PaperSize.Height)
                );

            var printableSize = SizeM.FromSize(pageSettings.PrintableArea.Size, UnitOfMeasure.HundredInch);
            var printableArea = new RectangleM(
                Measure.FromUnitMeasure(UnitOfMeasure.HundredInch, pageSettings.HardMarginX),
                Measure.FromUnitMeasure(UnitOfMeasure.HundredInch, pageSettings.HardMarginY),
                printableSize.Width, printableSize.Height
                );

            if (config.Landscape)
                printableArea = new RectangleM(printableArea.Y, printableArea.X, printableArea.Height, printableArea.Width);
            printLayout._PrintableArea = printableArea;

            double remainingWidth = config.TargetSize.Width.Cm, remainingHeight = config.TargetSize.Height.Cm;

            while (remainingWidth > 0)
            {
                remainingWidth -= printLayout._PrintableArea.Width.Cm - (printLayout._TotalPageX > 1 ? config.OverlapAmount.Cm : 0d);
                printLayout._TotalPageX++;
            }

            while (remainingHeight > 0)
            {
                remainingHeight -= printLayout._PrintableArea.Height.Cm - (printLayout._TotalPageY > 1 ? config.OverlapAmount.Cm : 0d);
                printLayout._TotalPageY++;
            }

            printLayout._TotalImageSize = new SizeM(
                config.TargetSize.Width + (config.OverlapAmount * (printLayout.TotalPageX - 1)),
                config.TargetSize.Height + (config.OverlapAmount * (printLayout.TotalPageY - 1)));

            printLayout._TotalPrintableSize = new SizeM(
                printLayout._PrintableArea.Width * printLayout.TotalPageX,
                printLayout._PrintableArea.Height * printLayout.TotalPageY);

            printLayout._TotalPaperSize = new SizeM(
               printLayout._PaperSize.Width * printLayout.TotalPageX,
               printLayout._PaperSize.Height * printLayout.TotalPageY);

            Measure paddingX = Measure.Zero, paddingY = Measure.Zero;

            switch (config.Alignment)
            {
                case ContentAlignment.BottomRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.TopRight:
                    paddingX = printLayout._TotalPrintableSize.Width - printLayout._TotalImageSize.Width;
                    break;
                case ContentAlignment.BottomCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.TopCenter:
                    paddingX = (printLayout._TotalPrintableSize.Width - printLayout._TotalImageSize.Width) / 2d;
                    break;
            }

            switch (config.Alignment)
            {
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomRight:
                    paddingY = printLayout._TotalPrintableSize.Height - printLayout._TotalImageSize.Height;
                    break;
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleRight:
                    paddingY = (printLayout._TotalPrintableSize.Height - printLayout._TotalImageSize.Height) / 2d;
                    break;
            }

            printLayout._AlignmentOffset = new PointM(paddingX, paddingY);
            return printLayout;
        }
    }
}
