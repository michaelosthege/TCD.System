using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCD.System.TUIO
{
    public class TuioChannel
    {
        #region Events
        //OnIncomingTuioObject
        public delegate void AddOnIncomingTuioObjectDelegate(TuioObject obj, IncomingType type);
        public static event AddOnIncomingTuioObjectDelegate OnIncomingTuioObject;
        public static void OnIncomingTuioObjectEvent(TuioObject obj, IncomingType type)
        {
            try { OnIncomingTuioObject(obj, type); }
            catch { }
        }
        //OnIncomingTuioCursor
        public delegate void AddOnIncomingTuioCursorDelegate(TuioCursor cur, IncomingType type);
        public static event AddOnIncomingTuioCursorDelegate OnIncomingTuioCursor;
        public static void OnIncomingTuioCursorEvent(TuioCursor cur, IncomingType type)
        {
            try { OnIncomingTuioCursor(cur, type); }
            catch { }
        }
        
        //OnTuioRefresh
        public delegate void AddOnTuioRefreshDelegate(TuioTime t);
        public static event AddOnTuioRefreshDelegate OnTuioRefresh;
        public static void OnTuioRefreshEvent(TuioTime t)
        {
            try { OnTuioRefresh(t); }
            catch { }
        }
        #endregion

        private TuioClient client;
        private TuioChannelHelper helper;

        public Dictionary<long, TuioObjectContainer> ObjectList { get; set; }
        public Dictionary<long, TuioCursorContainer> CursorList { get; set; }
        public bool IsRunning { get; set; }
        private object cursorSync = new object();
        private object objectSync = new object();

        public TuioChannel()
        {
            TuioChannelHelper.OnIncomingTuioObject += TuioChannelHelper_OnIncomingTuioObject;
            TuioChannelHelper.OnIncomingTuioCursor += TuioChannelHelper_OnIncomingTuioCursor;
            TuioChannelHelper.OnTuioRefresh += TuioChannelHelper_OnTuioRefresh;
            helper = new TuioChannelHelper();
        }

        public bool Connect(int port = 3333)
        {
            ObjectList = new Dictionary<long, TuioObjectContainer>();
            CursorList = new Dictionary<long, TuioCursorContainer>();
            try
            {
                client = new TuioClient(port);
                client.addTuioListener(helper);
                client.connect();
                IsRunning = true;
                return true;
            }
            catch
            {
                IsRunning = false;
                return false;
            }
        }
        public void Disconnect()
        {
            if (client == null) return;
            client.removeTuioListener(helper);
            client.disconnect();
            IsRunning = false;
        }

        private void TuioChannelHelper_OnIncomingTuioObject(TuioObject o, IncomingType type)
        {
            switch (type)
            {
                case IncomingType.New:
                    lock (objectSync)
                        ObjectList.Add(o.getSessionID(), new TuioObjectContainer(o, type));
                    break;
                case IncomingType.Update:
                    lock (objectSync)
                    {
                        ObjectList[o.getSessionID()].TuioObject.update(o);
                        ObjectList[o.getSessionID()].Type = type;
                    }
                    break;
                case IncomingType.Remove:
                    lock (objectSync)
                        ObjectList[o.getSessionID()].Type = type;//the one who produces the touch input is responsible for removing items
                    break;
            }
        }
        private void TuioChannelHelper_OnIncomingTuioCursor(TuioCursor c, IncomingType type)
        {
            switch (type)
            {
                case IncomingType.New:
                    lock (cursorSync)
                        CursorList.Add(c.getSessionID(), new TuioCursorContainer(c, type));
                    break;
                case IncomingType.Update:
                    lock (cursorSync)
                    {
                        CursorList[c.getSessionID()].TuioCursor.update(c);
                        CursorList[c.getSessionID()].Type = type;
                    }
                    break;
                case IncomingType.Remove:
                    lock (cursorSync)
                        CursorList[c.getSessionID()].Type = type;
                    break;
            }
        }
        private void TuioChannelHelper_OnTuioRefresh(TuioTime t)
        {
            OnTuioRefreshEvent(t);
        }
    }
}
