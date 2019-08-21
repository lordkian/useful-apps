using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Text_processor
{
    public partial class PatternEditor : Form
    {
        public Pattern OriginalPattern { get; set; }
        public Pattern NewPattern { get; set; }
        public Form1 Form1 { get; set; }
        public PatternEditor(Pattern pattern, Form1 form1)
        {
            InitializeComponent();
            Form1 = form1;
            OriginalPattern = pattern;
            textBox1.Text = pattern.Input;
            textBox2.Text = pattern.Output;
            NewPattern = new Pattern()
            {
                SettingButton = pattern.SettingButton,
                InputTextBox = pattern.InputTextBox,
                OutputTextBox = pattern.OutputTextBox,
                Label = pattern.Label
            };
            TextBox1_Leave(null, null);
        }

        private void PatternEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1.Enabled = true;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            NewPattern.Input = OriginalPattern.Input;
            NewPattern.Output = OriginalPattern.Output;
            NewPattern.SetSubPatterns();
            Close();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Form1.RemovePattern(OriginalPattern, false);
            NewPattern.SettingButton = new Button() { Height = 20, Width = 20, Top = NewPattern.InputTextBox.Top, Left = 7, Text = "*" };
            Form1.AddPattern(NewPattern);
            NewPattern.SetSubPatterns();
            Close();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Form1.RemovePattern(OriginalPattern, true);
            Close();
        }

        private void TextBox1_Leave(object sender, EventArgs e)
        {
            NewPattern.Input = textBox1.Text;
            NewPattern.SetSubPatterns();
            foreach (Control item in Controls)
                if (item is Label)
                    Controls.Remove(item);
            var i = 0;
            foreach (var item in NewPattern.SubPatterns)
                Controls.Add(new Label() { Font = new Font("Microsoft Sans Serif", 12), Text = item.ToString(), AutoSize = true, Top = 93 + i++ * 26, Left = 7 });
        }

        private void TextBox2_Layout(object sender, LayoutEventArgs e)
        {

        }
    }
}
