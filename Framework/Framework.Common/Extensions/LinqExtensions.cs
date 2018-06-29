using System;
using System.Linq.Expressions;

namespace Framework.Common.Extensions
{
	public static class LinqExtension
	{
		public static Expression Replace(this Expression expression,
			Expression searchEx, Expression replaceEx)
		{
			return new ReplaceVisitor(searchEx, replaceEx).Visit(expression);
		}

		private class ReplaceVisitor : ExpressionVisitor
		{
			private readonly Expression _from, _to;
			public ReplaceVisitor(Expression from, Expression to)
			{
				this._from = from;
				this._to = to;
			}
			public override Expression Visit(Expression node)
			{
				return node == _from ? _to : base.Visit(node);
			}
		}

		public static Expression<Func<T, TResult>> And<T, TResult>(this Expression<Func<T, TResult>> expFirst, Expression<Func<T, TResult>> expSecond)
		{
			var body = expSecond.Body.Replace(expSecond.Parameters[0], expFirst.Parameters[0]);
			return Expression.Lambda<Func<T, TResult>>(Expression.AndAlso(expFirst.Body, body), expFirst.Parameters);
		}
	}
}
