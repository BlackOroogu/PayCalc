using System;
using System.Collections.Generic;
using System.Linq;

namespace PayrollCalc.Employees
{
    public class ForwardYearException : Exception
    {
        public ForwardYearException() : base("The Hire year is in the future.")
        {
        }
    }

    public abstract class GenericEmployeeType : IPayrollable
    {
        protected readonly float AnnualBonus;
        protected readonly double BaseRate;
        public readonly string FirstName;
        protected readonly DateTime HireDate;
        public readonly string LastName;
        protected readonly float MaxAnnualBonus;
        protected readonly string PositionName;
        protected Managerial Manager;


        protected GenericEmployeeType(string firstName, string lastName, DateTime hireDate, double baseRate,
            string positionName,
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

        public string FullName => $"{FirstName} {LastName}";

        public int GetFullYears()
        {
            return GetFullYearsFromDate(DateTime.Now);
        }

        public double GetAnnualBonus()
        {
            return GetAnnualBonusFromDate(DateTime.Now);
        }


        private int GetFullYearsFromDate(DateTime payrollDate)
        {
            return payrollDate.Year - HireDate.Year;
        }

        protected double GetAnnualBonusFromDate(DateTime payrollDate)
        {
            var result = AnnualBonus*GetFullYearsFromDate(payrollDate);
            if (result < 0)
                throw new ForwardYearException();
            return result <= MaxAnnualBonus ? result : MaxAnnualBonus;
        }

        public void SetManager(Managerial manager)
        {
            Manager?.Subordinates.Remove(this);

            Manager = manager;
            manager.Subordinates.Add(this);
        }

        public abstract double CalcCurrentPay();
    }

    public abstract class Managerial : GenericEmployeeType, IManagerial
    {
        protected readonly float ManagerBonus;
        protected readonly sbyte ManagerBonusLevels;

        protected Managerial(string firstName, string lastName, DateTime hireDate, double baseRate, string positionName,
            float annualBonus, float maxAnnualBonus, float managerBonus, sbyte managerBonusLevels)
            : base(firstName, lastName, hireDate, baseRate, positionName, annualBonus, maxAnnualBonus)
        {
            ManagerBonus = managerBonus;
            ManagerBonusLevels = managerBonusLevels;
            Subordinates = new HashSet<GenericEmployeeType>();
        }

        public HashSet<GenericEmployeeType> Subordinates { get; set; }

        public int GetSubordinateCount()
        {
            return GetSubordinateCount(ManagerBonusLevels);
        }

        private int GetSubordinateCount(int intDepth)
        {
            if (intDepth == 0)
                return 0;

            int result = Subordinates.Count;

            result += Subordinates.OfType<Managerial>().Sum(subordinate => (subordinate).GetSubordinateCount(intDepth--));
            return result;
        }

        public override double CalcCurrentPay()
        {
            return CalcPayForDate(DateTime.Now);
        }

        private double CalcPayForDate(DateTime payrollDate)
        {
            return BaseRate*(1 + GetAnnualBonusFromDate(payrollDate)/100) + GetSubordinatePay()*(ManagerBonus/100);
        }

        public double GetSubordinatePay()
        {
            return GetSubordinatePay(ManagerBonusLevels);
        }

        public double GetSubordinatePay(int intDepth)
        {
            double result = Subordinates.Sum(subordinate => subordinate.CalcCurrentPay());
            return result;
        }
    }

    public class GruntEmployee : GenericEmployeeType
        {
            public GruntEmployee(string firstName, string lastName, DateTime hireDate, double baseRate,
                string positionName,
                float annualBonus, float maxAnnualBonus)
                : base(firstName, lastName, hireDate, baseRate, positionName, annualBonus, maxAnnualBonus)
            {
            }

            private double CalcPayForDate(DateTime payrollDate)
            {
                double  result=  (100 + GetAnnualBonusFromDate(payrollDate))/100*BaseRate;
                return result;
            }

            public override double CalcCurrentPay()
            {
                return CalcPayForDate(DateTime.Now);
            }
        }
    }
