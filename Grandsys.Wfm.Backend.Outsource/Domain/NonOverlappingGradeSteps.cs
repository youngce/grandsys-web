using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Grandsys.Wfm.Backend.Outsource.Domain
{
    [Serializable]
    public class NonOverlappingGradeSteps : IEnumerable<GradeStep>
    {
        private IList<GradeStep> _gradeSteps;

        public NonOverlappingGradeSteps(IEnumerable<GradeStep> gradeSteps)
        {
            _gradeSteps = gradeSteps.ToArray();
        }

        public IEnumerator<GradeStep> GetEnumerator()
        {
            return _gradeSteps.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}