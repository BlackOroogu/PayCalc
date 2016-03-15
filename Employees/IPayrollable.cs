using System;

namespace Payroll
{
    interface IPayrollable
    {

        int GetFullYears();
        int GetFullYearsFromDate(DateTime payrollDate);
        int GetSubordinateCount();
        int GetSubordinateCount(int subDepth);
        double GetAnnualBonus();
        double GetAnnualBonusFromDate(DateTime payrollDate);
        double CalcCurrentPay();
        double CalcPayForDate(DateTime payrollDate);
    }
}
