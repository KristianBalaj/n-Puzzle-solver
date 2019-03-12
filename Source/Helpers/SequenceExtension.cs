using System;
using System.Collections.Generic;
using System.Text;

namespace SequenceExtension
{
	public static class SequenceExtension
	{
		public static int[] Convert2dArray2Sequence(this int[,] state)
		{
			int[] result = new int[state.Length];

			int i = 0;

			foreach (var x in state)
			{
				result[i] = x;
				i++;
			}

			return result;
		}

		public static int[] SwapByCopy(this int[] currentState, int id1, int id2)
		{
			var newState = (int[])currentState.Clone();

			int temp = newState[id1];

			newState[id1] = newState[id2];
			newState[id2] = temp;

			return newState;
		}
	}
}
