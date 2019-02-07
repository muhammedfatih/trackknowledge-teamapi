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

namespace TeamAPI.Services.Tests
{
	[TestFixture]
	public class TeamServiceTests
	{
		[Test]
		public void GetId_ReturnStatus200AndEntityIfEntityExist()
		{
			var repository = new Mock<IRepository<Team>>();
			var restClient = new Mock<IRestClient>();

			var entity = new Team() {
				Id = 1
				,
				Name = "Test"
				,
				LeagueId = 3
			};

			repository.Setup(st => st.Get(1)).Returns(() => entity);

			var service = new TeamService(restClient.Object, repository.Object);
			var result = service.Get(1);

			Assert.IsNotNull(result.Content);
			Assert.Equals(result.StatusCode, HttpStatusCode.OK);
		}
	}
}
