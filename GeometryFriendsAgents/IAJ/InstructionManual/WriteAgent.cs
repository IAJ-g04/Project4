using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;



namespace GeometryFriendsAgents.InstructionManual
{
    public class WriteAgent
    {
        string mydocpath = AppDomain.CurrentDomain.BaseDirectory + @"\WriteLines.txt";
        public WriteAgent()
        {
              File.WriteAllText(mydocpath, String.Empty);
        }
       
        public void SaveToFile(String s) {
            using (Stream stream1 = File.Open(mydocpath, FileMode.Append))
            using (StreamWriter sWriter1 = new StreamWriter(stream1)){
                sWriter1.Write(s + Environment.NewLine);
            }
        }


    }
}
