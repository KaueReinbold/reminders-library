﻿using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reminders.Business.BusinessModels;
using Reminders.Business.Contracts;
using Reminders.Business.RepositoryEntities;
using Reminders.Context.RemindersContext;
using Reminders.Core.Options;
using Reminders.Core.Routines.Reminders;
using Reminders.Domain.Entities;
using Reminders.Domain.Models;

namespace Reminders.Mvc
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
            services.AddSingleton<IRepositoryEntityGeneric<ReminderEntity>, RepositoryReminderEntity>();

            services.AddScoped<IBusinessModelGeneric<ReminderModel>, BusinessReminderModel>();

            services.AddDbContext<RemindersDbContext>(options =>
                      options.UseSqlServer(Configuration.GetConnectionString("StringConnectionReminders")), ServiceLifetime.Singleton);

            services.AddSingleton<IHostedService, ReminderCompleteService>();

            services.AddOptions();

            services.Configure<ApplicationOptions>(Configuration.GetSection("AppSettings"));

            services.AddAutoMapper();

            services.AddLogging();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(Configuration["AppSettings:ErrorHandlerPath"]);
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Reminders}/{action=Index}/{id?}");
            });
        }
    }
}
