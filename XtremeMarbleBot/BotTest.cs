using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace XtremeMarbleBot
{
    class BotTest
    {
        public string botName;
        public string channelName;
        public string authToken;
        public DateTime stopTime;

        public BotTest(string botName, string channelName, string authToken, DateTime stopTime)
        {
            this.botName = botName;
            this.channelName = channelName;
            this.authToken = authToken;
            this.stopTime = stopTime;
        }

        public void Start()
        {
            while (DateTime.Now < stopTime)
            {
                Console.WriteLine(channelName);
                using (StreamWriter sr = new StreamWriter(@"C:\users\mcouture\desktop\" + channelName + stopTime.Second.ToString() + ".txt", false))
                {
                    sr.Write(channelName);
                    sr.Close();
                }

            }

            //irc.Close();
        }

    }
}
