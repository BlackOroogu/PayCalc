using System;
using System.Collections;
using NUnit.Framework;
using PayrollCalc.Employees;
using PayrollCalc.Organization;

namespace Payroll.Tests.Generators
{
    public class SalesGenerator
    {
        public class YearBonusGenerator
        {
            private static readonly string categoryName = "Annual Bonus Tests - Sales";
            private static readonly string namePattern = "{0:D2} Year Bonus - Sales";
            private static readonly string descriptionPatternYears = "{0:D2} full years";
            private static readonly string descriptionPatternBonus = "{0:D2} full years, {1}% bonus";
            private static readonly string descriptionPatternPay = "{0:D2} full years, {1} expected pay value";

            public static IEnumerable TestSalesPays
            {
                get
                {
                    yield return GenPayData(-1, Organization.BASEPAYRATE, new ForwardYearException());
                    yield return GenPayData(0, Organization.BASEPAYRATE);
                    yield return GenPayData(1, 10100.0d);
                    yield return GenPayData(2, 10200.0d);
                    yield return GenPayData(7, 10700.0d);
                    yield return GenPayData(34, 13400.0d);
                    yield return GenPayData(35, 13500.0d);
                    yield return GenPayData(36, 13500.0d);
                }
            }

            public static IEnumerable TestSalesYears
            {
                get
                {
                    yield return GenYearsData(0);
                    yield return GenYearsData(1);
                    yield return GenYearsData(2);
                    yield return GenYearsData(7);
                    yield return GenYearsData(34);
                    yield return GenYearsData(35);
                    yield return GenYearsData(36);
                }
            }

            public static IEnumerable TestSalesBonus
            {
                get
                {
                    yield return GenBonusData(0, 0);
                    yield return GenBonusData(1, 1);
                    yield return GenBonusData(2, 2);
                    yield return GenBonusData(7, 7);
                    yield return GenBonusData(34, 34);
                    yield return GenBonusData(35, 35);
                    yield return GenBonusData(36, 35);
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
                Organization.Sales salesEmployee = new Organization.Sales("Sales", "Salesoff",
                    DateTime.Now.AddYears(-fullYears), Organization.BASEPAYRATE);

                TestCaseData data = new TestCaseData(salesEmployee);
                data.SetName(string.Format(namePattern, fullYears));

                data.SetCategory(categoryName);
                return data;
            }
        }

        public class SubrodinatesGenerator
        {

            private static readonly string categoryName = "Subordinate Bonus Tests - Sales";
            private static readonly string descriptionPattern = "{0:D2} Yeas, 1st tire subordinates {1} Grunts, {2} Saless, {3} Sales";
            private static readonly string namePattern = "{0:D2} Years, 1st tire, {1} subordinates";


            public static IEnumerable TestSalesPays
            {
                get
                {
                    yield return GetSalesWithSubordinnatesData(0, Organization.BASEPAYRATE);
                    yield return GetSalesWithSubordinnatesData(0, 10050.0d, new int[] { 0 });
                    yield return GetSalesWithSubordinnatesData(0, 10100.0d, new int[] { 0 }, new int[] { 0 });
                    yield return GetSalesWithSubordinnatesData(0, 10150.0d, new int[] { 0 }, new int[] { 0 }, new int[] { 0 });
                    yield return GetSalesWithSubordinnatesData(1, 10550.0d, new int[] { 0 });
                    yield return GetSalesWithSubordinnatesData(9, 14050.0d, new int[] { 0 });
                    yield return GetSalesWithSubordinnatesData(1, 10551.5d, new int[] { 1 });
                    yield return GetSalesWithSubordinnatesData(0, 10104.0d, new int[] { 1 }, new int[] { 1 });
                    yield return GetSalesWithSubordinnatesData(1, 10604.0d, new int[] { 1 }, new int[] { 1 });
                }
            }

            public static IEnumerable TestSalesSubordinateCount
            {
                get
                {
                    yield return GetSalesWithSubordinnatesData(0, 0);
                    yield return GetSalesWithSubordinnatesData(0, 1, new int[] { 0 });
                    yield return GetSalesWithSubordinnatesData(0, 2, new int[] { 0 }, new int[] { 0 });
                    yield return GetSalesWithSubordinnatesData(0, 3, new int[] { 0 }, new int[] { 0 }, new int[] { 0 });

                }
            }


            private static TestCaseData GetSalesWithSubordinnatesData(int fullYears, double expectedResult, int[] gruntYears = null, int[] managersYears = null, int[] salesYears = null)
            {
                var salesMan = new Organization.Sales("Sales", "Salesoff", DateTime.Now.AddYears(-fullYears), Organization.BASEPAYRATE);
                int gruntsCount = 0;
                int managersCount = 0;
                int salesCount = 0;

                if (gruntYears != null)
                {
                    gruntsCount = gruntYears.Length;
                    foreach (var fullYear in gruntYears)
                    {
                        GetGrunt(fullYear).SetManager(salesMan);
                    }
                }

                if (managersYears != null)
                {
                    managersCount = managersYears.Length;
                    foreach (var fullYear in managersYears)
                    {
                        GetManager(fullYear).SetManager(salesMan);
                    }
                }

                if (salesYears != null)
                {
                    salesCount = salesYears.Length;

                    foreach (var fullYear in salesYears)
                    {
                        GetSales(fullYear).SetManager(salesMan);
                    }
                }
                var data = new TestCaseData(salesMan);

                data.SetCategory(categoryName);
                data.SetDescription(string.Format(descriptionPattern, fullYears, gruntsCount, managersCount, salesCount));
                data.SetName(string.Format(namePattern, fullYears, gruntsCount + managersCount + salesCount));
                data.Returns(expectedResult);

                return data;
            }

            private static Organization.Grunt GetGrunt(int fullYears)
            {
                return new Organization.Grunt("Ivan", "Ivanoff", DateTime.Now.AddYears(-fullYears), Organization.BASEPAYRATE);
            }

            private static Organization.Manager GetManager(int fullYears)
            {
                return new Organization.Manager("Manager", "Manageroff", DateTime.Now.AddYears(-fullYears), Organization.BASEPAYRATE);
            }

            private static Organization.Sales GetSales(int fullYears)
            {
                return new Organization.Sales("Sales", "SalesOff", DateTime.Now.AddYears(-fullYears), Organization.BASEPAYRATE);
            }
        }
    }
}
