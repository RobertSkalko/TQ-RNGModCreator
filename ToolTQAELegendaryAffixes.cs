using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TQ_RNGModCreator
{
    public class ToolTQAELegendaryAffixes : ToolButton
    {

        public class Config
        {
            public bool createRareVariants = true;
            public bool showAffixesOnRaresAndLegends = true;
            public bool createNameColorsForVariants = true;
        }

        public override string Name { get => "ToolTQAELegendaryAffixes"; }
        public override string Description { get; }

        public override Predicate<TQObject> GetObjectPredicate
        {
            get =>
            new Predicate<TQObject>(x => x.HasClass() && x.GetClass().Contains("LootItemTable") && x.getFirstObjectOfLootTable() != null);
        }

        public override Predicate<string> GetFilePathPredicate
        {
            get =>
            new Predicate<string>(x => x.Contains("loottables") || x.Contains("lootables"));
        }

        public void Action(Config config)
        {

            // uniques
            ConcurrentBag<TQObject> uniques = FileManager.GetAllObjects(Save.Instance.GetRecordsPath(), new Predicate<string>(x => x.Contains("item")), new Predicate<TQObject>(x => x.isUnique() && x.Dict.ContainsKey("hidePrefixName") && x.Dict.ContainsKey("hideSuffixName")));

            ConcurrentBag<TQObject> masterUniqueTables = FileManager.GetAllObjects(Save.Instance.GetRecordsPath(), new Predicate<string>(x => x.Contains("table")), new Predicate<TQObject>(x => x.Dict.ContainsKey("templateName") && x.Dict["templateName"].Contains("LootMasterTable")));

            List<TQObject> rares = new List<TQObject>();

            if (config.createRareVariants)
            {
                foreach (var item in uniques)
                {
                    var rare = item.CloneJson();
                    rare.Dict["itemClassification"] = "Rare";
                    rare.suffixFilename("rare");
                    rare.Dict["DisplayAsQuestItem"] = 1 + "";
                    rares.Add(rare);

                }
            }
            uniques.AddRange(rares);

            if (config.showAffixesOnRaresAndLegends)
            {
                foreach (var item in uniques)
                {
                    item.Dict["hidePrefixName"] = "0";
                    item.Dict["hideSuffixName"] = "0";
                }
            }

            //uniques

            TQObject chances = new TQObject(Path.Combine(Save.Instance.GetDataPath(), "chances.txt"));
            TQObject rareChances = new TQObject(Path.Combine(Save.Instance.GetDataPath(), "rareChances.txt"));
            ConcurrentBag<TQObject> allLootTables = GetAllObjects(Save.Instance.GetRecordsPath());
            //Console.WriteLine("There are " + allLootTables.Count + " item loot tables.");
            LootAffixes affixes = new LootAffixes(allLootTables);
            var uniqueitemLoottableList = allLootTables.Where(x => x.isUniqueItemLootTable()).ToList();
            //Console.WriteLine("There are " + uniqueitemLoottableList.Count + " unique item loot tables.");
            affixes.testPrint();

            foreach (TQObject loottable in uniqueitemLoottableList)
            {
                TQObject gear = loottable.getFirstObjectOfLootTable();

                if (gear != null)
                {
                    string type = gear.GetClass();

                    affixes.TryGiveAffixesToLoottable(loottable, type);
                }
                else
                {
                    Console.WriteLine("gear is null!!!");
                }
            }

            List<TQObject> rareLoottables = new List<TQObject>();

            uniqueitemLoottableList.ForEach(x => x.ReplaceWithAllValuesOf(chances));

            if (config.createRareVariants)
            {
                foreach (var item in uniqueitemLoottableList)
                {
                    var rare = item.CloneJson();
                    rare.Dict["itemNames"] = rare.Dict["itemNames"].Replace(".dbr", "rare.dbr"); // todo is this ok
                    rare.suffixFilename("rare");
                    rareLoottables.Add(rare);

                }
            }
            rareLoottables.ForEach(x => x.ReplaceWithAllValuesOf(rareChances));

            foreach (TQObject o in masterUniqueTables)
            {
                addRareVariantToMasterTable(o, uniqueitemLoottableList);

            }

            //masterUniqueTables.ForEach(x => addRareVariantToMasterTable(x, rareLoottables)); // todo which one do i use, rare or normal loot tables here..

            if (config.createNameColorsForVariants)
            {
                var oldids = new Dictionary<string, string>();

                foreach (var item in uniques)
                {
                    if (item.Dict["itemClassification"].Equals("Rare"))
                    {
                        if (item.Dict.ContainsKey("itemNameTag"))
                        {
                            String old = item.Dict["itemNameTag"];
                            item.Dict["itemNameTag"] = old + "rare"; // todo does this work
                            oldids[item.Dict["itemNameTag"]] = old;
                        }
                        else
                        {
                            Console.WriteLine(item.FilePath + " doesn't have an itemnametag, is this an actual item?");
                        }
                    }
                }
                string names = CommentRemover.RemoveComments(File.ReadAllText(Path.Combine(Save.Instance.GetFolder(), "text/ALL_TEXT_COMBINED.txt")));

                var lines = names.Split("\r\n");

                var map = new Dictionary<string, string>();
                foreach (var name in lines)
                {
                    var split = name.Split('=');
                    if (split.Length == 2)
                    {
                        map[split[0]] = split[1];
                    }
                }

                string newfile = "";

                foreach (var item in rares)
                {
                    if (item.Dict.ContainsKey("itemNameTag"))
                    {
                        var tag = item.Dict["itemNameTag"];
                        var oldtag = oldids[tag];
                        if (map.ContainsKey(oldtag))
                        {
                            var mapped = map[oldtag];

                            newfile += item.Dict["itemNameTag"] + "={^r}" + mapped + "\n";
                        }
                        else
                        {
                            Console.WriteLine(oldtag + " tag doesn't exist");
                        }
                    }
                }
                File.WriteAllText(Save.Instance.GetDataPath() + "dialogue.txt", newfile); // this is a trick to allow 1 file to work 
            }
            FileManager.WriteCopy(Save.Instance.GetOutputPath(), uniqueitemLoottableList);
            FileManager.WriteCopy(Save.Instance.GetOutputPath(), rareLoottables);
            FileManager.WriteCopy(Save.Instance.GetOutputPath(), masterUniqueTables);
            FileManager.WriteCopy(Save.Instance.GetOutputPath(), uniques);

        }

        public void addRandomAffixesToLootTable(TQObject obj, TQObject affixes)
        {
            foreach (KeyValuePair<string, string> entry in affixes.Dict)
            {
                obj.Dict[entry.Key] = entry.Value;
            }
        }

        public void addRareVariantToMasterTable(TQObject table, List<TQObject> uniqueLootTables)
        {

            List<KeyValuePair<int, string>> tables = new List<KeyValuePair<int, string>>();

            foreach (KeyValuePair<string, string> entry in table.Dict)
            {
                if (entry.Key.Contains("lootName"))
                {
                    String id = entry.Value;
                    if (id.Length > 0)
                    {

                        string filename = Path.GetFileName(id.ToLower());

                        // if it's a table of uniques
                        if (uniqueLootTables.Any(x => x.FilePath.ToLower().Contains(filename)))
                        {
                            string weightkey = entry.Key.Replace("lootName", "lootWeight");
                            string weightstring = table.Dict[weightkey];
                            int weight = int.Parse(weightstring);

                            tables.Add(KeyValuePair.Create(weight, id));
                        }
                    }
                }
            }

            int index = table.firstFreeNumberedIndexOf(x => "lootName" + x, 30);

            if (index > -1)
            {
                foreach (var item in tables)
                {
                    table.Dict["lootName" + index] = item.Value.Replace(".dbr", "rare.dbr");
                    int weight = item.Key / 10;
                    table.Dict["lootWeight" + index] = weight + "";
                    index++;
                }

            };

        }
    }
}
