using NUnit.Framework;
using Payroll.Tests.Generators;
using PayrollCalc.Employees;
using PayrollCalc.Organization;

namespace Payroll.Tests
{
    [TestFixture]
    public class ManagerTests
    {
        [Test, TestCaseSource(typeof (ManagersGenerator.YearBonusGenerator), nameof(ManagersGenerator.YearBonusGenerator.TestManagerYears))]
        public int TestManagerYearYears(Managerial manager)
        {
            return manager.GetFullYears();
        }

        [Test, TestCaseSource(typeof(ManagersGenerator.YearBonusGenerator), nameof(ManagersGenerator.YearBonusGenerator.TestManagerBonus))]
        public double TestManagerBonus(Managerial manager)
        {
            return manager.GetAnnualBonus();
        }

        [Test, TestCaseSource(typeof(ManagersGenerator.YearBonusGenerator), nameof(ManagersGenerator.YearBonusGenerator.TestManagerPays))]
        public double TestManagerPay(Managerial Manager)
        {
            return Manager.CalcCurrentPay();
        }

        [Test, TestCaseSource(typeof (ManagersGenerator.SubrodinatesGenerator),nameof(ManagersGenerator.SubrodinatesGenerator.TestManagerPays))]
        public double TestSubordinateBonus(Managerial Manager)
        {
            return Manager.CalcCurrentPay();
        }

        [Test, TestCaseSource(typeof(ManagersGenerator.SubrodinatesGenerator), nameof(ManagersGenerator.SubrodinatesGenerator.TestManagerSubordinateCount))]
        public double TestSubordinateCount(Managerial Manager)
        {
            return Manager.GetSubordinateCount();
        }

    }


}
