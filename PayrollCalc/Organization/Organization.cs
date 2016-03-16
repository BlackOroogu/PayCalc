using System;
using System.Collections.Generic;
using PayrollCalc.Employees;

namespace PayrollCalc.Organization
{
    public class Organization
    {
        public static double BASEPAYRATE = 10000.0d;
        public double BasePayRate { get; }
        public string OrgName { get; }

        public HashSet<GenericEmployeeType> Employees { get; set; } = new HashSet<GenericEmployeeType>();

        public Organization(string orgName)
        {
            OrgName = orgName;
            BasePayRate = BASEPAYRATE;
        }

        public Organization(int basePayRate, string orgName)
        {
            BasePayRate = basePayRate;
            OrgName = orgName;
        }

        public Organization(HashSet<GenericEmployeeType> employees, double basePayRate, string orgName)
        {
            Employees = employees;
            BasePayRate = basePayRate;
            OrgName = orgName;
        }

        public class Manager : Managerial
        {
            public Manager(string firstName, string lastName, DateTime hireDate, double baseRate)
                : base(
                    firstName, lastName, hireDate, baseRate, "Manager", 5, 40, (float)0.5, 1)
            {
            }
        }

        public class Sales : Managerial
        {
            public Sales(string firstName, string lastName, DateTime hireDate, double baseRate)
                : base(
                    firstName, lastName, hireDate, baseRate, "Sales", 1, 35, (float)0.3, -1)
            {
            }
        }

        public class Grunt : GruntEmployee
        {
            public Grunt(string firstName, string lastName, DateTime hireDate, double baseRate)
                : base(
                    firstName, lastName, hireDate, baseRate, "Employee", 3, 30)
            {
            }
        }


    }
}
