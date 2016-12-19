using System.Threading;

namespace SampleBusinessCode
{
    public class LengthyStuff
    {
        public bool DoLenghtyOperation(InfoObject info)
        {
            info.Log("In LenghtyOperation, before Sleep");
            Thread.Sleep(info.MillisToSleep);
            info.Log("In LenghtyOperation, after Sleep");

            return true;
        }
    }
}
