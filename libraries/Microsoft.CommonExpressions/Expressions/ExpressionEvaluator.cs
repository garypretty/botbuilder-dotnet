﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Expressions
{
    public delegate void ValidateExpressionDelegate(Expression expression);
    public delegate (object value, string error) EvaluateExpressionDelegate(Expression expression, object state);

    public class ExpressionEvaluator : IExpressionEvaluator
    {
        public ExpressionEvaluator(EvaluateExpressionDelegate evaluator,
            ExpressionReturnType returnType = ExpressionReturnType.Object,
            ValidateExpressionDelegate validator = null)
        {
            _evaluator = evaluator;
            ReturnType = returnType;
            _validator = validator;
        }

        private ValidateExpressionDelegate _validator;
        private EvaluateExpressionDelegate _evaluator;

        public (object value, string error) TryEvaluate(Expression expression, object state)
            => _evaluator(expression, state);

        public void ValidateExpression(Expression expression)
            => _validator(expression);

        public ExpressionReturnType ReturnType { get; }
    }
}
