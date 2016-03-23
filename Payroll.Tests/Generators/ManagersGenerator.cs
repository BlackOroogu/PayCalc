using System;
using System.Collections;
using System.Runtime.Remoting;
using NUnit.Framework;
using PayrollCalc.Employees;
using PayrollCalc.Organization;

namespace Payroll.Tests.Generators
{
    public class ManagersGenerator
    {
        public class YearBonusGenerator
        {
            private static readonly string categoryName = "Annual Bonus Tests - Manager";
            private static readonly string namePattern = "{0:D2} Year Bonus - Manager";
            private static readonly string descriptionPatternYears = "{0:D2} full years";
            private static readonly string descriptionPatternBonus = "{0:D2} full years, {1}% bonus";
            private static readonly string descriptionPatternPay = "{0:D2} full years, {1} expected pay value";

            public static IEnumerable TestManagerPays
            {
                get
                {
                    yield return GenPayData(-1, 0, new ForwardYearException());
                    yield return GenPayData(0, Organization.BASEPAYRATE);
                    yield return GenPayData(1, 10500.0d);
                    yield return GenPayData(2, 11000.0d);
                    yield return GenPayData(7, 13500.0d);
                    yield return GenPayData(8, 14000.0d);
                    yield return GenPayData(10, 14000.0d);
                    yield return GenPayData(15, 14000.0d);
                }
            }

            public static IEnumerable TestManagerYears
            {
                get
                {
                    yield return GenYearsData(0);
                    yield return GenYearsData(1);
                    yield return GenYearsData(2);
                    yield return GenYearsData(9);
                    yield return GenYearsData(10);
                    yield return GenYearsData(11);
                    yield return GenYearsData(15);
                }
            }

            public static IEnumerable TestManagerBonus
            {
                get
                {
                    yield return GenBonusData(0, 0);
                    yield return GenBonusData(1, 5);
                    yield return GenBonusData(2, 10);
                    yield return GenBonusData(7, 35);
                    yield return GenBonusData(8, 40);
                    yield return GenBonusData(9, 40);
                    yield return GenBonusData(10, 40);
                    yield return GenBonusData(15, 40);
                }
            }

            private static TestCaseData GenPayData(int fullYears, double expectedPay, Exception exception = null)
            {
                var data = GenData(fullYears);
                data.SetDescription(string.Format(descriptionPatternPay, fullYears, expectedPay));
                if (exception != null)
                    data.Throws(exception.GetType());
                else
                    data.Returns(expectedPay);
                return data;
            }

            private static TestCaseData GenYearsData(int fullYears)
            {
                var data = GenData(fullYears);
                data.SetDescription(string.Format(descriptionPatternYears, fullYears));
                data.Returns(fullYears);
                return data;
            }

            private static TestCaseData GenBonusData(int fullYears, int expectedBonus)
            {
                var data = GenData(fullYears);
                data.SetDescription(string.Format(descriptionPatternBonus, fullYears, expectedBonus));
                data.Returns(expectedBonus);
                return data;
            }



            private static TestCaseData GenData(int fullYears)
            {
                Organization.Manager managerEmployee = new Organization.Manager("Manager", "Manageroff",
                    DateTime.Now.AddYears(-fullYears), Organization.BASEPAYRATE);

                TestCaseData data = new TestCaseData(managerEmployee);
                data.SetName(string.Format(namePattern, fullYears));

                data.SetCategory(categoryName);
                return data;
            }
        }

        public class SubrodinatesGenerator
        {

            private static readonly string categoryName = "Subordinate Bonus Tests - Manager";
            private static readonly string descriptionPattern = "{0:D2} Yeas, 1st tire subordinates {1} Grunts, {2} MAnagers, {3} Sales";
            private static readonly string namePattern = "{0:D2} Years, 1st tire, {1} subordinates";


            public static IEnumerable TestManagerPays
            {
                get
                {
                    yield return GetManagerWithSubordinnatesData(0,Organization.BASEPAYRATE);
                    yield return GetManagerWithSubordinnatesData(0, 10050.0d, new int[] {0});   
                    yield return GetManagerWithSubordinnatesData(0, 10100.0d, new int[] { 0 }, new int[] { 0 });
                    yield return GetManagerWithSubordinnatesData(0, 10150.0d, new int[] { 0 }, new int[] { 0 }, new int[] { 0 });
                    yield return GetManagerWithSubordinnatesData(1, 10550.0d, new int[] { 0 });
                    yield return GetManagerWithSubordinnatesData(9, 14050.0d, new int[] { 0 });
                    yield return GetManagerWithSubordinnatesData(1, 10551.5d, new int[] { 1 });
                    yield return GetManagerWithSubordinnatesData(0, 10104.0d, new int[] { 1 }, new int[] { 1 });
                    yield return GetManagerWithSubordinnatesData(1, 10604.0d, new int[] { 1 }, new int[] { 1 });
                }
            }

            public static IEnumerable TestManagerSubordinateCount
            {
                get
                {
                    yield return GetManagerWithSubordinnatesData(0, 0);
                    yield return GetManagerWithSubordinnatesData(0, 1, new int[] { 0 });
                    yield return GetManagerWithSubordinnatesData(0, 2, new int[] { 0 }, new int[] { 0 });
                    yield return GetManagerWithSubordinnatesData(0, 3, new int[] { 0 }, new int[] { 0 }, new int[] { 0 });

                }
            }


            private static TestCaseData GetManagerWithSubordinnatesData(int fullYears, double expectedResult, int[] gruntYears = null, int[] managersYears = null, int[] salesYears = null)
            {
                var manager =  new Organization.Manager("Manager","Manageroff",DateTime.Now.AddYears(-fullYears), Organization.BASEPAYRATE);
                int gruntsCount = 0;
                int managersCount = 0;
                int salesCount = 0;

                if (gruntYears != null)
                {
                    gruntsCount = gruntYears.Length;
                    foreach (var fullYear in gruntYears)
                    {
                        GetGrunt(fullYear).SetManager(manager);
                    }
                }

                if (managersYears != null)
                {
                    managersCount = managersYears.Length;
                    foreach (var fullYear in managersYears)
                    {
                        GetManager(fullYear).SetManager(manager);
                    }
                }

                if (salesYears != null)
                {
                    salesCount = salesYears.Length;

                    foreach (var fullYear in salesYears)
                    {
                        GetSales(fullYear).SetManager(manager);
                    }
                }
                var data = new TestCaseData(manager);

                data.SetCategory(categoryName);
                data.SetDescription(string.Format(descriptionPattern, fullYears, gruntsCount, managersCount, salesCount));
                data.SetName(string.Format(namePattern, fullYears, gruntsCount + managersCount + salesCount));
                data.Returns(expectedResult);

                return data;
            }

            private static Organization.Grunt GetGrunt(int fullYears)
            {
                return new Organization.Grunt("Ivan","Ivanoff",DateTime.Now.AddYears(-fullYears),Organization.BASEPAYRATE);
            }

            private static Organization.Manager GetManager(int fullYears)
            {
                return new Organization.Manager("Manager","Manageroff",DateTime.Now.AddYears(-fullYears),Organization.BASEPAYRATE);
            }

            private static Organization.Sales GetSales(int fullYears)
            {
                return new Organization.Sales("Sales", "SalesOff", DateTime.Now.AddYears(-fullYears), Organization.BASEPAYRATE);
            }
        }
    }
}
