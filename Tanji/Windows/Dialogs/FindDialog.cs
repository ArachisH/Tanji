using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Text.RegularExpressions;

using Tanji.Controls;

namespace Tanji.Windows.Dialogs
{
    [DesignerCategory("Form")]
    public partial class FindDialog : ObservableForm
    {
        private RegexOptions _regexFlags;
        private RichTextBoxFinds _findFlags;

        private readonly RichLogBox _log;
        private readonly Point _topLeft, _bottomRight;

        #region Bindable Properties
        private bool _isMatchingCase = false;
        public bool IsMatchingCase
        {
            get => _isMatchingCase;
            set
            {
                _isMatchingCase = value;
                RaiseOnPropertyChanged();
                UpdateFlags(ref _regexFlags, RegexOptions.IgnoreCase, !value);
                UpdateFlags(ref _findFlags, RichTextBoxFinds.MatchCase, value);
            }
        }

        private bool _isMatchingWord = false;
        public bool IsMatchingWord
        {
            get => _isMatchingWord;
            set
            {
                _isMatchingWord = value;
                RaiseOnPropertyChanged();
                UpdateFlags(ref _findFlags, RichTextBoxFinds.WholeWord, value);
            }
        }

        private bool _isDirectionUp = false;
        public bool IsDirectionUp
        {
            get => _isDirectionUp;
            set
            {
                _isDirectionUp = value;
                RaiseOnPropertyChanged();
                UpdateFlags(ref _findFlags, RichTextBoxFinds.Reverse, value);
                UpdateFlags(ref _regexFlags, RegexOptions.RightToLeft, value);
            }
        }

        private bool _isWrappingAround = true;
        public bool IsWrappingAround
        {
            get => _isWrappingAround;
            set
            {
                _isWrappingAround = value;
                RaiseOnPropertyChanged();
            }
        }

        private bool _isNormalSearchMode = true;
        public bool IsNormalSearchMode
        {
            get => _isNormalSearchMode;
            set
            {
                _isNormalSearchMode = value;
                RaiseOnPropertyChanged();
            }
        }

        private string _input = null;
        public string Input
        {
            get => _input;
            set
            {
                _input = value;
                RaiseOnPropertyChanged();
            }
        }
        #endregion

        public int VisibleLines => (BottomLine - TopLine);
        public int TopLine => _log.GetLineFromCharIndex(TopLeftCharIndex);
        public int TopLeftCharIndex => _log.GetCharIndexFromPosition(_topLeft);
        public int BottomLine => _log.GetLineFromCharIndex(BottomRightCharIndex);
        public int BottomRightCharIndex => _log.GetCharIndexFromPosition(_bottomRight);
        public int CurrentLine => _log.GetLineFromCharIndex(_log.GetFirstCharIndexOfCurrentLine());

        public FindDialog(RichLogBox log)
        {
            _log = log;
            _topLeft = new Point(1, 1);
            _regexFlags = RegexOptions.Singleline;
            _findFlags = RichTextBoxFinds.NoHighlight;
            _bottomRight = new Point(1, log.Height - 1);

            InitializeComponent();

            Bind(FindWhatTxt, "Text", nameof(Input));
            Bind(UpRd, "Checked", nameof(IsDirectionUp));
            Bind(NormalRd, "Checked", nameof(IsNormalSearchMode));
            Bind(MatchCaseChbx, "Checked", nameof(IsMatchingCase));
            Bind(MatchWordChbx, "Checked", nameof(IsMatchingWord));
            Bind(WrapAroundChbx, "Checked", nameof(IsWrappingAround));
        }

        private void FindBtn_Click(object sender, EventArgs e)
        {
            string value = Input;
            int foundIndex = Find(ref value);
            if (foundIndex == -1)
            {
                MessageBox.Show($"Unable to find \"{value}\".",
                    "Tanji ~ Alert!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else if (foundIndex != _log.SelectionStart)
            {
                int middleOffset = (VisibleLines / 2);
                int foundOnLine = _log.GetLineFromCharIndex(foundIndex);
                if (foundOnLine >= BottomLine || foundOnLine <= TopLine)
                {
                    if (IsDirectionUp)
                    {
                        middleOffset *= -1;
                    }
                    int newBottomLine = (foundOnLine + middleOffset);
                    if (newBottomLine < 0) newBottomLine = 0;

                    int borderLineIndex = _log.GetFirstCharIndexFromLine(newBottomLine);
                    if (borderLineIndex != -1)
                    {
                        _log.SelectionStart = borderLineIndex;
                        _log.SelectionLength = 0;
                    }
                }
                _log.SelectionStart = foundIndex;
            }
            if (_log.SelectionLength != value.Length)
            {
                _log.SelectionLength = value.Length;
            }
        }
        private void FindWhatTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            e.SuppressKeyPress = true;
            FindBtn_Click(sender, e);
        }

        private int Find(ref string value)
        {
            int end = _log.TextLength;
            int start = (_log.SelectionStart);
            if (!IsDirectionUp) start += _log.SelectionLength;
            else if (IsNormalSearchMode)
            {
                end = start;
                start = 0;
            }

            int index = Find(ref value, start, end);
            if (index == -1 && IsWrappingAround)
            {
                start = (!IsNormalSearchMode && IsDirectionUp ? _log.TextLength : 0);
                end = (IsDirectionUp ? 0 : _log.TextLength);
                index = Find(ref value, start, end);
            }
            return index;
        }
        private int Find(ref string value, int start, int end)
        {
            if (!IsWrappingAround && start == 0 && end == 0)
            {
                return -1;
            }
            if (!IsNormalSearchMode)
            {
                try
                {
                    var expression = new Regex(value, _regexFlags);
                    Match match = expression.Match(_log.Text, start);
                    if (match.Success)
                    {
                        value = match.Value;
                        return match.Index;
                    }
                    else return -1;
                }
                catch { return -1; }
            }
            return _log.Find(value, start, end, _findFlags);
        }
        private void UpdateFlags<T>(ref T flags, T flag, bool isPresent) where T : struct, IConvertible
        {
            int iFlags = flags.ToInt32(null);
            if (isPresent)
            {
                iFlags |= flag.ToInt32(null);
            }
            else iFlags &= ~flag.ToInt32(null);

            flags = (T)Enum.ToObject(typeof(T), iFlags);
        }
    }
}