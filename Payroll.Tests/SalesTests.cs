using NUnit.Framework;
using System;
using System.Collections;
using PayrollCalc.Employees;
using PayrollCalc.Organization;

namespace Payroll.Tests
{
    [TestFixture]
    public class SalesTests
    {
        [Test, TestCaseSource(typeof (ManagersGenerator), "NoTearSubordinatesTestCases")]
        public void SubrodinatesNoYearBonusTest(Organization.Sales salesMan, Managerial[] manager, GruntEmployee[] subordinates)
        {

           
        }


        private void ValidateSubsandPay(Managerial manager, int subCount, double currentPay)
        {
            Assert.AreEqual(subCount, manager.GetSubordinateCount());
            Assert.AreEqual(currentPay, manager.CalcCurrentPay(), 0.01d);
        }

    }
}



    public class SalesGenerator
    {



        public static IEnumerable NoTearSubordinatesTestCases
        {
            get
            {
                yield return
                    new TestCaseData(GetSales(0),GetManagers(0, 1), GetGruntEmployees(0, 1)).SetName("NoYearBonusOneEmployee")
                        .SetDescription("0 full years, 1 manager, 1 employee");
                yield return
                    new TestCaseData(GetSales(0), GetManagers(0, 2), GetGruntEmployees(0, 2)).SetName("NoYearBonusTwoEmployee")
                        .SetDescription("0 full years, 2 managers, 2 employees");
                yield return
                    new TestCaseData(GetSales(0), GetManagers(0, 3), GetGruntEmployees(0, 5)).SetName("NoYearBonusFiveEmployee")
                        .SetDescription("0 full years, 3 managers, 3 employees");
            }
        }

    private static Managerial GetSales(int years)
    {
        return new Organization.Sales("Manager", "Manageroff", DateTime.Now.AddYears(-years), Organization.BASEPAYRATE);
    }

    private static Managerial[] GetManagers(int years, int num)
        {
            Organization.Manager[] list = new Organization.Manager[num];
            for (int i = 0; i < num; i++)
                list[i] = new Organization.Manager("Grunt", "Gruntovich", DateTime.Now.AddYears(-years),
                    Organization.BASEPAYRATE);

            return list;
        }

        private static Organization.Grunt[] GetGruntEmployees(int years, int num)
        {
            Organization.Grunt[] list = new Organization.Grunt[num];
            for (int i = 0; i < num; i++)
                list[i] = new Organization.Grunt("Grunt", "Gruntovich", DateTime.Now.AddYears(-years),
                    Organization.BASEPAYRATE);

            return list;
        }
    }

