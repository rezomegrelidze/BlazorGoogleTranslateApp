using System.Net;
using System.Net.Http.Json;
using BlazorTranslateApp.Services.Models;
using Newtonsoft.Json;

namespace BlazorTranslateApp.Services;

public class GoogleTranslateService
{
    private string key = "13c7003a63msh2e0d92efed51e4cp13fdc1jsnc2af03df1781";
    private string host = "google-translate1.p.rapidapi.com";
    
    private string endpoint = "https://google-translate1.p.rapidapi.com/language/translate/v2/";
    private HttpClient client;

    public GoogleTranslateService()
    {
        client = new();
        client.DefaultRequestHeaders.Add("X-RapidAPI-Key", key);
        client.DefaultRequestHeaders.Add("X-RapidAPI-Host", host);
        client.BaseAddress = new (endpoint);
    }


    public async IAsyncEnumerable<string> Languages()
    {
        var response = await client.GetFromJsonAsync<LanguagesResponse>("languages");
        var languages = response?.data.languages;

        if (languages != null)
        {    foreach (var item in languages)
            {
                yield return item.language;
            }
        }
        else
        {
            Console.WriteLine("Couldn't get languages!");
        }
    }

    public async Task<string> Detect(string text)
    {
        var response = await client.PostAsync("detect",new FormUrlEncodedContent(new KeyValuePair<string, string>[]
        {
            new ("q",text)
        }));
        try
        {
            response.EnsureSuccessStatusCode();
            if (response.Content is { } content)
            {
                dynamic body = JsonConvert.SerializeObject(await content.ReadAsStringAsync());
                
                dynamic firstDetection = body.data.detections.FirstOrDefault();
                return firstDetection.language;
            }
            else
            {
                Console.WriteLine("Invalid response!");
                return "";
            }
        }
        catch
        {
            Console.WriteLine("Exception happened, return en!");
            return "";
        }
    }

    public async Task<string> TranslateText(string q, string target, string source)
    {
        var response = await client.SendAsync(new HttpRequestMessage()
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(endpoint),
            Content = new FormUrlEncodedContent(new Dictionary<string,string>()
            {
                {"q" , q},
                {"target" ,target},
                {"source",source}
            })
        });
        response?.EnsureSuccessStatusCode();
        if(response?.Content is {} content){
            var body = await content.ReadFromJsonAsync<TranslateResponse>();
            var translation = body?.data.translations.FirstOrDefault();
            return translation?.translatedText ?? "error!";
        }
        else
        {
            Console.WriteLine("Invalid response!");
            return "server error!!";
        }
    }

}