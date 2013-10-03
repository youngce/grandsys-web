using System;

namespace Grandsys.Wfm.Backend.Outsource.Interface
{
    [Serializable]
    public class Range<T> where T : IComparable
    {
        private readonly T _min;
        private readonly T _max;

        public T Max
        {
            get
            {
                return _max;
            }
        }

        public T Min
        {
            get
            {
                return _min;
            }
        }

        public Range(T min, T max)
        {
            if (max.CompareTo(min) <= 0)
                throw new ArgumentException();
            _min = min;
            _max = max;
        }
    }
}