using Identity.Custom;
using Identity.Data;
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
            builder.Services.AddSingleton<UserRepository>();
            //builder.AddAspNetIdentity<User>();
            builder.AddProfileService<CustomProfileService>();
            builder.AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>();

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