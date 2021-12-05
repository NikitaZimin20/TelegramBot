using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace ConsoleApp6
{
    internal class Program
    {
        private static readonly string token = "2022806897:AAFAZI17CLSN7SIUfMjNZD2cOgBKZ1TEOMg";
        private static TelegramBotClient _telegramBotClient;
        private SqlCommands _commands;
        private static string _lastcommand = string.Empty;
        private readonly Dictionary<string, Item> dictionary;

        private Program(long id ,string text)
        {
            _commands = new SqlCommands(text, id);
            dictionary = new Dictionary<string, Item>()
            {
                {"/start", new Item(){description = "Напишите что нибудь чтобы начать",function = ShowMainButtons()} },
                {"Добавить шмотки",new Item(){description = "Выберите тип добавляемой одежды",function = ShowСlothes()}},
                {"Место проживания",new Item(){description = "Введите название населённого пункта в котором вы проживаете"}},
                {"Удалить вещь",new Item(){description ="Напишите название вещи",function=new ReplyKeyboardRemove() }},
                {"Доп.параметры вещей",new Item(){description ="Введите название вещи которой нужно изменить характеристику \n"+_commands.ShowItems(), function = new ReplyKeyboardRemove()} },
                {"Штаны,Шорты,Юбки",new Item(){description = "Введите название одежды",function =new ReplyKeyboardRemove() }},
                {"Подштанники,понталоны",new Item(){description = "Введите название одежды",function =new ReplyKeyboardRemove() }},
                {"Шарф",new Item(){description = "Введите название одежды",function =new ReplyKeyboardRemove() }},
                {"Головной убор",new Item(){description = "Введите название одежды",function =new ReplyKeyboardRemove() }},
                {"Обувь",new Item(){description = "Введите название одежды",function =new ReplyKeyboardRemove() }},
                {"Рубашки,Футболки",new Item(){description = "Введите название одежды",function =new ReplyKeyboardRemove() }},
                {"Кофты,свитеры,водолазки",new Item(){description = "Введите название одежды",function =new ReplyKeyboardRemove() }},
                {"Верхняя одежда",new Item(){description = "Введите название одежды",function =new ReplyKeyboardRemove() }},
                {"Платье и костюмы",new Item(){description = "Введите название одежды",function =new ReplyKeyboardRemove() }},
                {"Назад",new Item(){description = "Выберите необходимую функцию",function=ShowMainButtons()}},
                {"Посмотреть список вещей",new Item(){description =_commands.ShowItems() }},
                {"Период ношения вещи",new Item(){description = "введите количество дней ккоторое планируете носить вещь"}},
                {"Место ношения",new Item(){description = "укажите местро в котором вы хотели бы носить данную вещь"}},
                {"Температурные параметры вещи",new Item(){description = "Укажите при каких температурах должна носиться вещь в формате min-max"}},
                {"Условия ношения",new Item(){description="При каких погодных условиях стоит носить данную вещь"}},
                {"Подбери мне одежду",new Item(){description = "выбирите место куда желаете пойти",function = ShowPlaces()}},
                {_commands.GetDB(text),new Item(){description = "Выбирите необходимую функцию для изменения",function = ShowExtraFutures()}},
                {"Работа",new Item()},
                {"Свидание",new Item()},
                {"Встреча с друзьями",new Item()},
                {"Светский вечер",new Item()}

            };
         
        }
        [Obsolete]
        private static void Main()
        {
          
            _telegramBotClient = new TelegramBotClient(token);
            _telegramBotClient.StartReceiving();
            _telegramBotClient.OnMessage += TelegramBotClient_OnMessage;
            Console.ReadKey();
            _telegramBotClient.StopReceiving();
        }
        private void ShowInfo(long chatid,string itemkey)
        {
            ShowDescription(chatid,dictionary[itemkey].description,dictionary[itemkey].function);
        }
        private void ShowDescription(long chatid,string text,IReplyMarkup buttMarkup)
        {
            _telegramBotClient.SendTextMessageAsync(chatid, text, replyMarkup:buttMarkup);
        }
        
        [Obsolete]
        private static void TelegramBotClient_OnMessage(object sender, MessageEventArgs e)
        {
            var message = e.Message;
            var chatId = message.Chat.Id;
            Program program = new Program(chatId,message.Text);
            program.DoAction(chatId,message.Text,_lastcommand);
            _lastcommand = message.Text;
        } 
        private void DoAction(long chatid,string text,string lastcomm)
        {
            CheckSqlCommand(text, lastcomm, chatid);
            if (dictionary.ContainsKey(text))
            {
                ShowInfo(chatid, text);
            }
            else
            {
              ShowDescription(chatid,"Ваши данные были учтены",ShowMainButtons());
            }
        }
        private void CheckSqlCommand(string currentaction, string lastcom, long chatId)
        {

            _commands = new SqlCommands(currentaction, chatId);
            if (_commands.dict.ContainsKey(lastcom))
            {
                _commands.dict[lastcom]();
                
            }
        }

        private IReplyMarkup ShowExtraFutures()
        {

            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new()
                    {
                        new KeyboardButton() {Text = "Период ношения вещи"}, new KeyboardButton() {Text = "Место ношения"}
                    },
                    new()
                    {
                        new KeyboardButton() {Text = "Температурные параметры вещи"}, new KeyboardButton() {Text = "Условия ношения"}
                    },
                    new ()
                    {
                        new KeyboardButton() {Text = "Назад"}
                    }


                }

            };
        }

    
        private IReplyMarkup ShowMainButtons()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new()
                    {
                        new KeyboardButton() {Text ="Добавить шмотки"}, new KeyboardButton() {Text = "Удалить вещь"}
                    },
                    new()
                    {
                        new KeyboardButton() {Text = "Место проживания"}, new KeyboardButton() {Text = "Доп.параметры вещей"}
                    },
                    new(){
                        new KeyboardButton(){Text = "Посмотреть список вещей"},new KeyboardButton(){Text = "Подбери мне одежду"}
                         }

                }
            };
        }

        private IReplyMarkup ShowСlothes()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new()
                    {
                        new KeyboardButton() {Text = "Штаны,Шорты,Юбки"}, new KeyboardButton() {Text = "Подштанники,понталоны"}
                    },
                    new()
                    {
                        new KeyboardButton() {Text = "Шарф"}, new KeyboardButton() {Text = "Головной убор"}
                    },
                    new()
                    {
                        new KeyboardButton() {Text = "Обувь"},
                        new KeyboardButton() {Text = "Майки"}
                    },
                    new()
                    {
                        new KeyboardButton() {Text = "Рубашки,Футболки"},
                        new KeyboardButton() {Text ="Кофты,свитеры,водолазки"}
                    },
                    new() 
                        {new() {Text = "Верхняя одежда"}, new() {Text = "Платье и костюмы"}}
                }
            };
        }

        private IReplyMarkup ShowPlaces()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new()
                    {
                        new KeyboardButton() {Text ="Работа"}, new KeyboardButton() {Text = "Свидание"}
                    },
                    new()
                    {
                        new KeyboardButton() {Text = "Встреча с друзьями"}, new KeyboardButton() {Text = "Светский вечер"}
                    },
                  

                }
            };
        }
    }
}