using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ChartJs.Blazor.ChartJS.BarChart;
using ChartJs.Blazor.ChartJS.BarChart.Axes;
using ChartJs.Blazor.ChartJS.Common.Axes;
using ChartJs.Blazor.ChartJS.Common.Axes.Ticks;
using ChartJs.Blazor.ChartJS.Common.Properties;
using ChartJs.Blazor.ChartJS.MixedChart;
using ChartJs.Blazor.ChartJS.PieChart;
using ChartJs.Blazor.Charts;
using ChartJs.Blazor.Util;
using ChartJs.Blazor.ChartJS.Common.Wrappers;

namespace DynamicPotterTrivia.Pages
{
    public partial class Statistics
    {
        private PieConfig _PieConfig;
        private ChartJsPieChart _pieChartJs;
        private BarConfig _BarConfig;
        private ChartJsBarChart _barChartJs;

        protected override void OnInitialized()
        {
            //Add Pie Chart with scores by category
            _PieConfig = new PieConfig
            {
                Options = new PieOptions
                {
                    Title = new OptionsTitle
                    {
                        Display = true,
                        Text = "Total Score by Category"
                    },
                    Responsive = true,
                    Animation = new ArcAnimation
                    {
                        AnimateRotate = true,
                        AnimateScale = true
                    }
                }
            };

            _PieConfig.Data.Labels.AddRange(new[] { "Harry Potter", "Lord of the Rings" });

            var pieSet = new PieDataset
            {
                BackgroundColor = new[] { ColorUtil.RandomColorString(), ColorUtil.RandomColorString()},
                BorderWidth = 1,
                //HoverBackgroundColor = ColorUtil.RandomColorString(),
                //HoverBorderColor = ColorUtil.RandomColorString(),
                //HoverBorderWidth = 1,
                BorderColor = "#000000",
            };

            pieSet.Data.AddRange(new double[] { ScoreTrackerService.GetTotalHPScore(), ScoreTrackerService.GetTotalLOTRScore()});
            _PieConfig.Data.Datasets.Add(pieSet);
            //end of pie chart config

            //bar chart config
            _BarConfig = new BarConfig
            {
                Options = new BarOptions
                {
                    Title = new OptionsTitle
                    {
                        Display = true,
                        Text = "Correct vs. Wrong Answers"
                    },
                    Scales = new BarScales
                    {
                        XAxes = new List<CartesianAxis>
                        {
                            new BarCategoryAxis
                            {
                                BarPercentage = 0.5,
                                BarThickness = BarThickness.Flex
                            }
                        },
                        YAxes = new List<CartesianAxis>
                        {
                            new BarLinearCartesianAxis
                            {
                                Ticks = new LinearCartesianTicks
                                {
                                    BeginAtZero = true
                                }
                            }
                        }
                    },
                    Responsive = true,
                    Animation = new ArcAnimation
                    {
                        AnimateRotate = true,
                        AnimateScale = true
                    }
                }
            };
            _BarConfig.Data.Labels.AddRange(new [] {"Correct Answers", "Wrong Answers"});
            var barDataset = new BarDataset<Int32Wrapper>
            {
                BackgroundColor = new[] {ColorUtil.RandomColorString(), ColorUtil.RandomColorString() },
                BorderWidth = 1,
                //HoverBackgroundColor = ColorUtil.RandomColorString(),
                //HoverBorderColor = ColorUtil.RandomColorString(),
                //HoverBorderWidth = 1,
                BorderColor = "#000000",

            };

            barDataset.AddRange(new Int32Wrapper[] {ScoreTrackerService.GetTotalCorrectAnswers(), ScoreTrackerService.GetTotalWrongAnswers()});
            _BarConfig.Data.Datasets.Add(barDataset);
            //end of bar chart config
        }
    }
}
