using System.Collections.Generic;

namespace Allegi.SharedKernal.Services
{
    public interface ITemplateProcessor
    {
        string[] Process(string[] template, IDictionary<string, object> tokens, string[] ignoreTokens = null);
        string Process(string template, IDictionary<string, object> tokens);
    }
}