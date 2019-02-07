using AutoMapper;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using TeamAPI.Models;
using TeamAPI.Models.Response;
using TeamAPI.Repositories;

namespace TeamAPI.Services
{
	public class TeamService : IService<Team>
	{
		private readonly IRestClient RestClient;
		private readonly IRepository<Team> Repository;
		private HttpRequestMessage Request;
		public TeamService(IRestClient restClient, IRepository<Team> repository)
		{
			RestClient = restClient;
			Repository = repository;
			Request = new HttpRequestMessage();
		}
		public HttpResponseMessage Delete(int id)
		{
			var record = Repository.Get(id);
			if (record == null) {
				Request.CreateResponse(HttpStatusCode.NotFound, record);
			}
			else {
				Repository.Delete(record);
			}
			return Request.CreateResponse(HttpStatusCode.OK, record);
		}

		public HttpResponseMessage Get()
		{
			List<ResponseTeam> returnList = new List<ResponseTeam>();
			foreach (var item in Repository.List()) {
				ResponseTeam itemToAdd = new ResponseTeam();
				var Client = new RestClient(ConfigurationManager.AppSettings["SERVICE_ADDRESS_CONTENT"]);
				var uri = string.Format("/leagues/{0}", item.LeagueId);
				var request = new RestRequest(uri, Method.GET);
				request.AddParameter("Authorization", string.Format("Bearer " + ConfigurationManager.AppSettings["SERVICE_AUTHKEY"]), ParameterType.HttpHeader);
				IRestResponse response = Client.Execute(request);
				var responseLeague = JsonConvert.DeserializeObject<ResponseLeague>(response.Content);
				itemToAdd = Mapper.Map<ResponseTeam>(item);
				itemToAdd.League = responseLeague;
				returnList.Add(itemToAdd);
			}
			return Request.CreateResponse(HttpStatusCode.OK, returnList);
		}

		public HttpResponseMessage Get(int id)
		{
			ResponseTeam returnItem = new ResponseTeam();
			Team item = Repository.Get(id);
			if (item == null) {
				return Request.CreateResponse(HttpStatusCode.NotFound, returnItem);
			}
			else {
				var Client = new RestClient(ConfigurationManager.AppSettings["SERVICE_ADDRESS_CONTENT"]);
				var uri = string.Format("/leagues/{0}", item.LeagueId);
				var request = new RestRequest(uri, Method.GET);
				request.AddParameter("Authorization", string.Format("Bearer " + ConfigurationManager.AppSettings["SERVICE_AUTHKEY"]), ParameterType.HttpHeader);
				IRestResponse response = Client.Execute(request);
				var responseLeague = JsonConvert.DeserializeObject<ResponseLeague>(response.Content);
				returnItem = Mapper.Map<ResponseTeam>(item);
				returnItem.League = responseLeague;
				return Request.CreateResponse(HttpStatusCode.OK, returnItem);
			}
		}

		public HttpResponseMessage Insert(Team entity)
		{
			ValidationTeam validator = new ValidationTeam();
			FluentValidation.Results.ValidationResult result = validator.Validate(entity);
			if (result.IsValid) {
				Repository.Insert(entity);
				return Request.CreateResponse(HttpStatusCode.Created, entity);
			}
			else {
				return Request.CreateResponse(HttpStatusCode.NotAcceptable, result.Errors.Select(x => x.ErrorMessage).ToList());
			}
		}

		public HttpResponseMessage Update(Team entity)
		{
			var record = Repository.Get(entity.Id);
			if (record == null) {
				return Request.CreateResponse(HttpStatusCode.NotFound, record);
			}
			else {
				if (entity.LeagueId != 0) record.LeagueId = entity.LeagueId;
				if (!string.IsNullOrWhiteSpace(entity.Name)) record.Name = entity.Name;

				ValidationTeam validator = new ValidationTeam();
				FluentValidation.Results.ValidationResult result = validator.Validate(record);

				if (result.IsValid) {
					record.UpdatedAt = DateTime.Now;
					Repository.Update(record);
					return Request.CreateResponse(HttpStatusCode.OK, record);
				}
				else {
					return Request.CreateResponse(HttpStatusCode.NotAcceptable, result.Errors.Select(x => x.ErrorMessage).ToList());
				}
			}
		}
	}
}