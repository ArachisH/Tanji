using System;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;

using Tanji.Windows;
using Tangine.GitHub;

namespace Tanji.Pages.About
{
    public class AboutPage : TanjiPage
    {
        public GitRepository TanjiRepo { get; }

        public Version LocalVersion { get; }
        public Version LatestVersion { get; private set; }

        public AboutPage(MainFrm ui, TabPage tab)
            : base(ui, tab)
        {
            TanjiRepo = new GitRepository("ArachisH", "Tanji");
            LocalVersion = Assembly.GetExecutingAssembly().GetName().Version;

            UI.Shown += UI_Shown;
            UI.TanjiVersionTxt.Text = ("v" + LocalVersion);

            UI.ArachisBtn.Click += ArachisBtn_Click;
            UI.DarkStarBtn.Click += DarkStarBtn_Click;
            UI.SpeaqerBtn.Click += SpeaqerBtn_Click;

            UI.DonateBtn.Click += DonateBtn_Click;

            UI.HarbleBtn.Click += HarbleBtn_Click;
            UI.H4BB0Btn.Click += H4BB0Btn_Click;

            UI.DarkboxBtn.Click += DarkboxBtn_Click;
            UI.SNGButton.Click += SNGButton_Click;
            UI.ForbiddenBtn.Click += ForbiddenBtn_Click;
        }

        private void ArachisBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://twitter.com/ArachisH");
        }
        private void DarkStarBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://twitter.com/DarkStar851");
        }
        private void SpeaqerBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://twitter.com/SpeaqerDev");
        }

        private void DonateBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=HMYZ4GB5N2PAU");
        }

        private void HarbleBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/Vyc2gFC");
        }
        private void H4BB0Btn_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/v2e6GdJ");
        }

        private void DarkboxBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.Darkbox.nl/");
        }
        private void SNGButton_Click(object sender, EventArgs e)
        {
            Process.Start("https://sngforum.info/");
        }
        private void ForbiddenBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://forbidden.sh");
        }

        private async void UI_Shown(object sender, EventArgs e)
        {
            UI.Shown -= UI_Shown;
            await Task.Delay(225);

            GitRelease latestRelease =
                await TanjiRepo.GetLatestReleaseAsync();

            if (latestRelease != null)
            {
                LatestVersion = new Version(
                    latestRelease.TagName.Substring(1));

                UI.TanjiVersionTxt.IsLink = true;

                if (LatestVersion > LocalVersion &&
                   !latestRelease.IsPrerelease)
                {
                    UI.TanjiVersionTxt.Text = "Update Found!";
                }
            }
        }
    }
}