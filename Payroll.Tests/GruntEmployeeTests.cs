using NUnit.Framework;
using Payroll.Tests.Generators;
using PayrollCalc.Employees;

namespace Payroll.Tests
{
    [TestFixture]
    public class GruntEmployeeTests
    {


        [Test, TestCaseSource(typeof(GruntsGenerator.YearBonusGenerator), nameof(GruntsGenerator.YearBonusGenerator.TestGruntYears))]
        public int TestGruntYears(GruntEmployee gruntEmployee)
        {
            return gruntEmployee.GetFullYears();
        }

        [Test, TestCaseSource(typeof(GruntsGenerator.YearBonusGenerator), nameof(GruntsGenerator.YearBonusGenerator.TestGruntBonus))]
        public double TestGruntBonus(GruntEmployee gruntEmployee)
        {
            return gruntEmployee.GetAnnualBonus();
        }

        [Test, TestCaseSource(typeof(GruntsGenerator.YearBonusGenerator), nameof(GruntsGenerator.YearBonusGenerator.TestGruntPays))]
        public double TestGruntPay(GruntEmployee gruntEmployee)
        {
            return gruntEmployee.CalcCurrentPay();
        }
    }

 
}
