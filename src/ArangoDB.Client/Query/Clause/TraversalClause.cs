﻿using Remotion.Linq;
using Remotion.Linq.Clauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArangoDB.Client.Query.Clause
{
    public class TraversalClause : IBodyClause, ITraversalClause
    {
        public ConstantExpression TraversalContext { get; set; }

        public string Identifier { get; set; }

        public ConstantExpression Min { get; set; }

        public ConstantExpression Max { get; set; }

        public ConstantExpression Direction { get; set; }

        public Expression StartVertex { get; set; }

        public TraversalClause(ConstantExpression traversalContext, string identifier)
        {
            LinqUtility.CheckNotNull("traversalContext", traversalContext);
            LinqUtility.CheckNotNull("identifier", identifier);

            TraversalContext = traversalContext;
            Identifier = identifier;
        }

        public virtual void Accept(IQueryModelVisitor visitor, QueryModel queryModel, int index)
        {
            LinqUtility.CheckNotNull("visitor", visitor);
            LinqUtility.CheckNotNull("queryModel", queryModel);

            var arangoVisotor = visitor as ArangoModelVisitor;

            if (arangoVisotor == null)
                throw new Exception("QueryModelVisitor should be type of ArangoModelVisitor");

            arangoVisotor.VisitTraversalClause(this, queryModel, index);
        }

        public virtual TraversalClause Clone(CloneContext cloneContext)
        {
            LinqUtility.CheckNotNull("cloneContext", cloneContext);

            var clone = new TraversalClause(TraversalContext, Identifier);
            return clone;
        }

        IBodyClause IBodyClause.Clone(CloneContext cloneContext)
        {
            return Clone(cloneContext);
        }

        public void TransformExpressions(Func<Expression, Expression> transformation)
        {
            LinqUtility.CheckNotNull("transformation", transformation);
            TraversalContext = transformation(TraversalContext) as ConstantExpression;
        }
    }
}