
using System.Diagnostics;
using System.IO;


namespace ConsoleApp6
{
    class Parser
    {
        private void UpdateFile()
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/C " + "python API.py";
            p.StartInfo.CreateNoWindow = true;
            p.Start();
        }

        public int GetTemp()
        {
            UpdateFile();
            StreamReader f = new StreamReader("test.txt");
            string s = string.Empty;
            while (!f.EndOfStream)
            {
                s = f.ReadLine();

            }
            f.Close();
            return int.Parse(s);
        }
    }
}
