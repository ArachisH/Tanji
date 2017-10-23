using System;
using System.Windows.Forms;
using System.Threading.Tasks;

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
            LocalVersion = new Version(Application.ProductVersion);

            UI.Shown += UI_Shown;
            UI.TanjiVersionTxt.Text = ("v" + LocalVersion);
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