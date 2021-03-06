using Allegi.SharedKernal.Models;
using System.IO;
using System.Linq;
using static System.Environment;
using static System.IO.Path;
using static System.Text.Json.JsonSerializer;

namespace Allegi.SharedKernal.Services
{
    public interface IAngularJsonProvider
    {
        AngularJson Get(string directory = null);
    }
    public class AngularJsonProvider : IAngularJsonProvider
    {
        public AngularJson Get(string directory = null)
        {
            directory ??= CurrentDirectory;

            var parts = directory.Split(DirectorySeparatorChar);

            for (var i = 1; i <= parts.Length; i++)
            {
                var path = $"{string.Join(DirectorySeparatorChar, parts.Take(i))}{DirectorySeparatorChar}angular.json";

                if (File.Exists(path))
                {
                    var angularJson = Deserialize<AngularJson>(File.ReadAllText(path), new()
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                    angularJson.RootDirectory = new FileInfo(path).Directory.FullName;

                    return angularJson;
                }

                i++;
            }

            return AngularJson.Empty;

        }
    }
}
