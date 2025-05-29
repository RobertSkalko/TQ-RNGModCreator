using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TQ_RNGModCreator
{
    public class ToolTQUniquesShowAffixes : ToolButton
    {
        public override string Name { get => "ToolTQUniquesShowAffixes"; }
        public override string Description { get; }

        public override Predicate<TQObject> GetObjectPredicate
        {
            get =>
            new Predicate<TQObject>(x => x.isUnique() && x.Dict.ContainsKey("hidePrefixName") && x.Dict.ContainsKey("hideSuffixName"));
        }

        public override Predicate<string> GetFilePathPredicate
        {
            get =>
            new Predicate<string>(x => x.Contains("item"));
        }

        protected override void Action()
        {
            ConcurrentBag<TQObject> list = GetAllObjects(Save.Instance.GetRecordsPath());
            foreach (TQObject obj in list)
            {
                obj.Dict["hidePrefixName"] = "0";
                obj.Dict["hideSuffixName"] = "0";
            }
            FileManager.WriteCopy(Save.Instance.GetOutputPath(), list);
        }
    }
}
