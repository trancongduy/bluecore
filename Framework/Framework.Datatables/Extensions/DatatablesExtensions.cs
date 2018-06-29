using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Framework.Common.Custom;
using Framework.Common.Extensions;
using Framework.Constract.Interfaces;
using Framework.Constract.SeedWork;

namespace Framework.Datatables.Extensions
{
	public static class DatatablesExtensions
	{
		public static Task<DtResult<TResult>> GetResultDatatablesAsync<TResult, TEntity>(IQueryable<TEntity> query,
			string search, int? draw, string sortOrder, int? start, int? length, List<string> columnFilters) where TResult : BaseDto where TEntity : BaseEntity
		{
			try
			{
                //var mapper = DependencyResolver.Current.GetService<IMapper>();
                IMapper mapper = new CustomMapper();

				var filter = FilterResult(search, query, columnFilters);

                //var count = filter.CountAsync().Result;
                var count = filter.Count();

				if (sortOrder.StartsWith("String", StringComparison.Ordinal))
				{
					sortOrder = sortOrder.Replace("String", "");
				}
                var type = typeof(TEntity).GetTypeInfo().GetProperties().FirstOrDefault(a => sortOrder.StartsWith(a.Name, StringComparison.Ordinal));

				//var resultFilter = type != null ? filter.SortBy(sortOrder)
				//.Skip(start ?? 0)
				//.Take(length ?? 0).ToListAsync().Result : Activator.CreateInstance(typeof(List<>).MakeGenericType(typeof(TResult)));

                var resultFilter = type != null ? filter.OrderBy(a => sortOrder)
			        .Skip(start ?? 0)
                    .Take(length ?? 0) : Activator.CreateInstance(typeof(List<>).MakeGenericType(typeof(TResult)));


				var result = new DtResult<TResult>
				{
					draw = draw,
					data = mapper.MapTo<List<TResult>>(resultFilter),
					recordsFiltered = count,
					recordsTotal = count
				};

				return Task.FromResult(result);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		public static Task<DtResult<TResult>> GetResultQueryDatatablesAsync<TResult, TEntity>(IQueryable<TEntity> query,
			string search, int? draw, string sortOrder, int? start, int? length, List<string> columnFilters) where TResult : BaseDto
		{
			try
			{
                //var mapper = DependencyResolver.Current.GetService<IMapper>();
                IMapper mapper = new CustomMapper();

				var filter = FilterResult(search, query, columnFilters);

				//var count = filter.CountAsync().Result;
				var count = filter.Count();

				if (sortOrder.StartsWith("String", StringComparison.Ordinal))
				{
					sortOrder = sortOrder.Replace("String", "");
				}
				var type = typeof(TEntity).GetTypeInfo().GetProperties().FirstOrDefault(a => sortOrder.StartsWith(a.Name, StringComparison.Ordinal));
                var resultFilter = type != null ? mapper.MapToIQueryable<TEntity, TResult>(filter)
                    .OrderBy(a => sortOrder)
					.Skip(start ?? 0)
					.Take(length ?? 0) : Activator.CreateInstance(typeof(List<>).MakeGenericType(typeof(TResult)));


				var result = new DtResult<TResult>
				{
					draw = draw,
					data = mapper.MapTo<List<TResult>>(resultFilter),
					recordsFiltered = count,
					recordsTotal = count
				};

				return Task.FromResult(result);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		private static IQueryable<TEntity> FilterResult<TEntity>(string search, IQueryable<TEntity> dtResult,
			List<string> columnFilters)
		{
            var results = dtResult;

			if (!string.IsNullOrEmpty(search))
			{
				var id = search.ParseOrDefault<int>();
				var param = Expression.Parameter(typeof(TEntity), "a");
				var type = typeof(TEntity).GetTypeInfo().GetProperties().FirstOrDefault(a => a.Name == "Name");
				if (type == null)
				{
					return results;
				}
                var expression = Expression.Call(Expression.Property(param, "Name"),
                                                 typeof(string).GetTypeInfo().GetMethod("Contains", new[] { typeof(string) }),
                                                 Expression.Constant(search));
                //var expression2 = Expression.Call(Expression.Property(param, "Description"),
                //    typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                //    Expression.Constant(search));
                //var lamda =
                //    Expression.Lambda<Func<TEntity, bool>>(Expression.Or(expression,expression2)
                //        , param);
                var lamda =
                    Expression.Lambda<Func<TEntity, bool>>(expression
                                                           , param);

				results =
                    results.Where(lamda);
			}

			return results;
		}

		private static MemberExpression NestedExpressionProperty(Expression expression, string propertyName)
		{
			string[] parts = propertyName.Split('.');
			int partsL = parts.Length;

			return partsL > 1
				?
				Expression.Property(
					NestedExpressionProperty(
						expression,
						parts.Take(partsL - 1)
							.Aggregate((a, i) => a + "." + i)
					),
					parts[partsL - 1])
				:
				Expression.Property(expression, propertyName);
		}
	}
}
