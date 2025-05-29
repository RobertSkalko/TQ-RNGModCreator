using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TQ_RNGModCreator
{
    public class Save
    {
        private static Save instance = null;

        public static Save Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Save();
                }

                return instance;
            }

            set => instance = value;
        }

        private static string SaveFileName = "Save.txt";
        private static string SaveDataPathWithoutFileName = "D:/TQExtract/";

        public void CreateFoldersIfEmpty()
        {
            List<string> dirs = new List<string>
            {
                GetOutputPath(),
                GetRecordsPath(),
                GetDataPath()
            };

            foreach (string dir in dirs)
            {
                if (Directory.Exists(dir) == false)
                {
                    Directory.CreateDirectory(dir);
                }
            }
        }

        public string GetFolder()
        {
            return SaveDataPathWithoutFileName;
        }

        public string GetOutputPath()
        {
            return SaveDataPathWithoutFileName + "output/";
        }

        public string GetRecordsPath()
        {
            return SaveDataPathWithoutFileName + "records/";
        }

        public string GetDataPath()
        {
            return SaveDataPathWithoutFileName + "data/";
        }

        public string InputCommand = "";
    }
}
