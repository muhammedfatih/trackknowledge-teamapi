using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace TeamAPI.Helpers
{
    public class BearerAuthentication : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var identity = FetchFromHeader(actionContext);

            if (identity != null)
            {
                var Client = new RestClient(ConfigurationManager.AppSettings["SERVICE_ADDRESS_USER"]);
                var uri = string.Format("/users/validate/{0}", identity);
                var request = new RestRequest(uri, Method.GET);
                IRestResponse response = Client.Execute(request);
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    return;
                }
            }
            else
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                return;
            }
            base.OnAuthorization(actionContext);
        }

        private string FetchFromHeader(HttpActionContext actionContext)
        {
            string requestToken = null;

            var authRequest = actionContext.Request.Headers.Authorization;
            if (authRequest != null && !string.IsNullOrEmpty(authRequest.Scheme) && authRequest.Scheme == "Bearer")
                requestToken = authRequest.Parameter;

            return requestToken;
        }
    }
}