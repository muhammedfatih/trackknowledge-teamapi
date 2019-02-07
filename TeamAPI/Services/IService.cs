using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace TeamAPI.Services
{
	public interface IService<T> where T : class
	{
		HttpResponseMessage Get();
		HttpResponseMessage Get(int id);
		HttpResponseMessage Insert(T entity);
		HttpResponseMessage Update(T entity);
		HttpResponseMessage Delete(int id);
	}
}