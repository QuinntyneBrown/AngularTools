using Allegi.SharedKernal.Models;
using Allegi.SharedKernal.Services;
using AngularTools.Cli.Interfaces;
using CommandLine;
using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AngularTools
{
    internal class Default
    {
        [Verb("default")]
        internal class Request : IRequest<Unit> {

            [Option('n', "name", Required = false)]
            public string Name { get; set; } = "Foo";

            [Option('t',"templateName", Required = false)]
            public string TemplateName { get; set; } = "Page";

            [Option('p', "path", Required = false)]
            public string Path { get; set; } = @"C:\projects\AngularTemplates";

            [Option("tokens", Required = false)]
            public string Tokens { get; set; }

            [Option('d', Required = false)]
            public string Directory { get; set; } = System.Environment.CurrentDirectory;
        }

        internal class Handler : IRequestHandler<Request, Unit>
        {
            private readonly ITemplateSettingsProvider _templateSettingsProvider;
            private readonly ITemplateLocator _templateLocator;
            private readonly ITemplateProcessor _templateProcessor;
            private readonly IFileSystem _fileSystem;
            private readonly IAngularJsonProvider _angularJsonProvider;
            private readonly ICommandService _commandService;
  

            public Handler(ITemplateProcessor templateProcessor, ITemplateSettingsProvider templateSettingsProvider, ITemplateLocator templateLocator, IFileSystem fileSystem, IAngularJsonProvider angularJsonProvider, ICommandService commandService)
            {
                _templateLocator = templateLocator;
                _templateProcessor = templateProcessor;
                _fileSystem = fileSystem;
                _templateSettingsProvider = templateSettingsProvider;
                _angularJsonProvider = angularJsonProvider;
                _commandService = commandService;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var settings = _templateSettingsProvider.GetTemplateSettings($"{request.Path}{Path.DirectorySeparatorChar}{request.TemplateName}");

                var tokens = new TokensBuilder()
                                    .With("Name", (Token)request.Name) 
                                    .With("Directory", (Token)request.Directory)
                                    .With("Prefix", (Token)_angularJsonProvider.Get(request.Directory).Prefix)
                                    .Build();

                _commandService.Start(_templateProcessor.Process("ng g c {{ nameSnakeCase }} --skip-import", tokens), request.Directory);

                var componentDirectory = $"{request.Directory}{Path.DirectorySeparatorChar}{_templateProcessor.Process("{{ nameSnakeCase }}", tokens)}";

                var componentTemplate = _templateProcessor.Process(_templateLocator.Get($"{request.Path}{Path.DirectorySeparatorChar}{request.TemplateName}{Path.DirectorySeparatorChar}TypeScript"), tokens);

                var htmlTemplate = _templateProcessor.Process(_templateLocator.Get($"{request.Path}{Path.DirectorySeparatorChar}{request.TemplateName}{Path.DirectorySeparatorChar}Html"), tokens);

                var scssTemplate = _templateProcessor.Process(_templateLocator.Get($"{request.Path}{Path.DirectorySeparatorChar}{request.TemplateName}{Path.DirectorySeparatorChar}Scss"), tokens);

                _fileSystem.WriteAllLines($@"{componentDirectory}{Path.DirectorySeparatorChar}{_templateProcessor.Process(@$"{settings.FullFileNameToken}.ts", tokens)}", componentTemplate);

                _fileSystem.WriteAllLines($@"{componentDirectory}{Path.DirectorySeparatorChar}{_templateProcessor.Process(@$"{settings.FullFileNameToken}.html", tokens)}", htmlTemplate);

                _fileSystem.WriteAllLines($@"{componentDirectory}{Path.DirectorySeparatorChar}{_templateProcessor.Process(@$"{settings.FullFileNameToken}.scss", tokens)}", scssTemplate);

                return new();
            }
        }
    }
}
