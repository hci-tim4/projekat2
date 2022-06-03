using System;
using System.Collections.Generic;
using LiveCharts;

namespace railway.drivingLineReport
{
    public class LineChartInformation
    {
        
        public List<String> xAxisLabels { get; set; }
        public SeriesCollection LineSeriesCollection { get; set; }

        public LineChartInformation()
        {
            LineSeriesCollection = new SeriesCollection();
            xAxisLabels = new List<string>(){"tip"};
        }

        public void reset()
        {
            //if (LineSeriesCollection.Count > 0 && LineSeriesCollection != null)
                LineSeriesCollection.Clear();
            //if (xAxisLabels.Count > 0 && xAxisLabels != null)
                xAxisLabels.Clear();
        }

    }
}