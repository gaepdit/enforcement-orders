using Enfo.WebApp.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;

namespace Enfo.WebApp.Platform.RazorHelpers;

public static class TempDataDictionaryExtensions
{
    extension(ITempDataDictionary tempData)
    {
        private void Set<T>(string key, T value) where T : class => tempData[key] = JsonSerializer.Serialize(value);

        private T Get<T>(string key) where T : class
        {
            tempData.TryGetValue(key, out var o);
            return o == null ? null : JsonSerializer.Deserialize<T>((string)o);
        }

        public void SetDisplayMessage(Context context, string message) =>
            tempData.Set(nameof(DisplayMessage), new DisplayMessage(context, message));

        public DisplayMessage GetDisplayMessage() => tempData.Get<DisplayMessage>(nameof(DisplayMessage));
    }
}
