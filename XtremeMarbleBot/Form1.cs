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

            DateTime stopTime = DateTime.Now.AddMinutes(30);

            while (true)
            {
                if (stopTime < DateTime.Now)
                {
                    stopTime = DateTime.Now.AddMinutes(30);
                    List<string> topChannels = new List<string>();
                    
                    //509511 - marbles twitch id
                    string URL = "https://api.twitch.tv/helix/streams?game_id=509511&first=" + numberOfBots;

                    client.DefaultRequestHeaders.Add("Client-ID", "3mx4acqmwya0a96b67encfry7hqwbr");

                    //send request for top marble streams, serialize as objects
                    var response = await client.GetAsync(URL);
                    string json = response.Content.ReadAsStringAsync().Result;
                    GetStreamsRequest streams = JsonConvert.DeserializeObject<GetStreamsRequest>(json);

                    //get display names for top streams
                    for (int i = 0; i < streams.data.Count; i++)
                    {
                        URL = "https://api.twitch.tv/helix/users?id=" + streams.data[i].user_id;
                        response = await client.GetAsync(URL);
                        json = response.Content.ReadAsStringAsync().Result;

                        GetUserInfoRequest userInfo = JsonConvert.DeserializeObject<GetUserInfoRequest>(json);
                        for (int j = 0; j < userInfo.data.Count; j++)
                        {
                            topChannels.Add(userInfo.data[j].display_name);
                        }
                    }

                    //start bot for each of these streams
                    foreach (var channel in topChannels)
                    {
                        Bot b = new Bot(botName, channel, authToken, stopTime);
                        Thread thread = new Thread(new ThreadStart(b.Start));
                        thread.Start();
                    }

                    progress = new Progress<string>(s => lblMessage.Text = s);
                    await Task.Factory.StartNew(() => UiThread.WriteToInfoLabel(progress, "Bots started."));

                    Thread.Sleep(600000);
                }
                Thread.Sleep(60000);
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
    }
}
