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
    }
}