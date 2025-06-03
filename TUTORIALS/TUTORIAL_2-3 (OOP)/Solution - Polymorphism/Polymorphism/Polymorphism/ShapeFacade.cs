using System;

namespace Polymorphism.Polymorphism;

public sealed class ShapeFacade
{
    // Creating the Singleton
    private static readonly Lazy<ShapeFacade> Lazy = new(() => new ShapeFacade());
    
    public static ShapeFacade GetInstance => Lazy.Value;

    private ShapeFacade()
    {
    }
    
    public enum Shapes
    {
        Ellipse,
        Rectangle,
        Circle,
        Square
    };

    public string GetShapeInfo(Shapes shape, params double[] p)
    {
        switch (shape)
        {
            case Shapes.Ellipse:
                Ellipse ellipse = new Ellipse(p[0], p[1]);
                return ellipse.ToString();
            case Shapes.Rectangle:
                Rectangle rectangle = new Rectangle(p[0], p[1]);
                return rectangle.ToString();
            case Shapes.Circle:
                Circle circle = new Circle(p[0]);
                return circle.ToString();
            case Shapes.Square:
                Square square = new Square(p[0]);
                return square.ToString();
            default:
                return "Shape not recognized.";
        }
    }
}