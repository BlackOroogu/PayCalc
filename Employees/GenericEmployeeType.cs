using System;
using System.Collections.Generic;
using System.Linq;
using Payroll;

namespace PayrollCalc.Settings
{
    public abstract class GenericEmployeeType : IPayrollable
    {
        protected readonly string PositionName;
        protected readonly string FirstName;
        protected readonly string LastName;
        protected readonly DateTime HireDate;
        protected readonly float BaseRate;
        protected readonly float AnnualBonus;
        protected readonly float MaxAnnualBonus;
        protected Managerial Manager;
 

        protected GenericEmployeeType(string firstName, string lastName, DateTime hireDate, float baseRate, string positionName,
            float annualBonus, float maxAnnualBonus)
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

        public double GetAnnualBonus()
        {
            return GetAnnualBonusFromDate(DateTime.Now);
        }

        public double GetAnnualBonusFromDate(DateTime payrollDate)
        {
            var tmpBonus = AnnualBonus*GetFullYearsFromDate(payrollDate);
            return tmpBonus <= MaxAnnualBonus ? tmpBonus : MaxAnnualBonus;
        }

        public double CalcCurrentPay()
        {
            return CalcPayForDate(DateTime.Now);
        }

        public double CalcPayForDate(DateTime payrollDate)
        {
            return (1 + GetAnnualBonusFromDate(payrollDate))*BaseRate;
        }

        public void AddManager(Managerial manager)
        {
            this.Manager = manager;
            manager.Subordinates.Add(this);
        }

        public void UpdateManager(Managerial manager)
        {
            Manager.Subordinates.Remove(this);
            AddManager(manager);
        }
    }

    public abstract class Managerial : GenericEmployeeType, IManagerial
    {
        protected Managerial(string firstName, string lastName, DateTime hireDate, float baseRate, string positionName,
            float annualBonus, float maxAnnualBonus, float managerBonus, sbyte managerBonusLevels)
            : base(firstName, lastName, hireDate, baseRate, positionName, annualBonus, maxAnnualBonus)
        {
            ManagerBonus = managerBonus;
            ManagerBonusLevels = managerBonusLevels;
            Subordinates = new HashSet<GenericEmployeeType>();
        }

        public HashSet<GenericEmployeeType> Subordinates { get; set; }

        protected readonly float ManagerBonus;
        protected readonly sbyte ManagerBonusLevels;

        public int GetSubordinateCount()
        {
            return GetSubordinateCount(ManagerBonusLevels);
        }

        public int GetSubordinateCount(int subDepth)
        {
            if (subDepth-- == 0)
                return 0;

            return  Subordinates.Count + Subordinates.OfType<Managerial>().Sum(empl => empl.GetSubordinateCount(subDepth));
        }

        public new double CalcPayForDate(DateTime payrollDate)
        {
            return (1 + (GetAnnualBonusFromDate(payrollDate) + GetSubordinateCount()*ManagerBonus))*BaseRate;
        }



    }
}
