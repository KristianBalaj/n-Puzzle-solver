using System;
using System.Collections.Generic;
using System.Text;

namespace AI_Z1
{
	public static class GridLogger
	{
		public static void Log(int height, int width, int[] state)
		{
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					Console.Write(state[y * width + x]);
					Console.Write(" ");
				}

				Console.WriteLine();
			}

			Console.WriteLine();
		}
	}
}
