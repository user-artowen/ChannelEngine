using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ChannelEngineCommonData.Helper
{

    class APIHelper
    {
        public static async Task<string> Request(string uri, HttpMethod method, Dictionary<string, string> header, string body, string contenttype)
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    httpClient.BaseAddress = new Uri(uri);

                    // header
                    if (header != null)
                        foreach (string key in header.Keys)
                            httpClient.DefaultRequestHeaders.Add(key, header[key]);

                    // body
                    StringContent content = new StringContent(body, Encoding.UTF8, contenttype);

                    HttpResponseMessage httpResponse;
                    // verb
                    switch (method)
                    {
                        case HttpMethod m when m == HttpMethod.Post:
                            httpResponse = await httpClient.PostAsync(uri, content);
                            break;

                        case HttpMethod m when m == HttpMethod.Put:
                            httpResponse = await httpClient.PutAsync(uri, content);
                            break;

                        case HttpMethod m when m == HttpMethod.Delete:
                            httpResponse = await httpClient.DeleteAsync(uri);
                            break;

                        default:
                            httpResponse = await httpClient.GetAsync(uri);
                            break;
                    }

                    if (httpResponse == null)
                        throw new Exception("Request has no response.");

                    if (httpResponse.StatusCode >= System.Net.HttpStatusCode.OK && httpResponse.StatusCode <= System.Net.HttpStatusCode.Accepted)
                        return await httpResponse.Content.ReadAsStringAsync();
                    else
                        return "";

                }
            }
        }
    }
}
