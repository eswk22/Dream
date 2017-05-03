using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Application.Snapshot;
using Application.Messages;

namespace Application.WebApi
{
	public class DomainToEntityMappingProfile : Profile
	{
	

		[Obsolete]
		protected override void Configure()
		{
			this.CreateMap<ActionTaskSnapshot, ActionTaskMessage>();
			base.Configure();
		}
	}

	public static class AutoMapperInitializer
	{
		public static void Initialize(IEnumerable<Profile> profiles)
		{
			Mapper.Initialize(config => AddProfiles(config, profiles));
		}

		private static void AddProfiles(IMapperConfigurationExpression configuration, IEnumerable<Profile> profiles)
		{
			profiles.ToList().ForEach(configuration.AddProfile);
		}
	}

	

}