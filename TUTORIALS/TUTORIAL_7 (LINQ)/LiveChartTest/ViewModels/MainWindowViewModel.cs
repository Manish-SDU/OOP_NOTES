using System;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Defaults;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.Themes;
using SkiaSharp;
using System.Collections.Generic;

namespace LiveChartTest.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private string output = String.Empty;
    private List<double> counts = new();
    private List<string> genres = new();

    public ISeries[] Series { get; set; } = [];
    public Axis[] YAxes { get; set; } = [];

    public MainWindowViewModel()
    {
        TopMovieGenresQueryRunner runner = new();
        runner.Run(1000);

        counts = runner.counts;
        genres = runner.names;

        Series = [
            new RowSeries<double>
            {
                Values = counts,
            }
        ];

        YAxes = [
            new Axis
            {
                Labels = genres,
            }
        ];
    }
}