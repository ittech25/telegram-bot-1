using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Telegram.Bot;
using Telegram;

//This Bot Is Developed By Enerds.io

namespace EnerdsTelegramBot
{
    public partial class Form1 : Form
    {
        static string Token = "";
        private Thread botThread;
        Telegram.Bot.TelegramBotClient bot;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Token = txtToken.Text;
            botThread = new Thread(new ThreadStart(runBot));
            botThread.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        void runBot()
        {
            bot = new Telegram.Bot.TelegramBotClient(Token);
            this.Invoke(new Action(() =>
                {
                    lblStatus.Text = "ONLINE";
                    lblStatus.ForeColor = Color.Green;
                }));
            int offSet = 0;

            while (true)
            {
                Telegram.Bot.Types.Update[] update = bot.GetUpdatesAsync(offSet).Result;

                foreach (var up in update)
                {
                    offSet = up.Id + 1;
                    if (up.Message == null)
                        continue;

                    var text = up.Message.Text.ToLower();
                    var from = up.Message.From;
                    var chatId = up.Message.Chat.Id;

                    if (text.Contains("/start"))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(from.Username + "Welcome To our BOT");
                        sb.AppendLine("You can use the commands below");
                        sb.AppendLine("About our BOT : /AboutUs");
                        sb.AppendLine("Contact US : /Contact");
                        sb.AppendLine("Address : /Address");
                        bot.SendTextMessageAsync(chatId, sb.ToString());
                    }
                    // yademan bashad hatman az horof kochak estefade shavad be in elat .tolower estefade shode dar line 61 ***

                    else if (text.Contains("/aboutus"))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("Our Vision is to help streamers to build  theire career with ease");
                        bot.SendTextMessageAsync(chatId, sb.ToString());
                    }
                    else if (text.Contains("/contact"))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("contact us at info@enerds.io");
                        bot.SendTextMessageAsync(chatId, sb.ToString());
                    }
                    else if (text.Contains("/address"))
                    {
                        string website = "https://www.enerds.io";
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine($"Visit our website {website}");
                        bot.SendTextMessageAsync(chatId, sb.ToString());
                    }

                    dgReport.Invoke(new Action(()=>
                        {
                            dgReport.Rows.Add(chatId, from.Username, text, up.Message.MessageId, up.Message.Date.ToString("yyyy/MM/dd - HH:MM"));
                    }));

                }
            }


        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            botThread.Abort();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtToken.PasswordChar = '\0';
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            txtToken.PasswordChar = '*';
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            botThread.Abort();
        }
    }
}
