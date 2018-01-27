using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.DataVisualization.Charting;
using System.Globalization;
using TIPS.Entities.Assess;

namespace CMS.Utilities.ChartUtilities
{
    public class RptCardShareChart : RptCardChartBase
    {
        private CMS.Models.RptCardShareModel.RptCardShareChartData chartData;

        public RptCardShareChart(CMS.Models.RptCardShareModel.RptCardShareChartData chartData)
        {
            this.chartData = chartData;
        }

        //
        protected override void AddChartTitle()
        {
            ChartTitle = chartData.Title;
        }

        // Override the AddChartSeries method to provide the chart data
        protected override void AddChartSeries(List<string> subLst)
        {
            ChartSeriesData = new List<Series>();

            foreach (var item in subLst)
            {

                switch (item.Trim())
                {
                    case "Biology":
                        {
                            var BiologySeries = new Series()
                            {
                                Name = "Biology",
                                ChartType = SeriesChartType.Line,
                                  BorderWidth = 3
                            };
                            var BiologyShares = chartData.Biology;
                            foreach (var share in BiologyShares)
                            {
                                var point = new DataPoint();
                                point.IsValueShownAsLabel = true;
                                point.AxisLabel = share.Name;
                                point.YValues = new double[] { share.Mark };
                                BiologySeries.Points.Add(point);
                            }
                            ChartSeriesData.Add(BiologySeries);
                            break;
                        }
                    case "Chemistry":
                        {
                            var ChemistrySeries = new Series()
                            {
                                Name = "Chemistry",
                                ChartType = SeriesChartType.Line,
                                  BorderWidth = 3
                            };
                            var ChemistryShares = chartData.Chemistry;
                            foreach (var share in ChemistryShares)
                            {
                                var point = new DataPoint();
                                point.IsValueShownAsLabel = true;
                                point.AxisLabel = share.Name;
                                point.YValues = new double[] { share.Mark };
                                //point.LabelFormat = "P1";
                                ChemistrySeries.Points.Add(point);
                            }

                            ChartSeriesData.Add(ChemistrySeries);
                            break;
                        }
                    case "Combined Science":
                        {
                            var CombinedScienceSeries = new Series()
                            {
                                Name = "Combined Science",
                                ChartType = SeriesChartType.Line,
                                  BorderWidth = 3
                            };
                            var CombinedScienceShares = chartData.CombinedScience;
                            foreach (var share in CombinedScienceShares)
                            {
                                var point = new DataPoint();
                                point.IsValueShownAsLabel = true;
                                point.AxisLabel = share.Name;
                                point.YValues = new double[] { share.Mark };
                                //point.LabelFormat = "P1";
                                CombinedScienceSeries.Points.Add(point);
                            }

                            ChartSeriesData.Add(CombinedScienceSeries);
                            break;
                        }
                    case "ICT":
                        {
                            var ICTSeries = new Series()
                            {
                                Name = "ICT",
                                ChartType = SeriesChartType.Line,
                                  BorderWidth = 3
                            };
                            var ICTShares = chartData.ICT;
                            foreach (var share in ICTShares)
                            {
                                var point = new DataPoint();
                                point.IsValueShownAsLabel = true;
                                point.AxisLabel = share.Name;
                                point.YValues = new double[] { share.Mark };
                                //point.LabelFormat = "P1";
                                ICTSeries.Points.Add(point);
                            }

                            ChartSeriesData.Add(ICTSeries);
                            break;
                        }
                    case "Economics":
                        {
                            var EconomicsSeries = new Series()
                            {
                                Name = "Economics",
                                ChartType = SeriesChartType.Line,
                                  BorderWidth = 3
                            };
                            var EconomicsShares = chartData.Economics;
                            foreach (var share in EconomicsShares)
                            {
                                var point = new DataPoint();
                                point.IsValueShownAsLabel = true;
                                point.AxisLabel = share.Name;
                                point.YValues = new double[] { share.Mark };
                                //point.LabelFormat = "P1";
                                EconomicsSeries.Points.Add(point);
                            }

                            ChartSeriesData.Add(EconomicsSeries);
                            break;
                        }
                    case "English":
                        {
                            var EnglishSeries = new Series()
                            {
                                Name = "English",
                                ChartType = SeriesChartType.Line,
                                  BorderWidth = 3
                            };
                            var EnglishShares = chartData.English;
                            foreach (var share in EnglishShares)
                            {
                                var point = new DataPoint();
                                point.IsValueShownAsLabel = true;
                                point.AxisLabel = share.Name;
                                point.YValues = new double[] { share.Mark };
                                //point.LabelFormat = "P1";
                                EnglishSeries.Points.Add(point);
                            }

                            ChartSeriesData.Add(EnglishSeries);
                            break;
                        }
                    case "English as First Language":
                        {
                            var EnglishasFirstLanguageSeries = new Series()
                            {
                                Name = "English as First Language",
                                ChartType = SeriesChartType.Line,
                                  BorderWidth = 3
                            };
                            var EnglishasFirstLanguageShares = chartData.EnglishasFirstLanguage;
                            foreach (var share in EnglishasFirstLanguageShares)
                            {
                                var point = new DataPoint();
                                point.IsValueShownAsLabel = true;
                                point.AxisLabel = share.Name;
                                point.YValues = new double[] { share.Mark };
                                //point.LabelFormat = "P1";
                                EnglishasFirstLanguageSeries.Points.Add(point);
                            }

                            ChartSeriesData.Add(EnglishasFirstLanguageSeries);
                            break;
                        }
                    case "English as Second Language":
                        {
                            var EnglishasSecondLanguageSeries = new Series()
                            {
                                Name = "English as Second Language",
                                ChartType = SeriesChartType.Line,
                                  BorderWidth = 3
                            };
                            var EnglishasSecondLanguageShares = chartData.EnglishasSecondLanguage;
                            foreach (var share in EnglishasSecondLanguageShares)
                            {
                                var point = new DataPoint();
                                point.IsValueShownAsLabel = true;
                                point.AxisLabel = share.Name;
                                point.YValues = new double[] { share.Mark };
                                //point.LabelFormat = "P1";
                                EnglishasSecondLanguageSeries.Points.Add(point);
                            }

                            ChartSeriesData.Add(EnglishasSecondLanguageSeries);
                            break;
                        }
                    case "Fine Arts":
                        {
                            var FineArtsSeries = new Series()
                            {
                                Name = "Fine Arts",
                                ChartType = SeriesChartType.Line,
                                  BorderWidth = 3
                            };

                            var FineArtsShares = chartData.FineArts;
                            foreach (var share in FineArtsShares)
                            {
                                var point = new DataPoint();
                                point.IsValueShownAsLabel = true;
                                point.AxisLabel = share.Name;
                                point.YValues = new double[] { share.Mark };
                                //point.LabelFormat = "P1";
                                FineArtsSeries.Points.Add(point);
                            }

                            ChartSeriesData.Add(FineArtsSeries);
                            break;
                        }
                    case "French":
                        {
                            var FrenchSeries = new Series()
                            {
                                Name = "French",
                                ChartType = SeriesChartType.Line,
                                  BorderWidth = 3
                            };
                            var FrenchShares = chartData.French;
                            foreach (var share in FrenchShares)
                            {
                                var point = new DataPoint();
                                point.IsValueShownAsLabel = true;
                                point.AxisLabel = share.Name;
                                point.YValues = new double[] { share.Mark };
                                //point.LabelFormat = "P1";
                                FrenchSeries.Points.Add(point);
                            }

                            ChartSeriesData.Add(FrenchSeries);

                            break;
                        }
                    case "Hindi":
                        {
                            var HindiSeries = new Series()
                            {
                                Name = "Hindi",
                                ChartType = SeriesChartType.Line,
                                  BorderWidth = 3
                            };

                            var HindiShares = chartData.Hindi;
                            foreach (var share in HindiShares)
                            {
                                var point = new DataPoint();
                                point.IsValueShownAsLabel = true;
                                point.AxisLabel = share.Name;
                                point.YValues = new double[] { share.Mark };
                                //point.LabelFormat = "P1";
                                HindiSeries.Points.Add(point);
                            }
                            ChartSeriesData.Add(HindiSeries);
                            break;
                        }
                    case "History & Geography":
                        {
                            var HistoryAndGeographySeries = new Series()
                            {
                                Name = "History & Geography",
                                ChartType = SeriesChartType.Line,
                                  BorderWidth = 3
                            };
                            var HistoryAndGeographyShares = chartData.HistoryAndGeography;
                            foreach (var share in HistoryAndGeographyShares)
                            {
                                var point = new DataPoint();
                                point.IsValueShownAsLabel = true;
                                point.AxisLabel = share.Name;
                                point.YValues = new double[] { share.Mark };
                                //point.LabelFormat = "P1";
                                HistoryAndGeographySeries.Points.Add(point);
                            }

                            ChartSeriesData.Add(HistoryAndGeographySeries);
                            break;
                        }
                    case "Math":
                        {
                            var MathematicsSeries = new Series()
                            {
                                Name = "Mathematics",
                                ChartType = SeriesChartType.Line,
                                  BorderWidth = 3
                            };
                            var MathematicsShares = chartData.Mathematics;
                            foreach (var share in MathematicsShares)
                            {
                                var point = new DataPoint();
                                point.IsValueShownAsLabel = true;
                                point.AxisLabel = share.Name;
                                point.YValues = new double[] { share.Mark };
                                //point.LabelFormat = "P1";
                                MathematicsSeries.Points.Add(point);
                            }
                            ChartSeriesData.Add(MathematicsSeries);
                            break;
                        }
                    case "Physical Education":
                        {
                            var PhysicalEducationSeries = new Series()
                            {
                                Name = "Physical Education",
                                ChartType = SeriesChartType.Line,
                                  BorderWidth = 3
                            };
                            var PhysicalEducationShares = chartData.PhysicalEducation;
                            foreach (var share in PhysicalEducationShares)
                            {
                                var point = new DataPoint();
                                point.IsValueShownAsLabel = true;
                                point.AxisLabel = share.Name;
                                point.YValues = new double[] { share.Mark };
                                //point.LabelFormat = "P1";
                                PhysicalEducationSeries.Points.Add(point);
                            }
                            ChartSeriesData.Add(PhysicalEducationSeries);
                            break;
                        }
                    case "Physics":
                        {
                            var PhysicsSeries = new Series()
                            {
                                Name = "Physics",
                                ChartType = SeriesChartType.Line,
                                  BorderWidth = 3
                            };
                            var PhysicsShares = chartData.Physics;
                            foreach (var share in PhysicsShares)
                            {
                                var point = new DataPoint();
                                point.IsValueShownAsLabel = true;
                                point.AxisLabel = share.Name;
                                point.YValues = new double[] { share.Mark };
                                //point.LabelFormat = "P1";
                                PhysicsSeries.Points.Add(point);
                            }
                            ChartSeriesData.Add(PhysicsSeries);
                            break;
                        }
                    case "Spark Lab":
                        {
                            var SparkLabSeries = new Series()
                            {
                                Name = "Spark Lab",
                                ChartType = SeriesChartType.Line,
                                  BorderWidth = 3
                            };
                            var SparkLabShares = chartData.SparkLab;
                            foreach (var share in SparkLabShares)
                            {
                                var point = new DataPoint();
                                point.IsValueShownAsLabel = true;
                                point.AxisLabel = share.Name;
                                point.YValues = new double[] { share.Mark };
                                //point.LabelFormat = "P1";
                                SparkLabSeries.Points.Add(point);
                            }
                            ChartSeriesData.Add(SparkLabSeries);
                            break;
                        }
                    case "Sports & Games":
                        {
                            var SportsAndGamesSeries = new Series()
                            {
                                Name = "Sports & Games",
                                ChartType = SeriesChartType.Line,
                                  BorderWidth = 3
                            };
                            var SportsAndGamesShares = chartData.SportsAndGames;
                            foreach (var share in SportsAndGamesShares)
                            {
                                var point = new DataPoint();
                                point.IsValueShownAsLabel = true;
                                point.AxisLabel = share.Name;
                                point.YValues = new double[] { share.Mark };
                                //point.LabelFormat = "P1";
                                SportsAndGamesSeries.Points.Add(point);
                            }
                            ChartSeriesData.Add(SportsAndGamesSeries);
                            break;
                        }
                    case "Stem Lab":
                        {
                            var StemLabSeries = new Series()
                            {
                                Name = "Stem Lab",
                                ChartType = SeriesChartType.Line,
                                  BorderWidth = 3
                            };
                            var StemLabShares = chartData.StemLab;
                            foreach (var share in StemLabShares)
                            {
                                var point = new DataPoint();
                                point.IsValueShownAsLabel = true;
                                point.AxisLabel = share.Name;
                                point.YValues = new double[] { share.Mark };
                                //point.LabelFormat = "P1";
                                StemLabSeries.Points.Add(point);
                            }
                            ChartSeriesData.Add(StemLabSeries);
                            break;
                        }
                }
            }










        }
    }
}