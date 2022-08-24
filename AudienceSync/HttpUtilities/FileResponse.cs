using Newtonsoft.Json;
using System.Text.Json.Serialization;

public class FileResponse
{
    public string href { get; set; }

    [JsonProperty("ref")]
    public string _ref { get; set; }
    public string clientKey { get; set; }
    public string flowRef { get; set; }
    public string segmentRef { get; set; }
    public string status { get; set; }
    public string[] signedUrls { get; set; }
}
