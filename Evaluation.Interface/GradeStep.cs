using System;

namespace Grandsys.Wfm.Backend.Outsource.Interface
{
    [Serializable]
    public class GradeStep
    {
        public Range<double> Range{get { return _range; }} 
        private readonly Range<double> _range;
        private double _scale;

        public GradeStep(Range<double> range, double scale)
        {
            _range = range;
            Scale = scale;
        }

        public double Score { get; set; }

        // public Range<double> Range { get { return _range; } }

        public double Scale
        {
            get
            {
                return _scale;
            }
            private set
            {
                _scale = value;
                if (_scale <= 0)
                    throw new LessThanOrEqualToZeroException();
            }
        }
    }
}