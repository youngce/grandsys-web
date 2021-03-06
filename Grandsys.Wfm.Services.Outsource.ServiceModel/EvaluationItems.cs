﻿using System.Collections.Generic;
using ServiceStack.ServiceHost;

namespace Grandsys.Wfm.Services.Outsource.ServiceModel
{
    public class Link
    {
        public string Name { get; set; }
        public IReturn Request { get; set; }
        public string Method { get; set; }
    }


    [Route("/evaluationItems", "GET")]
    public class EvaluationItems : IReturn<List<ResponseEvaluationItem>>
    {
        //public string StatisticalWay { get; set; }
    }


    [Route("/evaluationItems/{Id}", "GET")]
    public class GetEvaluationItem : IReturn<ResponseEvaluationItem>
    {
        public string Id { get; set; }
        public string Mode { get; set; }
    }

    [Route("/evaluationItems/creationWays", "GET")]
    public class EvaluationItemsCreationWays : IReturn<EvaluationItemsCreationWays>
    {
        public IEnumerable<Link> Links { get; set; }
    }

    [Route("/evaluationItems/{Id}", "PUT PATCH")]
    public class UpdateEvaluationItem : IReturn<ResponseEvaluationItem>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public FormulaInfo Formula { get; set; }
        public DescriptionInfo Description { get; set; }
    }


    public class FormulaInfo
    {
        public string Type { get; set; }

        public double BaseIndicator { get; set; }
        public double BaseScore { get; set; }
        public double Scale { get; set; }

        //linear
        public double IncreaseStepScore { get; set; }
        public double DecreaseStepScore { get; set; }

        //slide
        public double StepScore { get; set; }
        public double StartIndicator { get; set; }
        public double FinalIndicator { get; set; }
    }

    [Route("/evaluationItems/{Id}", "DELETE")]
    public class DeleteEvaluationItem : IReturn<ResponseEvaluationItem>
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    [Route("/evaluationItems/ratio", "POST")]
    public class RatioEvaluationItem : IReturn<ResponseEvaluationItem>
    {
        public string Name { get; set; }
    }

    [Route("/evaluationItems/piece")]
    public class PieceEvaluationItem : IReturn<ResponseEvaluationItem>
    {
        public string Name { get; set; }
    }

 //Response DTO
    public class ResponseEvaluationItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string StatisticalWay { get; set; } //ratio, piece
        public string Status { get; set; }

        public object Description { get; set; }

        public object FormulaParams { get; set; }

        public IEnumerable<object> FormulaOptions { get; set; }

        public IEnumerable<Link> Links { get; set; }
    }

    public class DescriptionInfo
    {
        public string Title { get; set; }
        public string Unit { get; set; }
        public string Denominator { get; set; }
        public string Numerator { get; set; }
    }
}