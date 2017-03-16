using Svg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TemplatePrinter
{
    public partial class Form1 : Form
    {
        private Image SourceImage;
        private List<PrinterSettings> Printers;
        private Dictionary<object, bool> LoadingFlags;
        private SizeM TargetSize;
        private UnitOfMeasure DisplayUnit = UnitOfMeasure.Cm;

        private PrinterSettings SelectedPrinter
        {
            get { return cboPrinters.SelectedItem as PrinterSettings; }
        }

        private PaperSize SelectedPaperSize
        {
            get { return cboPageSizes.SelectedItem as PaperSize; }
        }

        public Form1()
        {
            InitializeComponent();
            LoadingFlags = new Dictionary<object, bool>();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadPrinters();
        }

        private void btnOpenImg_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    if (Path.GetExtension(dlg.FileName) == ".svg")
                    {
                        var svgDoc = SvgDocument.Open(dlg.FileName);
                        FixSvgStroke(svgDoc);
                        SourceImage = svgDoc.Draw();
                    }
                    else
                    {
                        SourceImage = Image.FromFile(dlg.FileName);
                    }

                    txtSourceImgPath.Text = dlg.FileName;
                }
            }
            UpdateSourceImageInfo();
        }

        private static void FixSvgStroke(SvgElement svgElem)
        {
            foreach (var svgChild in svgElem.Children)
            {
                if (svgChild.StrokeWidth != null && svgChild.StrokeWidth.Type == SvgUnitType.Percentage)
                {
                    svgChild.StrokeWidth = new SvgUnit(SvgUnitType.Pixel,
                        svgChild.StrokeWidth.ToDeviceValue(null, UnitRenderingType.Other, svgChild) / 100f);
                }
                FixSvgStroke(svgChild);
            }
        }

        const float InchToCm = 2.54000000001016f;

        private void UpdateSourceImageInfo()
        {
            pbxSourceImage.Image = SourceImage;
            if (SourceImage != null)
            {
                TargetSize = new SizeM(Measure.FromPixels(SourceImage.Width, SourceImage.HorizontalResolution),
                    Measure.FromPixels(SourceImage.Height, SourceImage.VerticalResolution));
                lblSourceImgInfo.Text = string.Format("Image Dimensions:\r\nDPI: {0:0.##}\r\nPhysical Size: {1:0.##}{3} x {2:0.##}{3}",
                    SourceImage.HorizontalResolution,
                    TargetSize.Width[DisplayUnit],
                    TargetSize.Height[DisplayUnit],
                    UnitSufix(DisplayUnit));

            }
        }

        private void LoadPrinters()
        {
            Printers = new List<PrinterSettings>();
            Task.Factory.StartNew(() =>
            {
                foreach (string printerName in PrinterSettings.InstalledPrinters)
                {
                    var printerSettings = new PrinterSettings() { PrinterName = printerName };
                    Printers.Add(printerSettings);
                }
                BeginInvoke((Action)(() =>
                {
                    LoadingFlags[cboPrinters] = true;
                    cboPrinters.DisplayMember = "PrinterName";
                    cboPrinters.DataSource = Printers;

                    var defaultPrinter = Printers.FirstOrDefault(p => p.IsDefaultPrinter);
                    cboPrinters.SelectedItem = defaultPrinter;
                    LoadingFlags[cboPrinters] = false;
                    FillPrinterSettings();
                }));
            });
        }

        private void FillPrinterSettings()
        {
            if (SelectedPrinter != null)
            {
                LoadingFlags[cboPageSizes] = true;
                cboPageSizes.DataSource = (from PaperSize x in SelectedPrinter.PaperSizes select x).ToList();
                cboPageSizes.DisplayMember = "PaperName";
                cboPageSizes.SelectedItem = SelectedPrinter.DefaultPageSettings.PaperSize;
                LoadingFlags[cboPageSizes] = false;
                UpdatePaperSizeInfo();
            }
        }

        private void UpdatePaperSizeInfo()
        {
            if (SelectedPaperSize != null)
            {
                var paperSize = new SizeM(
                    Measure.FromInch(SelectedPaperSize.Width / 100d),
                    Measure.FromInch(SelectedPaperSize.Height / 100d));
                lblPageSize.Text = string.Format("Paper Dimensions: {0:0.##}{2} x {1:0.##}{2}",
                    paperSize.Width[DisplayUnit], paperSize.Height[DisplayUnit], UnitSufix(DisplayUnit));
            }
        }

        static string UnitSufix(UnitOfMeasure unit)
        {
            switch (unit)
            {
                default:
                case UnitOfMeasure.Cm:
                case UnitOfMeasure.Mm:
                    return unit.ToString().ToLower();
                case UnitOfMeasure.Inch:
                    return "\"";
                case UnitOfMeasure.Foot:
                    return "'";
                case UnitOfMeasure.HundredInch:
                    return "pt";
            }
        }

        private void cboPrinters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LoadingFlags.ContainsKey(cboPrinters) && LoadingFlags[cboPrinters])
                return;
            FillPrinterSettings();
        }

        private void cboPageSizes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LoadingFlags.ContainsKey(cboPageSizes) && LoadingFlags[cboPageSizes])
                return;
            UpdatePaperSizeInfo();
        }

        class TemplatePrintSettings
        {
            public Image Image;
            public SizeM TargetSize;
            public Measure OverlapAmount;
            public bool Landscape;
            public ContentAlignment Alignment;
            public PrinterSettings Printer;
            public PaperSize PaperSize;
        }

        class PrintLayoutInfo
        {
            //public SizeM ImageSize;
            public SizeM PaperSize;
            public RectangleM PrintableArea;

            public int PageCountHeight;
            public int PageCountWidth;

            public SizeM ImageSpan;
            public SizeM PaperSpan;
            //public SizeM PrintMargins;

        }

        private static PrintLayoutInfo CalculatePrintLayout(TemplatePrintSettings config)
        {
            var printLayout = new PrintLayoutInfo();

            var pageSettings = (PageSettings)config.Printer.DefaultPageSettings.Clone();
            pageSettings.PaperSize = config.PaperSize;
            pageSettings.Landscape = config.Landscape;

            var paperSize = new SizeM(
                Measure.FromInch(config.PaperSize.Width / 100d),
                Measure.FromInch(config.PaperSize.Height / 100d));
            var printableSize = SizeM.FromSize(pageSettings.PrintableArea.Size, UnitOfMeasure.HundredInch);

            printLayout.PaperSize = new SizeM(
                config.Landscape ? paperSize.Height : paperSize.Width,
                config.Landscape ? paperSize.Width : paperSize.Height);

            printLayout.PrintableArea = new RectangleM(
                Measure.FromUnitMeasure(UnitOfMeasure.HundredInch, pageSettings.HardMarginX),
                Measure.FromUnitMeasure(UnitOfMeasure.HundredInch, pageSettings.HardMarginY),
                config.Landscape ? printableSize.Height : printableSize.Width,
                config.Landscape ? printableSize.Width : printableSize.Height);

            double remainingWidth = config.TargetSize.Width.Cm, remainingHeight = config.TargetSize.Height.Cm;

            while (remainingWidth > 0)
            {
                remainingWidth -= printLayout.PrintableArea.Width.Cm - (printLayout.PageCountWidth > 1 ? config.OverlapAmount.Cm : 0d);
                printLayout.PageCountWidth++;
            }

            while (remainingHeight > 0)
            {
                remainingHeight -= printLayout.PrintableArea.Height.Cm - (printLayout.PageCountHeight > 1 ? config.OverlapAmount.Cm : 0d);
                printLayout.PageCountHeight++;
            }

            printLayout.ImageSpan = new SizeM(
                config.TargetSize.Width + (config.OverlapAmount * (printLayout.PageCountWidth - 1d)),
                config.TargetSize.Height + (config.OverlapAmount * (printLayout.PageCountHeight - 1d)));

            printLayout.PaperSpan = new SizeM(
                printLayout.PrintableArea.Width * printLayout.PageCountWidth,
                printLayout.PrintableArea.Height * printLayout.PageCountHeight);

            return printLayout;
        }

        private void RenderPrintPreview(TemplatePrintSettings printSettings, PrintLayoutInfo printLayout)
        {
            double renderDPI = (double)printSettings.Image.HorizontalResolution;
            var pageSize = printLayout.PaperSize.ToDisplay(renderDPI);
            var printArea = printLayout.PrintableArea.ToDisplay(renderDPI);

            var bmpPreview = new Bitmap(
                (int)pageSize.Width * printLayout.PageCountWidth, 
                (int)pageSize.Height * printLayout.PageCountHeight);

            Measure paddingX = Measure.Zero, paddingY = Measure.Zero;

            switch (printSettings.Alignment) {
                case ContentAlignment.BottomRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.TopRight:
                    paddingX = printLayout.PaperSpan.Width - printLayout.ImageSpan.Width;
                    break;
                case ContentAlignment.BottomCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.TopCenter:
                    paddingX = (printLayout.PaperSpan.Width - printLayout.ImageSpan.Width) / 2d;
                    break;
            }
            switch (printSettings.Alignment)
            {
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomRight:
                    paddingY = printLayout.PaperSpan.Height - printLayout.ImageSpan.Height;
                    break;
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleRight:
                    paddingY = (printLayout.PaperSpan.Height - printLayout.ImageSpan.Height) / 2d;
                    break;
            }
            var imgPadding = new SizeF((float)paddingX.Pixels(renderDPI), (float)paddingY.Pixels(renderDPI));
            var renderOverlap = (float)printSettings.OverlapAmount.Pixels(renderDPI);
            using (var g = Graphics.FromImage(bmpPreview))
            {
                for (int x = 0; x < printLayout.PageCountWidth; x++)
                {
                    for (int y = 0; y < printLayout.PageCountHeight; y++)
                    {
                        g.TranslateTransform(pageSize.Width * x, pageSize.Height * y);
                        g.FillRectangle(Brushes.White, 0,0, pageSize.Width, pageSize.Height);
                        g.DrawRectangle(Pens.Black, 0, 0, pageSize.Width - 1, pageSize.Height - 1);
                        g.DrawRectangle(Pens.Red, printArea.X, printArea.Y, printArea.Width - 1, printArea.Height - 1);

                        g.SetClip(printArea);
                        g.TranslateTransform(printArea.X, printArea.Y);
                        var pageOffsetX = (printArea.Width * x) - (x > 0 ? renderOverlap : 0) - imgPadding.Width;
                        var pageOffsetY = (printArea.Height * y) - (y > 0 ? renderOverlap : 0) - imgPadding.Height;
                        g.DrawImage(printSettings.Image, -pageOffsetX, -pageOffsetY);
                        g.ResetClip();

                        g.ResetTransform();
                    }
                }
            }
            pictureBox1.Image = bmpPreview;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (SelectedPrinter != null && SelectedPaperSize != null && SourceImage != null)
            {
                var printSettings = new TemplatePrintSettings()
                {
                    Image = SourceImage,
                    Printer = SelectedPrinter,
                    PaperSize = SelectedPaperSize,
                    Landscape = chkLandscape.Checked,
                    Alignment = ContentAlignment.MiddleCenter,
                    OverlapAmount = Measure.FromCm(1),
                    TargetSize = TargetSize
                };
                var printLayout = CalculatePrintLayout(printSettings);
                RenderPrintPreview(printSettings, printLayout);
            }
        }
    }
}
