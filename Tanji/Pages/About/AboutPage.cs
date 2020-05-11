using System;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;

using Tanji.Windows;

using Octokit;

namespace Tanji.Pages.About
{
    public class AboutPage : TanjiPage
    {
        public Version LocalVersion { get; }
        public Release Latest { get; private set; }
        public Release Current { get; private set; }

        public AboutPage(MainFrm ui, TabPage tab)
            : base(ui, tab)
        {
            LocalVersion = new Version(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);
            UI.TanjiVersionTxt.Text = ("v" + LocalVersion.ToString(3));

#if !DEBUG
            var git = new GitHubClient(new ProductHeaderValue("Tanji"));
            git.Repository.Release.GetAll("ArachisH", "Tanji").ContinueWith(GrabbedReleases, TaskScheduler.FromCurrentSynchronizationContext());
            //git.Repository.Content.GetAllContents("ArachisH", "Tanji", "Tanji/Hashes.ini").ContinueWith(GrabbedHashes, TaskScheduler.FromCurrentSynchronizationContext());
#endif

            UI.ArachisBtn.Click += ArachisBtn_Click;
            UI.SpeaqerBtn.Click += SpeaqerBtn_Click;
            UI.DarkStarBtn.Click += DarkStarBtn_Click;

            UI.DonateBtn.Click += DonateBtn_Click;
            UI.HarbleDiscordBtn.Click += HarbleDiscordBtn_Click;

            UI.SNGButton.Click += SNGButton_Click;
            UI.DarkboxBtn.Click += DarkboxBtn_Click;
            UI.SelloutBtn.Click += SelloutBtn_Click;
            UI.Sellout2Btn.Click += Sellout2Btn_Click;
        }

        private void GrabbedReleases(Task<IReadOnlyList<Release>> getReleasesTask)
        {
            IReadOnlyList<Release> releases = getReleasesTask.Result;
            Latest = releases.FirstOrDefault();

            foreach (Release release in releases)
            {
                string version = release.TagName.Substring(1);
                if (version == LocalVersion.ToString())
                {
                    Current = release;
                    break;
                }
            }
            if (Current == null)
            {
                Current = Latest;
            }
            if (!Latest.Prerelease && new Version(Latest.TagName.Substring(1)) > LocalVersion)
            {
                if ((bool)Program.Settings["PromptUpdateNotification"])
                {
                    if (MessageBox.Show($"An update has been found, would you like to be taken to the download page? ({Latest.TagName})",
                        "Tanji - Alert!", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        Process.Start(Latest.HtmlUrl);
                    }
                }
            }
        }
        private void GrabbedHashes(Task<IReadOnlyList<RepositoryContent>> getHashFileTask)
        {
            RepositoryContent file = getHashFileTask.Result.FirstOrDefault();
            using (var sha1 = new SHA1Managed())
            {
                // TODO: How would local hash changes be handled when attempting to look for 'latest' changes
            }
        }

        private void ArachisBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/ArachisH");
        }
        private void SpeaqerBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/Speaqer");
        }
        private void DarkStarBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/scottstamp");
        }

        private void DonateBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=TDKPVTSNXJHYY&source=Tanji");
        }
        private void HarbleDiscordBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/wpfcBQF");
        }

        private void SelloutBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/sirjonasxx/G-Earth");
        }
        private void Sellout2Btn_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/Niewiarowski/HabboInterceptor");
        }

        private void SNGButton_Click(object sender, EventArgs e)
        {
            Process.Start("https://sngforum.info/");
        }
        private void DarkboxBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.Darkbox.nl/");
        }
    }
}