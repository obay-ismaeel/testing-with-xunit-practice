namespace SW.Payroll.Tests;

public class SalarySplitProcessorTests
{
    //[Fact]
    //public void Method_Senario_Outcome()
    //{
    //  1.Arrange
    //  2.Act
    //  3.Assert
    //}

    [Fact]
    public void CalculateBasicSalary_ForEmployeeIsNull_ThrowArgumentNullException()
    {
        Employee employee = null;

        var zone = new ZoneService();
        var processor = new SalarySlipProcessor(zone);

        Func<Employee, decimal> func = (e) => processor.CalculateBasicSalary(e);

        Assert.Throws<ArgumentNullException>(() => func(employee));
    }

    [Fact]
    public void CalculateBasicSalary_ForEmployeeWageAndWorkingDays_ReturnBasicSalary()
    {
        var employee = new Employee { Wage = 50, WorkingDays = 20 };

        var zone = new ZoneService();
        var processor = new SalarySlipProcessor(zone);

        var actual = processor.CalculateBasicSalary(employee);
        var expected = 1000m;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CalculateTransportationAllowece_ForEmployeeIsNull_ThrowArgumentNullException()
    {
        Employee employee = null;

        var zone = new ZoneService();
        var processor = new SalarySlipProcessor(zone);

        Func<Employee, decimal> func = (e) => processor.CalculateTransportationAllowece(e);

        Assert.Throws<ArgumentNullException>(() => func(employee));
    }

    [Fact]
    public void CalculateTransportationAllowece_ForEmployeeWorkInOffice_ReturnsTrasportationAllowece()
    {
        Employee employee = new Employee { WorkPlatform = WorkPlatform.Office };

        var zone = new ZoneService();
        var processor = new SalarySlipProcessor(zone);

        var actual = processor.CalculateTransportationAllowece(employee);
        var expected = Constants.TransportationAllowanceAmount;
        
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CalculateTransportationAllowece_ForEmployeeWorkRemotely_ReturnsTrasportationAllowece()
    {
        Employee employee = new Employee { WorkPlatform = WorkPlatform.Remote };

        var zone = new ZoneService();
        var processor = new SalarySlipProcessor(zone);

        var actual = processor.CalculateTransportationAllowece(employee);
        var expected = 0m;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CalculateTransportationAllowece_ForEmployeeWorkHybrid_ReturnsTrasportationAllowece()
    {
        Employee employee = new Employee { WorkPlatform = WorkPlatform.Hybrid };

        var zone = new ZoneService();
        var processor = new SalarySlipProcessor(zone);

        var actual = processor.CalculateTransportationAllowece(employee);
        var expected = Constants.TransportationAllowanceAmount / 2;

        Assert.Equal(expected, actual);
    }
}