using NUnit.Framework;
using Payroll.Tests.Generators;
using PayrollCalc.Employees;
using PayrollCalc.Organization;

namespace Payroll.Tests
{
    [TestFixture]
    public class SalesTests
    {
        [Test, TestCaseSource(typeof (SalesGenerator.YearBonusGenerator), nameof(SalesGenerator.YearBonusGenerator.TestSalesYears))]
        public double TestSalesYears(Managerial salesMan)
        {

            return salesMan.GetFullYears();

        }

        [Test,
         TestCaseSource(typeof (SalesGenerator.YearBonusGenerator),
             nameof(SalesGenerator.YearBonusGenerator.TestSalesBonus))]
        public double TestSalesBonus(Managerial salesMan)
        {
            return salesMan.GetAnnualBonus();
        }

        [Test,
         TestCaseSource(typeof (SalesGenerator.YearBonusGenerator),
             nameof(SalesGenerator.YearBonusGenerator.TestSalesPays))]
        public double TestSalesPay(Managerial salesMan)
        {
            return salesMan.CalcCurrentPay();
        }
    }
}