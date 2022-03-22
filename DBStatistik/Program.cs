using System;
using System.Collections.Generic;
using LiteDB;
using Microsoft.VisualBasic.FileIO;

namespace DBStatistik
{
    
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Mall> statistik = new List<Mall>();
            
            int year = 0;
            double price;

            using (var reader = new TextFieldParser(@"statistik.csv"))
            {
                reader.SetDelimiters(new string[] { "," });
                reader.ReadLine();

                while (!reader.EndOfData)
                {
                    string[] columns = reader.ReadFields();
                    //Console.WriteLine($"{columns[1]} - {columns[2]} - {columns[3]} - {columns[4]}");

                    bool checkYear = int.TryParse(columns[3], out year);
                    if (checkYear == false)
                    {
                        year = 0;
                    }

                    bool checkPrice = double.TryParse(columns[4], out price);
                    if (checkPrice == false)
                    {
                        price = 0;
                    }

                    Mall entry = new Mall(columns[1], columns[2], year, price);

                    statistik.Add(entry);
                }
            }
            using (var db = new LiteDatabase(@"statistik.db"))
            {
                var statistic = db.GetCollection<Mall>("statistik");
                foreach (Mall entry in statistik)
                    statistic.Insert(entry);
            }
        }
    }
}
