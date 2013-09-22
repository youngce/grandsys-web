﻿using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace Grandsys.Wfm.Services.Outsource.ServiceModel
{
    public class Link
    {
        public string Name { get; set; }
        public IReturn Request { get; set; }
        public string Method { get; set; }
    }

    

    [Route("/evaluationItems", "GET")]
    public class EvaluationItems : IReturn<List<EvaluationItem>>
    {
        //public string StatisticalWay { get; set; }
    }

   

    [Route("/evaluationItems/{Id}", "GET")]
    public class GetEvaluationItem : IReturn<EvaluationItem>
    {
        public string Id { get; set; }
        public string Mode { get; set; }
    }

    [Route("/evaluationItems/creationWays", "GET")]
    public class EvaluationItemsCreationWays : IReturn<EvaluationItemsCreationWays>
    {
        public IEnumerable<Link> Links { get; set; }
    }
    
    [Route("/evaluationItems/{Id}", "PUT")]
    public class UpdateEvaluationItem : IReturn
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
    }

    [Route("/evaluationItems/{Id}", "DELETE")]
    public class DeleteEvaluationItem : IReturn<EvaluationItem>
    {
        public string Id { get; set; }
    }

    [Route("/evaluationItems/ratio", "POST")]
    public class RatioEvaluationItem : IReturn<EvaluationItem>
    {
        public string Name { get; set; }
    }

    [Route("/evaluationItems/piece")]
    public class PieceEvaluationItem : IReturn<EvaluationItem>
    {
        public string Name { get; set; }
    }

    [Route("/formula/{EvaluationItemId}/Linear", "POST GET")]
    public class LinearFormula : IReturn<LinearFormula>
    {
        public string EvaluationItemId { get; set; }
        public double BaseIndicator { get; set; }
        public double Scal { get; set; }
    }

    [Route("/formula/{EvaluationItemId}/Slide", "POST GET")]
    public class SlideFormula : IReturn<SlideFormula>
    {
        public string EvaluationItemId { get; set; }
    }

    //Response DTO
    public class EvaluationItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string StatisticalWay { get; set; } //ratio, piece
        public string Status { get; set; }

        public IEnumerable<Link> SetFormulaOptions { get; set; }

        public IEnumerable<Link> Links { get; set; }
    }

}