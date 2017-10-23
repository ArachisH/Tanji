using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Sulakore.Habbo.Web
{
    public class HRequest
    {
        public IWebProxy Proxy { get; set; }
        public CookieContainer Cookies { get; }

        public HRequest()
        {
            Cookies = new CookieContainer();
        }

        public Task<string> DownloadStringAsync(Uri address)
        {
            return DownloadStringAsync(address, null);
        }
        public Task<string> DownloadStringAsync(Uri address, byte[] postData)
        {
            var request = (HttpWebRequest)WebRequest.Create(address);
            return DownloadStringAsync(request, postData);
        }

        public Task<string> DownloadStringAsync(string address)
        {
            return DownloadStringAsync(new Uri(address), null);
        }
        public Task<string> DownloadStringAsync(string address, byte[] postData)
        {
            return DownloadStringAsync(new Uri(address), postData);
        }

        public Task<string> DownloadStringAsync(HttpWebRequest request)
        {
            return DownloadStringAsync(request, null);
        }
        public async Task<string> DownloadStringAsync(HttpWebRequest request, byte[] postData)
        {
            request.UserAgent = SKore.ChromeAgent;
            request.AllowAutoRedirect = false;
            request.CookieContainer = Cookies;
            request.Method = "GET";
            request.Proxy = Proxy;

            if (postData != null)
            {
                request.Method = "POST";
                request.ContentLength = postData.Length;

                using (var reqStream = await request
                    .GetRequestStreamAsync().ConfigureAwait(false))
                {
                    await reqStream.WriteAsync(postData,
                        0, postData.Length).ConfigureAwait(false);
                }
            }

            return (await VerifyResponseBody(
                request).ConfigureAwait(false));
        }

        protected void UpdateCookies(string jScriptBody, HttpWebRequest request)
        {
            int setCookieIndex = -1;
            while ((setCookieIndex = jScriptBody.IndexOf("setCookie('")) != -1)
            {
                jScriptBody = jScriptBody.Substring(setCookieIndex + 11);
                string cookieName = jScriptBody.GetParent("'");

                string cookieValue = jScriptBody.GetChild(",", ',').Trim();
                if (!cookieValue.StartsWith("'"))
                {
                    switch (cookieValue)
                    {
                        case "document.referrer":
                        cookieValue = request.Referer;
                        break;
                    }
                }
                else cookieValue = cookieValue.GetChild("'", '\'');

                if (cookieValue != null)
                    cookieValue = Uri.EscapeDataString(cookieValue);

                string cookieObject =
                    (cookieName + "=" + cookieValue);

                Cookies.SetCookies(request.RequestUri, cookieObject);
            }
        }
        private async Task<string> VerifyResponseBody(HttpWebRequest request)
        {
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)(await request
                    .GetResponseAsync().ConfigureAwait(false));
            }
            catch (WebException e)
            {
                response = (HttpWebResponse)e.Response;
            }
            try
            {
                Cookies.SetCookies(request.RequestUri,
                    response.Headers["Set-Cookie"] ?? string.Empty);

                // contains(text/html)
                string body = string.Empty;
                using (Stream responseStream = response.GetResponseStream())
                using (var bodyStream = new StreamReader(responseStream))
                {
                    body = await bodyStream.ReadToEndAsync()
                       .ConfigureAwait(false);
                }

                if (body.Contains("setCookie('"))
                {
                    UpdateCookies(body, request);

                    body = await DownloadStringAsync(
                        request.RequestUri).ConfigureAwait(false);
                }
                return body;
            }
            finally { response?.Dispose(); }
        }
    }
}