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
    }
}
