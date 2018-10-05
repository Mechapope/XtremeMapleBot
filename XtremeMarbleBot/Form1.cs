using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XtremeMarbleBot
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient client = new HttpClient();
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            //Start
            Progress<string> progress = new Progress<string>(s => lblMessage.Text = s);
            await Task.Factory.StartNew(() => UiThread.WriteToInfoLabel(progress, "Starting bots"));

            string botName = txtBotName.Text.ToLower();
            string authToken = txtAuthToken.Text.ToLower();            
            int numberOfBots = (int)txtNumBots.Value;

            DateTime stopTime = DateTime.Now.AddSeconds(5);

            while (true)
            {
                if (stopTime < DateTime.Now)
                {
                    List<string> topChannels = new List<string>();
                    stopTime = DateTime.Now.AddSeconds(30);
                    //string URL = "https://api.twitch.tv/helix/streams?gameid=*marbleid*&first=" + numberOfBots;

                    //var response = await client.GetAsync(URL);

                    //response.Headers.Add("Authorization", "OAuth " + authToken);

                    ////parse channel id
                    //response.ToString();

                    string json = "";


                    RootObject data = JsonConvert.DeserializeObject<RootObject>(json);

                    for (int i = 0; i < data.data.Count; i++)
                    {
                        //URL = "https://api.twitch.tv/helix/users?id=" + data.data[i].id;
                        //response = await client.GetAsync(URL);

                        RootObject2 data2 = JsonConvert.DeserializeObject<RootObject2>(json);
                        for (int j = 0; j < data2.data.Count; j++)
                        {
                            topChannels.Add(data2.data[j].display_name);
                        }
                    }

                    foreach (var item in topChannels)
                    {
                        BotTest b = new BotTest(botName, item, authToken, stopTime);
                        Thread thread = new Thread(new ThreadStart(b.Start));
                        thread.Start();
                    }
                }

            }
            
        }
    }

    class UiThread
    {
        public static void WriteToInfoLabel(IProgress<string> progress, string text)
        {
            //why is this necessary
            //without the loop it doesnt run async
            for (var i = 0; i < 5; i++)
            {
                Task.Delay(10).Wait();
                progress.Report(text);
            }
        }

        public static void ChangeStartButton(IProgress<string> progress, string text)
        {
            //why is this necessary
            //without the loop it doesnt run async
            for (var i = 0; i < 5; i++)
            {
                Task.Delay(10).Wait();
                progress.Report(text);
            }
        }
    }
}
