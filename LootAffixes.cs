using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TQ_RNGModCreator
{
    public class LootAffixes
    {
        public LootAffixes(ConcurrentBag<TQObject> allLootTables)
        {
            this.dict = TQAffixUtils.makeAffixDict(allLootTables);
        }

        private Dictionary<string, LootAffixTable> dict = new Dictionary<string, LootAffixTable>();

        public void testPrint()
        {
            foreach (KeyValuePair<string, LootAffixTable> entry in dict)
            {
                Console.WriteLine("Gear type: " + entry.Key);

                entry.Value.testPrint();
            }
        }

        public void TryGiveAffixesToLoottable(TQObject loottable, string gearType)
        {
            if (dict.ContainsKey(gearType))
            {
                // should i make it so only tables with 1 item can get random affixes?..
                dict[gearType].GetAffixesFor(loottable, loottable.getFirstObjectOfLootTable());

            }
            else
            {
                Console.WriteLine("no key: " + gearType);
            }
        }
    }
}
