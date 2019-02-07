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
		private readonly IService<Team> Service;
		public TeamsController(IService<Team> service)
		{
			Service = service;
		}
		public HttpResponseMessage Get()
		{
			return Service.Get();
		}

		public HttpResponseMessage Get(int id)
		{
			return Service.Get(id);
		}

		[HttpPost]
		public HttpResponseMessage Post([FromBody]Team request)
		{
			return Service.Insert(request);
		}

		[HttpPut]
		public HttpResponseMessage Put(int id, [FromBody]Team request)
		{
			request.Id = id;
			return Service.Update(request);
		}

		[HttpDelete]
		public HttpResponseMessage Delete(int id)
		{
			return Service.Delete(id);
		}
	}
}