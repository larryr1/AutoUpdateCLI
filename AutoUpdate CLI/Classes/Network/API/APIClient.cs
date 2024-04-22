using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using WUApiLib;

namespace AutoUpdate_CLI.Classes.Network.API
{
    internal class APIClient
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly HttpClient client = new HttpClient();
#pragma warning restore IDE0052 // Remove unread private members
#pragma warning disable IDE0052 // Remove unread private members
        private static ClientConfiguration configuration;
#pragma warning restore IDE0052 // Remove unread private members

        internal class REPORT_URLS
        {
            public static string REGISTER = "/api/client/{id}/register";
            public static string PHASE = "/api/client/{id}/phase/";
            public static string UPDATE_PROGRESS = "/api/client/{id}/update_progress/";
            public static string SET_UPDATES = "/api/client/{id}/set_updates/";
        }

        private static string formatUrl(string url)
        {
            return $"http://{configuration.ServerEndpoint}{url}";
        }

        public static void InitializeClient(ClientConfiguration newConfiguration)
        {
            configuration = newConfiguration;
        }

        public static string fillUrl(string url)
        {
            string newUrl = url.Replace("{id}", configuration.ClientIdentifier);
            return newUrl;
        }

        private static HttpResponseMessage APICallGet(string url, HttpStatusCode? expectedCode, string query)
        {
            string formattedUrl = fillUrl(formatUrl(url) + query);
            Console.WriteLine("GET " + formattedUrl);
            Task<HttpResponseMessage> call = Task.Run(() => client.GetAsync(formattedUrl));
            

            try
            {
                call.Wait();
            }
            catch (Exception e)
            {
                throw new Exception("API task failed: " + e.InnerException.ToString(), e);
            }
            
            if (call.Result.StatusCode != expectedCode && expectedCode != null)
            {
                throw new HttpRequestException($"API call to url ${formattedUrl} failed with status code {call.Result.StatusCode} and request result {call.Result.Content}");
            }

            return call.Result;
        }

        private static HttpResponseMessage APICallPost(string url, HttpStatusCode? expectedCode, string data)
        {
            string formattedUrl = fillUrl(formatUrl(url));
            Console.WriteLine("POST " + formattedUrl + " DATA " + data);
            Task<HttpResponseMessage> call = Task.Run(() => client.PostAsync(formattedUrl, new StringContent(data, Encoding.UTF8, "application/json")));

            try
            {
                call.Wait();
            }
            catch (Exception e)
            {
                throw new Exception("API task failed: " + e.InnerException.ToString(), e);
            }

            if (call.Result.StatusCode != expectedCode && expectedCode != null)
            {
                throw new HttpRequestException($"API call to url ${formattedUrl} failed with status code {call.Result.StatusCode} and request result {call.Result.Content}");
            }

            return call.Result;
        }

        private static string GetQueryString(NameValueCollection nvc)
        {
            string[] keys = nvc.AllKeys;
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < keys.Length; i++)
            {
                stringBuilder.Append(i > 0 ? "&" : "?");
                stringBuilder.Append(keys[i]);
                stringBuilder.Append("=");
                stringBuilder.Append(nvc.Get(keys[i]));
            }

            return stringBuilder.ToString();
        }

        public static void RegisterClient()
        {
            // URL to GET
            string url = REPORT_URLS.REGISTER;

            // Set query parameters
            string query = GetQueryString(new NameValueCollection
            {
                { "clientHostname", configuration.ClientHostname },
                { "clientDomain", configuration.ClientDomain }
            });

            // Make the call
            HttpResponseMessage result = APICallGet(url, null, query);

            // Check for error rsponses
            if (result.StatusCode != HttpStatusCode.Created && result.StatusCode != HttpStatusCode.Conflict)
            {
                throw new HttpRequestException("Status code of RegisterClient response was " + result.StatusCode);
            }
        }

        
        public static class ApplicationPhase
        {
            public static string SCAN = "scan";
            public static string DOWNLOAD = "download";
            public static string INSTALL = "scan";
            public static string CHECK = "scan";
            public static string COMPLETE = "complete";
        }

        public static void SetPhase(string phase)
        {
            // URL to GET
            string url = REPORT_URLS.PHASE;

            // Set query parameters
            string query = GetQueryString(new NameValueCollection
            {
                { "phase", phase }
            });

            // Make the call
            HttpResponseMessage result = APICallGet(url, HttpStatusCode.OK, query);
        }

        public static void SetUpdates(IUpdate[] updates)
        {
            Console.WriteLine("Passed update array has # members: " +  updates.Length);
            JArray updateArray = new JArray();
            for (int i = 0; i < updates.Length; i++)
            {
                JObject updateObject = new JObject
                {
                    { "id", updates[i].Identity.UpdateID },
                    { "title", updates[i].Title }
                };
                updateArray.Add(updateObject);
            }

            Console.WriteLine("Sending following updates: " + JsonConvert.SerializeObject(updateArray));

            // Make the call
            HttpResponseMessage result = APICallPost(REPORT_URLS.SET_UPDATES, HttpStatusCode.OK, JsonConvert.SerializeObject(updateArray));
        }

        public static void SetUpdateProgress(string uid, int progress)
        {
            // URL to GET
            string url = REPORT_URLS.UPDATE_PROGRESS;

            // Set query parameters
            string query = GetQueryString(new NameValueCollection
            {
                { "updateId", uid },
                { "progress", progress.ToString() }
            });

            // Make the call
            HttpResponseMessage result = APICallGet(url, HttpStatusCode.OK, query);
        }

        public static void ClearUpdates()
        {
            HttpResponseMessage result = APICallPost(REPORT_URLS.SET_UPDATES, HttpStatusCode.OK, null);
        }
    }
}
