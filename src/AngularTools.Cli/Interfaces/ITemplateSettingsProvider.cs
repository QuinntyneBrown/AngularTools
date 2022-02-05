namespace AngularTools.Cli.Interfaces
{
    internal interface ITemplateSettingsProvider
    {
        TemplateSettings GetTemplateSettings(string directory);
    }
}
