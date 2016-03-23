using System;

namespace PayrollCalc.Employees
{
    public interface IPayrollable
    {
        int GetFullYears();
        double GetAnnualBonus();
        double CalcCurrentPay();

    }

    public interface IManagerial : IPayrollable
    {
        int GetSubordinateCount();
        double GetSubordinatePay();
    }
}
