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

namespace TeamAPI.Controllers
{
    [AsdFilter]
    public class TeamsController : ApiController
    {
        public HttpResponseMessage Get()
        {
            using (TeamDBContext db = new TeamDBContext())
            {
                return Request.CreateResponse(HttpStatusCode.OK, db.Teams.ToList());
            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (TeamDBContext db = new TeamDBContext())
            {
                return db.Teams.Find(id) == null ? Request.CreateResponse(HttpStatusCode.OK, new Team()) : Request.CreateResponse(HttpStatusCode.Created, db.Teams.Find(id));
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