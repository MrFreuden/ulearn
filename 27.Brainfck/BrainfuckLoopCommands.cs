using System;
using System.Collections.Generic;

namespace func.brainfuck
{
    public class BrainfuckLoopCommands
    {
        public static void RegisterTo(IVirtualMachine vm)
        {
            var openToCloseMap = new Dictionary<int, int>();
            var closeToOpenMap = new Dictionary<int, int>();
            MapBrackets(openToCloseMap, closeToOpenMap, vm.Instructions);
            vm.RegisterCommand('[', b =>
            {
                if (b.Memory[b.MemoryPointer] == 0)
                {
                    b.InstructionPointer = openToCloseMap[b.InstructionPointer];
                }
            });

            vm.RegisterCommand(']', b =>
            {
                if (b.Memory[b.MemoryPointer] != 0)
                {
                    b.InstructionPointer = closeToOpenMap[b.InstructionPointer];
                }
            });
        }

        private static void MapBrackets(Dictionary<int, int> openToCloseMap, Dictionary<int, int> closeToOpenMap, string instructions)
        {
            var stack = new Stack<int>();
            for (int i = 0; i < instructions.Length; i++)
            {
                if (instructions[i] == '[')
                {
                    stack.Push(i);
                }
                else if (instructions[i] == ']')
                {
                    if (stack.Count == 0) throw new ArgumentException("Unmatched closing bracket at position " + i);
                    int openIndex = stack.Pop();
                    openToCloseMap[openIndex] = i;
                    closeToOpenMap[i] = openIndex;
                }
            }
            if (stack.Count > 0) throw new ArgumentException("Unmatched opening bracket at position " + stack.Peek());
        }
    }
}