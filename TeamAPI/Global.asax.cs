using AutoMapper;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TeamAPI.Models;
using TeamAPI.Models.Response;

namespace TeamAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
		private static IWindsorContainer _container;
		protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Team, ResponseTeam>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.LeagueId, opt => opt.MapFrom(src => src.LeagueId))
                    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                    .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                    .ForAllOtherMembers(o => o.Ignore());
            });

			ConfigureWindsor(GlobalConfiguration.Configuration);
		}
		public static void ConfigureWindsor(HttpConfiguration configuration)
		{
			_container = new WindsorContainer();
			_container.Install(FromAssembly.This());
			_container.Kernel.Resolver.AddSubResolver(new CollectionResolver(_container.Kernel, true));
			var dependencyResolver = new WindsorDependencyResolver(_container);
			configuration.DependencyResolver = dependencyResolver;
		}
	}
}
