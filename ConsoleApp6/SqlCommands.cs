using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace ConsoleApp6
{
    class SqlCommands
    {
        public delegate void AllComands();

        public Dictionary<string, AllComands> dict;
        private List<string> _list;
        private string text { get; }
        private long Id { get;}
        private static string _currentitem;

        public SqlCommands(string text,long ID )
        {
            this.text = text;
            this.Id = ID;
            dict = new Dictionary<string, AllComands>()
            {
                {"Место проживания",SetUserCity },
                {"Добавить шмотки",SetItemID },
                {"Штаны,Шорты,Юбки",SetTitle },
                {"Подштанники,понталоны",SetTitle },
                {"Головной убор",SetTitle },
                {"Обувь",SetTitle },
                {"Майки",SetTitle },
                {"Рубашки,Футболки",SetTitle },
                {"Кофты,свитеры,водолазк",SetTitle },
                {"Верхняя одежда",SetTitle },
                {"Платье и костюмы",SetTitle },
                {"Шарф",SetTitle },
                {"Удалить вещь",DeleteItem},
                {"Период ношения вещи",SetWashClot},
                {"Место ношения",SetPlace},
                {"Температурные параметры вещи",SetTemp},
                {"Условия ношения",SetWeather}
            };

        }
       
        
        DataBase data = new DataBase();
        private string LastId = "(SELECT max(AddID) FROM Bot)";

        
        private void SetUserID()
        {
            data.Open("UPDATE BOT SET UserID='"+Id+ "'WHERE AddID="+ LastId +"");

        }

        private void SetWeather()
        {
            data.Open("Update Bot SET Wcat='" + text + "'WHERE Title=" + _currentitem + "");
        }
        private void SetUserCity()
        {
            data.Open("UPDATE UsersTable SET Town='"+text+"' WHERE USERID='"+Id+"'");
        }
        private void SetItemID()
        {
             
            data.Open("INSERT INTO Bot(ItemID) Values('"+text+"')");
            SetUserID();
        }
        private void SetTitle()
        {
            data.Open("UPDATE Bot SET Title='"+text+"'WHERE AddID="+LastId+"");
        }

        private void DeleteItem()
        {
            data.Open("DELETE FROM Bot WHERE Title='" + text + "'");
        }

        public string ShowItems()
        {
            
            return string.Join(Environment.NewLine, data.Open("SELECT ItemID,Title FROM Bot WHERE UserID='"+Id+"'").Rows.OfType<DataRow>().Select(x => string.Join(": ", x.ItemArray)));
        }
        private void SetTemp()
        {
            data.Open("UPDATE Bot SET MaxTemp='"+text+"'WHERE Title='"+_currentitem+"' ");
        }
        private void SetPlace()
        {
            data.Open("UPDATE Bot Set Place='"+text+"'WHERE Title='"+_currentitem+"'");
        }

        public string CheckItem(string text)
        {
            string  builder= string.Join(Environment.NewLine, data.Open("SELECT Title FROM Bot WHERE UserID='" + Id + "'").Rows.OfType<DataRow>().Select(x => string.Join(",", x.ItemArray)));
            _list = new List<string>();
            _list.AddRange(builder.Split(new char[]{ '\n', '\r' }));
            if (_list.Contains(text))
            {
                _currentitem= _list.Find(x => x == text);
                return _currentitem;
            }

            return "Кавабанга";
        }

        private void SetWashClot()
        {
            data.Open("Update Bot SET WashCloth='"+text+"'WHERE UserID='"+Id+"'AND Title='"+_currentitem+"'");
        }

    }
}
