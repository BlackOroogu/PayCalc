using System;
using System.Collections.Generic;
using Payroll;

namespace PayrollCalc.Settings
{
    public enum ManagerBonusLevels
    { Immediate, All}

    abstract class EmployeeType : IPayrollable
    {
        protected readonly string PositionName;
        protected readonly DateTime HireDate;
        protected readonly float BaseRate;
        protected readonly float AnnualBonus;
        protected readonly float MaxAnnualBonus;

        protected EmployeeType(string positionName, DateTime hireDate, float baseRate, float annualBonus, float maxAnnualBonus)
        {
            PositionName = positionName;
            HireDate = hireDate;
            BaseRate = baseRate;
            AnnualBonus = annualBonus;
            MaxAnnualBonus = maxAnnualBonus;
        }

        public int GetFullYears()
        {
            return GetFullYearsFromDate(DateTime.Now);
        }

        public int GetFullYearsFromDate(DateTime payrollDate)
        {
            return payrollDate.Year - HireDate.Year;
        }

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
        public abstract double CalcPayForDate(DateTime payrollDate);
    }

    internal abstract class Managerial : EmployeeType
    {
        protected Managerial(string positionName, DateTime hireDate, float baseRate, float annualBonus, float maxAnnualBonus, float managerBonus, ManagerBonusLevels managerBonusLevels) : base(positionName, hireDate, baseRate, annualBonus, maxAnnualBonus)
        {
            ManagerBonus = managerBonus;
            ManagerBonusLevels = managerBonusLevels;
            Subordinates = new List<EmployeeType>();
        }

        public Managerial Manager { get; set; }
        public List<EmployeeType> Subordinates { get; set; }

        protected readonly float ManagerBonus;
        protected readonly ManagerBonusLevels ManagerBonusLevels;

        public abstract int GetSubordinateCount();

        public override double CalcPayForDate(DateTime payrollDate)
        {
            return (1 +(GetAnnualBonusFromDate(payrollDate) + GetSubordinateCount() * ManagerBonus)) * BaseRate;
        }

    }

    class Manager : Managerial
    {
        public Manager(string positionName, DateTime hireDate, float baseRate) : base(positionName, hireDate, baseRate, annualBonus: 5, maxAnnualBonus: 40, managerBonus: (float) 0.5, managerBonusLevels: ManagerBonusLevels.Immediate)
        {
        }

        public override int GetSubordinateCount()
        {
            return Subordinates.Count;
        }
    }

    class Sales : Managerial
    {
        public Sales(string positionName, DateTime hireDate, float baseRate) : base(positionName, hireDate, baseRate, annualBonus:1, maxAnnualBonus:35, managerBonus : (float)0.3, managerBonusLevels: ManagerBonusLevels.All)
        {
        }

        public override int GetSubordinateCount()
        {
            throw new NotImplementedException();
        }
    }

    class Grunt : EmployeeType {


        public Managerial Manager { get; set; }

        public Grunt(string positionName, DateTime hireDate, float baseRate)
            : base(positionName, hireDate, baseRate, annualBonus: 3, maxAnnualBonus: 30)
        {
        }

        public Grunt(string positionName, DateTime hireDate, float baseRate,Managerial manager)
    : base(positionName, hireDate, baseRate, annualBonus: 3, maxAnnualBonus: 30)
        {
            Manager = manager;
        }


        public override double CalcPayForDate(DateTime payrollDate)
        {
            return (1 + GetAnnualBonusFromDate(payrollDate)) * BaseRate;
        }
    }
}
