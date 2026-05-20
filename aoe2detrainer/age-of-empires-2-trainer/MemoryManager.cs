using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AgeOfEmpires2Trainer
{
    /// <summary>
    /// Provides low-level memory operations for the AoE2: DE process.
    /// Encapsulates Windows API calls for reading and writing process memory.
    /// </summary>
    public class MemoryManager : IDisposable
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        private const uint ProcessAllAccess = 0x1F0FFF;
        private IntPtr _processHandle;
        private bool _disposed = false;

        /// <summary>
        /// Initializes a new instance of the MemoryManager class and opens the AoE2: DE process.
        /// </summary>
        /// <param name="processName">Name of the game process (e.g., "AoE2DE_s").</param>
        /// <exception cref="InvalidOperationException">Thrown if the process is not found or cannot be opened.</exception>
        public MemoryManager(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            if (processes.Length == 0)
            {
                throw new InvalidOperationException($"Process '{processName}' not found. Ensure the game is running.");
            }

            int processId = processes[0].Id;
            _processHandle = OpenProcess(ProcessAllAccess, false, processId);
            if (_processHandle == IntPtr.Zero)
            {
                throw new InvalidOperationException("Failed to open process. Try running as Administrator.");
            }
        }

        /// <summary>
        /// Reads a 4-byte integer from the specified memory address.
        /// </summary>
        /// <param name="address">Memory address to read from.</param>
        /// <returns>The integer value read from memory.</returns>
        public int ReadInt32(IntPtr address)
        {
            byte[] buffer = new byte[4];
            if (!ReadProcessMemory(_processHandle, address, buffer, buffer.Length, out int bytesRead))
            {
                throw new InvalidOperationException($"Failed to read memory at {address.ToString("X")}.");
            }
            return BitConverter.ToInt32(buffer, 0);
        }

        /// <summary>
        /// Writes a 4-byte integer to the specified memory address.
        /// </summary>
        /// <param name="address">Memory address to write to.</param>
        /// <param name="value">Integer value to write.</param>
        public void WriteInt32(IntPtr address, int value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            if (!WriteProcessMemory(_processHandle, address, buffer, buffer.Length, out int bytesWritten))
            {
                throw new InvalidOperationException($"Failed to write memory at {address.ToString("X")}.");
            }
        }

        /// <summary>
        /// Writes a 4-byte float to the specified memory address.
        /// </summary>
        /// <param name="address">Memory address to write to.</param>
        /// <param name="value">Float value to write.</param>
        public void WriteFloat(IntPtr address, float value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            if (!WriteProcessMemory(_processHandle, address, buffer, buffer.Length, out int bytesWritten))
            {
                throw new InvalidOperationException($"Failed to write memory at {address.ToString("X")}.");
            }
        }

        /// <summary>
        /// Releases the process handle.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed && _processHandle != IntPtr.Zero)
            {
                CloseHandle(_processHandle);
                _processHandle = IntPtr.Zero;
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}
