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

namespace TimerForStudy
{
    public partial class frmTimerForStudy : Form
    {
        private byte _Seconds = 0;
        private byte _Minutes = 0;
        private byte _Hours = 0;

        private string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "HistoryTime.txt");

        public frmTimerForStudy()
        {
            InitializeComponent();

            EnsureFileExists();

            LoadTimeFromFile();
        }

        private void EnsureFileExists()
        {
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "0:0:0"); // Create file with default time
            }
        }

        private void SaveTimeInFile()
        {
            using (StreamWriter sr = new StreamWriter(filePath))
            {
                sr.WriteLine($"{_Hours}:{_Minutes}:{_Seconds}");
            }
        }

       private void LoadTimeFromFile()
       {
            string timeString = File.ReadAllText(filePath);

            if (string.IsNullOrEmpty(timeString))
            {
                _Hours = 0;
                _Minutes = 0;
                _Seconds = 0;
                return;
            }

            string[] timeParts = timeString.Split(':'); 

            _Hours = byte.Parse(timeParts[0]);
            _Minutes = byte.Parse(timeParts[1]);
            _Seconds = byte.Parse(timeParts[2]);

            lbSeconds.Text = _Seconds.ToString("D2");
            lbMinutes.Text = _Minutes.ToString("D2");
            lbHours.Text = _Hours.ToString("D2");

        }
        private void ClearFile()
        {
            using (StreamWriter sr = new StreamWriter(filePath))
            {
                sr.WriteLine($"{_Hours}:{_Minutes}:{_Seconds}");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _Seconds++;

            if (_Seconds == 60)
            {
                _Seconds = 0;
                _Minutes++;

                if (_Minutes == 60)
                {
                    _Minutes = 0;
                    _Hours++;
                }
            }

            lbSeconds.Text = _Seconds.ToString("D2");
            lbMinutes.Text = _Minutes.ToString("D2");
            lbHours.Text = _Hours.ToString("D2");
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (btnStartStop.Text == "Stop")
            {
                timer1.Stop();
                SaveTimeInFile();
                btnStartStop.Text = "Start";
                btnStartStop.ForeColor = Color.White;
                return;
            }
      
            timer1.Start();
            btnStartStop.Text = "Stop";
            btnStartStop.ForeColor = Color.Red;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            btnStartStop.Text = "Start";

            timer1.Stop();

            _Seconds = 0;
            _Minutes = 0;
            _Hours = 0;

            lbSeconds.Text = "00";
            lbMinutes.Text = "00";
            lbHours.Text = "00";

            ClearFile();
        }

        private void frmTimerForStudy_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveTimeInFile();
        }
    }
}
