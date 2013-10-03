using System;
using System.Data;
using System.Linq;
using ENode.Domain;
using ENode.Eventing;
using Grandsys.Wfm.Backend.Outsource.Events;

namespace Grandsys.Wfm.Backend.Outsource.Domain
{
    [Serializable]
    public class FormItem
    {
        private NonOverlappingGradeSteps _gradeSteps;

        public FormItem(){}

        public FormItem(EvaluationItemValue value)
        {
           _gradeSteps = new NonOverlappingGradeSteps(value.Formula.GenGradeSteps());
        }
    }

    public class DuplicateEvaluationItemException : Exception { }

    [Serializable]
    public class EvaluationForm : AggregateRoot<Guid>, IEventHandler<EvaluationFormCreated>
    {
        private FormItem[] _items;

        public EvaluationForm() { }

        public EvaluationForm(Guid id, EvaluationItemValue[] items)
            : base(id)
        {
            if (!items.Any())
                throw new ArgumentException("ImmutableList");

            if (items.Select(o => o.EvaluationId).Distinct().Count() != items.Count())
                throw new DuplicateEvaluationItemException();

            RaiseEvent(new EvaluationFormCreated { EvaluationItems = items });
        }

        void IEventHandler<EvaluationFormCreated>.Handle(EvaluationFormCreated evnt)
        {
            _items = evnt.EvaluationItems.Select(o => new FormItem(o)).ToArray();
        }
    }
}
