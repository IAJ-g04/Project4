using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using GeometryFriendsAgents.Model;
using System.Collections;


namespace GeometryFriendsAgents.InstructionManual
{
    public class ReadAgent
    {
        string mydocpath = AppDomain.CurrentDomain.BaseDirectory + @"\ExternalDatabase.txt";
        string mybackuppath = AppDomain.CurrentDomain.BaseDirectory + @"Logs\" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "Log.txt";
        DataTable table;

        //Reads the file and makes a security backup
        public ReadAgent()
        {

            table = new DataTable();
            table.Columns.Add("Type", typeof(string));
            table.Columns.Add("Direction", typeof(string));
            table.Columns.Add("Distance", typeof(string));
            table.Columns.Add("Solution", typeof(string));

            foreach (string f in File.ReadAllLines(mydocpath))
            {
                string[] words = f.Split('\\');
                table.Rows.Add(words[0].ToString(), words[2].ToString(), words[4].ToString(), words[6].ToString());
            }
            BackupToFile();
        }

        //Prints the current table 
        public void printTable()
        {

            foreach (DataRow row in table.Rows)
            {
                // ... Write value of first field as integer.
                ConsolePrinter.PrintLine("Type: " + row.Field<string>(0) + " Dir: " + row.Field<string>(1) + " Dis: " + row.Field<string>(2) + " Sol: " + row.Field<string>(3));
            }

        }

        //Saves the current table to file
        public void SaveToFile()
        {
            string toWrite = "";
            foreach (DataRow row in table.Rows)
            {
                toWrite = toWrite + row.Field<string>(0) + "\\\\" + row.Field<string>(1) + "\\\\" + row.Field<string>(2) + "\\\\" + row.Field<string>(3) + Environment.NewLine;
            }
            File.WriteAllText(mydocpath, toWrite);
        }

        //Backs up the current table
        public void BackupToFile()
        {
            string toWrite = "";
            foreach (DataRow row in table.Rows)
            {
                toWrite = toWrite + row.Field<string>(0) + "\\\\" + row.Field<string>(1) + "\\\\" + row.Field<string>(2) + "\\\\" + row.Field<string>(3) + Environment.NewLine;
            }
            File.WriteAllText(mybackuppath, toWrite);
        }

        //Updates a value on the table, or creates a new one in case it doesn't exist
        public void Update(Connection c, string s)
        {
            DataRow foundRows;

            string search = "Type = " + c.categorie.ToString() + " and Direction = " + c.side.ToString();

            foundRows = table.Select(search).FirstOrDefault();

            if (foundRows != null)
            {
                foundRows["Solution"] = s; //changes the Solution
            }
            else
            {
                table.Rows.Add(c.categorie.ToString(), c.side.ToString(), "distance", s);
            }
        }

        public String getSolution(Connection cc)
        {
            DataRow foundRows;

            string search = "Type = " + cc.categorie.ToString() + " and Direction = " + cc.side.ToString();

            foundRows = table.Select(search).FirstOrDefault();

            if (foundRows != null)
            {
                return foundRows["Solution"].ToString();
            }
            else
            {
                return null;
            }

        }

        public String getAlternative(Connection cc)
        {
          /*  DataRow[] foundRows;

            string search = "Type = " + cc.categorie.ToString() + " and Direction = " + cc.side.ToString();
            foundRows = table.Select(search);

            if (foundRows != null)
            {
                foreach()


            }*/


                return null;
        }

    }
}

    
