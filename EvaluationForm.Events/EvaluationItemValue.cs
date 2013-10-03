using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grandsys.Wfm.Backend.Outsource.Interface;

namespace Grandsys.Wfm.Backend.Outsource.Events
{
    [Serializable]
    public class EvaluationItemValue
    {
        private readonly IFormula _formula;
        public EvaluationItemValue() {}

        public Guid EvaluationId { get; set; }

        public EvaluationItemValue(IFormula formula)
        {
            if(formula == null)
                throw new ArgumentException();
            _formula = formula;
        }

        public IFormula Formula{ get { return _formula; }}
    }
}
