using System;

namespace AgeOfEmpires2Trainer
{
    /// <summary>
    /// Manages in-game resources for Age of Empires II: Definitive Edition.
    /// Uses known memory offsets to modify food, wood, gold, and stone.
    /// </summary>
    public class ResourceManager
    {
        private readonly MemoryManager _memory;
        private readonly IntPtr _baseAddress;

        // Offsets relative to the base address for each resource (example values)
        private const int FoodOffset = 0x00;
        private const int WoodOffset = 0x04;
        private const int GoldOffset = 0x08;
        private const int StoneOffset = 0x0C;

        /// <summary>
        /// Initializes a new instance of the ResourceManager class.
        /// </summary>
        /// <param name="memory">An initialized MemoryManager instance.</param>
        /// <param name="baseAddress">Base memory address for player resources.</param>
        public ResourceManager(MemoryManager memory, IntPtr baseAddress)
        {
            _memory = memory ?? throw new ArgumentNullException(nameof(memory));
            _baseAddress = baseAddress;
        }

        /// <summary>
        /// Sets the amount of food.
        /// </summary>
        /// <param name="amount">New food value.</param>
        public void SetFood(int amount)
        {
            _memory.WriteInt32(_baseAddress + FoodOffset, amount);
        }

        /// <summary>
        /// Sets the amount of wood.
        /// </summary>
        /// <param name="amount">New wood value.</param>
        public void SetWood(int amount)
        {
            _memory.WriteInt32(_baseAddress + WoodOffset, amount);
        }

        /// <summary>
        /// Sets the amount of gold.
        /// </summary>
        /// <param name="amount">New gold value.</param>
        public void SetGold(int amount)
        {
            _memory.WriteInt32(_baseAddress + GoldOffset, amount);
        }

        /// <summary>
        /// Sets the amount of stone.
        /// </summary>
        /// <param name="amount">New stone value.</param>
        public void SetStone(int amount)
        {
            _memory.WriteInt32(_baseAddress + StoneOffset, amount);
        }

        /// <summary>
        /// Gets the current food value from memory.
        /// </summary>
        /// <returns>Current food amount.</returns>
        public int GetFood()
        {
            return _memory.ReadInt32(_baseAddress + FoodOffset);
        }

        /// <summary>
        /// Gets the current wood value from memory.
        /// </summary>
        /// <returns>Current wood amount.</returns>
        public int GetWood()
        {
            return _memory.ReadInt32(_baseAddress + WoodOffset);
        }

        /// <summary>
        /// Gets the current gold value from memory.
        /// </summary>
        /// <returns>Current gold amount.</returns>
        public int GetGold()
        {
            return _memory.ReadInt32(_baseAddress + GoldOffset);
        }

        /// <summary>
        /// Gets the current stone value from memory.
        /// </summary>
        /// <returns>Current stone amount.</returns>
        public int GetStone()
        {
            return _memory.ReadInt32(_baseAddress + StoneOffset);
        }
    }
}
