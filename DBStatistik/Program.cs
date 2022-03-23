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
            //var db = new LiteDatabase(@"statistik.db");

            List<Mall> statistik = new List<Mall>();

            int deltagare = 0;
            int gotLicense = 0;

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

            //Skriv till databasen från listan statistik
            using (var db = new LiteDatabase(@"statistik.db"))
            {
                var statistic = db.GetCollection<Mall>("statistik");
                foreach (Mall entry in statistik)
                    statistic.Insert(entry);
            }

            //Antal deltagare
            using (var db = new LiteDatabase(@"statistik.db"))
            {
                var statistic = db.GetCollection<Mall>("statistik");
                int count = 0;

                foreach (Mall entry in statistik)
                {
                    count++;
                }
                Console.WriteLine($"1. Deltagare som deltog i undersökningen: {count}");

                deltagare = count;
            }

            //Antal som svarade "Ja" på körkort
            using (var db = new LiteDatabase(@"statistik.db"))
            {
                var statistic = db.GetCollection<Mall>("statistik");
                int count = 0;

                foreach (Mall entry in statistik)
                {
                    if (entry.driversLicense == "Ja")
                    {
                        count++;
                    }
                }

                double procent = (double)count / (double)deltagare * 100;

                Console.WriteLine($"2. Utav de {deltagare} svaren så var det {count} som hade körtkort. Det är en procentsats på {(double)Math.Round(procent, 2)}%");

                gotLicense = count;
            }

            //Hur många körkort äger bil?
            using (var db = new LiteDatabase(@"statistik.db"))
            {
                var statistic = db.GetCollection<Mall>("statistik");
                int count = 0;
                int noLicenseGotCar = 0;

                foreach (Mall entry in statistik)
                {
                    if (entry.driversLicense == "Ja" && entry.ownCar == "Ja")
                    {
                        count++;
                    }
                }

                foreach (Mall entry in statistik)
                {
                    if (entry.driversLicense == "Nej" && entry.ownCar == "Ja")
                    {
                        noLicenseGotCar++;
                    }
                }

                double procent = ((double)count / gotLicense) * 100;

                Console.WriteLine($"3. Utav de {gotLicense} som har körkort äger {count} även bil. Det är en procentsats på {(double)Math.Round(procent, 2)}%");
                Console.WriteLine($"Dessutom så är det {noLicenseGotCar} som inte har körtkort men har bil.");

            }

            //Medelvärdet på modellår
            using (var db = new LiteDatabase(@"statistik.db"))
            {
                var statistic = db.GetCollection<Mall>("statistik");
                int count = 0;
                int total = 0;

                foreach (Mall entry in statistik)
                {
                    if (entry.modelYear != 0)
                    {
                        count++;
                        total += entry.modelYear;
                    }
                }

                Console.WriteLine($"4. Medelvärdet på alla bilars årsmodeller är {total/count}");
            }

            //Medelvärdet på dieseln
            using (var db = new LiteDatabase(@"statistik.db"))
            {
                var statistic = db.GetCollection<Mall>("statistik");
                double count = 0;
                double total = 0;

                foreach (Mall entry in statistik)
                {
                    if (entry.dieselPrice != 0)
                    {
                        count++;
                        total += entry.dieselPrice;
                    }
                }

                Console.WriteLine($"5. Medelvärdet på dieselpriset är {(double)Math.Round(total/count, 2)}");
            }
        }
    }
}
