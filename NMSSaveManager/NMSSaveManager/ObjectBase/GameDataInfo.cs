using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMSSaveManager.ObjectBase
{
    [Serializable]
    public class GameDataInfo
    {
        public String Name { get; set; }
        public String SavePath { get; set; }
        public String DirectoryName { get; set; }
        public DateTime LastSaveTime { get; set; }
        public GameDataInfo() { Name = DirectoryName = SavePath = String.Empty; LastSaveTime = new DateTime(); }
        public GameDataInfo(String name, String directoryName, String savePath) { Name = name; DirectoryName = directoryName; SavePath = savePath; LastSaveTime = new DateTime(); }
        public GameDataInfo(String name, String directoryName, String savePath, DateTime lastSaveTime) { Name = name; DirectoryName = directoryName; SavePath = savePath; LastSaveTime = lastSaveTime; }
    }
}
