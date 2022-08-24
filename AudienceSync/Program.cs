using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using AudienceSync.HttpUtilities;
using Newtonsoft.Json;

public partial class Program
{
    public static void Main(string[] args)
    {
        try
        {            
             Caller().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw ex;
        }
    }
   
    public async static Task<bool> Caller()
    {
        string baseUrl = "https://{api}.boxever.com/v2";

        var request = new TriggerRequest();
        var auth = new HttpBasicAuthorization();

        string authTOken = auth.GetAuthToken();

        var client = new HttpClient();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authTOken);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var content = new StringContent(JsonConvert.SerializeObject(request), null, "application/json");

        var response = await client.PostAsync($"{baseUrl}/batchFlowsTrigger", content);

        if (response.StatusCode == System.Net.HttpStatusCode.Created)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var triggerResponse = JsonConvert.DeserializeObject<TriggerResponse>(responseContent);

            if (triggerResponse?.status == "PENDING")
            {
                Console.WriteLine("Pending");

                while (true)
                {
                    Thread.Sleep(10000);
                    var statusHttpResponse = await client.GetAsync($"{baseUrl}/batchFlowsJob/{triggerResponse._ref}");
                    var statusResponseContent = await statusHttpResponse.Content.ReadAsStringAsync();
                    var statusResponse = JsonConvert.DeserializeObject<StatusResponse>(statusResponseContent);

                    Console.WriteLine($"Heart beat- {statusResponse?.status}");
                    if (statusResponse?.status == "FAILED")
                    {
                        break;
                    }
                    else if (statusResponse?.status == "SUCCESS")
                    {
                        var fileHttpResponse = await client.GetAsync($"{baseUrl}/batchFlowOutput/{statusResponse._ref}/files");
                        var fileResponseContent = await fileHttpResponse.Content.ReadAsStringAsync();
                        var fileResponse = JsonConvert.DeserializeObject<FileResponse>(fileResponseContent);
                        if (fileResponse?.status == "SUCCESS")
                        {
                            Console.WriteLine($"Signed URL - {fileResponse.signedUrls[0]}");
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));


                            WebClient webClient = new WebClient();
                            webClient.DownloadFile(fileResponse.signedUrls[0], "MyAudience.gz");
                            Console.WriteLine("Saved to MyAudience.gz");                            
                        }
                        else
                        {
                            Console.WriteLine($"There was an error while getting file information - {fileResponse?.status}");
                        }

                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine($"There seems to be something wrong {triggerResponse?.status}");
            }
        }

        return true;
    }
}