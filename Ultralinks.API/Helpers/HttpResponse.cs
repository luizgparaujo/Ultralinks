using Newtonsoft.Json;

namespace Ultralinks.API.Helpers
{
    public class HttpResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; } = true;

        [JsonProperty("content", NullValueHandling = NullValueHandling.Ignore)]
        public object Content { get; set; }

        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; } = null;
    }
}
