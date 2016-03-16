using NUnit.Framework;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using PayrollCalc.Employees;
using PayrollCalc.Organization;

namespace Payroll.Tests
{
    [TestFixture]
    public class ManagerTests
    {
        [Test, TestCaseSource(typeof(ManagersGenerator), "YearTestCases")]
        public void TestManagerYearBonus(int expectedYears, double expectedPay)
        {
            DoYearsTest(expectedYears, expectedPay);
        }

        [Test, TestCaseSource(typeof(ManagersGenerator), "NoTearSubordinatesTestCases")]
        public void SubrodinatesNoYearBonusTest(Managerial manager, GruntEmployee[] subordinates)
        {
            ValidateSubsandPay(manager, 0, Organization.BASEPAYRATE);

            foreach (var subordinate in subordinates)
            {
                subordinate.SetManager(manager);
            }

            ValidateSubsandPay(manager, subordinates.Length,
                Organization.BASEPAYRATE + (0.005*Organization.BASEPAYRATE)*subordinates.Length);


        }

        [Test, TestCaseSource(typeof(ManagersGenerator), "YearsOneSubordinateTestCases")]
        public void SubordinatesYearBonusTest(Managerial manager, GruntEmployee[] subordinates, double expectedPay)
        {

            foreach (var subordinate in subordinates)
            {
                subordinate.SetManager(manager);
            }

            ValidateSubsandPay(manager, subordinates.Length, expectedPay);
        }


        private void ValidateSubsandPay(Managerial manager, int subCount, double currentPay)
        {
            Assert.AreEqual(subCount, manager.GetSubordinateCount());
            Assert.AreEqual(currentPay, manager.CalcCurrentPay(), 0.01d);
        }

        private void DoYearsTest(int yearDiff, double expectedPay)
        {
            Organization.Manager managerEmployee = new Organization.Manager("Manager", "Mamnageroff", new DateTime(DateTime.Now.Year - yearDiff, 01, 01), Organization.BASEPAYRATE);
            Assert.AreEqual(yearDiff, managerEmployee.GetFullYears());
            Assert.AreEqual(expectedPay, managerEmployee.CalcCurrentPay(), 0.01d);
        }
    }


    public class ManagersGenerator
    {
        public static IEnumerable YearTestCases
        {
            get
            {
                yield return new TestCaseData(0, 10000.0d).SetName("NoYearBonus").SetDescription("0 full years, no bonus");
                yield return new TestCaseData(1, 10500.0d).SetName("OneYearBonus").SetDescription("1 full year, 5% bonus");
                yield return new TestCaseData(2, 11000.0d).SetName("TwoYearBonus").SetDescription("2 full years, 10 bonus");
                yield return new TestCaseData(7, 13500.0d).SetName("SevenYearBonus").SetDescription("7 full years, 35% bonus");
                yield return new TestCaseData(8, 14000.0d).SetName("EightYearBonus").SetDescription("8 full years, 40% bonus");
                yield return new TestCaseData(9, 14000.0d).SetName("NineYearBonus").SetDescription("9 full years, max (40%) bonus");
            }
        }


        public static IEnumerable NoTearSubordinatesTestCases
        {
            get
            {
                yield return new TestCaseData(GetManager(0), GetGruntEmployees(0, 1)).SetName("NoYearBonusOneEmployee").SetDescription("0 full years, 1 employee");
                yield return new TestCaseData(GetManager(0), GetGruntEmployees(0, 2)).SetName("NoYearBonusTwoEmployee").SetDescription("0 full years, 2 employees");
                yield return new TestCaseData(GetManager(0), GetGruntEmployees(0, 5)).SetName("NoYearBonusFiveEmployee").SetDescription("0 full years, 5 employees");
            }
        }

        public static IEnumerable YearsOneSubordinateTestCases
        {
            get
            {
                yield return new TestCaseData(GetManager(0), GetGruntEmployees(0, 1), 10050d).SetName("NoYearBonusOneEmployee").SetDescription("0 full years, 1 employee");
                yield return new TestCaseData(GetManager(1), GetGruntEmployees(0, 2), 10600d).SetName("OneYearBonusTwoEMployees").SetDescription("1 full years, 2 employees");
                yield return new TestCaseData(GetManager(2), GetGruntEmployees(0, 3), 11150d).SetName("TwoYearBonusThreeEmployees").SetDescription("2 full years, 3 employees");
                yield return new TestCaseData(GetManager(7), GetGruntEmployees(0, 1), 13550d).SetName("SevenYearBonusOneEmployees").SetDescription("7 full years, 1 employees");
                yield return new TestCaseData(GetManager(8), GetGruntEmployees(0, 2), 14100d).SetName("EightYearBonusTwoEmployees").SetDescription("8 full years, 2 employees");
                yield return new TestCaseData(GetManager(9), GetGruntEmployees(0, 3), 14150).SetName("NineYearBonusThreeEmployees").SetDescription("9 full years, 3 employees");
            }
        }

        private static Managerial GetManager(int years)
        {
            return new Organization.Manager("Manager","Manageroff",DateTime.Now.AddYears(-years), Organization.BASEPAYRATE);
        }

        private static Organization.Grunt[] GetGruntEmployees(int years, int num)
        {
            Organization.Grunt[] list = new Organization.Grunt[num];
            for (int i =0; i<num; i++)
                list[i] = new Organization.Grunt("Grunt","Gruntovich",DateTime.Now.AddYears(-years),Organization.BASEPAYRATE);
            
            return list;
        }

    }
}
