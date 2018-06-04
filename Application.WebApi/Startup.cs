using Owin;
using Application.WebApi.Configuration;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Logging;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.Default;

namespace Application.WebApi
{
    internal class Startup
    {
        public void Configuration(IAppBuilder app)
        {
         
            app.Map("/core", coreApp =>
            {
                var factory = new IdentityServerServiceFactory()
                    .UseInMemoryClients(Clients.Get())
                    .UseInMemoryScopes(Scopes.Get());

                // different examples of custom user services
                //var userService = new RegisterFirstExternalRegistrationUserService();
                //var userService = new ExternalRegistrationUserService();
                var userService = new EulaAtLoginUserService();
                //var userService = new LocalRegistrationUserService();

                // note: for the sample this registration is a singletone (not what you want in production probably)
                factory.UserService = new Registration<IUserService>(resolver => userService);

                var options = new IdentityServerOptions
                {
                    SiteName = "IdentityServer3 - CustomUserService",

                    SigningCertificate = Certificate.Get(),
                    Factory = factory,

                    AuthenticationOptions = new AuthenticationOptions
                    {
                        IdentityProviders = ConfigureAdditionalIdentityProviders,
                        LoginPageLinks = new LoginPageLink[] {
                            new LoginPageLink{
                                Text = "Register",
                                //Href = "~/localregistration"
                                Href = "localregistration"
                            }
                        }
                    },

                    EventsOptions = new EventsOptions
                    {
                        RaiseSuccessEvents = true,
                        RaiseErrorEvents = true,
                        RaiseFailureEvents = true,
                        RaiseInformationEvents = true
                    }
                };

                coreApp.UseIdentityServer(options);
            });
        }

        public static void ConfigureAdditionalIdentityProviders(IAppBuilder app, string signInAsType)
        {
          //register external providers
        }
    }
}