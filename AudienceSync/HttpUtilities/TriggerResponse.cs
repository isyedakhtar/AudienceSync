using Newtonsoft.Json;
using System.Text.Json.Serialization;

public class TriggerResponse
{
    public string href { get; set; }

    [JsonProperty("ref")]
    public string _ref { get; set; }
    public string clientKey { get; set; }
    public string flowRef { get; set; }
    public string segmentRef { get; set; }
    public DateTime datasetDate { get; set; }
    public string status { get; set; }
    public string s3GuestContextPath { get; set; }
    public string s3FlowExecutionPath { get; set; }
    public DateTime startTime { get; set; }
    public int guestContextSize { get; set; }
    public int segmentSize { get; set; }
    public int numberPartitions { get; set; }
    public int applicationVersion { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime modifiedAt { get; set; }
}