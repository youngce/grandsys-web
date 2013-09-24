using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ENode.Domain;
using ENode.Eventing;
using Grandsys.Wfm.Backend.Outsource.Events;

namespace Grandsys.Wfm.Backend.Outsource.Domain
{
    [Serializable]
    public class EvaluationItem : AggregateRoot<Guid>
        , IEventHandler<EvaluationItemCreated>, IEventHandler<EvaluationItemAvailability>, IEventHandler<EvaluationItemFormulaChanged>
    {
        private IFormula _formula;
        private string _statisticalWay;
        private string _name;
        private bool _inuse;
        
        private ParametersInfo _formulaParams;

        public string Name { get { return _name; } }
        public string StatisticalWay { get { return _statisticalWay; } }
        public string Formula { get; private set; }
        public ParametersInfo FormulaParams { get { return _formulaParams; } }

        public EvaluationItem() { }

        public EvaluationItem(Guid evaluationItemId, string name, string statisticalWay)
            : base(evaluationItemId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("name can not be null or whiteSpace");

            RaiseEvent(new EvaluationItemCreated(evaluationItemId, name, statisticalWay));
        }
        
        public void ChangeGradeSteps(ParametersInfo paramsInfo, IFormula formula)
        {
            _statisticalWay.IsValid(paramsInfo);

            RaiseEvent(new EvaluationItemFormulaChanged() { Formula = formula ,Parameters = paramsInfo });
        }

        public void Disable()
        {
            if (!_inuse)
                throw new Exception(string.Format("The item '{0}' is already disabled.", _name));

            RaiseEvent(new EvaluationItemAvailability(Id, false));
        }

        public void Enable()
        {
            if (_inuse)
                throw new Exception(string.Format("The item '{0}' is already enabled.", _name));

            RaiseEvent(new EvaluationItemAvailability(Id, _name, _statisticalWay));
        }

        void IEventHandler<EvaluationItemCreated>.Handle(EvaluationItemCreated evnt)
        {
            _inuse = true;
            _name = evnt.Name;
            _statisticalWay = evnt.StatisticalWay;
        }

        void IEventHandler<EvaluationItemAvailability>.Handle(EvaluationItemAvailability evnt)
        {
            _inuse = evnt.Inuse;
        }

        void IEventHandler<EvaluationItemFormulaChanged>.Handle(EvaluationItemFormulaChanged evnt)
        {
            _formula = evnt.Formula;
            _formulaParams = evnt.Parameters;
            Formula = _formula.GetType().Name;
        }
    }
}
