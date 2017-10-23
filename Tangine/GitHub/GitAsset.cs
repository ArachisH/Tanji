using System.Runtime.Serialization;

namespace Tangine.GitHub
{
    [DataContract]
    public sealed class GitAsset
    {
        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "label")]
        public string Label { get; set; }

        [DataMember(Name = "uploader")]
        public GitProfile Uploader { get; set; }

        [DataMember(Name = "content_type")]
        public string ContentType { get; set; }

        [DataMember(Name = "state")]
        public string State { get; set; }

        [DataMember(Name = "size")]
        public int Size { get; set; }

        [DataMember(Name = "download_count")]
        public int DownloadCount { get; set; }

        [DataMember(Name = "created_at")]
        public string CreatedOn { get; set; }

        [DataMember(Name = "updated_at")]
        public string UpdatedOn { get; set; }

        [DataMember(Name = "browser_download_url")]
        public string BrowserDownloadUrl { get; set; }
    }
}