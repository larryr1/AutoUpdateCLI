using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Timers;

namespace AutoUpdate_CLI.Classes.Utility
{
    internal class SleepPrevention
    {
        private static System.Timers.Timer _timer;

        [FlagsAttribute]
        public enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001
            // Legacy flag, should not be used.
            // ES_USER_PRESENT = 0x00000004
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        public static void DisableSleep()
        {
            _timer = new System.Timers.Timer(30000);
            // Hook up the Elapsed event for the timer. 
            _timer.Elapsed += TimerTriggered;
            _timer.AutoReset = true;
            _timer.Enabled = true;

            KeepAwake();
        }

        public static void AllowSleep()
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
        }

        private static void TimerTriggered(Object source, ElapsedEventArgs e)
        {
            KeepAwake();
        }

        private static void KeepAwake()
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED);
            Debug.WriteLine("Thread execution state set.");
        }
    }
}
