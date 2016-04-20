using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCD.System.TUIO
{
    public class TuioObjectContainer
    {
        public TuioObject TuioObject { get; set; }
        public IncomingType Type { get; set; }

        public TuioObjectContainer(TuioObject obj, IncomingType type)
        {
            TuioObject = obj;
            Type = type;
        }
    }
    public class TuioCursorContainer
    {
        public TuioCursor TuioCursor { get; set; }
        public IncomingType Type { get; set; }

        public TuioCursorContainer(TuioCursor cur, IncomingType type)
        {
            TuioCursor = cur;
            Type = type;
        }
    }
}
