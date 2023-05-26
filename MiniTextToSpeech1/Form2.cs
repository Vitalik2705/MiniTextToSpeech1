using System;
using System.Drawing;
using System.Windows.Forms;

namespace MiniTextToSpeech1
{
    public partial class CustomMessageBoxForm : Form
    {
        public string MessageText
        {
            get { return labelMessage.Text; }
            set { labelMessage.Text = value; }
        }

        public Color MessageForeColor
        {
            get { return labelMessage.ForeColor; }
            set { labelMessage.ForeColor = value; }
        }

        public string WindowTitle
        {
            get { return this.Text; }
            set { this.Text = value; }
        }

        public int FormWidth { get; set; }
        public int FormHeight { get; set; }

        public CustomMessageBoxForm()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.BackColor = Color.LightGray;
            this.Padding = new Padding(10);
            WindowTitle = "Повідомлення";
            FormWidth = 400;
            FormHeight = 300;
            this.Size = new Size(FormWidth, FormHeight);


            labelMessage.Font = new Font("Arial", 12, FontStyle.Regular);
            labelMessage.Margin = new Padding(0, 0, 0, 0);

            buttonOK.ForeColor = Color.Black;
            buttonOK.FlatStyle = FlatStyle.Flat;
            buttonOK.FlatAppearance.BorderSize = 0;
            buttonOK.Font = new Font("Arial", 10, FontStyle.Bold);

            buttonOK.Click += ButtonOK_Click;
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

