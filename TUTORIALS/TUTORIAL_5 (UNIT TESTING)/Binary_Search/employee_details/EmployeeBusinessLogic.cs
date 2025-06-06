namespace Binary_Search.employee_details;

public class EmployeeBusinessLogic
{
    // Calculate the yearly salary of employee
    public double CalculateYearlySalary(Employee employee)
    {
        var yearlySalary = employee.MonthlySalary * 12;
        return yearlySalary;
    }
    
    // Calculate the appraisal amount of employee
    public double CalculateAppraisal(Employee employee) {
        double appraisal;

        if(employee.MonthlySalary < 10000){
            appraisal = 500;
        }else{
            appraisal = 1000;
        }

        return appraisal;
    }
}