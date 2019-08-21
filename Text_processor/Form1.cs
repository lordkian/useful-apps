using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Text_processor
{
    public partial class Form1 : Form
    {
        public List<Pattern> Patterns { get; set; } = new List<Pattern>();
        public Form1()
        {
            InitializeComponent();
            AddPattern(GetNewPattern());
            AddPattern(GetNewPattern());
        }
        public Pattern GetNewPattern()
        {
            var top = 20 + Patterns.Count * 26;
            var pattern = new Pattern()
            {
                SettingButton = new Button() { Height = 20, Width = 20, Top = top, Left = 7, Text = "*" },
                InputTextBox = new TextBox() { Height = 20, Width = 220, Top = top, Left = 33 },
                OutputTextBox = new TextBox() { Height = 20, Width = 220, Top = top, Left = 259 },
                Label = new Label() { Height = 20, Top = top, Left = 485, Font = new Font("Microsoft Sans Serif", 12), AutoSize = true }
            };
            return pattern;
        }
        public void AddPattern(Pattern pattern)
        {
          //  pattern.SettingButton.Click -= (sender, e) => { new PatternEditor(pattern, this).Show(); Enabled = false; };
            pattern.SettingButton.Click += (sender, e) => { new PatternEditor(pattern, this).Show(); Enabled = false; };
            groupBox2.Controls.Add(pattern.SettingButton);
            groupBox2.Controls.Add(pattern.OutputTextBox);
            groupBox2.Controls.Add(pattern.InputTextBox);
            groupBox2.Controls.Add(pattern.Label);
            Patterns.Add(pattern);
        }
        public void RemovePattern(Pattern pattern)
        {
            groupBox2.Controls.Remove(pattern.SettingButton);
            groupBox2.Controls.Remove(pattern.OutputTextBox);
            groupBox2.Controls.Remove(pattern.InputTextBox);
            groupBox2.Controls.Remove(pattern.Label);
            Patterns.Remove(pattern);
            pattern.SettingButton.Dispose();
            pattern.InputTextBox.Dispose();
            pattern.OutputTextBox.Dispose();
            pattern.Label.Dispose();
        }
        public void RemovePattern(Pattern pattern, bool fullRemove)
        {
            if (fullRemove)
                RemovePattern(pattern);
            else
                Patterns.Remove(pattern);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            AddPattern(GetNewPattern());
        }

        private void Button5_Click(object sender, EventArgs e)
        {

        }
    }
}
