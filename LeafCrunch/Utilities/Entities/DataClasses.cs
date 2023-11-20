using LeafCrunch.Utilities.Animation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LeafCrunch.Utilities.Entities
{
    //let's try some basic stuff
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
        public Dictionary<string, Dictionary<Direction, ImageSequence>> Load(string jsonString)
        {
            var spriteData = LoadFromJson<SpriteData>(jsonString);
            var loader = new DirectionalSpriteLoader();
            return new Dictionary<string, Dictionary<Direction, ImageSequence>>()
            {
                { "static", loader.LoadFromData(spriteData.Static) },
                { "moving", loader.LoadFromData(spriteData.Moving) }
            };
        }
    }
}
