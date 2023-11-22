using LeafCrunch.GameObjects.Items.Obstacles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeafCrunch.Utilities.Entities
{
    //i could probably have a base class with a load method tbh
    public class ObstacleFactory
    {
        private const string _configFile = "obstacles.json";

        private string AppendRoomName(string roomName)
        {
            return (string.IsNullOrEmpty(roomName)) ? string.Empty : "Rooms/" + roomName + "/";
        }

        public List<Obstacle> LoadObstacles(string roomName = null)
        {
            var jsonString = File.ReadAllText(UtilityMethods.GetConfigPath($"{AppendRoomName(roomName)}{_configFile}"));

            var jsonLoader = new JsonLoader();
            var obstacleData = jsonLoader.LoadFromJson<ObstacleDataCollection>(jsonString);

            var obstacles = new List<Obstacle>();
            foreach (var obstacle in obstacleData.Obstacles)
            {
                //there's probably a less stupid way of doing this with reflection but
                //my patience grows thin
                switch (obstacle.Type)
                {
                    case "Obstacle":
                        {
                            var o = new Obstacle(obstacle);
                            obstacles.Add(o);
                        }
                        break;
                    case "HazardousObstacle":
                        {
                            var o = new HazardousObstacle(obstacle);
                            obstacles.Add(o);
                        }
                        break;
                    case "MovingObstacle":
                        {
                            var o = new MovingObstacle(obstacle);
                            obstacles.Add(o);
                        }
                        break;
                    case "HazardousMovingObstacle":
                        {
                            var o = new HazardousMovingObstacle(obstacle);
                            obstacles.Add(o);
                        }
                        break;
                }
            }
            return obstacles;
        }
    }
}
