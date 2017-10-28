using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamux.GameModel
{
    public class BaseModel
    {
        public readonly IList<string> tags;
        public IDictionary<BiomeTypes, float> biomeInstantiationProbabilities = new Dictionary<BiomeTypes, float>();        
        public string state;
    }

    public class Plant : BaseModel
    {
    }

    public class Tree : Plant
    {
        
    }


}
