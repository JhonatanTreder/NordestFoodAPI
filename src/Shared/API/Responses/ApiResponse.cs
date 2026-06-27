using System.Text.Json.Serialization;

namespace NordesteFoodAPI.Shared.API.Responses
{
    public class ApiResponse
    {
        [JsonPropertyOrder(1)]
        public int Status { get; set; }

        [JsonPropertyOrder(3)]
        public string Message { get; set; } = string.Empty;
    }

    public class  ApiResponse<T> : ApiResponse
    {
        [JsonPropertyOrder(2)]
        public T? Data { get; set; }
    }
}
