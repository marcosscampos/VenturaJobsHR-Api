namespace VenturaJobsHR.Common.Extensions;

public static class FileExtensions
{
    public static async Task<string> GetContentAsync(this Type type, string path)
    {
        var fileContent = type.Assembly.GetManifestResourceStream($"{type.Assembly.GetName().Name}.{path}");

        if (fileContent != null)
        {
            using (var reader = new StreamReader(fileContent))
            {
                return await reader.ReadToEndAsync();
            }
        }

        return null;
    }
}
