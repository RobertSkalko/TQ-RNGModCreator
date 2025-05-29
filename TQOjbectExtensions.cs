using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TQ_RNGModCreator
{

    public static class TQOjbectExtensions
    {
        private static string ITEM_CLASSIFICATION = "itemClassification";
        private static string ITEM_NAMES = "itemNames";

        public static TQObject getFirstObjectOfLootTable(this TQObject loottable)
        {
            if (loottable.Dict.ContainsKey(ITEM_NAMES))
            {
                string path = loottable.Dict[ITEM_NAMES].getFirstRecord().GetPathOfRecord();

                if (File.Exists(path))
                {
                    return new TQObject(path);
                }
                else
                {
                    Console.WriteLine("file doesn't exist " + path);
                }
            }
            return null;
        }

        public static bool isLegendary(this TQObject @this)
        {
            return @this.Dict.ContainsKey(ITEM_CLASSIFICATION) && @this.Dict[ITEM_CLASSIFICATION] == "Legendary";
        }

        public static bool isEpic(this TQObject @this)
        {
            return @this.Dict.ContainsKey(ITEM_CLASSIFICATION) && @this.Dict[ITEM_CLASSIFICATION] == "Epic";
        }

        public static bool isUnique(this TQObject @this)
        {
            return @this.isEpic() || @this.isLegendary();
        }

        public static bool isUniqueItemLootTable(this TQObject obj)
        {
            //List<string> mustbezero = new List<string>() { "suffixOnly", "prefixOnly", "rarePrefixOnly", "bothPrefixSuffix", "rareBothPrefixSuffix" };

            if (obj.Dict.ContainsKey(ITEM_NAMES))
            {
                string path = obj.Dict[ITEM_NAMES].getFirstRecord().GetPathOfRecord();

                if (File.Exists(path))
                {
                    TQObject item = new TQObject(path);

                    if (item.isUnique())
                    {
                        return true;
                    }
                }
                else
                {
                    Console.WriteLine(path + " doesn't exist");
                }
            }

            return false;
        }
    }
}
