using System;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using Tanji.Components;

namespace Tanji.Applications.Dialogs
{
    public partial class FindDialog : TanjiForm
    {
        private RichTextBoxFinds _findOptions;

        private readonly RichTextBox _logger;
        private readonly Point _topLeft, _bottomRight;

        public int TopLine
        {
            get { return _logger.GetLineFromCharIndex(TopLeftCharIndex); }
        }
        public int BottomLine
        {
            get { return _logger.GetLineFromCharIndex(BottomRightCharIndex); }
        }
        public int CurrentLine
        {
            get { return _logger.GetLineFromCharIndex(_logger.GetFirstCharIndexOfCurrentLine()); }
        }
        public int TopLeftCharIndex
        {
            get { return _logger.GetCharIndexFromPosition(_topLeft); }
        }
        public int BottomRightCharIndex
        {
            get { return _logger.GetCharIndexFromPosition(_bottomRight); }
        }
        public int VisibleLines => (BottomLine - TopLine);

        private bool _matchCase = false;
        public bool MatchCase
        {
            get { return _matchCase; }
            set
            {
                _matchCase = value;
                RaiseOnPropertyChanged(nameof(MatchCase));
                ModifyOptions(value, RichTextBoxFinds.MatchCase);
            }
        }

        private bool _matchWord = false;
        public bool MatchWord
        {
            get { return _matchWord; }
            set
            {
                _matchWord = value;
                RaiseOnPropertyChanged(nameof(MatchWord));
                ModifyOptions(value, RichTextBoxFinds.WholeWord);
            }
        }

        private bool _isDirectionUp = false;
        public bool IsDirectionUp
        {
            get { return _isDirectionUp; }
            set
            {
                _isDirectionUp = value;
                RaiseOnPropertyChanged(nameof(IsDirectionUp));
                ModifyOptions(value, RichTextBoxFinds.Reverse);
            }
        }

        private bool _wrapAround = true;
        public bool WrapAround
        {
            get { return _wrapAround; }
            set
            {
                _wrapAround = value;
                RaiseOnPropertyChanged(nameof(WrapAround));
            }
        }

        private bool _isNormalMode = true;
        public bool IsNormalMode
        {
            get { return _isNormalMode; }
            set
            {
                _isNormalMode = value;
                RaiseOnPropertyChanged(nameof(IsNormalMode));
            }
        }

        private string _findWhat = string.Empty;
        public string FindWhat
        {
            get { return _findWhat; }
            set
            {
                _findWhat = value;
                RaiseOnPropertyChanged(nameof(FindWhat));
            }
        }

        public FindDialog(RichTextBox richTextBox)
        {
            _logger = richTextBox;
            _topLeft = new Point(1, 1);
            _findOptions = RichTextBoxFinds.NoHighlight;
            _bottomRight = new Point(1, _logger.Height - 1);

            InitializeComponent();

            FindWhatTxt.DataBindings.Add("Text", this,
                nameof(FindWhat), false, DataSourceUpdateMode.OnPropertyChanged);

            NormalRd.DataBindings.Add("Checked", this,
                nameof(IsNormalMode), false, DataSourceUpdateMode.OnPropertyChanged);

            WrapAroundChckbx.DataBindings.Add("Checked", this,
                nameof(WrapAround), false, DataSourceUpdateMode.OnPropertyChanged);

            MatchCaseChckbx.DataBindings.Add("Checked", this,
                nameof(MatchCase), false, DataSourceUpdateMode.OnPropertyChanged);

            MatchWordChckbx.DataBindings.Add("Checked", this,
                nameof(MatchWord), false, DataSourceUpdateMode.OnPropertyChanged);

            UpRd.DataBindings.Add("Checked", this,
                nameof(IsDirectionUp), false, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void FindTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                FindNextBtn_Click(sender, e);
            }
        }
        private void FindNextBtn_Click(object sender, EventArgs e)
        {
            string value = FindWhat;
            int foundIndex = Find(ref value);
            if (foundIndex == -1)
            {
                MessageBox.Show($"Cannot find \"{value}\".",
                    "Tanji ~ Alert!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else if (foundIndex != _logger.SelectionStart)
            {
                int middleOffset = (VisibleLines / 2);
                int foundOnLine = _logger.GetLineFromCharIndex(foundIndex);
                if (foundOnLine >= BottomLine || foundOnLine <= TopLine)
                {
                    if (IsDirectionUp)
                        middleOffset *= -1;

                    int newBottomLine = (foundOnLine + middleOffset);
                    if (newBottomLine < 0) newBottomLine = 0;

                    int borderLineIndex = _logger.GetFirstCharIndexFromLine(newBottomLine);
                    if (borderLineIndex != -1)
                    {
                        _logger.SelectionStart = borderLineIndex;
                        _logger.SelectionLength = 0;
                    }
                }
                _logger.SelectionStart = foundIndex;
            }

            if (_logger.SelectionLength != value.Length)
                _logger.SelectionLength = value.Length;
        }

        public int Find(ref string value)
        {
            int end = _logger.TextLength;
            int start = (_logger.SelectionStart);
            if (!IsDirectionUp) start += _logger.SelectionLength;
            else if (IsNormalMode)
            {
                end = start;
                start = 0;
            }

            int index = Find(ref value, start, end);
            if (index == -1 && WrapAround)
            {
                start = (!IsNormalMode && IsDirectionUp ? _logger.TextLength : 0);
                end = (IsDirectionUp ? 0 : _logger.TextLength);
                index = Find(ref value, start, end);
            }
            return index;
        }
        protected int Find(ref string value, int start, int end)
        {
            if (!WrapAround && start == 0 && end == 0)
                return -1;

            if (!IsNormalMode)
            {
                var expression = new Regex(value, GetRegExOptions());
                Match match = expression.Match(_logger.Text, start);
                if (match.Success)
                {
                    value = match.Value;
                    return match.Index;
                }
                else return -1;
            }

            return _logger.Find(
                value, start, end, _findOptions);
        }

        private RegexOptions GetRegExOptions()
        {
            var options = RegexOptions.None;

            if (!MatchCase)
                options |= RegexOptions.IgnoreCase;

            if (IsDirectionUp)
                options |= RegexOptions.RightToLeft;

            options |= RegexOptions.Singleline;
            return options;
        }
        private void ModifyOptions(bool isFlagPresent, RichTextBoxFinds option)
        {
            if (isFlagPresent)
                _findOptions |= option;
            else
                _findOptions &= ~option;
        }

        protected override void OnLoad(EventArgs e)
        {
            CenterToParent();
            FindWhatTxt.Focus();

            base.OnLoad(e);
        }
    }
}