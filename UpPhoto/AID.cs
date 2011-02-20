using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpPhoto
{
    public class AID
    {
        public String aidStr = String.Empty;
        public AID(String newAID)
        {
            aidStr = newAID;
        }
        public override string ToString()
        {
            return aidStr;
        }
    }
}
