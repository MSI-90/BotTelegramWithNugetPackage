using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks.Dataflow;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BotTelegram
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new TelegramBotClient("5331687709:AAEXvmiQ1qObjihZeUh85cs903W7s4wKJok");

            
            client.StartReceiving(Update, Error);
            Console.ReadLine();
            
        }
        async private static Task Update(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var message = update.Message;
            var me = await botClient.GetMeAsync();
            var userCount = await botClient.GetChatMemberCountAsync(message.Chat.Id);

            if (message.Text != null)
            {   
                if (message.Text == "/start")
                {
                    await botClient.SendTextMessageAsync
                        (
                            chatId: message.Chat.Id,
                            text: $"Приветствуем в нашем чате @{message.Chat.Username}"
                        );
                }
                
                if ((message.Text.ToLower().Contains("здаров")) || (message.Text.ToLower().Contains("здоров")))
                {
                    await botClient.SendTextMessageAsync
                        (
                            chatId: message.Chat.Id, text: "Здоровее видали!",
                            //parseMode: ParseMode.Html,
                            replyToMessageId: message.MessageId
                        );
                    
                    return;
                }
            }
            if (message.Photo != null)
            {
                await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Хорошее фото, но лучше кинь документом.");
                return;
            }
            if (message.Document != null)
            {
                await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ща, погодь, сделаю лучше...");

                var fileId = message.Document.FileId;
                var fileInfo = await botClient.GetFileAsync(fileId);
                var filePath = fileInfo.FilePath;


                DirectoryInfo userDirectory = new DirectoryInfo($@"H:\UDEMY\C#\Боты\telegram\First\BotTelegram\BotTelegram\download\{message.Chat.Id}");
                userDirectory.Create();

                string destinationFilePath = $@"{userDirectory}\{message.Document.FileName}";
                Console.WriteLine(destinationFilePath);

                await using Stream fileStream = System.IO.File.OpenWrite(destinationFilePath);
                await botClient.DownloadFileAsync(filePath: filePath, destination: fileStream);
                fileStream.Close();

                return;
            }
        }
        async private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {  
        }
    }
}