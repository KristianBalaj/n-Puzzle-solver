using System;
using System.Collections.Generic;
using System.Text;

namespace AI_Z1
{
	public class SolverResult
	{
		public readonly int Height;
		public readonly int Width;
		public readonly List<int[]> ResultSequence;

		public SolverResult(int height, int width, List<int[]> resultSequence)
		{
			Height = height;
			Width = width;
			ResultSequence = resultSequence;
		}
	}
}
