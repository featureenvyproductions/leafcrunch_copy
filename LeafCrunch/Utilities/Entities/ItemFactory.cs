using LeafCrunch.GameObjects.Items;
using LeafCrunch.GameObjects.Items.InstantItems;
using LeafCrunch.GameObjects.Items.TemporaryItems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeafCrunch.Utilities.Entities
{
    //i mean I should have classes like this for other things too but this one is the easiest to do I think
    //with operations I need to tweak it so when they're created they add themselves to the registry
    //like some items do
    public class ItemFactory
    {
        private const string _configFile = "items.json";

        public List<GenericItem> LoadItems()
        {
            var jsonString = File.ReadAllText(UtilityMethods.GetConfigPath(_configFile));

            var jsonLoader = new JsonLoader();
            var itemData = jsonLoader.LoadFromJson<ItemDataCollection>(jsonString);

            var gi = new List<GenericItem>();
            foreach (var item in itemData.Items)
            {
                //there's probably a less stupid way of doing this with reflection but
                //my patience grows thin
                switch (item.Type)
                {
                    case "Leaf":
                        {
                            var leaf = new Leaf(item);
                            gi.Add(leaf);
                        }
                        break;
                    case "PineCone":
                        {
                            var pinecone = new PineCone(item);
                            gi.Add(pinecone);
                        }
                        break;
                }
            }
            return gi;
        }
    }
}
