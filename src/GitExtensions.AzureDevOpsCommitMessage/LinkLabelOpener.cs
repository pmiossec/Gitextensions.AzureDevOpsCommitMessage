using System;
using System.Diagnostics;
using System.Windows.Forms;
using ResourceManager;

namespace GitExtensions.AzureDevOpsCommitMessage
{
    public class LinkLabelOpener : LinkLabel
    {
        private static readonly TranslationString Error = new TranslationString("Error");
        private static readonly TranslationString LinkInvalid = new TranslationString("The link to open is invalid");
        private static readonly TranslationString OpenLinkFailed = new TranslationString("Fail to open the link:\n{0}\n\nCopy url to clipboard?");
        public LinkLabelOpener()
        {
            Click += LinkLabelOpener_Click;
        }

        private void LinkLabelOpener_Click(object sender, EventArgs e)
        {
            OpenLink();
        }

        public void OpenLink()
        {
            if (Tag == null || !(Tag is string url) || string.IsNullOrWhiteSpace(url))
            {
                MessageBox.Show(LinkInvalid.Text, Error.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                OpenUrl(url);
            }
            catch (Exception)
            {
                if (MessageBox.Show(string.Format(OpenLinkFailed.Text, url), Error.Text, MessageBoxButtons.YesNo,
                        MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    Clipboard.SetText(url);
                }
            }
        }

        public static void OpenUrl(string url)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
    }
}
