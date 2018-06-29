using System;
using System.Collections;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using IMapper = Framework.Constract.Interfaces.IMapper;

namespace Framework.Common.Custom
{
    public class CustomMapper : IMapper
    {
        public TResult MapTo<TResult>(IEnumerable source)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }
            return (TResult)Mapper.Map(source, source.GetType(), typeof(TResult));
        }

        public TResult MapTo<TResult>(object source)
        {
			if (source == null)
			{
				throw new ArgumentNullException();
			}

			return (TResult)Mapper.Map(source, source.GetType(), typeof(TResult));
        }

        public TResult MapToInstance<TResult>(object source, TResult dest)
        {
			if (source == null)
			{
				throw new ArgumentNullException();
			}

			return (TResult)Mapper.Map(source, dest, source.GetType(), typeof(TResult));
        }

        public IQueryable<TDestination> MapToIQueryable<TSource, TDestination>(IQueryable<TSource> query)
        {
            return query.ProjectTo<TDestination>();
        }
    }
}
