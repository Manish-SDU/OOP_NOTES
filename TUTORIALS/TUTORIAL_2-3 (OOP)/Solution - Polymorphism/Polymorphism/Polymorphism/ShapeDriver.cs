using System;
using System.Collections.Generic;

namespace Polymorphism.Polymorphism;

public class ShapeDriver
{

    public static void Main(string[] args)
    {
        List<IShapeInterface> list = new List<IShapeInterface>();
        
        // Remove comments after implementing classes.
        list.Add(new Circle(3.4));
        list.Add(new Rectangle(3.4, 4.4));
        list.Add(new Ellipse(3.4, 2.7));
        list.Add(new Square(3.4));
        list.ForEach(Console.WriteLine);
    }
}