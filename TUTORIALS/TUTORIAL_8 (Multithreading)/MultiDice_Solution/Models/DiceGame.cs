namespace MultiDice.Models;

using MultiDice.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

using Avalonia;
using Avalonia.Media.Imaging; 


public class DiceGame 
{
    private bool _isRunning = false;
    private MainWindowViewModel _viewModel;

    public DiceGame(MainWindowViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public async Task Start()
    {
        if (_isRunning) return;
        else _isRunning = true;

        await DiceRolling();
    }

    public void Stop()
    {
        _isRunning = false;
    }

    private Task DiceRolling()
    {
        return Task.Run( async () =>
        {
            try 
            {
                Dice dice = new Dice();
                while (_isRunning && !dice.EqualsMax())
                {
                    await Task.Delay(100);
                    dice.ThrowDice();

                    await _viewModel.Update(dice.Die1, dice.Die2, dice.Count);
                } 

                Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DiceGame Task Error: {ex.Message}");
            }
        });
    }

}