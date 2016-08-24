using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMSSaveManager.ObjectBase.Collections
{
    [Serializable]
    public class GameDataInfoCollection : List<GameDataInfo>
    {
        public GameDataInfo this[String name]
        {
            get { return this.Single(g => g.Name == name); }
        }

        public bool Contains(String name)
        {
            return this.Any(g => g.Name == name);
        }
    }
}
