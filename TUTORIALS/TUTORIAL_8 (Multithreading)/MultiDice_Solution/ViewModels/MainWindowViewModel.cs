namespace MultiDice.ViewModels;

using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using MultiDice.Models;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private Bitmap? dieOneImage;

    [ObservableProperty]
    private Bitmap? dieTwoImage;

    [ObservableProperty]
    private int result;

    private DiceGame _diceGame;
    
    private List<Bitmap> _images;

    public MainWindowViewModel()
    {
        _images = new List<Bitmap>();

        for (int i = 1; i < 7; i++)
        {
            _images.Add(ImageHelper.LoadFromResource(new Uri($"avares://MultiDice/Assets/dice{i}.png")));
        }

        _diceGame = new DiceGame(this);

        dieOneImage = _images[0];
        dieTwoImage = _images[1];
    }

    [RelayCommand]
    public async Task Start()
    {
       await _diceGame.Start();
    }

    [RelayCommand]
    public void Stop()
    {
        _diceGame.Stop();
    }

    public async Task Update(int d1, int d2, int count)
    {
        await Dispatcher.UIThread.InvokeAsync( () =>
        {
            DieOneImage = _images[d1 - 1];
            DieTwoImage = _images[d2 - 1];
            Result = count;
        });
    }

}
