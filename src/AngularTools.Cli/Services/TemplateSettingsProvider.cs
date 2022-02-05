using AngularTools.Cli.Interfaces;
using System.IO;
using static System.IO.Path;
using static System.Text.Json.JsonSerializer;

namespace AngularTools.Cli.Services
{
    internal class TemplateSettingsProvider : ITemplateSettingsProvider
    {
        public TemplateSettings GetTemplateSettings(string directory)
        {            
            var path = $"{directory}{DirectorySeparatorChar}cliFileSettings.json";

            var settings = Deserialize<TemplateSettings>(File.ReadAllText(path), new()
            {
                PropertyNameCaseInsensitive = true,
            });

            return settings;

        }
    }
}
