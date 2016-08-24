using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcuseManager
{
    public class Excuse
    {
       
        public string Description { get; set; }
        public string Results { get; set; }
        public DateTime LastUsed { get; set; }
        public string ExcusePath { get; set; }

        public Excuse()
        {
            ExcusePath = "";
        }


        public Excuse(string fileName)
        {
            OpenFile(fileName);
        }


        public Excuse(Random random, string folderName)
        {
            string[] fileNames = Directory.GetFiles(folderName, "*.txt");
            OpenFile(fileNames[random.Next(fileNames.Length)]);
        }

       

        public void OpenFile(string fileName)
        {
            ExcusePath = fileName;
            using (StreamReader reader = new StreamReader(fileName))
            {
                Description = reader.ReadLine();
                Results = reader.ReadLine();
                LastUsed = Convert.ToDateTime(reader.ReadLine());

            }
        }

        public void Save(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine(Description);
                writer.WriteLine(Results);
                writer.WriteLine(LastUsed);
            }
        }
    }
}
