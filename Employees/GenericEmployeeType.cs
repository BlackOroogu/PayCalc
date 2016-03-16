using System;
using System.Collections.Generic;
using System.Linq;

namespace PayrollCalc.Employees
{
    public abstract class GenericEmployeeType : IPayrollable
    {
        protected readonly string PositionName;
        public readonly string FirstName;
        public readonly string LastName;
        protected readonly DateTime HireDate;
        protected readonly double BaseRate;
        protected readonly float AnnualBonus;
        protected readonly float MaxAnnualBonus;
        protected Managerial Manager;
        public string FullName => string.Format("{0} {1}",FirstName, LastName);


        protected GenericEmployeeType(string firstName, string lastName, DateTime hireDate, double baseRate, string positionName,
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

        public abstract double CalcPayForDate(DateTime payrollDate);


        public void SetManager(Managerial manager)
        {
            Manager?.Subordinates.Remove(this);

            Manager = manager;
            manager.Subordinates.Add(this);
        }

    }

    public abstract class Managerial : GenericEmployeeType, IManagerial
    {
        protected Managerial(string firstName, string lastName, DateTime hireDate, double baseRate, string positionName,
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

        public double GetSubordinatePay()
        {
            return GetSubordinatePay(ManagerBonusLevels);
        }

        public double GetSubordinatePay(int subDepth)
        {
            if (subDepth-- == 0)
                return 0;

            double result = Subordinates.Sum(subordinate => subordinate.CalcCurrentPay());
            return result;
        }


        public override double CalcPayForDate(DateTime payrollDate)
        {
            double result = (100 + GetAnnualBonusFromDate(payrollDate))/100 * BaseRate + GetSubordinatePay() * ManagerBonus / 100;
            return result;
        }

        public void AddEmployee(GenericEmployeeType employee)
        {
            employee.SetManager(this);
        }


    }

    public class GruntEmployee : GenericEmployeeType
    {
        public GruntEmployee(string firstName, string lastName, DateTime hireDate, double baseRate, string positionName,
            float annualBonus, float maxAnnualBonus)
            : base(firstName, lastName, hireDate, baseRate, positionName, annualBonus, maxAnnualBonus)
        {
        }

        public override double CalcPayForDate(DateTime payrollDate)
        {
            return ((100 + GetAnnualBonusFromDate(payrollDate)) / 100) * BaseRate;
        }
    }
}
