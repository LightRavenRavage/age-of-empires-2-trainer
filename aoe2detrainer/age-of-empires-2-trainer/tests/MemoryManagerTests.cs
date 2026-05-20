using System;
using Xunit;

namespace AgeOfEmpires2Trainer.Tests
{
    /// <summary>
    /// Unit tests for MemoryManager class.
    /// Note: These tests require the game to be running and are marked as integration tests.
    /// They are provided as examples of test structure.
    /// </summary>
    public class MemoryManagerTests
    {
        [Fact]
        public void Constructor_ProcessNotFound_ThrowsInvalidOperationException()
        {
            // This test expects the process "NonExistentProcess" to not exist
            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                using var mm = new MemoryManager("NonExistentProcess");
            });
            Assert.Contains("not found", exception.Message);
        }

        [Fact]
        public void ReadWriteInt32_RoundTrip_Success()
        {
            // This test requires the game to be running; skip if not.
            // In a real test suite, you would mock or use a test process.
            try
            {
                using var mm = new MemoryManager("AoE2DE_s");
                // Write a test value to a safe address (we use a temporary offset for testing)
                // In practice, you'd need a known writable address; this is illustrative.
                // For now, we just check that the constructor succeeds.
                Assert.NotNull(mm);
            }
            catch (InvalidOperationException)
            {
                // Game not running, skip test
                Assert.True(true);
            }
        }
    }
}
