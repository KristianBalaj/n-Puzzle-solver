using System;
using System.Collections.Generic;
using System.Text;

namespace AI_Z1.InputMethods
{
	public class ConsoleInputMethod : IInputMethod
	{
		public void Dispose() { }

		public string ReadLine()
		{
			return Console.ReadLine();
		}
	}
}
