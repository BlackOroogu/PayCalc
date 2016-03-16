using System;

namespace PayrollCalc.Employees
{
    public interface IPayrollable
    {

        int GetFullYears();
        int GetFullYearsFromDate(DateTime payrollDate);

        double GetAnnualBonus();
        double GetAnnualBonusFromDate(DateTime payrollDate);
        double CalcCurrentPay();
        double CalcPayForDate(DateTime payrollDate);
    }

    public interface IManagerial : IPayrollable
    {
        int GetSubordinateCount();
        int GetSubordinateCount(int subDepth);
        double GetSubordinatePay();
    }
}
