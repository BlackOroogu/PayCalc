using System;
using System.Collections;
using NUnit.Framework;
using PayrollCalc.Employees;
using PayrollCalc.Organization;

namespace Payroll.Tests.Generators
{
    public class GruntsGenerator
    {
        public class YearBonusGenerator
        {
        private static readonly
            string categoryName = "Annual Bonus Tests - Grunt Employee";
            private static readonly string namePattern = "{0:D2} Year Bonus - Grunt";
            private static readonly string descriptionPatternYears = "{0:D2} full years";
            private static readonly string descriptionPatternBonus = "{0:D2} full years, {1}% bonus";
            private static readonly string descriptionPatternPay = "{0:D2} full years, {1} expected pay value";

            public static
            IEnumerable
            TestGruntPays
            {
                get
                {
                    yield return GenPayData(-1, Organization.BASEPAYRATE, new ForwardYearException());
                    yield return GenPayData(0, Organization.BASEPAYRATE);
                    yield return GenPayData(1, 10300.0d);
                    yield return GenPayData(2, 10600.0d);
                    yield return GenPayData(9, 12700.0d);
                    yield return GenPayData(10, 13000.0d);
                    yield return GenPayData(11, 13000.0d);
                    yield return GenPayData(15, 13000.0d);
                }
            }

        public static
            IEnumerable
            TestGruntYears
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

        public static
            IEnumerable
            TestGruntBonus
            {
                get
                {
                    yield return GenBonusData(0, 0);
                    yield return GenBonusData(1, 3);
                    yield return GenBonusData(2, 6);
                    yield return GenBonusData(9, 27);
                    yield return GenBonusData(10, 30);
                    yield return GenBonusData(11, 30);
                    yield return GenBonusData(15, 30);
                }
            }

        private static
            TestCaseData GenPayData 
            (int fullYears, double expectedPay, Exception exception = null)
            {
                TestCaseData data = GenData(fullYears);
                data.SetDescription(string.Format(descriptionPatternPay, fullYears, expectedPay));
                if (exception != null)
                    data.Throws(exception.GetType());
                 else
                    data.Returns(expectedPay);
                return data;
            }

        private static
            TestCaseData GenYearsData 
            (int
            fullYears)
            {
                var data = GenData(fullYears);
                data.SetDescription(string.Format(descriptionPatternYears, fullYears));
                data.Returns(fullYears);
                return data;
            }

        private static
            TestCaseData GenBonusData 
            (int fullYears, int expectedBonus)
            {
                var data = GenData(fullYears);
                data.SetDescription(string.Format(descriptionPatternBonus, fullYears, expectedBonus));
                data.Returns(expectedBonus);
                return data;
            }


        private static TestCaseData GenData (int fullYears)
            {
                var gruntEmployee = new Organization.Grunt("Ivan", "Ivanov", DateTime.Now.AddYears(-fullYears),
                    Organization.BASEPAYRATE);
                var data = new TestCaseData(gruntEmployee);
                data.SetName(string.Format(namePattern, fullYears));
                data.SetCategory(categoryName);
                return data;
            }
        }
    }
}