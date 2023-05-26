using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace MiniTextToSpeech1
{
    public partial class Form1 : Form
    {
        private bool isPaused = false;
        MediaPlayer player = new MediaPlayer();
        string GetAudioUrl(string Text, string Language)
        {
            return $@"http://translate.google.com/translate_tts?client=tw-ob&q={Text}&tl={Language}";
        }

        public Form1()
        {
            InitializeComponent();

            trackBar1.DataBindings.Add("Value", player, "SpeedRatio");
            player.MediaEnded += player_MediaEnded;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            panel1.Capture = false;
            Message m = Message.Create(base.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            WndProc(ref m);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            button4.BackColor = System.Drawing.Color.FromArgb(255, 110, 110);
        }

        private void button5_MouseEnter(object sender, EventArgs e)
        {
            button5.BackColor = System.Drawing.Color.FromArgb(128 / 2, 255, 128 / 2);
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            button4.BackColor = System.Drawing.Color.Red;
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            button5.BackColor = System.Drawing.Color.Green;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string language = comboBox1.Text.Split(' ')[0];
            if (language != "uk")
            {
                player.Open(new Uri(GetAudioUrl(richTextBox1.Text, comboBox1.Text.Split(' ')[0]), UriKind.RelativeOrAbsolute));
                button3.Image = new Bitmap("C:/Users/Vitalik/Downloads/pause (1).png");
                player.Play();
                timerDuration.Start();
            }
            else
            {
                string audioUrl = GetAudioUrl(richTextBox1.Text, comboBox1.Text.Split(' ')[0]);
                Process.Start(audioUrl);
            }
        }

        private void timerDuration_Tick(object sender, EventArgs e)
        {
            if (player.NaturalDuration.HasTimeSpan)
            {
                TimeSpan currentPosition = player.Position;
                TimeSpan duration = player.NaturalDuration.TimeSpan;

                if (currentPosition <= duration)
                {
                    labelDuration.Text = currentPosition.ToString(@"mm\:ss");
                }
                else
                {
                    labelDuration.Text = duration.ToString(@"mm\:ss");
                    timerDuration.Stop();
                }
            }
        }

        private void player_MediaEnded(object sender, EventArgs e)
        {
            timerDuration.Stop();
            button3.Image = new Bitmap("C:/Users/Vitalik/Downloads/pause (1).png");

            DialogResult result = MessageBox.Show($"Довжина аудіо: {labelDuration.Text}. Бажаєте зберегти файл?", "Повідомлення", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                SaveAudioToFile();
            }
            labelDuration.Text = "00:00";
        }

        private void SaveAudioToFile()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Audio File(*.mp3)|*.mp3";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    new WebClient().DownloadFile(GetAudioUrl(richTextBox1.Text, comboBox1.Text.Split(' ')[0]), saveFileDialog1.FileName);
                    ShowCustomMessageBox("Файл збережено");
                }
                catch (Exception)
                {
                    ShowCustomMessageBoxError("Сталася помилка");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Audio File(*.mp3)|*.mp3";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    new WebClient().DownloadFile(GetAudioUrl(richTextBox1.Text, comboBox1.Text.Split(' ')[0]), saveFileDialog1.FileName);
                    ShowCustomMessageBox("Файл збережено");
                }
                catch (Exception)
                {
                    ShowCustomMessageBoxError("Сталася помилка");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (player.Source != null)
            {
                if (player.CanPause)
                {
                    if (player.Position >= player.NaturalDuration.TimeSpan)
                    {
                        player.Stop();
                    }
                    else if (isPaused)
                    {
                        player.Play();
                        button3.Image = new Bitmap("C:/Users/Vitalik/Downloads/pause (1).png");
                        isPaused = false;
                    }
                    else
                    {
                        player.Pause();
                        button3.Image = new Bitmap("C:/Users/Vitalik/Downloads/play-button-arrowhead.png");
                        isPaused = true;
                    }
                }
                else
                {
                    player.Play();
                    button3.Image = new Bitmap("C:/Users/Vitalik/Downloads/pause (1).png");
                    isPaused = false;
                }
            }
        }


        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            float playbackSpeed = 1.0f;

            if (trackBar1.Value == 1)
            {
                playbackSpeed = 1.0f;
            }
            else if (trackBar1.Value == 2)
            {
                playbackSpeed = 1.01f;
            }
            else if (trackBar1.Value == 3)
            {
                playbackSpeed = 1.02f;
            }

            player.SpeedRatio = playbackSpeed;
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            float volume = trackBar2.Value / 100.0f;
            player.Volume = volume;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            TimeSpan newPosition = player.Position + TimeSpan.FromSeconds(3);
            player.Position = newPosition;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            TimeSpan newPosition = player.Position - TimeSpan.FromSeconds(3);
            if (newPosition < TimeSpan.Zero)
            {
                newPosition = TimeSpan.Zero;
            }
            player.Position = newPosition;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Text Files (*.txt)|*.txt";
            openFileDialog1.Title = "Виберіть текстовий файл";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;

                try
                {
                    string fileText = File.ReadAllText(fileName);
                    richTextBox1.Text = fileText;
                }
                catch (Exception)
                {
                    ShowCustomMessageBoxError("Помилка під час відкриття файлу");
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = string.Empty;
        }

        private void ShowCustomMessageBox(string message)
        {
            using (CustomMessageBoxForm messageBox = new CustomMessageBoxForm())
            {
                messageBox.MessageText = message;
                messageBox.WindowTitle = "Успіх";
                messageBox.ShowDialog();
            }
        }

        private void ShowCustomMessageBoxError(string message)
        {
            CustomMessageBoxForm messageBox = new CustomMessageBoxForm();
            messageBox.MessageText = message;
            messageBox.MessageForeColor = System.Drawing.Color.Red;
            messageBox.WindowTitle = "Помилка";
            messageBox.ShowDialog();
        }
    }
}
