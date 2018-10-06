using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XtremeMarbleBot
{
    class Bot
    {
        public string botName;
        public string channelName;
        public string authToken;
        public DateTime stopTime;

        public Bot(string botName, string channelName, string authToken, DateTime stopTime)
        {
            this.botName = botName;
            this.channelName = channelName;
            this.authToken = authToken;
            this.stopTime = stopTime;
        }

        public void Start()
        {
            DateTime lastPostTime = DateTime.Now.AddMinutes(-5);
            List<string> pastmessages = new List<string>();

            // Initialize and connect to Twitch chat
            IrcClient irc = new IrcClient("irc.twitch.tv", 6667, botName, authToken, channelName);

            // Ping to the server to make sure this bot stays connected to the chat
            // Server will respond back to this bot with a PONG (without quotes):
            // Example: ":tmi.twitch.tv PONG tmi.twitch.tv :irc.twitch.tv"
            PingSender ping = new PingSender(irc);
            ping.Start();

            // Listen to the chat until program exits
            while (DateTime.Now < stopTime)
            {
                //Read any message from the chat room
                string message = irc.ReadMessage();
                //Console.WriteLine(message);

                if (message.Contains("PRIVMSG"))
                {
                    // Message Format: ":[user]![user]@[user].tmi.twitch.tv PRIVMSG #[channel] :[message]"

                    //Parse username and message
                    int intIndexParseSign = message.IndexOf('!');
                    string userName = message.Substring(1, intIndexParseSign - 1);

                    intIndexParseSign = message.IndexOf(" :");
                    message = message.Substring(intIndexParseSign + 2);

                    pastmessages.Add(message);
                    if (pastmessages.Count > 3)
                    {
                        pastmessages.RemoveAt(0);
                    }

                    //check all last 3 messages have been !play
                    if (!pastmessages.Any(m => !m.StartsWith("!play")) && lastPostTime.AddMinutes(2) < DateTime.Now)
                    {
                        //only post once every 2 minutes
                        lastPostTime = DateTime.Now;
                        irc.SendPublicChatMessage("!play");
                    }
                }
            }

            irc.Close();
        }
    }
}
