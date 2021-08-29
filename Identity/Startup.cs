using Identity.Custom;
using Identity.Data;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

namespace Identity
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // cookie policy to deal with temporary browser incompatibilities
            services.AddSameSiteCookiePolicy();

            var identityServer = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryApiResources(Config.Apis)
                .AddInMemoryIdentityResources(Config.IdentityResources)
                //.AddResourceStore<ResourceStore>()
                .AddClientStore<ClientStore>()
                .AddCustomUserStore();

#if DEBUG
            identityServer
                .AddDeveloperSigningCredential(persistKey: false)
                .AllowAllRedirectUris()
                .AllowAllCorsOrigins();
#else
            identityServer
                .LoadSigningCertificate(Configuration["SigningCertificate:Path"], Configuration["SigningCertificate:Password"]);
#endif

            services.AddAuthentication()
                //.AddGoogle("Google", options =>
                //{
                //    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                //    options.ClientId = Configuration["Secret:GoogleClientId"];
                //    options.ClientSecret = Configuration["Secret:GoogleClientSecret"];
                //})
                //.AddOpenIdConnect("aad", "Sign-in with Azure AD", options =>
                //{
                //    options.Authority = "https://login.microsoftonline.com/common";
                //    options.ClientId = "https://leastprivilegelabs.onmicrosoft.com/38196330-e766-4051-ad10-14596c7e97d3";

                //    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                //    options.SignOutScheme = IdentityServerConstants.SignoutScheme;

                //    options.ResponseType = "id_token";
                //    options.CallbackPath = "/signin-aad";
                //    options.SignedOutCallbackPath = "/signout-callback-aad";
                //    options.RemoteSignOutPath = "/signout-aad";

                //    options.TokenValidationParameters = new TokenValidationParameters
                //    {
                //        ValidateIssuer = false,
                //        ValidAudience = "165b99fd-195f-4d93-a111-3e679246e6a9",

                //        NameClaimType = "name",
                //        RoleClaimType = "role"
                //    };
                //})
                .AddLocalApi(options =>
                {
                    options.ExpectedScope = "api";
                });

            // preserve OIDC state in cache (solves problems with AAD and URL lenghts)
            //services.AddOidcStateDataFormatterCache("aad");

            // add CORS policy for non-IdentityServer endpoints
            services.AddCors(options =>
            {
                options.AddPolicy("api", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            //Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration).CreateLogger();

            #region ApiVersion
            // Api Versioning
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service  
                // note: the specified format code will format the version as "'v'major[.minor][-status]"  
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat  
                // can also be used to control the format of the API version in route templates  
                options.SubstituteApiVersionInUrl = true;
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCookiePolicy();
            //app.UseSerilogRequestLogging();

            app.UseCors("api");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
