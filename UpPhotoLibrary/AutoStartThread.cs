using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace UpPhotoLibrary
{
    public class AutoStartThread
    {
        Thread thread;
        public AutoStartThread(ThreadStart newThreadStart, ApartmentState newApartmentState = ApartmentState.MTA)
        {
            thread = new Thread(newThreadStart);
            thread.SetApartmentState(newApartmentState);
            thread.Start();
        }

        public void Join()
        {
            thread.Join();
        }

        public void Abort()
        {
            thread.Abort();
        }
    }
}
