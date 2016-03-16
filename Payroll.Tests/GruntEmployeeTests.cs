using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayrollCalc.Employees;
using PayrollCalc.Organization;

namespace Payroll.Tests
{
    [TestFixture]
    public class GruntEmployeeTests
    {
        [Test, TestCaseSource(typeof(GruntsGenerator), "TestCases")]
        public void TestManagerYearBonus(int expectedYears, double expectedPay)
        {
            // No bonus
            DoYearsTest(expectedYears, expectedPay);
        }

        public void DoYearsTest(int yearDiff, double expectedPay)
        {
            Organization.Grunt gruntEmployee = new Organization.Grunt("Ivan", "Ivanov", new DateTime(DateTime.Now.Year - yearDiff, 01, 01), Organization.BASEPAYRATE);
            Assert.AreEqual(yearDiff, gruntEmployee.GetFullYears());
            Assert.AreEqual(expectedPay, gruntEmployee.CalcCurrentPay(), 0.01d);
        }
    }

    public class GruntsGenerator
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData(0, 10000.0d).SetName("NoYearBonus").SetDescription("0 full years, no bonus"); ;
                yield return new TestCaseData(1, 10300.0d).SetName("OneYearBonus").SetDescription("1 full year, 3% bonus");
                yield return new TestCaseData(2, 10600.0d).SetName("TwoYearBonus").SetDescription("2 full years, 6% bonus");
                yield return new TestCaseData(9, 12700.0d).SetName("NineYearBonus").SetDescription("9 full years, 27% bonus"); ;
                yield return new TestCaseData(10, 13000.0d).SetName("TenYearBonus").SetDescription("10 full years, 30% bonus"); ;
                yield return new TestCaseData(11, 13000.0d).SetName("ElevenYearBonus").SetDescription("11 full years, max (30%) bonus"); ;
            }
        }
    }
}
