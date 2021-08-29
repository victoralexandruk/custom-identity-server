using Identity.Custom;
using Identity.Data.Repositories;
using Identity.Data.Stores;
using Identity.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddCustomUserStore(this IIdentityServerBuilder builder)
        {
            builder.Services.Configure<PasswordHasherOptions>(opt =>
            {
                opt.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2;
            });
            builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
            builder.Services.AddScoped<UserRepository>();
            //builder.AddAspNetIdentity<User>();
            builder.AddProfileService<CustomProfileService>();
            builder.AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>();

            return builder;
        }

        public static IIdentityServerBuilder AddCustomClientStore(this IIdentityServerBuilder builder)
        {
            builder.Services.AddScoped<ClientRepository>();
            builder.AddClientStore<ClientStore>();
            return builder;
        }

        public static IIdentityServerBuilder AddCustomResourceStore(this IIdentityServerBuilder builder)
        {
            builder.Services.AddScoped<ResourceRepository>();
            builder.AddResourceStore<ResourceStore>();
            return builder;
        }

        public static IIdentityServerBuilder LoadSigningCertificate(this IIdentityServerBuilder builder, string certificatePath, string certificatePassword)
        {
            if (!File.Exists(certificatePath))
            {
                certificatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, certificatePath);
            }
            if (!File.Exists(certificatePath))
            {
                throw new FileNotFoundException("Signing Certificate File Not Found!");
            }
            var cert = new X509Certificate2(certificatePath, certificatePassword);
            builder.AddSigningCredential(cert);
            return builder;
        }

        public static IIdentityServerBuilder AllowAllRedirectUris(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransient<IRedirectUriValidator, AllowAllRedirectUriValidator>();
            return builder;
        }

        public static IIdentityServerBuilder AllowAllCorsOrigins(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransient<ICorsPolicyService, AllowAllCorsPolicyService>();
            return builder;
        }
    }
}