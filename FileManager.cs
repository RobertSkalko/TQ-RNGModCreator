﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TQ_RNGModCreator
{
    public static class FileManager
    {
        private static void CompileAllFiles() // todo trying something
        {
            Parallel.ForEach(Directory.EnumerateFiles(Save.Instance.GetDataPath(), "*", SearchOption.AllDirectories), (file) =>
            {
                if (file.EndsWith(".code"))
                {
                    string txt = File.ReadAllText(file);
                }
            });
        }

        public static void WriteCopy(string path, IEnumerable<TQObject> files)
        {
            foreach (TQObject obj in files)
            {
                string newpath = Save.Instance.GetOutputPath() + "/" + obj.FilePath.Replace(Save.Instance.GetRecordsPath(), "");

                string dirpath = Path.GetDirectoryName(newpath);

                if (Directory.Exists(dirpath) == false)
                {
                    Directory.CreateDirectory(dirpath);
                }

                File.WriteAllText(newpath, obj.GetTextRepresentation());
            }
        }

        public static ConcurrentBag<TQObject> GetAllObjects(string path, Predicate<string> pathpred, Predicate<TQObject> objpred, string ending = ".dbr")
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var list = new ConcurrentBag<TQObject>();

            Parallel.ForEach(Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories), (file) =>
            {
                if (file.EndsWith(ending) && pathpred(file))
                {
                    TQObject obj = new TQObject(file);
                    if (objpred(obj))
                    {
                        list.Add(obj);
                    }
                }
            });

            stopwatch.Stop();
            Console.WriteLine("Getting Objects from files took: " + stopwatch.ElapsedMilliseconds + " Miliseconds or " + stopwatch.ElapsedMilliseconds / 1000 + " Seconds");

            return list;
        }
    }
}
