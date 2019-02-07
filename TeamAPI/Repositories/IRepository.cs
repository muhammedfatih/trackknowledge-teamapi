using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamAPI.Repositories
{
	public interface IRepository<T> where T : class
	{
		T Get(object id);
		T Insert(T entity);
		List<T> List();
		void Update(T entity);
		void Delete(T entity);
	}
}