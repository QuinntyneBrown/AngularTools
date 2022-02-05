using Allegi.SharedKernal.Services;
using System;
using System.IO;

namespace AngularTools.Cli.Services
{
    public class TemplateLocator : ITemplateLocator
    {
        public string[] Get(string filename)
            => File.ReadAllLines($@"{filename}.txt");
    }
}
