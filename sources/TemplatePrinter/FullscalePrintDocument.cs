using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;

namespace TemplatePrinter
{
    public class FullscalePrintDocument : PrintDocument
    {
        private PrintParameters printJob;
        private PrintLayout printLayout;
        private int currentPageX;
        private int currentPageY;

        public FullscalePrintDocument(PrintParameters printParams)
        {
            printJob = printParams;
            PrinterSettings = printJob.Printer ?? PrinterSettings;
            if (printJob.PaperSize != null)
                DefaultPageSettings.PaperSize = printJob.PaperSize;
            DefaultPageSettings.Landscape = printJob.Landscape;
        }

        protected override void OnBeginPrint(PrintEventArgs e)
        {
            base.OnBeginPrint(e);
            OriginAtMargins = false;
            printJob.Printer = PrinterSettings;
            printJob.PaperSize = DefaultPageSettings.PaperSize;
            printJob.Landscape = DefaultPageSettings.Landscape;
            printLayout = PrintLayout.CalculateLayout(printJob);
            currentPageX = currentPageY = 0;
        }

        protected override void OnEndPrint(PrintEventArgs e)
        {
            base.OnEndPrint(e);
        }

        protected override void OnPrintPage(PrintPageEventArgs e)
        {

            base.OnPrintPage(e);
            var g = e.Graphics;

            var printArea = printLayout.PrintableArea.ToDisplay(100);
            var imageBounds = new RectangleM(printLayout.AlignmentOffset, printJob.TargetSize).ToDisplay(100);
            var renderOverlap = (float)printJob.OverlapAmount.Pixels(100);

            //g.DrawRectangle(Pens.Red, printArea.X, printArea.Y, printArea.Width, printArea.Height);
            g.SetClip(printArea);

            g.TranslateTransform(printArea.X, printArea.Y);
            g.TranslateTransform(
                -((printArea.Width * currentPageX) - (currentPageX * renderOverlap)),
                -((printArea.Height * currentPageY) - (currentPageY * renderOverlap))
                );
            foreach (var marker in printJob.Markers)
            {
                var markerBounds = new RectangleM(
                    marker.Position.X - marker.Size / 2, 
                    marker.Position.Y - marker.Size / 2, 
                    marker.Size, marker.Size
                    );
                g.DrawEllipse(Pens.Black, markerBounds.ToDisplay(100));
            }
            //g.DrawRectangle(Pens.Red, imageBounds.X, imageBounds.Y, imageBounds.Width, imageBounds.Height);

            g.DrawImage(printJob.Image, imageBounds);

            g.ResetTransform();
            g.ResetClip();

            currentPageX++;
            if (currentPageX >= printLayout.TotalPageX)
            {
                currentPageX = 0;
                currentPageY++;
            }

            e.HasMorePages = currentPageY < printLayout.TotalPageY;
        }
    }
}
