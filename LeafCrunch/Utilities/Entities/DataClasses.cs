using LeafCrunch.Utilities.Animation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeafCrunch.Utilities.Entities
{
    public class ConditionData
    {
        public string PropertyName { get; set; }
        public object Value { get; set; }
        public string Comparison { get; set; }
        public string ValueType { get; set; }
    }

    public class MenuText
    {
        public string Text { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Transparent { get; set; }
    }

    public class MenuData
    {
        public string Type { get; set; }
        public string Display { get; set; }
        public string BackgroundImagePath { get; set; }
        public string ActivationKey { get; set; }
        public MenuText Text { get; set; } //we probably want to be able to have some more formatting and some controls but this is good enough for testing
    }

    //I mean I call it menus but this includes anything that pauses gameplay tbh
    public class MenuCollection
    {
        public List<MenuData> Menus { get; set; }
    }

    public class RoomData
    {
        public string Name { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int TileSizeH { get; set; }
        public int TileSizeW { get; set; }
        public int InitialPlayerX { get; set; }
        public int InitialPlayerY { get; set; }
        public string BackgroundImagePath { get; set; }
        public List<ConditionData> WinConditions { get; set; }
        public List<ConditionData> LoseConditions { get; set; }
        public string RoomType { get; set; }
        public string ContinueKey { get; set; }
        public string StatsBackgroundImage { get; set; }
    }

    public class ObstacleData
    {
        public string Type { get; set; }
        public string SingleImage { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int InitialSpeedX { get; set; }
        public int InitialSpeedY { get; set; }

        public string OperationName { get; set; }
    }

    public class ObstacleDataCollection
    {
        public List<ObstacleData> Obstacles { get; set; }
    }

    public class DisplayControlData
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class ItemData
    {
        public string Type { get; set; }
        public string Operation { get; set; }
        public string SingleImage { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
     //   public string ActivationKey { get; set; } //I don't use this anywhere yet but I might
        
        //honestly I should really have a property list here
        //so I'm not limited to the exact things here
        //but I can fix that later on
        public int PointIncrement { get; set; }
        public int PointMultiplier { get; set; }

        public DisplayControlData DisplayControl { get; set; }
    }

    public class ItemDataCollection
    {
        public List<ItemData> Items { get; set; }
    }

    public class ParameterData
    {
        public string Name { get; set; }
        public string Value { get; set; } //I guess this could be a json string representing a more complex object if I need it to be but rn it's just numbers tbh
        public string ValueType { get; set; } //tells us what to convert to so we can make this a little more generic
        //if it says "property" then we look for a property in the initializing class.
    }

    public class OperationData
    {
        //the thing that'll make this tricky is we'll either need some kind of reflection
        //or some kind of hash table with registered operation methods
        //so we can grab it
        //that could be an alright way to start. 
        public string TargetName { get; set; }
        public string OperationName { get; set; }
        public string MethodToExecute { get; set; }
        public string TargetType { get; set; }
        public List<ParameterData> ParameterList { get; set; }
    }

    public class OperationDataCollection
    {
        public List<OperationData> OperationList { get; set; }
    }

    public class ImageSequenceData
    {
        public List<string> ImagePaths { get; set; }
    }

    public class DirectionalSpriteData
    {
        public ImageSequenceData Default { get; set; }
        public ImageSequenceData South { get; set; }
        public ImageSequenceData North { get; set; }
        public ImageSequenceData East { get; set; }
        public ImageSequenceData West { get; set; }
    }

    public class SpriteData
    {
        public DirectionalSpriteData Static { get; set; }
        public DirectionalSpriteData Moving { get; set; }
    }

    public class StatsData
    {
        public int InitialSpeedX { get; set; }
        public int InitialSpeedY { get; set; }

        public int InitialX { get; set; }
        public int InitialY { get; set; }

        public int MaxPoints { get; set; } //not everything needs this...we can always make it 0
    }

    public class PlayerData
    {
        public StatsData Stats {get; set;}
        public SpriteData SpriteData {get; set;}

        //it should just ignore this right?
        public AnimatedSprite Sprite { get; set;}
    }

    //load stuff from a json file into an entity
    public class JsonLoader
    {
        public string JsonString { get; set; }
        public T LoadFromJson<T>(string jsonString)
        {
            return JsonSerializer.Deserialize<T>(jsonString);
        }
    }

    //to do....figure out a cleaner way to do this
    //might need some like factory classes or whatever for initializing types
    //this gets the json stuff into a form the image sequence animation class can use
    public class ImageSequenceLoader: JsonLoader
    {
        public List<Image> Load(string jsonString)
        {
            var imageSequence = LoadFromJson<ImageSequenceData>(jsonString);
            if (imageSequence == null) throw new Exception(); //we shouldn't continue if we can't load images

            return LoadFromSequenceData(imageSequence);
        }

        public List<Image> LoadFromSequenceData(ImageSequenceData data)
        {
            var images = new List<Image>();
            foreach (var path in data.ImagePaths)
            {
                images.Add(UtilityMethods.ImageFromPath(path));
            }
            return images;
        }
    }

    public class DirectionalSpriteLoader : JsonLoader
    {
        public Dictionary<Direction, ImageSequence> Load(string jsonString)
        {
            var spriteData = LoadFromJson<DirectionalSpriteData>(jsonString);
            if (spriteData == null) throw new Exception();
            return LoadFromData(spriteData);   
        }

        public Dictionary<Direction, ImageSequence> LoadFromData(DirectionalSpriteData spriteData)
        {
            var loader = new ImageSequenceLoader();
            return new Dictionary<Direction, ImageSequence>()
            {
                { Direction.None, new ImageSequence(loader.LoadFromSequenceData(spriteData.Default)) },
                { Direction.South, new ImageSequence(loader.LoadFromSequenceData(spriteData.South)) },
                { Direction.North, new ImageSequence(loader.LoadFromSequenceData(spriteData.North)) },
                { Direction.East, new ImageSequence(loader.LoadFromSequenceData(spriteData.East)) },
                { Direction.West, new ImageSequence(loader.LoadFromSequenceData(spriteData.West)) }
            };
        }
    }

    public class SpriteLoader : JsonLoader
    {
        public Dictionary<string, Dictionary<Direction, ImageSequence>> LoadToDictionary(string jsonString)
        {
            var spriteData = LoadFromJson<SpriteData>(jsonString);
            var loader = new DirectionalSpriteLoader();
            return new Dictionary<string, Dictionary<Direction, ImageSequence>>()
            {
                { "Static", loader.LoadFromData(spriteData.Static) },
                { "Moving", loader.LoadFromData(spriteData.Moving) }
            };
        }

        public AnimatedSprite Load(string jsonString)
        {
            var spriteData = LoadFromJson<SpriteData>(jsonString);
            var loader = new DirectionalSpriteLoader();
            return new AnimatedSprite(
                loader.LoadFromData(spriteData.Static),
                loader.LoadFromData(spriteData.Moving));
        }
    }

    public class PlayerLoader: JsonLoader
    {
        public PlayerData Load(string jsonString)
        {
            var playerData = LoadFromJson<PlayerData>(jsonString);
            var spriteData = playerData.SpriteData;
            var loader = new DirectionalSpriteLoader();
            playerData.Sprite = new AnimatedSprite(
                loader.LoadFromData(spriteData.Static),
                loader.LoadFromData(spriteData.Moving));
            return playerData;
        }
    }
}
