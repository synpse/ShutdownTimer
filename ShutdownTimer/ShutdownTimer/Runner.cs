using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace ShutdownTimer
{
    class Runner
    {
        // Import DLL for monitor use
        [DllImport("user32.dll")]
        private static extern int SendMessage(int hWnd, int hMsg, int wParam, int lParam);

        // Import DLL for control over console being closed
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

        // Keeps it from getting garbage collected
        private ConsoleEventDelegate handler;

        // Delegate for console close
        private delegate bool ConsoleEventDelegate(int eventType);

        public void Setup()
        {
            // Console event (Used for detecting user closing it)
            handler = new ConsoleEventDelegate(ConsoleEventCallback);
            SetConsoleCtrlHandler(handler, true);
        }

        public void Run()
        {
            // Clear and print information
            Console.Clear();

            Console.WriteLine("Save your downloads and your planet <3");
            Console.WriteLine("Made by Tiago Alves - github.com/synpse\n");

            // Ask when to shutdown and give instructions
            Console.WriteLine("When to shutdown? (Hours:Minutes)");
            Console.WriteLine("1:30 => " +
                "Will shutdown in 1 hour and 30 minutes.");
            Console.Write("Time /> ");

            // Store time told by user
            int time = 0;

            // Store initial time told by user
            int initTime = 0;

            // Potentially unstable block of code (oh well...)
            try
            {
                // Get user input and try to parse it 
                string str = Console.ReadLine();

                string[] param = str.Split(':');

                time = int.Parse(param[0]) * 3600 + int.Parse(param[1]) * 60;

                initTime = time;
                Console.WriteLine();

                // Call shutdown process
                Process.Start("shutdown", $"/s /t {time}");
            }
            catch (Exception e)
            {
                // Error running, retry...
                Console.WriteLine($"Error when running command.");
                Thread.Sleep(1500);

                // Call method again using recursion
                Run();
            }

            while (time > 0)
            {
                // Information here
                Console.Clear();
                Console.WriteLine($"Close this window to cancel.");
                Console.WriteLine($"If in blackscreen mode, spam your " +
                    $"keyboard till your monitor(s) turns on. However, by " +
                    $"doing this will cause the process to be halted.");

                // Subtract time by one
                time--;

                // Check if we can turn off monitor yet
                if (initTime - time >= 10)
                {
                    Console.WriteLine($"Monitor off...");
                    SetMonitorInState(MonitorState.MonitorStateOff);
                }
                else if (initTime - time < 10)
                {
                    Console.WriteLine($"Monitor shutting down in: " +
                        $"{10 - (initTime - time)} seconds...");
                }

                // Format and print current time
                TimeSpan t = TimeSpan.FromSeconds(time);

                string tFormated = string.Format("{0:D2}H:{1:D2}M:{2:D2}S",
                    t.Hours,
                    t.Minutes,
                    t.Seconds);

                Console.WriteLine($"Time to shutdown: {tFormated} seconds...");

                // Cancel the process and turn on monitor(s)
                if (Console.KeyAvailable)
                {
                    Console.WriteLine("KEY PRESSED! Stopping shutdown...");
                    Thread.Sleep(4000);
                    Process.Start("shutdown", $"/a");

                    return;
                }

                // Wait 1 second
                Thread.Sleep(1000);
            }
        }

        // Monitor state method
        private void SetMonitorInState(MonitorState state)
        {
            SendMessage(0xFFFF, 0x112, 0xF170, (int)state);
        }

        // Event callback, in case the user tries to close the console
        private bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 2)
            {
                // Cancel everything and exit
                Console.WriteLine("Console window closing, " +
                    "stopping shutdown...");
                Thread.Sleep(1500);
                Process.Start("shutdown", $"/a");
            }
            return false;
        }
    }
}
