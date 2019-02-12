using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeamAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TeamAPI.Repositories;
using Moq;
using RestSharp;
using TeamAPI.Models;
using System.Net;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using System.Configuration;
using TeamAPI.Models.Response;
using Newtonsoft.Json;
using AutoMapper;
using System.Net.Http;

namespace TeamAPI.Services.Tests
{
	[TestClass]
	public class TeamServiceTests
	{
		public TeamServiceTests()
		{
			Mapper.Initialize(cfg => {
				cfg.CreateMap<Team, TeamModel>()
					.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
					.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
					.ForMember(dest => dest.LeagueId, opt => opt.MapFrom(src => src.LeagueId))
					.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
					.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
					.ForAllOtherMembers(o => o.Ignore());
			});
		}

		#region Get by Id
		[TestMethod]
		public void GetId_ReturnIfEntityExist()
		{
			var repository = new Mock<IRepository<Team>>();
			var restClient = new Mock<IRestClient>();

			var entity = new Team() {
				Id = 1,
				Name = "Test",
				LeagueId = 3
			};

			var leagueResponse = new ResponseLeague() {
				Id = 3,
				Name = "Test League",
			};

			var response = new RestResponse<ResponseLeague>() {
				Content = JsonConvert.SerializeObject(leagueResponse),
				StatusCode = HttpStatusCode.OK
			};

			restClient.Setup(st => st.Execute(It.IsAny<RestRequest>())).Returns(() => response);
			repository.Setup(st => st.Get(1)).Returns(() => entity);

			var service = new TeamService(restClient.Object, repository.Object);
			var result = service.Get(1);

			Assert.IsNotNull(result);
			Assert.AreEqual(result.Id, 1);
		}
		[TestMethod]
		public void GetId_ReturnNullModelIfEntityIsNotExist()
		{
			var repository = new Mock<IRepository<Team>>();
			var restClient = new Mock<IRestClient>();

			var entity = new Team() {
				Id = 1,
				Name = "Test",
				LeagueId = 3
			};

			var leagueResponse = new ResponseLeague() {
				Id = 3,
				Name = "Test League",
			};

			var response = new RestResponse<ResponseLeague>() {
				Content = JsonConvert.SerializeObject(leagueResponse),
				StatusCode = HttpStatusCode.OK
			};

			restClient.Setup(st => st.Execute(It.IsAny<RestRequest>())).Returns(() => response);
			repository.Setup(st => st.Get(2)).Returns(() => entity);

			var service = new TeamService(restClient.Object, repository.Object);
			var result = service.Get(1);

			Assert.IsNotNull(result);
			Assert.AreEqual(result.Id, 0);
		}
		#endregion

		#region List All
		[TestMethod]
		public void Get_ReturnIfEntityExist()
		{
			var repository = new Mock<IRepository<Team>>();
			var restClient = new Mock<IRestClient>();

			var entity = new List<Team>() {
				new Team(){
					Id = 1,
					Name = "Test",
					LeagueId = 3
				},
				new Team(){
					Id = 2,
					Name = "Test2",
					LeagueId = 3
				}
			};

			var leagueResponse = new ResponseLeague() {
				Id = 3,
				Name = "Test League",
			};

			var response = new RestResponse<ResponseLeague>() {
				Content = JsonConvert.SerializeObject(leagueResponse),
				StatusCode = HttpStatusCode.OK
			};

			restClient.Setup(st => st.Execute(It.IsAny<RestRequest>())).Returns(() => response);
			repository.Setup(st => st.List()).Returns(() => entity);

			var service = new TeamService(restClient.Object, repository.Object);
			var result = service.Get();

			Assert.IsNotNull(result);
			Assert.Greater(result.Count, 0);
		}
		[TestMethod]
		public void Get_ReturnNullListIfEntityIsNotExist()
		{
			var repository = new Mock<IRepository<Team>>();
			var restClient = new Mock<IRestClient>();

			var entity = new List<Team>() {
			};

			var leagueResponse = new ResponseLeague() {
				Id = 3,
				Name = "Test League",
			};

			var response = new RestResponse<ResponseLeague>() {
				Content = JsonConvert.SerializeObject(leagueResponse),
				StatusCode = HttpStatusCode.OK
			};

			restClient.Setup(st => st.Execute(It.IsAny<RestRequest>())).Returns(() => response);
			repository.Setup(st => st.List()).Returns(() => entity);

			var service = new TeamService(restClient.Object, repository.Object);
			var result = service.Get();

			Assert.IsNotNull(result);
			Assert.AreEqual(result.Count, 0);
		}
		#endregion
	}
}
