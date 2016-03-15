using System;
using System.Collections.Generic;
using System.Linq;
using Payroll;

namespace PayrollCalc.Settings
{
    abstract class EmployeeType : IPayrollable
    {
        protected readonly string PositionName;
        protected readonly string FirstName;
        protected readonly string LastName;
        protected readonly DateTime HireDate;
        protected readonly float BaseRate;
        protected readonly float AnnualBonus;
        protected readonly float MaxAnnualBonus;

        protected EmployeeType(string firstName, string lastName, DateTime hireDate, float baseRate, string positionName, float annualBonus, float maxAnnualBonus)
        {
            PositionName = positionName;
            HireDate = hireDate;
            BaseRate = baseRate;
            AnnualBonus = annualBonus;
            MaxAnnualBonus = maxAnnualBonus;
            FirstName = firstName;
            LastName = lastName;
        }

        public int GetFullYears()
        {
            return GetFullYearsFromDate(DateTime.Now);
        }

        public int GetFullYearsFromDate(DateTime payrollDate)
        {
            return payrollDate.Year - HireDate.Year;
        }

        public abstract int GetSubordinateCount();
        public abstract int GetSubordinateCount(int subDepth);


        public double GetAnnualBonus()
        {
            return GetAnnualBonusFromDate(DateTime.Now);
        }

        public double GetAnnualBonusFromDate(DateTime payrollDate)
        {
            float tmpBonus = AnnualBonus * GetFullYearsFromDate(payrollDate);
            return tmpBonus <= MaxAnnualBonus ? tmpBonus : MaxAnnualBonus;
        }

        public double CalcCurrentPay()
        {
           return  CalcPayForDate(DateTime.Now);
        }
        public double CalcPayForDate(DateTime payrollDate)
        {
            return (1 + GetAnnualBonusFromDate(payrollDate)) * BaseRate;
        }
    }

    internal abstract class Managerial : EmployeeType
    {
        protected Managerial(string firstName, string lastName, DateTime hireDate, float baseRate, string positionName,
            float annualBonus, float maxAnnualBonus, float managerBonus, sbyte managerBonusLevels)
            : base(firstName, lastName, hireDate, baseRate, positionName, annualBonus, maxAnnualBonus)
        {
            ManagerBonus = managerBonus;
            ManagerBonusLevels = managerBonusLevels;
            Subordinates = new List<EmployeeType>();
        }

        public List<EmployeeType> Subordinates { get; set; }

        protected readonly float ManagerBonus;
        protected readonly sbyte ManagerBonusLevels;

        public override int GetSubordinateCount()
        {
            return GetSubordinateCount(ManagerBonusLevels);
        }

        public override int GetSubordinateCount(int subDepth)
        {
            if (subDepth == 0)
                return 0;

            subDepth -= 1;

            return Subordinates.Count + Subordinates.Sum(employee => employee.GetSubordinateCount(subDepth));
        }


        public new double CalcPayForDate(DateTime payrollDate)
        {
            return (1 + (GetAnnualBonusFromDate(payrollDate) + GetSubordinateCount()*ManagerBonus))*BaseRate;
        }


        class Manager : Managerial
        {
            public Manager(String firstName, string lastName, DateTime hireDate, float baseRate)
                : base(
                    firstName, lastName, hireDate, baseRate, positionName: "Manager", annualBonus: 5, maxAnnualBonus: 40,
                    managerBonus: (float) 0.5, managerBonusLevels: 1)
            {
            }

        }

        class Sales : Managerial
        {
            public Sales(String firstName, string lastName, DateTime hireDate, float baseRate)
                : base(
                    firstName, lastName, hireDate, baseRate, positionName: "Sales", annualBonus: 1, maxAnnualBonus: 35,
                    managerBonus: (float) 0.3, managerBonusLevels: -1)
            {
            }

        }

        class Grunt : EmployeeType
        {


            public Grunt(string firstName, string lastName, DateTime hireDate, float baseRate)
                : base(
                    firstName, lastName, hireDate, baseRate, positionName: "Employee", annualBonus: 3,
                    maxAnnualBonus: 30)
            {
            }

            public override int GetSubordinateCount()
            {
                return GetSubordinateCount(0);
            }

            public override int GetSubordinateCount(int subDepth)
            {
                return 0;
            }


        }
    }
}
