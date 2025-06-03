using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace TQ_RNGModCreator
{
    public abstract class ToolButton
    {
        public abstract string Name { get; }
        public abstract string Description { get; }

        public abstract Predicate<TQObject> GetObjectPredicate { get; }
        public abstract Predicate<string> GetFilePathPredicate { get; }

        public string GetInput()
        {
            return Save.Instance.InputCommand;
        }

        public ConcurrentBag<TQObject> GetAllObjects(string path)
        {
            return FileManager.GetAllObjects(path, this.GetFilePathPredicate, this.GetObjectPredicate);
        }

        public ConcurrentBag<TQObject> GetAllObjectsInRecordsPath()
        {
            return FileManager.GetAllObjects(Save.Instance.GetRecordsPath(), this.GetFilePathPredicate, this.GetObjectPredicate);
        }

        public void WriteToOutput(IEnumerable<TQObject> list)
        {
            FileManager.WriteCopy(Save.Instance.GetOutputPath(), list);
        }

        //protected abstract void Action();
    }
}
