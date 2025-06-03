using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Polymorphism.Polymorphism;

public partial class MainWindow : Window
{
    public MainWindow() 
    {
        InitializeComponent();
        RadioEllipse.Content = ShapeFacade.Shapes.Ellipse;
        RadioRectangle.Content = ShapeFacade.Shapes.Rectangle;
        RadioCircle.Content = ShapeFacade.Shapes.Circle;
        RadioSquare.Content = ShapeFacade.Shapes.Square;
        
    }

    private ShapeFacade.Shapes _selectedShape;
    private void ButtonHandler(object? sender, RoutedEventArgs e)
    {
        Console.WriteLine(_selectedShape);
        switch (_selectedShape)
        {
            case ShapeFacade.Shapes.Ellipse:
            case ShapeFacade.Shapes.Rectangle:
                if (double.TryParse(Parameter1.Text, out double p1) && double.TryParse(Parameter2.Text, out double p2))
                {
                    TextBlock.Text += ShapeFacade.GetInstance.GetShapeInfo(_selectedShape, p1, p2)+ "\n";
                }
                else
                {
                    Console.WriteLine("Failed to parse parameters");
                }
                break;
            case ShapeFacade.Shapes.Circle:
            case ShapeFacade.Shapes.Square:
                if (!double.TryParse(Parameter1.Text, out double p))
                {
                    Console.WriteLine("Failed to parse parameter");
                    break;
                }
                TextBlock.Text += ShapeFacade.GetInstance.GetShapeInfo(_selectedShape, p) + "\n";
                break;
        }
    }

    private void RadioButtonHandler(object? sender, RoutedEventArgs e)
    {
        if (sender!.Equals(RadioEllipse) || sender.Equals(RadioRectangle))
        {
            Parameter2.IsVisible = true;
        } 
        else if (sender.Equals(RadioCircle) || sender.Equals(RadioSquare))
        {
            Parameter2.IsVisible = false;
        }
        _selectedShape = (ShapeFacade.Shapes)(sender as RadioButton)!.Content!;
    }
}