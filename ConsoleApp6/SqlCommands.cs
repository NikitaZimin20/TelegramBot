using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ConsoleApp6
{
    internal class SqlCommands
    {
       
        public delegate void AllComands();
        private static string _currentitem;
        private List<string> _list;
        private readonly DataBase data = new();
      
        public Dictionary<string, AllComands> dict;
        private readonly string LastId = "(SELECT max(AddID) FROM Bot)";

        public SqlCommands(string text, long ID)
        {
            this.text = text;
            Id = ID;
            dict = new Dictionary<string, AllComands>
            {
                {"Место проживания", SetUserCity},
                {"Добавить шмотки", SetItemID},
                {"Штаны,Шорты,Юбки", SetTitle},
                {"Подштанники,понталоны", SetTitle},
                {"Головной убор", SetTitle},
                {"Обувь", SetTitle},
                {"Майки", SetTitle},
                {"Рубашки,Футболки", SetTitle},
                {"Кофты,свитеры,водолазк", SetTitle},
                {"Верхняя одежда", SetTitle},
                {"Платье и костюмы", SetTitle},
                {"Шарф", SetTitle},
                {"Удалить вещь", DeleteItem},
                {"Период ношения вещи", SetWashClot},
                {"Место ношения", SetPlace},
                {"Температурные параметры вещи", SetTemp},
                {"Условия ношения", SetWeather},
              
            };
            

        }

     
        private string text { get; }
        private long Id { get; }

      
        private void SetUserID()
        {
            data.Open("UPDATE BOT SET UserID='" + Id + "'WHERE AddID=" + LastId + "");
        }

        private void SetWeather()
        {
            data.Open("Update Bot SET Wcat='" + text + "'WHERE Title=" + _currentitem + "");
        }

        private void SetUserCity()
        {
            data.Open("UPDATE UsersTable SET Town='" + text + "' WHERE USERID='" + Id + "'");
        }

        private void SetItemID()
        {
            data.Open("INSERT INTO Bot(ItemID) Values('" + text + "')");
            SetUserID();
        }

        private void SetTitle()
        {
            data.Open("UPDATE Bot SET Title='" + text + "'WHERE AddID=" + LastId + "");
        }

        private void DeleteItem()
        {
            data.Open("DELETE FROM Bot WHERE Title='" + text + "'");
            
        }

        public string ShowItems()
        {
            return string.Join(Environment.NewLine,
                data.Open("SELECT ItemID,Title FROM Bot WHERE UserID='" + Id + "'").Rows.OfType<DataRow>()
                    .Select(x => string.Join(": ", x.ItemArray)));
        }

        private void SetTemp()
        {
            data.Open("UPDATE Bot SET MaxTemp='" + text + "'WHERE Title='" + _currentitem + "' ");
        }

        private void SetPlace()
        {
            data.Open("UPDATE Bot Set Place='" + text + "'WHERE Title='" + _currentitem + "'");
        }

        public string GetDB(string text)
        {
            var builder = string.Join(Environment.NewLine,
                data.Open("SELECT Title FROM Bot WHERE UserID='" + Id + "'").Rows.OfType<DataRow>()
                    .Select(x => string.Join(",", x.ItemArray)));
            _list = new List<string>();
            _list.AddRange(builder.Split('\n', '\r'));
            if (_list.Contains(text))
            {
                _currentitem = _list.Find(x => x == text);
                return _currentitem;
            }

            return "";
        }
     

        private string ChooseItem()
        {
            Parser pars = new Parser();
            var builder = string.Join(Environment.NewLine, data.Open("SELECT Title FROM Bot WHERE UserID='" + Id + "'AND MinTemp>'" + pars.GetTemp() + "'AND MaxTemp<'" + pars.GetTemp() + "'AND Place='"+text+"' "));

            return string.Empty;
        }
        private void SetWashClot()
        {
            data.Open(
                "Update Bot SET WashCloth='" + text + "'WHERE UserID='" + Id + "'AND Title='" + _currentitem + "'");
        }
    }
}