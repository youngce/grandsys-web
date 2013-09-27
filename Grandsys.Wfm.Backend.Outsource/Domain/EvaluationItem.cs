using System;
using ENode.Domain;
using ENode.Eventing;
using Grandsys.Wfm.Backend.Outsource.Events;

namespace Grandsys.Wfm.Backend.Outsource.Domain
{
    [Serializable]
    public class PieceDescription : IItemDescription
    {
        public PieceDescription()
        {
            Title = string.Empty;
            Unit = string.Empty;
        }
        public string Title { get; set; }
        public string Unit { get; set; }
        public object ToDto()
        {
            return new { Title, Unit };
        }
    }

    [Serializable]
    public class RatioDescription : IItemDescription
    {
        public RatioDescription()
        {
            Denominator = string.Empty;
            Numerator = string.Empty;
            Unit = string.Empty;
        }

        public string Denominator { get; set; }
        public string Numerator { get; set; }
        public string Unit { get; set; }
        public object ToDto()
        {
            return new {Denominator, Numerator, Unit};
        }
    }

    [Serializable]
    public class EvaluationItem : AggregateRoot<Guid>
        , IEventHandler<EvaluationItemCreated>, IEventHandler<EvaluationItemAvailability>,
        IEventHandler<EvaluationItemFormulaChanged>, IEventHandler<EvaluationItemNameChanged>,
        IEventHandler<EvaluationItemDescriptionChanged>
    {
        private IFormula _formula;

        private ParametersInfo _formulaParams;
        private bool _inuse;
        private IItemDescription _itemDescription;
        private string _name;
        private string _statisticalWay;

        public EvaluationItem()
        {
        }

        public EvaluationItem(Guid evaluationItemId, string name, string statisticalWay)
            : base(evaluationItemId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("name can not be null or whiteSpace");

            RaiseEvent(new EvaluationItemCreated(evaluationItemId, name, statisticalWay));
        }

        public string Name { get { return _name; } }

        public string StatisticalWay { get { return _statisticalWay; } }

        //public string Formula { get; private set; }

        public object FormulaParams
        {
            get
            {
                if (_formula == null)
                    return null;
                return _formula.ToValue();
            }
        }

        public ParametersInfo AllFormulaParams { get { return _formulaParams; }}

        public IItemDescription Description { get { return _itemDescription; } }

        void IEventHandler<EvaluationItemAvailability>.Handle(EvaluationItemAvailability evnt)
        {
            _inuse = evnt.Inuse;
        }

        void IEventHandler<EvaluationItemCreated>.Handle(EvaluationItemCreated evnt)
        {
            _inuse = true;
            _name = evnt.Name;
            _statisticalWay = evnt.StatisticalWay;
            _itemDescription = _statisticalWay.Create();
        }

        public void Handle(EvaluationItemDescriptionChanged evnt)
        {
            _itemDescription = evnt.Description;
        }

        void IEventHandler<EvaluationItemFormulaChanged>.Handle(EvaluationItemFormulaChanged evnt)
        {
            _formula = evnt.Formula;
            _formulaParams = _formula.Parameters;
            //Formula = _formula.GetType().Name;
        }

        public void Handle(EvaluationItemNameChanged evnt)
        {
            _name = evnt.NewName;
        }

        public void ChangeName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("name can not be null or whiteSpace");
            RaiseEvent(new EvaluationItemNameChanged { ItemId = Id, NewName = newName });
        }

        public void ChangeGradeSteps(IFormula formula)
        {
            _statisticalWay.IsValid(formula.Parameters);
            RaiseEvent(new EvaluationItemFormulaChanged { Formula = formula });
        }

        public void SetDescription(object values)
        {
            RaiseEvent(new EvaluationItemDescriptionChanged
            {
                ItemId = Id,
                Description = _statisticalWay.Create(values)
            });
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
    }
}