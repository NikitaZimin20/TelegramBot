using System;
using System.Data;
using System.Data.SQLite;


namespace ConsoleApp6
{
    class DataBase
    {
        SQLiteConnection connection;
        string sourse = @"ForTelegramBot.db";

        public DataTable Open(string quary)
        {
            if (Connect(sourse))
            {
                return FillData(quary);
            }
            return null;
        }
        private bool Connect(string fileName)
        {
            try
            {
                connection = new SQLiteConnection("Data Source=" + fileName);
                connection.Open();
                return true;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Ошибка доступа к базе данных. Исключение: {ex.Message}");
                return false;
            }
        }
        private DataTable FillData(string query)
        {

            SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection);
            var table = new DataTable();
            adapter.Fill(table);
            return table;

        }
    }
}
