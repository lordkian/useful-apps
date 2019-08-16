using System;
using System.Windows.Forms;
using Empty_File_Genarator;

namespace Empty_File_Genarator_WinForm
{
    public partial class Form1 : Form
    {
        Genarator Genarator = new Genarator(0, 0) { Message = (str) => { MessageBox.Show(str); } };
        public Form1()
        {
            InitializeComponent();
            comboBox1.DataSource = Enum.GetValues(typeof(FileSizeUnit));
            comboBox1.SelectedIndex = 3;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog() { SelectedPath = textBox1.Text };
            dialog.ShowDialog();
            if (textBox1.Text != dialog.SelectedPath)
                textBox1.Text = dialog.SelectedPath;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            int count = 1;
            if (radioButton1.Checked)
                count = -1;
            else if (radioButton2.Checked)
                count = (int)numericUpDown1.Value;
            Enabled = false;
            FileSizeUnit fileSizeUnit;
            Enum.TryParse(comboBox1.Text, out fileSizeUnit);
            Genarator.Path = textBox1.Text;
            Genarator.IntegerPart = (int)numericUpDown2.Value;
            Genarator.DoublePart = int.Parse(numericUpDown2.Value.ToString().Replace(Genarator.IntegerPart + ".", ""));
            Genarator.FileGen(count, fileSizeUnit, MakeEnable);
        }
        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown1.Enabled = radioButton2.Checked;
        }
        private void MakeEnable()
        {
            if (InvokeRequired)
                Invoke(new Action(MakeEnable));
            else
                Enabled = true;
        }
    }
}
