using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AI_Z1.InputMethods
{
	public class TextFileInputMethod : IInputMethod
	{
		private readonly string fileName;

		private StreamReader sr;
		private Stream stream;

		public TextFileInputMethod(string fileName)
		{
			this.fileName = fileName;
		}

		public bool SetupStream()
		{
			try
			{
				if (File.Exists(fileName))
				{
					stream = File.OpenRead(fileName);
					sr = new StreamReader(stream);

					return true;
				}
			}
			catch
			{
				if (sr != null)
				{
					sr.Dispose();
				}

				if (stream != null)
				{
					stream.Dispose();
				}
			}

			return false;
		}

		public void Dispose()
		{
			sr.Dispose();
			stream.Dispose();
		}

		public string ReadLine()
		{
			return sr.ReadLine();
		}
	}
}
