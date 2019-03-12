using System;
using System.Collections.Generic;
using System.Text;

namespace AI_Z1
{
	public class SolverInput
	{
		public readonly int MaxIterationsCount;
		public readonly int Width;
		public readonly int Height;
		public readonly int[,] InitialState;
		public readonly int[,] FinalState;

		public SolverInput(int maxIterationsCount, int width, int height, int[,] initialState, int[,] finalState)
		{
			this.MaxIterationsCount = maxIterationsCount;
			this.Height = height;
			this.Width = width;
			this.InitialState = initialState;
			this.FinalState = finalState;
		}

		public bool IsInputCorrect()
		{
			bool initialStateSizeCorrectness = InitialState.GetLength(0) == Height && InitialState.GetLength(1) == Width;
			bool finalStateSizeCorrectness = FinalState.GetLength(0) == Height && FinalState.GetLength(1) == Width;

			return initialStateSizeCorrectness && finalStateSizeCorrectness && testStateCorrectness(InitialState) && testStateCorrectness(FinalState);
		}

		private bool testStateCorrectness(int[,] state)
		{
			int[] temp = new int[Height * Width];

			foreach (var item in state)
			{
				if (item < 0)
				{
					return false;
				}

				temp[item]++;

				if (temp[item] > 1)
				{
					return false;
				}
			}

			return true;
		}
	}
}
