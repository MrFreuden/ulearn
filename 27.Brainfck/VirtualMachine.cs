using System;
using System.Collections.Generic;

namespace func.brainfuck
{
    public class VirtualMachine : IVirtualMachine
    {
        public string Instructions { get; }
        public int InstructionPointer { get; set; }
        public byte[] Memory { get; }
        public int MemoryPointer { get; set; }
        private Dictionary<char, Action<IVirtualMachine>> _actions;

        public VirtualMachine(string program, int memorySize)
        {
            Instructions = program;
            Memory = new byte[memorySize];
            InstructionPointer = 0;
            MemoryPointer = 0;
            _actions = new();
        }

        public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
        {
            _actions.Add(symbol, execute);
        }

        public void Run()
        {
            while (InstructionPointer < Instructions.Length)
            {
                if (_actions.TryGetValue(Instructions[InstructionPointer], out Action<IVirtualMachine> action))
                {
                    action.Invoke(this);
                }

                InstructionPointer++;
            }
        }
    }
}