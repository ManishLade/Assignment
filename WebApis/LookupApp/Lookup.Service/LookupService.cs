using Lookup.Service.Models;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Net;

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

    public LookupResponse GetStateByZipCode(string zipCode)
    {
        var lookupResponse = new LookupResponse();

        if (!string.IsNullOrEmpty(zipCode))
        {
            try
            {
                var request = new RestRequest();
                request.Method = Method.Post;
                request.AddHeader("Content-Type", "text/plain");
                var body =
                    $@"&XML=<CityStateLookupRequest USERID=""{appSettings.Username}""><ZipCode ID= ""0""><Zip5>{zipCode}</Zip5></ZipCode></CityStateLookupRequest>";
                request.AddParameter("text/plain", body, ParameterType.RequestBody);
                var response = client.Execute<CityStateLookupResponse>(request);

                if (response?.StatusCode == HttpStatusCode.OK)
                {
                    if (response?.Data?.ZipCode.Error == null && response?.Data?.ZipCode?.State != null)
                    {
                        lookupResponse.State = response?.Data?.ZipCode?.State;
                        lookupResponse.StatusCode = HttpStatusCode.OK;
                    }
                    else if(response?.Data?.ZipCode?.Error?.Description != null)
                    {
                        lookupResponse.StatusCode = HttpStatusCode.BadRequest;
                        lookupResponse.ErrorMessage = response?.Data?.ZipCode?.Error?.Description;
                    }
                    //CityStateLookupService returning error with Ok StatusCode thus need to add InternalServerError StatusCode
                    else
                    {
                        lookupResponse.StatusCode = HttpStatusCode.InternalServerError;
                        lookupResponse.ErrorMessage = response?.ErrorMessage;
                    }
                }
                else
                {
                    lookupResponse.StatusCode = HttpStatusCode.InternalServerError;
                    lookupResponse.ErrorMessage = response?.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                lookupResponse.StatusCode = HttpStatusCode.InternalServerError;
                lookupResponse.ErrorMessage = ex.Message;
            }
        }
        else
        {
            lookupResponse.StatusCode = HttpStatusCode.BadRequest;
            lookupResponse.ErrorMessage = "Invalid Zip Code.";
        }

        return lookupResponse;
    }
}