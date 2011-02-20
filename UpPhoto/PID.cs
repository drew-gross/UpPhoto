using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpPhoto
{
    public class PID
    {
        public String pidStr = String.Empty;
        
        public PID(String newPid)
        {
            pidStr = newPid;
        }
        public override string ToString()
        {
            return pidStr;
        }
        public override bool Equals(object obj)
        {
            if ((obj == null) || (obj as PID == null)) return false;
            return pidStr.Equals((obj as PID).pidStr);
        }
        public override int GetHashCode()
        {
            return pidStr.GetHashCode();
        }
    }
}