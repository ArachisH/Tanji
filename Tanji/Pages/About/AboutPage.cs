using System;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;

using Tanji.Windows;

namespace Tanji.Pages.About
{
    public class AboutPage : TanjiPage
    {
        public Version LocalVersion { get; }

        public AboutPage(MainFrm ui, TabPage tab)
            : base(ui, tab)
        {
            LocalVersion = new Version(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);
            UI.TanjiVersionTxt.Text = ("v" + LocalVersion.ToString(3));

            UI.ArachisBtn.Click += ArachisBtn_Click;
            UI.SpeaqerBtn.Click += SpeaqerBtn_Click;
            UI.DarkStarBtn.Click += DarkStarBtn_Click;

            UI.DonateBtn.Click += DonateBtn_Click;
            UI.HarbleDiscordBtn.Click += HarbleDiscordBtn_Click;

            UI.SNGButton.Click += SNGButton_Click;
            UI.DarkboxBtn.Click += DarkboxBtn_Click;
            UI.SelloutBtn.Click += SelloutBtn_Click;
        }

        private void ArachisBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/ArachisH");
        }
        private void SpeaqerBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://twitter.com/SpeaqerDev");
        }
        private void DarkStarBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://twitter.com/DarkStar851");
        }

        private void DonateBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://beerpay.io/ArachisH/Tanji");
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