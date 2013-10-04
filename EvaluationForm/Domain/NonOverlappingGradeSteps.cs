using System;
using System.Collections;
using System.Collections.Generic;
using Grandsys.Wfm.Backend.Outsource.Interface;

namespace Grandsys.Wfm.Backend.Outsource.Domain
{
    [Serializable]
    public class NonOverlappingGradeSteps : IEnumerable<GradeStep>
    {
        private readonly IEnumerable<GradeStep> _gradeSteps;
        private readonly bool _isVaild;
        private readonly Exception _status;

        public NonOverlappingGradeSteps(IEnumerable<GradeStep> gradeSteps)
        {
            _gradeSteps = gradeSteps;

            if (_isVaild = IsOverlapping())
            {
                _status = new Exception("GradeSteps is overlapping!!");
            }
        }

        public IEnumerable<GradeStep> GradeSteps
        {
            get { return _gradeSteps; }
        }

        public Exception Status
        {
            get { return _status; }
        }

        public bool IsVaild
        {
            get { return _isVaild; }
        }


        public IEnumerator<GradeStep> GetEnumerator()
        {
            return _gradeSteps.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private bool IsOverlapping()
        {
            var linkedList = new LinkedList<GradeStep>(_gradeSteps);

            LinkedListNode<GradeStep> current = linkedList.First;
            while (current != null)
            {
                if (current.Value.Range.Max > current.Value.Range.Min)
                    return true;
                current = current.Next;
            }
            return false;
        }
    }
}