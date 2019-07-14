using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Manga.Application.Authentication;
using Manga.Application.Repositories;
using Manga.Application.Service;
using Manga.Application.Services;
using Manga.Domain;
using Manga.Domain.Identity;
using Manga.Infrastructure.EntityFrameworkDataAccess;
using Manga.Infrastructure.IdentityAuthentication;
using Manga.Infrastructure.IdentityAuthentication.JwtToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;

namespace Manga.WebApi
{
    public sealed class StartupDevelopment
    {
        public StartupDevelopment(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            AddSwagger(services);
            AddMangaCore(services);
            AddSQLPersistence(services);
            AddIdentity(services);
            AddAuthentication(services);
            AddJWTService(services);
        }

        private void AddJWTService(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = Configuration["JwtIssuer"],
                        ValidAudience = Configuration["JwtIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });

            services.AddScoped<IGenerateToken, GenerateJwtToken>();
            services.Configure<JWTConfig>(Configuration).AddSingleton(sp => sp.GetRequiredService<IOptions<JWTConfig>>().Value);

        }

        private void AddAuthentication(IServiceCollection services)
        {
            services.AddScoped<IRegisterUserService, RegisterUser>();
            services.AddScoped<ILoginUserService, LoginUser>();
        }
        private void AddIdentity(IServiceCollection services)
        {
            services.AddIdentity<AppIdentityUser, IdentityRole>()
                        .AddEntityFrameworkStores<MangaContext>();
        }
        private void AddSQLPersistence(IServiceCollection services)
        {
            services.AddDbContext<MangaContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            //services.AddScoped<ICustomerRepository, CustomerRepository>();
            //services.AddScoped<ICustomerRepository, CustomerRepository>();
        }
        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API (Development)", Version = "v1" });

                c.AddSecurityDefinition("oauth2", new ApiKeyScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                    In = "header",
                    Name = "Authorization",
                    Type = "apiKey"
                });
                c.OperationFilter<SecurityRequirementsOperationFilter>();

            });
        }

        private void AddMangaCore(IServiceCollection services)
        {
            services.AddScoped<IEntitiesFactory, DefaultEntitiesFactory>();
            services.AddScoped<Manga.WebApi.UseCases.CloseAccount.Presenter, Manga.WebApi.UseCases.CloseAccount.Presenter>();
            services.AddScoped<Manga.WebApi.UseCases.Deposit.Presenter, Manga.WebApi.UseCases.Deposit.Presenter>();
            services.AddScoped<Manga.WebApi.UseCases.GetAccountDetails.Presenter, Manga.WebApi.UseCases.GetAccountDetails.Presenter>();
            services.AddScoped<Manga.WebApi.UseCases.GetCustomerDetails.Presenter, Manga.WebApi.UseCases.GetCustomerDetails.Presenter>();
            services.AddScoped<Manga.WebApi.UseCases.Register.Presenter, Manga.WebApi.UseCases.Register.Presenter>();
            services.AddScoped<Manga.WebApi.UseCases.Withdraw.Presenter, Manga.WebApi.UseCases.Withdraw.Presenter>();
            services.AddScoped<Manga.WebApi.UseCases.Login.Presenter, Manga.WebApi.UseCases.Login.Presenter>();

            services.AddScoped<Manga.Application.Boundaries.CloseAccount.IOutputHandler>(x => x.GetRequiredService<Manga.WebApi.UseCases.CloseAccount.Presenter>());
            services.AddScoped<Manga.Application.Boundaries.Deposit.IOutputHandler>(x => x.GetRequiredService<Manga.WebApi.UseCases.Deposit.Presenter>());
            services.AddScoped<Manga.Application.Boundaries.GetAccountDetails.IOutputHandler>(x => x.GetRequiredService<Manga.WebApi.UseCases.GetAccountDetails.Presenter>());
            services.AddScoped<Manga.Application.Boundaries.GetCustomerDetails.IOutputHandler>(x => x.GetRequiredService<Manga.WebApi.UseCases.GetCustomerDetails.Presenter>());
            services.AddScoped<Manga.Application.Boundaries.Register.IOutputHandler>(x => x.GetRequiredService<Manga.WebApi.UseCases.Register.Presenter>());
            services.AddScoped<Manga.Application.Boundaries.Withdraw.IOutputHandler>(x => x.GetRequiredService<Manga.WebApi.UseCases.Withdraw.Presenter>());
            services.AddScoped<Manga.Application.Boundaries.Login.IOutputHandler>(x => x.GetRequiredService<Manga.WebApi.UseCases.Login.Presenter>());


            services.AddScoped<Manga.Application.Boundaries.CloseAccount.IUseCase, Manga.Application.UseCases.CloseAccount>();
            services.AddScoped<Manga.Application.Boundaries.Deposit.IUseCase, Manga.Application.UseCases.Deposit>();
            services.AddScoped<Manga.Application.Boundaries.GetAccountDetails.IUseCase, Manga.Application.UseCases.GetAccountDetails>();
            services.AddScoped<Manga.Application.Boundaries.GetCustomerDetails.IUseCase, Manga.Application.UseCases.GetCustomerDetails>();
            services.AddScoped<Manga.Application.Boundaries.Register.IUseCase, Manga.Application.UseCases.Register>();
            services.AddScoped<Manga.Application.Boundaries.Withdraw.IUseCase, Manga.Application.UseCases.Withdraw>();
            services.AddScoped<Manga.Application.Boundaries.Login.IUseCase, Manga.Application.UseCases.Login>();

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
            UseSwagger(app);
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
        private void UseSwagger(IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}