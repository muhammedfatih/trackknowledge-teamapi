using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace TeamAPI.Services
{
	public interface IService<T> where T : class
	{
		List<T> Get();
		T Get(int id);
		T Insert(T entity);
		bool Update(T entity);
		bool Delete(int id);
	}
}