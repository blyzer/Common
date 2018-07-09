using Common.Api.Configurations;
using Common.Core.Repositories;
using Common.Core.Rules;
using Common.Data;
using Common.Data.Seeders;
using Common.Entities.Enums;
using Common.Entities.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            var hostingEnv = sp.GetService<IHostingEnvironment>();

            // Add framework services.
            services.AddOptions();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            ConfigIdentity(services, hostingEnv);

            services.AddTransient<DataSeeder>();

            services.AddTransient<UserRepository>();
            services.AddTransient<UserRules>();
            services.AddTransient<ProvinceRepository>();
            services.AddTransient<ProvinceRules>();
            services.AddTransient<PersonRepository>();
            services.AddTransient<ContactInformationRepository>();
            services.AddTransient<RoleRules>();

            RegisterPolicies(services);

            // nothing after this
            services.AddMvc(config =>
            {
                // Make authentication compulsory across the board (i.e. shut
                // down EVERYTHING unless explicitly opened up).
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            }).AddJsonOptions(JsonOptions.Configure);

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Common api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        private void RegisterPolicies(IServiceCollection services)
        {
            // Use policy auth.
            // implement persision/ role base policy
            // see this: https://stackoverflow.com/a/40824351/436494 
            // and this: http://benfoster.io/blog/asp-net-identity-role-claims //DevSkim: ignore DS137138

            IEnumerable<string> accessList = Enum.GetValues(typeof(AccessList)).Cast<string>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(JwtOptions.ClaimAcessName, policy => policy.RequireClaim("Access", accessList));
            });

        }

        private void ConfigIdentity(IServiceCollection services, IHostingEnvironment hostingEnv)
        {
            var jwtOptions = new JwtOptions();
            Configuration.GetSection(nameof(JwtOptions)).Bind(jwtOptions);

            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.SecretKey));

            // ConfigureMaps JwtIssuerOptions
            services.Configure<JwtOptions>(options =>
            {
                options.Issuer = jwtOptions.Issuer;
                options.Audience = jwtOptions.Audience;
                options.CommonClaimName = jwtOptions.CommonClaimName;
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtOptions.Issuer,

                ValidateAudience = true,
                ValidAudience = jwtOptions.Audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            var tokenCache = new JwtTokenCache();
            services.AddSingleton(tokenCache);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = tokenValidationParameters;
                    options.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = OnRedirectToLogin,
                        OnMessageReceived = (context) =>
                        {
                            return Task.CompletedTask;
                        }
                    };
                });

            var identityOptions = new CustomIdentityOptions();
            Configuration.GetSection(nameof(CustomIdentityOptions)).Bind(identityOptions);
            IdentityBuilder builder = services.AddIdentityCore<User>(options =>
            {
                options.User.RequireUniqueEmail = identityOptions.UserRequireUniqueEmail;
                options.Password.RequiredLength = identityOptions.PasswordRequiredLength;
                options.Password.RequireDigit = identityOptions.PasswordRequireDigit;
                options.Password.RequireLowercase = identityOptions.PasswordRequireLowercase;
                options.Password.RequireNonAlphanumeric = identityOptions.PasswordRequireNonAlphanumeric;
                options.Password.RequireUppercase = identityOptions.PasswordRequireUppercase;
                options.Lockout.MaxFailedAccessAttempts = identityOptions.LockoutMaxFailedAccessAttempts;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services)
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<User>>();
            builder.AddUserValidator<UserValidator<User>>();
        }

        private static Task OnRedirectToLogin(AuthenticationFailedContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                // return 401 if not "logged in" from an API Call
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                context.Success();
            }

            // Redirect users to login page
            return Task.CompletedTask;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DataSeeder seeder, ProvinceRules provinceRules)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == 404 &&
                    !Path.HasExtension(context.Request.Path.Value) &&
                    !context.Request.Path.Value.StartsWith("/api/", StringComparison.CurrentCulture))
                {
                    context.Request.Path = "/";
                    await next();
                }
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthentication();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Common 360 api");
            });

            app.UseMvc();

            app.ConfigureMaps();

            seeder.Seed(provinceRules.GetPredefinedProvincesAsync().Result);
        }
    }
}
