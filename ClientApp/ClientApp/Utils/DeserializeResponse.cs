using System.Text.Json;

namespace ClientApp.Utils
{
    public static class DeserializeResponse
    {
        public static async Task<TValue?> DeserializeAsync<TValue>(this HttpResponseMessage httpResponseMessage)
        {
            try
            {
                var deserializationOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var json = await httpResponseMessage.Content.ReadAsStringAsync();
                var deserializedValue = JsonSerializer.Deserialize<TValue>(json ?? string.Empty, deserializationOptions);
                return deserializedValue;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " with type: " + typeof(TValue));
            }

            return default;
        }
    }
}
