﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            double renderDPI = 96 * zoomAmount;
            var imageBounds = new RectangleM(_PrintLayout.AlignmentOffset, PrintParameters.TargetSize).ToDisplay(renderDPI);
            var renderOverlap = (float)PrintParameters.OverlapAmount.Pixels(renderDPI);
            var imgSize = PrintParameters.TargetSize.ToDisplay(renderDPI);
            var pageSize = _PrintLayout.PaperSize.ToDisplay(renderDPI);
            var printArea = _PrintLayout.PrintableArea.ToDisplay(renderDPI);
            var totalPaper = _PrintLayout.TotalPaperSize.ToDisplay(renderDPI);

            if (LayoutMode)
            {

            }
            else
            {
                for (int x = 0; x < _PrintLayout.TotalPageX; x++)
                {
                    for (int y = 0; y < _PrintLayout.TotalPageY; y++)
                    {
                        g.TranslateTransform(pageSize.Width * x, pageSize.Height * y);
                        g.FillRectangle(Brushes.White, 0, 0, pageSize.Width, pageSize.Height);
                        g.DrawRectangle(Pens.Black, 0, 0, pageSize.Width - 1, pageSize.Height - 1);
                        g.DrawRectangle(Pens.Red, printArea.X, printArea.Y, printArea.Width - 1, printArea.Height - 1);

                        g.SetClip(printArea);
                        g.TranslateTransform(printArea.X, printArea.Y);
                        g.TranslateTransform(
                        -((printArea.Width * x) - (x * renderOverlap)),
                        -((printArea.Height * y) - (y * renderOverlap))
                        );

                        g.DrawImage(PrintParameters.Image, imageBounds);
                        g.DrawRectangle(Pens.Blue, imageBounds.X, imageBounds.Y, imageBounds.Width, imageBounds.Height);
                        g.ResetClip();
                        g.ResetTransform();
                    }
                }
            }
        }

        public void RecalculatePrintLayout()
        {
            if (PrintParameters != null)
            {
                _PrintLayout = PrintLayout.CalculateLayout(PrintParameters);
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
                    zoomAmount = Height / _PrintLayout.TotalPaperSize.Height.Pixels(96);
                }
                else if (Height * ratio > Width)
                {
                    zoomAmount = Width / _PrintLayout.TotalPaperSize.Width.Pixels(96);
                }
            }
        }
    }
}
