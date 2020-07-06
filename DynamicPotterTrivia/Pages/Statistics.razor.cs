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
using ChartJs.Blazor.ChartJS.RadarChart;

namespace DynamicPotterTrivia.Pages
{
    public partial class Statistics
    {
        private PieConfig _PieConfigScoreByCategory;
        private ChartJsPieChart _pieChartJsScoreByCategory;
        private BarConfig _BarConfig;
        private ChartJsBarChart _barChartJs;
        private RadarConfig _RadarConfigHP;
        private ChartJsRadarChart _radarChartJsHP;
        private RadarConfig _RadarConfigLOTR;
        private ChartJsRadarChart _radarChartJsLOTR;

        protected override void OnInitialized()
        {
            //Add Pie Chart with scores by category
            _PieConfigScoreByCategory = new PieConfig
            {
                Options = new PieOptions
                {
                    Title = new OptionsTitle
                    {
                        Display = true,
                        Text = "Score by Category"
                    },
                    Responsive = true,
                    Animation = new ArcAnimation
                    {
                        AnimateRotate = true,
                        AnimateScale = true
                    }
                }
            };

            _PieConfigScoreByCategory.Data.Labels.AddRange(new[] { "Harry Potter", "Lord of the Rings" });

            var pieSet = new PieDataset
            {
                BackgroundColor = new[] { ColorUtil.RandomColorString(), ColorUtil.RandomColorString()},
                BorderWidth = 1,
                //HoverBackgroundColor = ColorUtil.RandomColorString(),
                //HoverBorderColor = ColorUtil.RandomColorString(),
                //HoverBorderWidth = 1,
                BorderColor = "#000000"
            };

            pieSet.Data.AddRange(new double[] { ScoreTrackerService.GetTotalHPScore(), ScoreTrackerService.GetTotalLOTRScore()});
            _PieConfigScoreByCategory.Data.Datasets.Add(pieSet);
            //end of pie chart config

            //bar chart config
            _BarConfig = new BarConfig
            {
                Options = new BarOptions
                {
                    Title = new OptionsTitle
                    {
                        Display = true,
                        Text = "Hints & Clues"
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
            _BarConfig.Data.Labels.AddRange(new [] {"HP Hints Used", "LOTR Hints Used", "HP Clues Used", "LOTR Clues Used"});
            var barDataset = new BarDataset<Int32Wrapper>
            {
                BackgroundColor = new[] {ColorUtil.RandomColorString(), ColorUtil.RandomColorString(), ColorUtil.RandomColorString(), ColorUtil.RandomColorString() },
                BorderWidth = 1,
                //HoverBackgroundColor = ColorUtil.RandomColorString(),
                //HoverBorderColor = ColorUtil.RandomColorString(),
                //HoverBorderWidth = 1,
                BorderColor = "#000000",
                Label = "Use of Hints & Clues"
            };

            barDataset.AddRange(new Int32Wrapper[] {ScoreTrackerService.GetHPHintsUsed(), ScoreTrackerService.GetLOTRHintsUsed(), ScoreTrackerService.GetHPCluesUsed(), ScoreTrackerService.GetLOTRCluesUsed()});
            _BarConfig.Data.Datasets.Add(barDataset);
            //end of bar chart config

            //start of HP radar config
            _RadarConfigHP = new RadarConfig
            {
                Options = new RadarOptions
                {
                    Title = new OptionsTitle
                    {
                        Display = true,
                        Text = "Harry Potter Data"
                    },
                    Responsive = true
                }
            };
            _RadarConfigHP.Data.Labels.AddRange(new string[] { "Score", "Hints Used", "Clues Used", "Correct Answers", "Wrong Answers" });
            RadarDataset dataset_hp = new RadarDataset
            {
                //Label = $"Participant {_RadarConfigHP.Data.Datasets.Count + 1}",
                BorderColor = ColorUtil.RandomColorString(),
                BorderWidth = 1,
                Data = new List<double>()
            };
            double[] radarDataHP = new double[5]
            {
                ScoreTrackerService.GetTotalHPScore(), ScoreTrackerService.GetHPHintsUsed(),
                ScoreTrackerService.GetHPCluesUsed(), ScoreTrackerService.GetHPCorrectAnswers(),
                ScoreTrackerService.GetHPWrongAnswers()
            };

            dataset_hp.Data.AddRange(radarDataHP);
            _RadarConfigHP.Data.Datasets.Add(dataset_hp);
            //end of HP radar config

            //start of LOTR radar config
            _RadarConfigLOTR = new RadarConfig
            {
                Options = new RadarOptions
                {
                    Title = new OptionsTitle
                    {
                        Display = true,
                        Text = "Lord of the Rings Data"
                    },
                    Responsive = true
                }
            };
            _RadarConfigLOTR.Data.Labels.AddRange(new string[] { "Score", "Hints Used", "Clues Used", "Correct Answers", "Wrong Answers" });
            RadarDataset dataset_lotr = new RadarDataset
            {
                //Label = $"Participant {_RadarConfigHP.Data.Datasets.Count + 1}",
                BorderColor = ColorUtil.RandomColorString(),
                BorderWidth = 1,
                Data = new List<double>()
            };
            double[] radarDataLOTR = new double[5]
            {
                ScoreTrackerService.GetTotalLOTRScore(), ScoreTrackerService.GetLOTRHintsUsed(),
                ScoreTrackerService.GetLOTRCluesUsed(), ScoreTrackerService.GetLOTRCorrectAnswers(),
                ScoreTrackerService.GetLOTRWrongAnswers()
            };

            dataset_lotr.Data.AddRange(radarDataLOTR);
            _RadarConfigLOTR.Data.Datasets.Add(dataset_lotr);
            //end of HP radar config
        }
    }
}
