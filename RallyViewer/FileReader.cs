using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RallyViewer
{
    class FileReader
    {
        public static Dictionary<string, string> OpenDictionaryFile(string filepath)
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            try
            {
                string line;
                System.IO.StreamReader file = new System.IO.StreamReader(filepath);
                while ((line = file.ReadLine()) != null)
                {
                    string[] values = line.Split('=');
                    if (values.Length != 2)
                        continue;
                    list.Add(values[0].Trim(), values[1].Trim());
                }
                file.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return list;
        }

        public static List<string> OpenSagaFeatureFile(string filePath)
        {
            List<string> sagafeatures = new List<string>();
            try
            {
                string line;
                System.IO.StreamReader file = new System.IO.StreamReader(filePath);
                while ((line = file.ReadLine()) != null)
                {
                    line = ParseLine(line);
                    if (string.IsNullOrEmpty(line))
                        continue;
                    sagafeatures.Add(line);
                }
                file.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return sagafeatures;
        }

        private static string ParseLine(string line)
        {
            string[] words = line.Split(' ');
            if (words.Length == 0)
                return string.Empty;
            return words[0].TrimEnd(new char[] { ':', '-' });
        }
    
        public static List<string> OpenPeopleFile(string filepath)
        {
            List<string> ppl = new List<string>();
            try
            {
                string line;
                System.IO.StreamReader file = new System.IO.StreamReader(filepath);
                while ((line = file.ReadLine()) != null)
                {
                    ppl.Add(line.Trim());
                }
                file.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return ppl;
        }
    }
}
