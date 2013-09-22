using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Grandsys.Wfm.Backend.Outsource.Domain;

namespace Grandsys.Wfm.Backend.Outsource.Tests
{
    [TestFixture]
    public class GradeStepsTest
    {
        [Test]
        public void CalculateTest()
        {
            var baseScore = 100;
            var baseIndicator = 3;
            var userIndicator = 3;
            var scale = 1;
            var gradeSteps = new[]
                {
                    new GradeStep(new Range<double>(double.MinValue, 3),1){ Score = 3},
                    new GradeStep(new Range<double>(3,14),scale){Score = -5},
                    new GradeStep(new Range<double>(14,double.MaxValue),scale){Score = -10}
                };

           /* double myScore = baseScore;

            double cumulateValue = 0;

            foreach (var gradeStep in gradeSteps)
            {
                var range = gradeStep.Range;

                if (baseIndicator < userIndicator)
                {
                    if (range.Min < baseIndicator)
                        continue;
                    if (range.Max <= userIndicator)
                        cumulateValue += (range.Max - range.Min) / scale * gradeStep.Score;
                    if (range.Min <= userIndicator && userIndicator < range.Max)
                    {
                        cumulateValue += Math.Abs(userIndicator - range.Min) / scale * gradeStep.Score;
                        break;
                    }
                }
                else
                {
                    if (range.Max > baseIndicator)
                        continue;
                    if (userIndicator < range.Min)
                        cumulateValue += (range.Max - range.Min) / scale * gradeStep.Score;
                    if (range.Min <= userIndicator && userIndicator < range.Max)
                    {
                        cumulateValue += Math.Abs(range.Max - userIndicator) / scale * gradeStep.Score;
                        break;
                    }

                }
            }
            myScore += cumulateValue;
            Assert.AreEqual(100, myScore);*/
        }
    }
}
