﻿using MenuOrder.MicroService.SeriviceInstallers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace MenuOrder.MicroService.Extensions
{
    public static class ServiceInstallerExtension
    {
        public static void InstallServiceAssembly(this IServiceCollection services, IConfiguration Configuration)
        {
            //for readbility I have created this way diffrent installers will register diffrent capability so that we dont touch the StartUp.cs because the code keeps growing and readbilty will be
            //difficult if we put evrything in the startip.ca class
            var installers = typeof(Startup).Assembly.ExportedTypes.Where(x =>
            typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).Select(Activator.CreateInstance).Cast<IInstaller>().ToList();

            installers.ForEach(installers => installers.InstallServices(services, Configuration));
        }
    }
}
