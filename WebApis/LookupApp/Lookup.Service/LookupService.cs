using System.Net;
using Microsoft.Extensions.Options;
using RestSharp;

namespace Lookup.Service;

public class LookupService : ILookupService
{
    private readonly AppSettings appSettings;
    private readonly RestClient client;

    public LookupService(IOptions<AppSettings> appSettings)
    {
        this.appSettings = appSettings.Value;
        client = new RestClient(this.appSettings.LookupUrl);
    }

    public string GetStateByZipCode(string zipCode)
    {
        var request = new RestRequest();
        request.Method = Method.Post;
        request.AddHeader("Content-Type", "text/plain");
        var body =
            $@"&XML=<CityStateLookupRequest USERID=""{appSettings.Username}""><ZipCode ID= ""0""><Zip5>{zipCode}</Zip5></ZipCode></CityStateLookupRequest>";
        request.AddParameter("text/plain", body, ParameterType.RequestBody);
        var response = client.Execute<CityStateLookupResponse>(request);
        return response?.StatusCode == HttpStatusCode.OK
            ? response?.Data?.ZipCode.Error == null
                ? response?.Data?.ZipCode?.State
                : response?.Data?.ZipCode?.Error?.Description
            : response.ErrorMessage;
    }
}