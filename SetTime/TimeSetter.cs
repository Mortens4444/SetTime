using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SetTime
{
    class TimeSetter
    {
        [DllImport("Kernel32.dll")]
        static extern uint GetLastError();

        [DllImport("Kernel32.dll", SetLastError = true)]
        static extern bool SetSystemTime(ref SystemTime systemTime);

        public void Set(SystemTime systemTime)
        {
            var result = SetSystemTime(ref systemTime);
            if (!result)
            {
                GetLastError();
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }
    }
}
