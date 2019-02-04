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

namespace TeamAPI.Controllers
{
    [BearerAuthentication]
    public class TeamsController : ApiController
    {
        public HttpResponseMessage Get()
        {
            using (TeamDBContext db = new TeamDBContext())
            {
                List<ResponseTeam> returnList = new List<ResponseTeam>();
                foreach (var item in db.Teams.ToList())
                {
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
        }

        public HttpResponseMessage Get(int id)
        {
            using (TeamDBContext db = new TeamDBContext())
            {
                ResponseTeam returnItem=new ResponseTeam();
                Team item = db.Teams.Find(id);
                if (item == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, returnItem);
                }
                else
                {
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
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody]Team request)
        {
            using (TeamDBContext db = new TeamDBContext())
            {
                ValidationTeam validator = new ValidationTeam();
                ValidationResult result = validator.Validate(request);
                if (result.IsValid)
                {
                    db.Teams.Add(request);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.Created, request);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, result.Errors.Select(x => x.ErrorMessage).ToList());
                }
            }
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, [FromBody]Team request)
        {
            using (TeamDBContext db = new TeamDBContext())
            {
                var record = db.Teams.Find(id);
                if (record == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, record);
                }
                else
                {
                    if (request.LeagueId != 0) record.LeagueId = request.LeagueId;
                    if (!string.IsNullOrWhiteSpace(request.Name)) record.Name= request.Name;

                    ValidationTeam validator = new ValidationTeam();
                    ValidationResult result = validator.Validate(record);

                    if (result.IsValid)
                    {
                        record.UpdatedAt = DateTime.Now;
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, record);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable, result.Errors.Select(x => x.ErrorMessage).ToList());
                    }
                }
            }
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            using (TeamDBContext db = new TeamDBContext())
            {
                var record = db.Teams.Find(id);
                if (record == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, record);
                }
                else
                {
                    db.Teams.Remove(record);
                }
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, record);
            }
        }
    }
}