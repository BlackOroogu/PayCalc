using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Security.Cryptography;
using System.Text;

namespace Payroll
{
    interface IPayrollable
    {
        int GetFullYears();
        int GetFullYearsFromDate(DateTime payrollDate);
        double GetAnnualBonus();
        double GetAnnualBonusFromDate(DateTime payrollDate);
        double CalcCurrentPay();
        double CalcPayForDate(DateTime payrollDate);
    }
}
