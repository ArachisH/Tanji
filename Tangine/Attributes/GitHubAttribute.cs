using System;

using Tangine.GitHub;

namespace Tangine
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class GitHubAttribute : Attribute
    {
        public GitRepository Repository { get; }

        public GitHubAttribute(string ownerName, string repoName)
        {
            Repository = new GitRepository(ownerName, repoName);
        }
    }
}