using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TeamAPI.Models;
using DBContext;
using System.Net;
using System.Net.Http;
using FluentValidation.Results;
using AutoMapper;
using TeamAPI.Helpers;
using TeamAPI.Models.Response;
using System.Configuration;
using RestSharp;
using Newtonsoft.Json;
using System.Data.Entity;
using TeamAPI.Repositories;
using TeamAPI.Services;

namespace TeamAPI.Controllers
{
	[BearerAuthentication]
	public class TeamsController : ApiController
	{
		private readonly IService<TeamModel> Service;
		public TeamsController(IService<TeamModel> service)
		{
			Service = service;
		}
		public List<TeamModel> Get()
		{
			return Service.Get();
		}

		public TeamModel Get(int id)
		{
			return Service.Get(id);
		}

		[HttpPost]
		public TeamModel Post([FromBody]TeamModel request)
		{
			return Service.Insert(request);
		}

		[HttpPut]
		public bool Put(int id, [FromBody]TeamModel request)
		{
			request.Id = id;
			return Service.Update(request);
		}

		[HttpDelete]
		public bool Delete(int id)
		{
			return Service.Delete(id);
		}
	}
}