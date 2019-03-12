using System;
using System.Collections.Generic;
using System.Text;

namespace AI_Z1
{
	public interface IInputMethod : IDisposable
	{
		string ReadLine();
	}
}
