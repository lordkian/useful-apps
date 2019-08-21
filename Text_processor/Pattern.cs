using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Text_processor
{
    public class Pattern
    {
        Regex Regex1 = new Regex(@"<[^<>]+>");
        private TextBox outputTextBox;
        private TextBox inputTextBox;

        public TextBox InputTextBox
        {
            get => inputTextBox; set
            {
                value.Leave += (s, e) => { SetSubPatterns(); };
                inputTextBox = value;
            }
        }
        public TextBox OutputTextBox
        {
            get => outputTextBox; set
            {
                value.Leave += (s, e) => { SetSubPatterns(); };
                outputTextBox = value;
            }
        }
        public Button SettingButton { get; set; }
        public Label Label { get; set; }
        public List<SubPattern> SubPatterns { get; set; } = new List<SubPattern>();
        public string Input
        {
            get { return InputTextBox.Text; }
            set { InputTextBox.Text = value; }
        }
        public string Output
        {
            get { return OutputTextBox.Text; }
            set { OutputTextBox.Text = value; }
        }
        public void SetSubPatterns()
        {
            SubPatterns.Clear();
            foreach (Match item in Regex1.Matches(Input))
                SubPatterns.Add(new SubPattern() { Value = item.Value });
            var str = "";
            foreach (var item in SubPatterns)
                str += $"{item}  ";
            Label.Text = str;
        }
    }
}
