using System;
using System.Collections.Generic;
using LiveCharts;

namespace railway.monthlyReport
{
    public class barChartInformation
    {
        
        public List<String> xAxisLabels { get; set; }
        public SeriesCollection BarLineSeriesCollection { get; set; }

        public barChartInformation()
        {
            BarLineSeriesCollection = new SeriesCollection();
            xAxisLabels = new List<string>(){"tip"};
        }

        public void reset()
        {
            if (BarLineSeriesCollection.Count > 0)
                BarLineSeriesCollection.Clear();
            if (xAxisLabels.Count > 0)
                xAxisLabels.Clear();
        }

    }
}