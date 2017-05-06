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
        private List<PrinterSettings> Printers;
        private Dictionary<object, bool> LoadingFlags;
        private UnitOfMeasure DisplayUnit = UnitOfMeasure.Cm;
        private PrintParameters PrintConfig;

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
            PrintConfig = new PrintParameters()
            {
                Alignment = ContentAlignment.MiddleCenter,
                OverlapAmount = Measure.FromCm(1.5)
            };
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
                    Image selectedImage = null;
                    if (Path.GetExtension(dlg.FileName) == ".svg")
                    {
                        var svgDoc = SvgDocument.Open(dlg.FileName);
                        FixSvgStroke(svgDoc);
                        selectedImage = svgDoc.Draw();
                    }
                    else
                    {
                        selectedImage = Image.FromFile(dlg.FileName);
                    }
                    PrintConfig.Image = selectedImage;
                    PrintConfig.TargetSize = new SizeM(
                        Measure.FromPixels(selectedImage.Width, selectedImage.HorizontalResolution),
                    Measure.FromPixels(selectedImage.Height, selectedImage.VerticalResolution));
                    printPreviewControl1.PrintParameters = PrintConfig;
                    printPreviewControl1.RecalculatePrintLayout();
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

        private void UpdateSourceImageInfo()
        {
            pbxSourceImage.Image = PrintConfig.Image;
            if (PrintConfig.Image != null)
            {
                lblSourceImgInfo.Text = string.Format("Image Dimensions:\r\nDPI: {0:0.##}\r\nPhysical Size: {1:0.##}{3} x {2:0.##}{3}",
                    PrintConfig.Image.HorizontalResolution,
                    PrintConfig.TargetSize.Width[DisplayUnit],
                    PrintConfig.TargetSize.Height[DisplayUnit],
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
                SetPrintSettings();
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
                SetPrintSettings();
            }
        }

        private void SetPrintSettings()
        {
            PrintConfig.Printer = SelectedPrinter;
            PrintConfig.PaperSize = SelectedPaperSize;
            PrintConfig.Landscape = chkLandscape.Checked;
            //PlaceMarkers();

            if (printPreviewControl1.PrintParameters == null && PrintConfig.Image != null)
                printPreviewControl1.PrintParameters = PrintConfig;
            printPreviewControl1.RecalculatePrintLayout();
            
        }

        //private void PlaceMarkers()
        //{
        //    if (PrintConfig.PaperSize != null)
        //    {

        //    }
        //}

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
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (SelectedPrinter != null && SelectedPaperSize != null && PrintConfig != null && PrintConfig.Image != null)
            {
                var doc = new FullscalePrintDocument(PrintConfig);

                using (var dlg = new PrintPreviewDialog())
                {
                    dlg.Document = doc;
                    dlg.ShowDialog();
                }
            }
        }

        private void chkLandscape_CheckedChanged(object sender, EventArgs e)
        {
            SetPrintSettings();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            printPreviewControl1.LayoutMode = !printPreviewControl1.LayoutMode;
        }
    }
}
