using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TQ_RNGModCreator
{

    public class ToolBiggerPotionCd : ToolButton
    {
        public override string Name { get => "ToolBiggerPotionCd"; }
        public override string Description { get; }

        public override Predicate<TQObject> GetObjectPredicate
        {
            get =>
            new Predicate<TQObject>(x => x.HasTemplate() && x.GetTemplate().ToLower().Contains("potion") && x.Dict.ContainsKey("useDelayTime"));
        }

        public override Predicate<string> GetFilePathPredicate
        {
            get =>
            new Predicate<string>(x => x.Contains("item"));
        }

        public void Action()
        {
            ConcurrentBag<TQObject> list = GetAllObjects(Save.Instance.GetRecordsPath());
            foreach (TQObject obj in list)
            {
                // we up potion cd from 6 seconds to 60
                obj.Dict["useDelayTime"] = "60.0";
            }
            FileManager.WriteCopy(Save.Instance.GetOutputPath(), list);
        }
    }
}
