using System;
using System.Windows.Forms;

using Sulakore.Protocol;

namespace Tanji.Pages.Toolbox
{
    public class ToolboxPage : TanjiPage
    {
        public ToolboxPage(MainFrm ui, TabPage tab)
            : base(ui, tab)
        {
            UI.TTDecodeIntBtn.Click += TTDecodeIntBtn_Click;
            UI.TTDecodeUShortBtn.Click += TTDecodeUShortBtn_Click;

            UI.TTIntInputTxt.ValueChanged += TTIntInputTxt_ValueChanged;
            UI.TTUShortInputTxt.ValueChanged += TTUShortInputTxt_ValueChanged;
        }

        public string EncodeInt32(int value)
        {
            return Encode(BigEndian.GetBytes, value);
        }
        public string EncodeUInt16(ushort value)
        {
            return Encode(BigEndian.GetBytes, value);
        }

        public int DecodeInt32(string value)
        {
            return Decode(BigEndian.ToInt32, value);
        }
        public ushort DecodeUInt16(string value)
        {
            return Decode(BigEndian.ToUInt16, value);
        }

        private void TTDecodeIntBtn_Click(object sender, EventArgs e)
        {
            UI.TTIntInputTxt.Value =
                DecodeInt32(UI.TTIntOutputTxt.Text);
        }
        private void TTDecodeUShortBtn_Click(object sender, EventArgs e)
        {
            UI.TTUShortInputTxt.Value =
                DecodeUInt16(UI.TTUShortOutputTxt.Text);
        }

        private void TTIntInputTxt_ValueChanged(object sender, EventArgs e)
        {
            UI.TTIntOutputTxt.Text =
                EncodeInt32((int)UI.TTIntInputTxt.Value);
        }
        private void TTUShortInputTxt_ValueChanged(object sender, EventArgs e)
        {
            UI.TTUShortOutputTxt.Text =
                EncodeUInt16((ushort)UI.TTUShortInputTxt.Value);
        }

        protected virtual string Encode<T>(Func<T, byte[]> encoder, T value)
        {
            return HMessage.ToString(encoder(value));
        }
        protected virtual T Decode<T>(Func<byte[], int, T> decoder, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return default(T);

            byte[] data = HMessage.ToBytes(value);
            return decoder(data, 0);
        }
    }
}