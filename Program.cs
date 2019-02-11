using System;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Newtonsoft.Json;

using Hudson.Resources.DataType;
using Hudson.Resources.Settings;


namespace Hudson
{

    class Program
    {
        private DiscordSocketClient Client;
        private CommandService Commands;
        public class Token
        {
            public string tokenID { get; set; }
        }
        static void Main(string[] args) 
            => new Program().MainAsync().GetAwaiter().GetResult();


        private async Task MainAsync()
        {
            //Settings file parsing
            string JSON = "";
            string SettingsLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location).Replace(@"bin\Debug\netcoreapp2.1", @"Data\Settings.json");
            if (!File.Exists(SettingsLocation)) {
                Console.WriteLine("No Settings file found");
            }
            using (var Stream = new FileStream(SettingsLocation, FileMode.Open, FileAccess.Read))
                using (var ReadSettings = new StreamReader(Stream))
            {
                JSON = ReadSettings.ReadToEnd();
            }
            Settings Settings = JsonConvert.DeserializeObject<Settings>(JSON);
            ESettings.Log = Settings.log;
            ESettings.Owner = Settings.owner;
            ESettings.Token = Settings.token;
            ESettings.Version = Settings.version;

            //logging
            Client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug
            });

            Commands = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = true,
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Debug
            });

            Client.MessageReceived += Client_MessageRecieved;
            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);

            Client.Ready += Client_Ready;
            Client.Log += Client_Log;

            await Client.LoginAsync(TokenType.Bot, ESettings.Token);
            await Client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task Client_Log(LogMessage Message)
        {
            Console.WriteLine($"{DateTime.Now} at {Message.Source}] {Message.Message}");
        }

        private async Task Client_Ready()
        {
            await Client.SetGameAsync("Counting Gio's Fuck Ups");
        }

        private async Task Client_MessageRecieved(SocketMessage MessageParam)
        {
            var Message = MessageParam as SocketUserMessage;
            var Context = new SocketCommandContext(Client, Message);

            if (Context.Message == null || Context.Message.Content == "") return;
            if (Context.User.IsBot) return;

            int ArgPos = 0;

            if (!(Message.HasStringPrefix(".", ref ArgPos) || Message.HasMentionPrefix(Client.CurrentUser, ref ArgPos))) return;

            var Result = await Commands.ExecuteAsync(Context, ArgPos, null);
            if (!Result.IsSuccess)
            {
                Console.WriteLine($"{DateTime.Now} at Commands] Something went wrong with executing a command. Text {Context.Message.Content} | Error: {Result.ErrorReason}");
            }
        }
    }
}
