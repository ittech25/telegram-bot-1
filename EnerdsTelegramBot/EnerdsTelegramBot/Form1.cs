﻿
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
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Xml;
using System.IO;
//This Bot Is Developed By Enerds.io

namespace EnerdsTelegramBot
{
    public partial class Form1 : Form
    {
        static string Token = "";
        private Thread botThread;
        private Telegram.Bot.TelegramBotClient bot;
        private ReplyKeyboardMarkup mainKeyboardMarkup;


        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.BackColor = Color.Transparent;
            string writerfile = @".\token.txt";
            using (StreamWriter writer = new StreamWriter(writerfile))
            {
                writer.Write(txtToken.Text);

            }
            //

            var fileText = System.IO.File.ReadAllText(@".\token.txt");
            txtToken.Text = fileText;





            Token = txtToken.Text;
            botThread = new Thread(new ThreadStart(runBot));
            botThread.Start();

        }

        private void Form1_Load(object sender, EventArgs e)
        // address site emoji ha : https://apps.timwhitlock.info/emoji/tables/unicode
        {
            //save akhbar to file txt

            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
            timer1.Start();



            //khandane akhbar
            var fileText = System.IO.File.ReadAllText(@".\news.txt");
            textBox1.Text = fileText;
            //

            //Sakht Dokme
            mainKeyboardMarkup = new ReplyKeyboardMarkup();
            KeyboardButton[] row1 =
            {
                new KeyboardButton("آدرس وب سایت ما"+ " " + "\U0001F310"),new KeyboardButton("نظرسنجی"+ " " + "\U0001F4CA"),new KeyboardButton("آخرین اخبار"+ " " + "\U0001F5DE"), new KeyboardButton("وضعیت استریم"+" "+"\U0001F4FA"),
            };


            KeyboardButton[] row2 =
            {
                new KeyboardButton("ارتباط با ما"+ " " + "\U0001F4E7"),
            };


            mainKeyboardMarkup.Keyboard = new KeyboardButton[][]
            {
                row1,row2
            };


            //
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
                        sb.AppendLine("Address : /Website");
                        sb.AppendLine("News : /News");
                        sb.AppendLine("Stream Status : /StreamStatus");
                        bot.SendTextMessageAsync(chatId, sb.ToString(), ParseMode.Default, false, false, 0, mainKeyboardMarkup);
                    }
                    // yademan bashad hatman az horof kochak estefade shavad be in elat .tolower estefade shode dar line 61 ***

                    else if (text.Contains("/aboutus") || text.Contains("درباره ما"))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("Our Vision is to help streamers to build  theire career with ease");
                        bot.SendTextMessageAsync(chatId, sb.ToString());
                    }
                    else if (text.Contains("/contact") || text.Contains("ارتباط با ما"))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("contact us at info@enerds.io");


                        ReplyKeyboardMarkup contactKeyboardMarkup = new ReplyKeyboardMarkup();
                        KeyboardButton[] row1 =
                        {
                            new KeyboardButton("تماس با مدیریت"+ " "+ "\U0001F454") , new KeyboardButton("تماس با پشتیبانی"+" "+"\U0001F477"), new KeyboardButton("تماس با استریمر"+" "+"\U0000270C"),

                        };
                        KeyboardButton[] row2 =
                        {
                            new KeyboardButton("بازگشت"+" "+ "\U0001F519")
                        };

                        contactKeyboardMarkup.Keyboard = new KeyboardButton[][]
                        {
                            row1,row2
                        };

                        bot.SendTextMessageAsync(chatId, sb.ToString(), ParseMode.Default, false, false, 0, contactKeyboardMarkup);
                    }
                    else if (text.Contains("بازگشت"))
                    {
                        bot.SendTextMessageAsync(chatId, "بازگشت به منو اصلی", ParseMode.Default, false, false, 0, mainKeyboardMarkup);
                    }
                    else if (text.Contains("/website") || text.Contains("آدرس وب سایت ما"))
                    {
                        string website = "https://www.alibesi.tv";
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine($"Visit our website {website}");
                        bot.SendTextMessageAsync(chatId, sb.ToString());
                    }



                    else if (text.Contains("/news") || text.Contains("آخرین اخبار"))
                    {
                        string news = textBox1.Text;
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine($"آخرین اخبار : {news}");
                        bot.SendTextMessageAsync(chatId, sb.ToString());

                    }

                    else if (text.Contains("/streamstatus") || text.Contains("وضعیت استریم"))
                    {
                        StringBuilder sb = new StringBuilder();
                        string twitch = "https://www.twitch.tv/alibesi";
                        string website = "https://www.alibesi.tv";
                        if (rdoBtn1.Checked)
                        {
                            sb.AppendLine($"استریم در حال پخش می باشد برای تماشا به آدرس{twitch} مراجه نمایید ");
                            bot.SendTextMessageAsync(chatId, sb.ToString());
                        }
                        else if (rdoBtn2.Checked)
                        {
                            sb.AppendLine($"استریم در حال حاضر آفلاین می باشد برای اطلاع از آخرین اخبار به {website} مراجعه نمایید.");
                            bot.SendTextMessageAsync(chatId, sb.ToString());
                        }

                    }

                    dgReport.Invoke(new Action(() =>
                    {
                        dgReport.Rows.Add(chatId, from.Username, text, up.Message.MessageId, up.Message.Date.ToString("yyyy/MM/dd - HH:MM"));
                    }));
                }



            }

        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //save akhbar to file txt
            string writerfile = @".\news.txt";
            using (StreamWriter writer = new StreamWriter(writerfile))
            {
                writer.Write(textBox1.Text);

            }
            //


            if (lblStatus.Text == "ONLINE" || e.Cancel == true)
            {
                try
                {
                    // Do not initialize this variable here.
                    botThread.Abort();
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }


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
            if (lblStatus.Text == "ONLINE")
            {
                try
                {
                    // Do not initialize this variable here.
                    botThread.Abort();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            lblStatus.Text = "OFFLINE";
            lblStatus.ForeColor = Color.Red;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void lbl1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.enerds.io");
        }

        
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.alibesi.tv");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblTime_Click(object sender, EventArgs e)
        {

            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
            timer1.Start();
        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }







}

