using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;

using Sulakore;

namespace Tangine.GitHub
{
    public class GitRepository
    {
        private static readonly DataContractJsonSerializer _singleReleaseDeserializer;
        private static readonly DataContractJsonSerializer _multipleReleaseDeserializer;

        public string RepoName { get; }
        public string OwnerName { get; }

        public GitRelease LatestRelease { get; private set; }
        public List<GitRelease> Releases { get; private set; }

        static GitRepository()
        {
            _singleReleaseDeserializer = new DataContractJsonSerializer(typeof(GitRelease));
            _multipleReleaseDeserializer = new DataContractJsonSerializer(typeof(List<GitRelease>));
        }
        public GitRepository(string ownerName, string repoName)
        {
            RepoName = repoName;
            OwnerName = ownerName;
        }

        public async Task<GitRelease> GetLatestReleaseAsync()
        {
            LatestRelease = await DownloadJsonObjectAsync<GitRelease>(
                $"https://api.github.com/repos/{OwnerName}/{RepoName}/releases/latest",
                _singleReleaseDeserializer).ConfigureAwait(false);

            return LatestRelease;
        }
        public async Task<List<GitRelease>> GetReleasesAsync()
        {
            Releases = await DownloadJsonObjectAsync<List<GitRelease>>(
                $"https://api.github.com/repos/{OwnerName}/{RepoName}/releases",
                _multipleReleaseDeserializer).ConfigureAwait(false);

            return Releases;
        }

        protected async Task<T> DownloadJsonObjectAsync<T>(string address, DataContractJsonSerializer serializer)
        {
            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.Proxy = null;
                    webClient.Headers[HttpRequestHeader.UserAgent] = SKore.ChromeAgent;

                    byte[] jsonData = await webClient
                        .DownloadDataTaskAsync(address).ConfigureAwait(false);

                    using (var jsonStream = new MemoryStream(jsonData))
                        return (T)serializer.ReadObject(jsonStream);
                }
            }
            catch { return default(T); }
        }
    }
}