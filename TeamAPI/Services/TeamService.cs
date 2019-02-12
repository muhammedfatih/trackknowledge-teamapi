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
	public class TeamService : IService<TeamModel>
	{
		private readonly IRestClient RestClient;
		private readonly IRepository<Team> Repository;
		public TeamService(IRestClient restClient, IRepository<Team> repository)
		{
			RestClient = restClient;
			Repository = repository;
		}
		public bool Delete(int id)
		{
			var record = Repository.Get(id);
			if (record == null) {
				return false;
			}
			else {
				Repository.Delete(record);
			}
			return true;
		}

		public List<TeamModel> Get()
		{
			List<TeamModel> returnList = new List<TeamModel>();
			foreach (var item in Repository.List()) {
				TeamModel itemToAdd = new TeamModel();
				RestClient.BaseUrl = new Uri(ConfigurationManager.AppSettings["SERVICE_ADDRESS_CONTENT"]);
				var uri = string.Format("/leagues/{0}", item.LeagueId);
				var request = new RestRequest(uri, Method.GET);
				request.AddParameter("Authorization", string.Format("Bearer " + ConfigurationManager.AppSettings["SERVICE_AUTHKEY"]), ParameterType.HttpHeader);
				IRestResponse response = RestClient.Execute(request);
				var responseLeague = JsonConvert.DeserializeObject<ResponseLeague>(response.Content);
				itemToAdd = Mapper.Map<TeamModel>(item);
				itemToAdd.League = responseLeague;
				returnList.Add(itemToAdd);
			}
			return returnList;
		}

		public TeamModel Get(int id)
		{
			TeamModel returnItem = new TeamModel();
			Team item = Repository.Get(id);
			if (item == null) {
				return returnItem;
			}
			else {
				RestClient.BaseUrl = new Uri(ConfigurationManager.AppSettings["SERVICE_ADDRESS_CONTENT"]);
				var uri = string.Format("/leagues/{0}", item.LeagueId);
				var request = new RestRequest(uri, Method.GET);
				request.AddParameter("Authorization", string.Format("Bearer " + ConfigurationManager.AppSettings["SERVICE_AUTHKEY"]), ParameterType.HttpHeader);
				IRestResponse response = RestClient.Execute(request);
				var responseLeague = JsonConvert.DeserializeObject<ResponseLeague>(response.Content);
				returnItem = Mapper.Map<TeamModel>(item);
				returnItem.League = responseLeague;
				return returnItem;
			}
		}

		public TeamModel Insert(TeamModel teamModel)
		{
			ValidationTeam validator = new ValidationTeam();
			Team entity = Mapper.Map<Team>(teamModel);
			FluentValidation.Results.ValidationResult result = validator.Validate(entity);
			if (result.IsValid) {
				Repository.Insert(entity);
				return teamModel;
			}
			else {
				return teamModel;
			}
		}

		public bool Update(TeamModel entity)
		{
			var record = Repository.Get(entity.Id);
			if (record == null) {
				return false;
			}
			else {
				if (entity.LeagueId != 0) record.LeagueId = entity.LeagueId;
				if (!string.IsNullOrWhiteSpace(entity.Name)) record.Name = entity.Name;

				ValidationTeam validator = new ValidationTeam();
				FluentValidation.Results.ValidationResult result = validator.Validate(record);

				if (result.IsValid) {
					record.UpdatedAt = DateTime.Now;
					Repository.Update(record);
					return true;
				}
				else {
					return false;
				}
			}
		}
	}
}