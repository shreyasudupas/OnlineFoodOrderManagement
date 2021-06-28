﻿using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.MicroService.Installers
{
    public class AddControllerInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers(options =>
            {
                options.Conventions.Add(new GroupingByNamespaceConvention());
            });
        }

        public class GroupingByNamespaceConvention : IControllerModelConvention
        {
            public void Apply(ControllerModel controller)
            {
                var controllerNamespace = controller.ControllerType.Namespace;
                var apiVersion = controllerNamespace.Split(".").Last().ToLower();
                if (!apiVersion.StartsWith("v")) { apiVersion = "v1"; }
                controller.ApiExplorer.GroupName = apiVersion;
            }
        }
    }
}
