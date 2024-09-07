using System;
using System.Collections.Generic;
using System.Linq;

namespace func.brainfuck
{
	public class BrainfuckBasicCommands
	{
		public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
		{
			vm.RegisterCommand('.', b => { });
			vm.RegisterCommand('+', b => {});
			vm.RegisterCommand('-', b => {});
            vm.RegisterCommand('>', b => { });
            vm.RegisterCommand('<', b => { });
            //vm.RegisterCommand('', b => { });
			//A-Z, a-z, 0-9	сохранить ASCII-код этого символа в байт памяти, на который указывает указатель


        }
    }
}