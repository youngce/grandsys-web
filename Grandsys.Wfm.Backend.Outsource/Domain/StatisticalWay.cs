using System;
using System.Data;
using Grandsys.Wfm.Backend.Outsource.Events;

namespace Grandsys.Wfm.Backend.Outsource.Domain
{
    public static class StatisticalWay
    {
        private static void IsNotIntergerThrow(double number)
        {
            if (number != Math.Truncate(number))
                throw new InvalidConstraintException();
        }

        public static void IsValid(this string way, ParametersInfo values)
        {
            if (way == "ratio")
            {
            }
            if (way == "piece")
            {
                IsNotIntergerThrow(values.BaseIndicator);
                IsNotIntergerThrow(values.Scale);
            }
        }

        public static IItemDescription Create(this string way, dynamic values)
        {
            if (way == "ratio")
            {
                return new RatioDescription
                {
                    Denominator = values.Denominator,
                    Numerator = values.Numerator,
                    Unit = values.Unit
                };
            }
            if (way == "piece")
            {
                return new PieceDescription { Title = values.Title, Unit = values.Unit };
            }
            throw new NotImplementedException();
        }

        public static IItemDescription Create(this string way)
        {
            if (way == "ratio")
            {
                return new RatioDescription();
            }
            if (way == "piece")
            {
                return new PieceDescription();
            }
            throw new NotImplementedException();
        }
    }
}