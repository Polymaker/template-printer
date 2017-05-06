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
            CalculateLayout();
            currentPageX = currentPageY = 0;
        }

        public void CalculateLayout()
        {
            printLayout = PrintLayout.CalculateLayout(printJob);
            printJob.SetDefaultMarkers(printLayout);
        }

        protected override void OnEndPrint(PrintEventArgs e)
        {
            base.OnEndPrint(e);
        }

        protected override void OnPrintPage(PrintPageEventArgs e)
        {

            base.OnPrintPage(e);
            var g = e.Graphics;
            
            var targetDPI = g.PageUnit == GraphicsUnit.Display ? 100 : g.DpiX;
            //e.Graphics.p
            var testDPI = printLayout.PaperSize.Width[UnitOfMeasure.HundredInch] / (double)e.PageBounds.Width;
            var printArea = printLayout.PrintableArea.ToDisplay(targetDPI);
            var imageBounds = new RectangleM(printLayout.AlignmentOffset, printJob.TargetSize).ToDisplay(targetDPI);
            var renderOverlap = (float)printJob.OverlapAmount.Pixels(targetDPI);
            var markerPen = new Pen(Color.Black, (float)Measure.FromMm(0.4).Pixels(targetDPI));
            //g.DrawRectangle(Pens.Red, printArea.X, printArea.Y, printArea.Width, printArea.Height);
            g.SetClip(printArea);
            var currentTransform = g.Transform;
            g.TranslateTransform(printArea.X, printArea.Y);
            g.TranslateTransform(
                -((printArea.Width * currentPageX) - (currentPageX * renderOverlap)),
                -((printArea.Height * currentPageY) - (currentPageY * renderOverlap))
                );
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            foreach (var marker in printJob.Markers)
            {
                var markerBounds = new RectangleM(
                    marker.Position.X - marker.Size / 2, 
                    marker.Position.Y - marker.Size / 2, 
                    marker.Size, marker.Size
                    ).ToDisplay(targetDPI);
                g.DrawEllipse(markerPen, markerBounds);
                g.DrawLine(markerPen, markerBounds.Left, markerBounds.Top, markerBounds.Right, markerBounds.Bottom);
                g.DrawLine(markerPen, markerBounds.Right, markerBounds.Top, markerBounds.Left, markerBounds.Bottom);
            }
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            //g.DrawRectangle(Pens.Red, imageBounds.X, imageBounds.Y, imageBounds.Width, imageBounds.Height);

            g.DrawImage(printJob.Image, imageBounds);
            g.Transform = currentTransform;
            //g.ResetTransform();
            g.ResetClip();

            currentPageX++;
            if (currentPageX >= printLayout.TotalPageX)
            {
                currentPageX = 0;
                currentPageY++;
            }
            markerPen.Dispose();
            e.HasMorePages = currentPageY < printLayout.TotalPageY;
        }

        //public void DrawPage(Graphics g, int pageNb)
        //{

        //}

        public void DrawPage(Graphics g, int pageX, int pageY)
        {
            currentPageX = pageX;
            currentPageY = pageY;
            var pageSize = printLayout.PaperSize.ToDisplay(g.PageUnit == GraphicsUnit.Display ? 100 : g.DpiX);
            var pageBounds = new Rectangle(0, 0, (int)pageSize.Width, (int)pageSize.Height);
            OnPrintPage(new PrintPageEventArgs(g, Rectangle.Empty, pageBounds, DefaultPageSettings));
        }
    }
}
