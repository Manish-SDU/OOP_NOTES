namespace Polymorphism.Polymorphism;

public class Rectangle : AbstractShape
{
    private double l1;
    private double l2;

    public Rectangle(double l1, double l2)
    {
        this.l1 = l1;
        this.l2 = l2;
    }

    public override double GetArea()
    {
        return l1 * l2;
    }

    public override double GetCircumference()
    {
        return 2 * (l1 + l2);
    }
}