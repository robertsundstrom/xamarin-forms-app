﻿using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShellApp.Application.Common.Interfaces;
using ShellApp.Infrastructure;
using ShellApp.Items.WebApi;
using ShellApp.Identity.WebApi;
using ShellApp.Web.Services;
using NSwag;
using System.Linq;
using NSwag.Generation.Processors.Security;

namespace ShellApp.Web
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
            services.AddInfrastructure(Configuration)
                    .AddMyIdentity(Configuration)
                    .AddItems(Configuration)
                    .AddScoped<ICurrentUserService, CurrentUserService>()
                    .AddScoped<IImageUploader, ImageUploader>();

            services.AddControllersWithViews() //.AddControllers()
                .AddIdentityControllers()
                .AddItemsControllers()
                .AddNewtonsoftJson();

            services.AddSignalR();

            services.AddHttpContextAccessor();

            BlobClientOptions options = new BlobClientOptions(BlobClientOptions.ServiceVersion.V2019_07_07);

            services.AddScoped(sp => new BlobContainerClient(
                Configuration.GetConnectionString("Azure:Storage"), "images", options));

            services.AddScoped<IImageUploader, ImageUploader>();

            services.AddOpenApiDocument(configure =>
            {
                configure.Title = "ShellApp API";
                configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}."
                });

                configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));

                configure.SchemaNameGenerator = new CustomSchemaNameGenerator();
            });


            // add CORS policy for non-IdentityServer endpoints
            services.AddCors(options =>
            {
                options.AddPolicy("api", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();


            app.UseCors("api");

            app.UseStaticFiles();

            app.UseRouting();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapIdentityEndpoints();
                endpoints.MapItemsEndpoints();
            });
        }
    }
}
