Quick port of my older mod generator that was made in unity.. don't ask

Big feature is it uses code to give each epic/legendary item's loot table chance to get affixes
It automatically picks the correct affixes per item type and level, although it's very old code and probably has holes

I created it so whenever TQ gets an update, i just need to run the program once and i'm done

Anyway, mod generation goes like this:

- Save.SaveDataPathWithoutFileName = [your folder where you extracted the Titan quest ARZ]
- create a data folder inside that one and put the chances.txt into this
- now your folder should have a data folder with chances.txt and a records folder with all the extracted TQ records
- Open project with visual studio, go to Program.cs
- A few features are here, like turning on affix names for all unique items, adding affixes to unique item loot tables.. You can comment out the ones you don't want
- click run Program, wait for it to finish (takes me 20s)
- Go back to your extracted ARZ folder, notice a new output folder
- Copy the files from output to your mod's working folder that the artmanager created
- build the mod
- can now play it!

