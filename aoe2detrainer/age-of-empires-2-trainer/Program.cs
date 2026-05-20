using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace AgeOfEmpires2Trainer
{
    /// <summary>
    /// Main entry point for the Age of Empires II: Definitive Edition Trainer.
    /// Provides in-game resource manipulation and game speed control via memory editing.
    /// </summary>
    internal class Program
    {
        // Import required Windows API functions for memory manipulation
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);

        private const uint ProcessAllAccess = 0x1F0FFF;

        // Example memory offsets for AoE2: DE (these are illustrative; actual offsets vary by version)
        // Food, Wood, Gold, Stone resources are often stored as 4-byte integers in a contiguous block.
        private static readonly IntPtr ResourceBaseOffset = new IntPtr(0x00A1B2C0); // Base address for player resources
        private static readonly IntPtr GameSpeedOffset = new IntPtr(0x00B3D4E0);   // Game speed multiplier

        private static IntPtr _processHandle;
        private static int _processId;

        static void Main(string[] args)
        {
            Console.WriteLine("Age of Empires II: Definitive Edition Trainer");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("Make sure the game is running before using this trainer.");
            Console.WriteLine("Press keys: F1 = 10000 Food, F2 = 10000 Wood, F3 = 10000 Gold, F4 = 10000 Stone");
            Console.WriteLine("F5 = Increase Game Speed, F6 = Reset Game Speed");
            Console.WriteLine("Press Escape to exit.\n");

            // Find the AoE2: DE process
            Process[] processes = Process.GetProcessesByName("AoE2DE_s");
            if (processes.Length == 0)
            {
                Console.WriteLine("Error: Age of Empires II: DE is not running.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            _processId = processes[0].Id;
            _processHandle = OpenProcess(ProcessAllAccess, false, _processId);
            if (_processHandle == IntPtr.Zero)
            {
                Console.WriteLine("Error: Could not open process. Try running as Administrator.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Connected to process ID: " + _processId);
            Console.WriteLine("Trainer active. Use keys as described above.\n");

            // Main loop: listen for key presses
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.F1:
                            SetResource(ResourceBaseOffset, 10000);
                            Console.WriteLine("Food set to 10000.");
                            break;
                        case ConsoleKey.F2:
                            SetResource(ResourceBaseOffset + 4, 10000); // Wood offset (example: +4 bytes)
                            Console.WriteLine("Wood set to 10000.");
                            break;
                        case ConsoleKey.F3:
                            SetResource(ResourceBaseOffset + 8, 10000); // Gold offset
                            Console.WriteLine("Gold set to 10000.");
                            break;
                        case ConsoleKey.F4:
                            SetResource(ResourceBaseOffset + 12, 10000); // Stone offset
                            Console.WriteLine("Stone set to 10000.");
                            break;
                        case ConsoleKey.F5:
                            SetGameSpeed(2.0f);
                            Console.WriteLine("Game speed doubled.");
                            break;
                        case ConsoleKey.F6:
                            SetGameSpeed(1.0f);
                            Console.WriteLine("Game speed reset to normal.");
                            break;
                        case ConsoleKey.Escape:
                            Console.WriteLine("Exiting trainer...");
                            CloseHandle(_processHandle);
                            return;
                    }
                }
                Thread.Sleep(100); // Prevent CPU overuse
            }
        }

        /// <summary>
        /// Writes an integer value to a specified memory address in the game process.
        /// </summary>
        /// <param name="address">Memory address to write to.</param>
        /// <param name="value">Integer value to write.</param>
        private static void SetResource(IntPtr address, int value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            if (!WriteProcessMemory(_processHandle, address, buffer, buffer.Length, out int bytesWritten))
            {
                Console.WriteLine("Failed to write memory at " + address.ToString("X") + ". Check offsets.");
            }
        }

        /// <summary>
        /// Modifies the game speed multiplier by writing a float value to memory.
        /// </summary>
        /// <param name="speed">Speed multiplier (e.g., 1.0 normal, 2.0 double).</param>
        private static void SetGameSpeed(float speed)
        {
            byte[] buffer = BitConverter.GetBytes(speed);
            if (!WriteProcessMemory(_processHandle, GameSpeedOffset, buffer, buffer.Length, out int bytesWritten))
            {
                Console.WriteLine("Failed to write game speed. Check offset.");
            }
        }
    }
}
