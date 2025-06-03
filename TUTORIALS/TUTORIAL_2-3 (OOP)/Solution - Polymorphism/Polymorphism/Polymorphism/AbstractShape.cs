using System;

namespace Polymorphism.Polymorphism;

public abstract class AbstractShape : IShapeInterface
{
    public override string ToString()
    {
        return  GetType().Name + ": \t" + " Area: \t" + GetArea().ToString("0.00") + "\t Circumference: " + GetCircumference().ToString("0.00");
    }

    public double Pi { get; } = Math.PI;
    public abstract double GetArea();
    public abstract double GetCircumference();
}