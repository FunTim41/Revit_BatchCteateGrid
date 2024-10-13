using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BatchCteateGrid
{
    [XmlType(typeName: "Info")]
    public class GridInfo
    {
        [XmlAttribute]
        public double Distance { get; set; }
        [XmlAttribute]
        public int Count { get; set; }

    }
}
