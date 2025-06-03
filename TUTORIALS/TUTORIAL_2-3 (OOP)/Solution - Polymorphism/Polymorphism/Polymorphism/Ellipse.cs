using System;

namespace Polymorphism.Polymorphism;

public class Ellipse : AbstractShape
{
    private double r1;
    private double r2;

    public Ellipse(double r1, double r2)
    {
        this.r1 = r1;
        this.r2 = r2;
    }
    public override double GetArea()
    {
        return Pi * r1 * r2;
    }

    public override double GetCircumference()
    {
        return 2*Pi*Math.Sqrt(0.5*(Math.Pow(r1,2) + Math.Pow(r2,2)));
    }
}