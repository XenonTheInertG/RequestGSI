# Request for GSI via a telegram bot.

# Setup & Run

open the Program.cs file and config the following:
```c#
        // replace with your own bot token
        private static readonly TelegramBotClient bot = new TelegramBotClient("YOUR-BOT-TOKEN-GOES-HERE");
        // replace with the chat you want the bot to work on
        private static long chat = 123; // you can type your own id if you want the bot to message you with the link.
        // replace with the chat you want the bot to send the gsi messages to.
        private static long chat_send = 321;
```

- you must have the .NET sdk installed on your PC

- `git clone https://github.com/XenonTheInertG/Request-GSI bot`
- `cd bot`
- `dotnet run -c Release`
# Request:

`/request <link> <optinal: info>`

e.g: <br>

`/request https://pilotfiber.dl.sourceforge.net/project/havoc-os//tulip/Havoc-OS-v3.12-20210103-tulip-Official.zip don't resign this rom!`