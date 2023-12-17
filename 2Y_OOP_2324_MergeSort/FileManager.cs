using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace _2Y_OOP_2324_MergeSort
{
    internal class FileManager
    {
        public List<string> fileReader(string filePath)
        {
            List<string> lines = new List<string>();
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines;
        }

        public bool processContent(List<string> content, out int[] prContent)
        {
            bool parse = false;
            int[] pContent = new int[] { };

            if (content.Count == 5)
            {
                pContent = new int[5];

                for(int x = 0; x < pContent.Length; x++) 
                {
                    parse = int.TryParse(content[x], out pContent[x]);
                    if (!parse)
                        break;
                }

                if (!parse)
                {
                    prContent = pContent;
                    return false;
                }
            }

            prContent = pContent;
            return true;
        }

        public async Task massFileWriteAsync(string filePath, bool append, List<string> lines)
        {
            using (StreamWriter sw = new StreamWriter(filePath, append))
            {
                foreach(string line in lines)
                {
                    sw.WriteLine(line);
                }    
            }
        }

        public async Task fileWriteAsync(string filePath, bool append, string message)
        {
            using (StreamWriter sw = new StreamWriter(filePath, append))
            {
                sw.WriteLine(message);
            }
        }
    }
}
