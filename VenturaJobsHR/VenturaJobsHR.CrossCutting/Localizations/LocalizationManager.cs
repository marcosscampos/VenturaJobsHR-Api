using Newtonsoft.Json.Linq;
using System.Text;

namespace VenturaJobsHR.CrossCutting.Localizations;

public class LocalizationManager : ILocalizationManager
{
    private readonly JObject _json = null;

    public LocalizationManager()
    {
        _json = GetJson();
    }

    public string GetValue(string key)
    {
        return _json[key]?.Value<string>() ?? $"[{key}]";
    }

    private JObject GetJson()
    {
        if (_json == null)
        {
            var path = File.ReadAllText(@"Json/Localization/pt-BR.json", Encoding.UTF8);
            return JObject.Parse(path);
        }

        return _json;
    }
}
