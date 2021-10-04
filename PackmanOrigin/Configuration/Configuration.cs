using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackmanOrigin
{
    static public class Configuration
    {
       
        public static int TotalPoints { get;  set; }

        private  const  int _TileSize = 24;
        public static int TileSize { get { return _TileSize; } }


       
    }
}
