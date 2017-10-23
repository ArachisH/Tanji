using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Tangine.GitHub
{
    [DataContract]
    public sealed class GitRelease
    {
        private Version _version;

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "assets_url")]
        public string AssetsUrl { get; set; }

        [DataMember(Name = "upload_url")]
        public string UploadUrl { get; set; }

        [DataMember(Name = "html_url")]
        public string HtmlUrl { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "tag_name")]
        public string TagName { get; set; }

        [DataMember(Name = "target_commitish")]
        public string TargetCommitish { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "draft")]
        public bool IsDraft { get; set; }

        [DataMember(Name = "author")]
        public GitProfile Author { get; set; }

        [DataMember(Name = "prerelease")]
        public bool IsPrerelease { get; set; }

        [DataMember(Name = "created_at")]
        public string CreatedOn { get; set; }

        [DataMember(Name = "published_at")]
        public string PublishedOn { get; set; }

        [DataMember(Name = "assets")]
        public List<GitAsset> Assets { get; set; }

        [DataMember(Name = "tarball_url")]
        public string TarballUrl { get; set; }

        [DataMember(Name = "zipball_url")]
        public string ZipballUrl { get; set; }

        [DataMember(Name = "body")]
        public string Body { get; set; }

        public Version GetVersion()
        {
            if (_version != null)
                return _version;

            string version = TagName;
            if (version.StartsWith("v"))
                version = version.Substring(1);

            return (_version = Version.Parse(version));
        }

        public override string ToString()
        {
            return TagName;
        }
    }
}