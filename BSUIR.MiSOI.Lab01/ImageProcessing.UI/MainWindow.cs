﻿using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ImageProcessing.Service;

namespace ImageProcessing.UI
{
    public partial class MainWindow : Form
    {
        private readonly IFilterBuilder _filterBuilder = new FilterBuilder();
        private string _filePath = "0398.jpg";

        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void btn_RChannel_Click(object sender, EventArgs e)
        {
            using (var bitMap = new Bitmap(this._filePath))
            {
                using (var imageProcessor = new ImageProcessor(bitMap))
                {
                    this.colorHistogram.Series.Clear();
                    ColorHistogramDescriptor channelDistribution = imageProcessor.GetChannel(ColorChannel.R);
                    this.AddColorSeries(channelDistribution, Color.Red);
                }
            }
        }

        private void AddColorSeries(ColorHistogramDescriptor channelDistribution, Color color)
        {
            var series = new Series(channelDistribution.ColorChannel.ToString() + " channel");
            series.ChartType = SeriesChartType.Column;
            series.Color = color;
            series.XValueType = ChartValueType.Int32;
            series.YValueType = ChartValueType.Int32;
            this.colorHistogram.ChartAreas[0].AxisY.Maximum = channelDistribution.Distribution.Max();
            for (int i = 0; i < channelDistribution.Distribution.Length; ++i)
            {
                series.Points.AddXY(i, channelDistribution.Distribution[i]);
            }

            this.colorHistogram.Series.Add(series);
        }

        private void btn_BChannel_Click(object sender, EventArgs e)
        {
            using (var bitMap = new Bitmap(this._filePath))
            {
                using (var imageProcessor = new ImageProcessor(bitMap))
                {
                    this.colorHistogram.Series.Clear();
                    ColorHistogramDescriptor channelDistribution = imageProcessor.GetChannel(ColorChannel.B);
                    this.AddColorSeries(channelDistribution, Color.Blue);
                }
            }
        }

        private void btn_GChannel_Click(object sender, EventArgs e)
        {
            using (var bitMap = new Bitmap(this._filePath))
            {
                using (var imageProcessor = new ImageProcessor(bitMap))
                {
                    this.colorHistogram.Series.Clear();
                    ColorHistogramDescriptor channelDistribution = imageProcessor.GetChannel(ColorChannel.G);
                    this.AddColorSeries(channelDistribution, Color.Green);
                }
            }
        }

        private void btn_RGBChannels_Click(object sender, EventArgs e)
        {
            using (var bitMap = new Bitmap(this._filePath))
            {
                using (var imageProcessor = new ImageProcessor(bitMap))
                {
                    this.colorHistogram.Series.Clear();
                    ColorHistogramDescriptor rChannelDistribution = imageProcessor.GetChannel(ColorChannel.R);
                    ColorHistogramDescriptor bChannelDistribution = imageProcessor.GetChannel(ColorChannel.B);
                    ColorHistogramDescriptor gChannelDistribution = imageProcessor.GetChannel(ColorChannel.G);
                    this.AddColorSeries(rChannelDistribution, Color.Red);
                    this.AddColorSeries(bChannelDistribution, Color.Blue);
                    this.AddColorSeries(gChannelDistribution, Color.Green);
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName = string.Empty;

            var ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.BMP;*.GIF;*.JPG;*.JPEG;*.PNG";
            DialogResult dr = ofd.ShowDialog();

            if (dr == DialogResult.OK)
            {
                fileName = ofd.FileName;
            }

            if (string.IsNullOrEmpty(fileName) == false)
            {
                this._filePath = fileName;
            }
        }

        private void btn_Solarization_Click(object sender, EventArgs e)
        {
            using (var bitMap = new Bitmap(this._filePath))
            {
                using (var imageProcessor = new ImageProcessor(bitMap))
                {
                    IFilter filter = this._filterBuilder.CreateSolarizationFilter(0.01);
                    imageProcessor.ApplyFilter(filter);
                    imageProcessor.SaveChanges("solarize.jpg");
                }
            }
        }

        private void btn_Stamping_Click(object sender, EventArgs e)
        {
            using (var bitMap = new Bitmap(this._filePath))
            {
                using (var imageProcessor = new ImageProcessor(bitMap))
                {
                    IFilter filter = this._filterBuilder.CreateStampingLinearFilter();
                    imageProcessor.ApplyFilter(filter);
                    imageProcessor.SaveChanges("stamping.jpg");
                }
            }
        }

        private void btn_Halftone_Click(object sender, EventArgs e)
        {
            using (var bitMap = new Bitmap(this._filePath))
            {
                using (var imageProcessor = new ImageProcessor(bitMap))
                {
                    IFilter filter = this._filterBuilder.CreateBinaryHalftoneFilter();
                    imageProcessor.ApplyFilter(filter);
                    imageProcessor.SaveChanges("halftone.jpg");
                }
            }
        }
    }
}
