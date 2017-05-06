using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TemplatePrinter
{
    public partial class PrintPreviewControl : Control
    {
        private double zoomAmount = 1d;
        private PrintParameters _PrintParameters;
        private PrintLayout _PrintLayout;
        private bool _LayoutMode;

        public bool LayoutMode
        {
            get { return _LayoutMode; }
            set
            {
                if (value != _LayoutMode)
                {
                    _LayoutMode = value;
                    if (IsHandleCreated)
                        Invalidate();
                }
            }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PrintParameters PrintParameters
        {
            get { return _PrintParameters; }
            set
            {
                if (value != _PrintParameters)
                {
                    _PrintParameters = value;
                    RecalculatePrintLayout();
                }
            }
        }

        public PrintPreviewControl()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (_PrintLayout == null || PrintParameters == null)
            {
                base.OnPaint(pe);
                return;
            }
            ZoomToFit();
            var g = pe.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            double renderDPI = 100 * zoomAmount;
            var imageBounds = new RectangleM(_PrintLayout.AlignmentOffset, PrintParameters.TargetSize).ToDisplay(renderDPI);
            var renderOverlap = (float)PrintParameters.OverlapAmount.Pixels(renderDPI);
            var imgSize = PrintParameters.TargetSize.ToDisplay(renderDPI);
            var pageSize = _PrintLayout.PaperSize.ToDisplay(renderDPI);
            var printArea = _PrintLayout.PrintableArea.ToDisplay(renderDPI);

            if (LayoutMode)
            {
                var pageMarginX = (float)(_PrintLayout.PaperSize.Width - _PrintLayout.PrintableArea.Width).Pixels(renderDPI);
                var pageMarginY = (float)(_PrintLayout.PaperSize.Height - _PrintLayout.PrintableArea.Height).Pixels(renderDPI);
                var printedBounds = _PrintLayout.TotalCoveredSize.ToDisplay(renderDPI);
                g.FillRectangle(Brushes.White, 0, 0, printedBounds.Width + pageMarginX, printedBounds.Height + pageMarginY);
                g.DrawRectangle(Pens.Black, 0, 0, printedBounds.Width + pageMarginX - 1, printedBounds.Height + pageMarginY - 1);
                g.DrawImage(PrintParameters.Image, 
                    (printedBounds.Width - imgSize.Width + pageMarginX) /2,
                    (printedBounds.Height - imgSize.Height + pageMarginY) / 2, 
                    imgSize.Width, imgSize.Height);

                using (var overlapBrush = new SolidBrush(Color.FromArgb(50, 90, 200, 90)))
                {
                    for (int x = 1; x < _PrintLayout.TotalPageX; x++)
                    {
                        var posX = printArea.Left + (printedBounds.Width / _PrintLayout.TotalPageX) * x;
                        g.FillRectangle(overlapBrush, posX - renderOverlap / 2f, 0, renderOverlap, printedBounds.Height + pageMarginY);
                    }
                    for (int y = 1; y < _PrintLayout.TotalPageY; y++)
                    {
                        var posY = printArea.Top + (printedBounds.Height / _PrintLayout.TotalPageY) * y;
                        g.FillRectangle(overlapBrush, 0, posY - renderOverlap / 2f, printedBounds.Width + pageMarginX, renderOverlap);
                    }
                }

                g.TranslateTransform(pageMarginX / 2, pageMarginY / 2);
                foreach (var marker in PrintParameters.Markers)
                {
                    var markerBounds = new RectangleM(
                        marker.Position.X - marker.Size / 2,
                        marker.Position.Y - marker.Size / 2,
                        marker.Size, marker.Size
                        ).ToDisplay(renderDPI);
                    g.DrawEllipse(Pens.Black, markerBounds);
                    g.DrawLine(Pens.Black, markerBounds.Left, markerBounds.Top, markerBounds.Right, markerBounds.Bottom);
                    g.DrawLine(Pens.Black, markerBounds.Right, markerBounds.Top, markerBounds.Left, markerBounds.Bottom);
                }
                g.ResetTransform();
            }
            else
            {
                //g.ScaleTransform((float)zoomAmount, (float)zoomAmount);
                for (int x = 0; x < _PrintLayout.TotalPageX; x++)
                {
                    for (int y = 0; y < _PrintLayout.TotalPageY; y++)
                    {
                        //var currentTransform = g.Transform;
                        g.TranslateTransform(pageSize.Width * x, pageSize.Height * y);
                        g.FillRectangle(Brushes.White, 0, 0, pageSize.Width, pageSize.Height);
                        g.DrawRectangle(Pens.Black, 0, 0, pageSize.Width - 1, pageSize.Height - 1);
                        g.DrawRectangle(Pens.Red, printArea.X, printArea.Y, printArea.Width - 1, printArea.Height - 1);
                        //g.ScaleTransform((float)zoomAmount, (float)zoomAmount);
                        //testDoc.DrawPage(g, x, y);
                        g.SetClip(printArea);
                        g.TranslateTransform(printArea.X, printArea.Y);
                        g.TranslateTransform(
                        -((printArea.Width * x) - (x * renderOverlap)),
                        -((printArea.Height * y) - (y * renderOverlap))
                        );

                        g.DrawImage(PrintParameters.Image, imageBounds);

                        foreach (var marker in PrintParameters.Markers)
                        {
                            var markerBounds = new RectangleM(
                                marker.Position.X - marker.Size / 2,
                                marker.Position.Y - marker.Size / 2,
                                marker.Size, marker.Size
                                ).ToDisplay(renderDPI);
                            g.DrawEllipse(Pens.Black, markerBounds);
                            g.DrawLine(Pens.Black, markerBounds.Left, markerBounds.Top, markerBounds.Right, markerBounds.Bottom);
                            g.DrawLine(Pens.Black, markerBounds.Right, markerBounds.Top, markerBounds.Left, markerBounds.Bottom);
                        }

                        g.DrawRectangle(Pens.Blue, imageBounds.X, imageBounds.Y, imageBounds.Width, imageBounds.Height);
                        g.ResetClip();
                        g.ResetTransform();
                        //g.Transform = currentTransform;
                    }
                }
            }
        }

        public void RecalculatePrintLayout()
        {
            if (PrintParameters != null)
            {
                _PrintLayout = PrintLayout.CalculateLayout(PrintParameters);
                PrintParameters.SetDefaultMarkers(_PrintLayout);
                //ZoomToFit();
                if (IsHandleCreated)
                    Invalidate();
            }
        }

        public void ZoomToFit()
        {
            if (_PrintLayout != null)
            {
                var ratio = (_PrintLayout.TotalPaperSize.Width / _PrintLayout.TotalPaperSize.Height).Cm;
                if (Width / ratio > Height)
                {
                    zoomAmount = Height / _PrintLayout.TotalPaperSize.Height.Pixels(100);
                }
                else if (Height * ratio > Width)
                {
                    zoomAmount = Width / _PrintLayout.TotalPaperSize.Width.Pixels(100);
                }
            }
        }
    }
}
