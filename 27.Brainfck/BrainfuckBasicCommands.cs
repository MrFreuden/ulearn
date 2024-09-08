using System;

namespace func.brainfuck
{
    public class BrainfuckBasicCommands
    {
        public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
        {
            vm.RegisterCommand('.', b => { write((char)b.Memory[b.MemoryPointer]); });
            vm.RegisterCommand('+', b => { unchecked { b.Memory[b.MemoryPointer]++; } });
            vm.RegisterCommand('-', b => { unchecked { b.Memory[b.MemoryPointer]--; } });
            vm.RegisterCommand('>', b => { b.MemoryPointer = b.MemoryPointer == b.Memory.Length - 1 ? 0 : b.MemoryPointer + 1; } );
            vm.RegisterCommand('<', b => { b.MemoryPointer = b.MemoryPointer == 0 ? b.Memory.Length - 1 : b.MemoryPointer - 1; } );
             
            vm.RegisterCommand(',', b =>
            {
                var input = read();
                if (input != -1)
                {
                    b.Memory[b.MemoryPointer] = (byte)input;
                }
            });

            RegisterRangeOfCommands(vm, 'A', 'Z');
            RegisterRangeOfCommands(vm, 'a', 'z');
            RegisterRangeOfCommands(vm, '0', '9');
        }

        private static void RegisterRangeOfCommands(IVirtualMachine vm, char start, char end)
        {
            for (char i = start; i <= end; i++)
            {
                var ch = i;
                vm.RegisterCommand(i, b => { b.Memory[b.MemoryPointer] = (byte)ch; });
            }
        }
    }
}