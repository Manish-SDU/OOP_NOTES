namespace Binary_Search.employee_details;

public class Employee
{
    public Employee(string name, double monthlySalary, int age)
    {
        Name = name;
        MonthlySalary = monthlySalary;
        Age = age;
    }

    public string Name { get; set; }

    public double MonthlySalary { get; set; }

    public int Age { get; set; }
}