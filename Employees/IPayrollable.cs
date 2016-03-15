using System;
using PayrollCalc.Settings;

namespace Payroll
{
    public interface IPayrollable
    {

        int GetFullYears();
        int GetFullYearsFromDate(DateTime payrollDate);

        double GetAnnualBonus();
        double GetAnnualBonusFromDate(DateTime payrollDate);
        double CalcCurrentPay();
        double CalcPayForDate(DateTime payrollDate);

        void AddManager(Managerial manager);
        void UpdateManager(Managerial manager);
    }

    public interface IManagerial : IPayrollable
    {
        int GetSubordinateCount();
        int GetSubordinateCount(int subDepth);
    }
}
