using System;
using Telegram.Bot;
using System.Text.RegularExpressions;
using System.Threading;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RequestBot
{
    class Program
    {
        // here you place your telegram token
        private static readonly TelegramBotClient bot = new TelegramBotClient("YOUR-TOKEN-GOES-HERE");
        // replace with the chat you want the bot to work on
        private static long chat = 123; // you can type your own id if you want the bot to message you with the link
        // replace with the chat you want the bot to send the gsi messages to
        private static long chat_send = 321;
        static void Main(string[] args)
        {
            bot.OnMessage += request;
            bot.StartReceiving();
            Console.WriteLine($"bot ({bot.BotId}) has started");
            bot.SendTextMessageAsync(chat,
                "Bot has started!\nyou can use this bot by typing /request <url> <more info>\nfor example:\n/request https://pilotfiber.dl.sourceforge.net/project/havoc-os//tulip/Havoc-OS-v3.12-20210103-tulip-Official.zip don't resign this rom",
                disableWebPagePreview:true);
            Thread.Sleep(-1);
            bot.StopReceiving();
        }

        static async void request(object sender, Telegram.Bot.Args.MessageEventArgs message)
        {
            try
            {
                Message msg = message.Message;
                string MessageText = msg.Text.Replace("/request", "");
                if (msg.Text.StartsWith("/request"))
            {
                if (msg.Chat.Id != chat)
                    {
                        var chat_name = await bot.GetChatAsync(chat);
                        await bot.SendTextMessageAsync(msg.Chat.Id,
                            $"<b>Hello {msg.From.FirstName} (<code>{msg.From.Id}</code>) you can only use this bot in <code>{chat_name.Title}</code> group</b>",
                            parseMode: ParseMode.Html);
                        return;
                    }
                    // work around url
                    string url = "";
                    for (int c = 1; c < 200; c++)
                    {
                        if (c >= MessageText.Length) break;
                        var e = MessageText[c];
                        url += e;
                        if (e.ToString() == " " || e.ToString() == "") break;
                    }
                    Console.WriteLine(url);
                    var info = MessageText.Replace(url.ToString(), "");
                    if (info == "" || info == " ") info = "The user did not provide any additional info..."; 
                    var match = Regex.Match(msg.Text, "(http:\\/\\/|https:\\/\\/)?[a-z0-9]+([\\-\\.]{1}[a-z0-9]+)*\\.[a-z]{2,5}(:[0-9]{1,5})?(\\/.*)?",
                        RegexOptions.IgnoreCase);
                    if (!match.Success)
                    {
                        await bot.DeleteMessageAsync(msg.Chat.Id, msg.MessageId);
                        await bot.SendTextMessageAsync(msg.Chat.Id,
                            $"<b>Hello @{msg.From.Username} (<code>{msg.From.Id}</code>) Your link appeares to be invalid!\nplease make sure to use the correct link format</b>",
                            parseMode:ParseMode.Html);
                        return;
                    }

                    await bot.DeleteMessageAsync(msg.Chat.Id, msg.MessageId);
                    await bot.SendTextMessageAsync(msg.Chat.Id,
                        $"Hello @{msg.From.Username} ({msg.From.Id})\n Your GSI link has been sent! please wait for it to be made soon!");
                    string last_name = string.IsNullOrEmpty(msg.From.LastName) ? "null" : msg.From.LastName;
                    string language_code = string.IsNullOrEmpty(msg.From.LanguageCode) ? "null" : msg.From.LanguageCode;
                    await bot.SendTextMessageAsync(chat_send,
                        $"New link received\\!\n\n📎URL:\n`{url}`\n\nMore info:\n`{info}`\n\nUserInfo:\n`•first_name: {msg.From.FirstName}\n•last_name: {last_name}\n•username: @{msg.From.Username}\n•id: {msg.From.Id}\n•language_code: {language_code}`",
                        parseMode:ParseMode.MarkdownV2,
                        disableWebPagePreview:true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"BOT_ERROR: {e}");
            }
        }
    }
}