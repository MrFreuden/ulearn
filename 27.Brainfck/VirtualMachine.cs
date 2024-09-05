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

		public VirtualMachine(string program, int memorySize)
		{
		}

		public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
		{
			throw new NotImplementedException();
		}

		public void Run()
		{
			throw new NotImplementedException();
		}
	}
}