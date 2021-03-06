using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Allegi.SharedKernal.Services;
using AngularTools.Cli.Interfaces;
using AngularTools.Cli.Services;

namespace AngularTools.Cli
{
    public static class Dependencies
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddMediatR(typeof(Program));
            services.AddSingleton<ICommandService, CommandService>();
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<ITemplateLocator, AngularTools.Cli.Services.TemplateLocator>();
            services.AddSingleton<ITemplateProcessor, LiquidTemplateProcessor>();
            services.AddSingleton<INamingConventionConverter, NamingConventionConverter>();
            services.AddSingleton<ISettingsProvider, SettingsProvider>();
            services.AddSingleton<ITenseConverter, TenseConverter>();
            services.AddSingleton<IContext, Context>();
            services.AddSingleton<IAngularJsonProvider, AngularJsonProvider>();
            services.AddSingleton<INearestModuleNameProvider, NearestModuleNameProvider>();
            services.AddSingleton<IPackageJsonService, PackageJsonService>();

            services.AddSingleton<ITemplateSettingsProvider, TemplateSettingsProvider>();
        }
    }
}
