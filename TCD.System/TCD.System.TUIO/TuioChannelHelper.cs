
namespace TCD.System.TUIO
{
    internal class TuioChannelHelper : TuioListener
    {
        #region Events
        //OnIncomingTuioObject
        public delegate void AddOnIncomingTuioObjectDelegate(TuioObject obj, IncomingType type);
        public static event AddOnIncomingTuioObjectDelegate OnIncomingTuioObject;
        //OnIncomingTuioCursor
        public delegate void AddOnIncomingTuioCursorDelegate(TuioCursor cur, IncomingType type);
        public static event AddOnIncomingTuioCursorDelegate OnIncomingTuioCursor;
        
        //OnTuioRefresh
        public delegate void AddOnTuioRefreshDelegate(TuioTime t);
        public static event AddOnTuioRefreshDelegate OnTuioRefresh;
        #endregion

        #region Incoming
        public void addTuioObject(TuioObject o)
        {
            try { OnIncomingTuioObject(o, IncomingType.New); }
            catch { }
        }

        public void updateTuioObject(TuioObject o)
        {
            try { OnIncomingTuioObject(o, IncomingType.Update); }
            catch { }
        }

        public void removeTuioObject(TuioObject o)
        {
            try { OnIncomingTuioObject(o, IncomingType.Remove); }
            catch { }
        }

        public void addTuioCursor(TuioCursor c)
        {
            try { OnIncomingTuioCursor(c, IncomingType.New); }
            catch { }
        }

        public void updateTuioCursor(TuioCursor c)
        {
            try { OnIncomingTuioCursor(c, IncomingType.Update); }
            catch { }
        }

        public void removeTuioCursor(TuioCursor c)
        {
            try { OnIncomingTuioCursor(c, IncomingType.Remove); }
            catch { }
        }

        public void refresh(TuioTime frameTime)
        {
            try { OnTuioRefresh(frameTime); }
            catch { }
        }
        #endregion
    }
}
