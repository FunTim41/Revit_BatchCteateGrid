using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BatchCteateGrid.XmlSer
{
    [XmlType(TypeName = "List")]
    public class GridList
    {
        [XmlArray("gridListX")]
        public List<GridInfo> gridListX { get; set; }
        [XmlArray("gridListY")]
        public List<GridInfo> gridListY { get; set; }
    }
}
