using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;

namespace TemplatePrinter
{
    public class PrintParameters
    {
        public Image Image;
        public SizeM TargetSize;
        public Measure OverlapAmount;
        public bool Landscape;
        public ContentAlignment Alignment;
        public List<AlignmentMarker> Markers;
        public PrinterSettings Printer;
        public PaperSize PaperSize;

        public PrintParameters()
        {
            OverlapAmount = Measure.FromCm(1);
            Markers = new List<AlignmentMarker>();
        }

        public void SetDefaultMarkers(PrintLayout printLayout)
        {
            Markers.Clear();
            var startX = printLayout.PrintableArea.Width - OverlapAmount / 2;
            var startY = printLayout.PrintableArea.Height - OverlapAmount / 2;
            var spacingX = printLayout.PrintableArea.Width - OverlapAmount;
            var spacingY = printLayout.PrintableArea.Height - OverlapAmount;
            //spacingX += OverlapAmount / 2;
            //spacingY += OverlapAmount / 2;
            for (int x = 0; x <= printLayout.TotalPageX; x++)
            {
                Measure posX = startX + (spacingX * (x - 1));
                if (x == 0)
                    posX = OverlapAmount / 2;
                else if (x == printLayout.TotalPageX)
                    posX = printLayout.TotalCoveredSize.Width - (OverlapAmount / 2);

                for (int y = 0; y <= printLayout.TotalPageY; y++)
                {
                    Measure posY = startY + (spacingY * (y - 1));
                    if (y == 0)
                        posY = OverlapAmount / 2;
                    else if (y == printLayout.TotalPageY)
                        posY = printLayout.TotalCoveredSize.Height - (OverlapAmount / 2);
                    Markers.Add(new AlignmentMarker()
                    {
                        Position = new PointM(posX, posY),
                        Size = Measure.FromCm(2)
                    });
                }
            }

        }
    }
}
