using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SequenceExtension;
using System.IO;

namespace AI_Z1
{
	public static class UIController
	{
		private const int RESULT_STATES_PARTLY_DISPLAY_COUNT = 5;

		public static void ProcessSolverResult(SolverResult solverResult)
		{
			Console.WriteLine("Result:");

			if (solverResult == null)
			{
				Console.WriteLine("Didn't find solution in the given iterations.");
			}
			else
			{
				bool listAllPossibilities;
				Console.WriteLine("Solution has {0} steps.", solverResult.ResultSequence.Count);
				Console.WriteLine();

				Console.WriteLine("Applied operators:");
				Console.WriteLine();

				foreach (var op in solverResult.AppliedOperators)
				{
					Console.WriteLine(op);
				}

				Console.WriteLine();

				while (true)
				{
					Console.Write("Do you want to list all {0} steps? Y/N ", solverResult.ResultSequence.Count);

					var input = Console.ReadLine();
					var decision = processDecision(input);

					if (decision == null)
					{
						continue;
					}

					listAllPossibilities = (bool)decision;
					break;
				}

				displayResult(solverResult, listAllPossibilities);
			}
		}

		public static IInputMethod SetupInputMethod()
		{
			while (true)
			{
				Console.WriteLine("Input method?");
				Console.WriteLine("1 - Text file");
				Console.WriteLine("2 - Console");

				if (int.TryParse(Console.ReadLine(), out int result))
				{
					switch (result)
					{
						case 1:

							printAvailableTxtFiles();

							Console.WriteLine("What file to process?");
							var fileName = Console.ReadLine();

							var textInput = new InputMethods.TextFileInputMethod(fileName);
							if (!textInput.SetupStream())
							{
								Console.WriteLine("Problem processing the file.");
								continue;
							}

							return textInput;

						case 2:

							return new InputMethods.ConsoleInputMethod();
					}
				}
			}
		}

		/// <summary>
		/// Returns false if the input was wrong.
		/// </summary>
		public static bool ProcessUserInput(IInputMethod inputMethod, out SolverInput solverInputModel)
		{
			solverInputModel = null;

			int maxIterationsCount = 0;
			int width = 0;
			int height = 0;
			int[,] startState = null;
			int[,] finalState = null;

			try
			{
				Console.Write("Max iterations count: ");
				maxIterationsCount = Math.Abs(int.Parse(inputMethod.ReadLine()));

				Console.Write("Width: ");
				width = Math.Abs(int.Parse(inputMethod.ReadLine()));

				Console.Write("Height: ");
				height = Math.Abs(int.Parse(inputMethod.ReadLine()));

				startState = new int[height, width];
				finalState = new int[height, width];

				Console.WriteLine();
				Console.WriteLine("Max iterations count: {0}", maxIterationsCount);
				Console.WriteLine("Width: {0}", width);
				Console.WriteLine("Height: {0}", height);
				Console.WriteLine();

				Console.WriteLine("Insert the start state (line format - \"val1, val2, ...\" eg. 0, 1, 2):");
				Console.WriteLine("Value {0} is the empty space.", Solver.EMPTY_SPACE_REPRESENTATION);

				processGridInput(inputMethod, width, height, startState);

				Console.WriteLine();
				GridLogger.Log(width, height, startState.Convert2dArray2Sequence());

				Console.WriteLine("Insert the final state:");

				processGridInput(inputMethod, width, height, finalState);

				Console.WriteLine();
				GridLogger.Log(width, height, finalState.Convert2dArray2Sequence());
			}
			catch (Exception e)
			{
				Console.WriteLine(string.Format("Error: {0}", e.Message));
				return false;
			}


			solverInputModel = new SolverInput(maxIterationsCount, width, height, startState, finalState);
			return true;
		}

		/// <summary>
		/// Returns true if user wants to exit.
		/// </summary>
		/// <returns></returns>
		public static bool ProcessExit()
		{
			while (true)
			{
				Console.Write("Exit? N/Y: ");
				var input = Console.ReadLine();

				var decision = processDecision(input);
				if (decision == null)
				{
					continue;
				}

				return (bool)decision;
			}
		}

		private static void displayResult(SolverResult result, bool displayAll)
		{
			Console.WriteLine();

			if (displayAll || result.ResultSequence.Count <= RESULT_STATES_PARTLY_DISPLAY_COUNT * 2)
			{
				foreach (var state in result.ResultSequence)
				{
					GridLogger.Log(result.Height, result.Width, state);
				}
			}
			else
			{
				for (int i = 0; i < RESULT_STATES_PARTLY_DISPLAY_COUNT; i++)
				{
					GridLogger.Log(result.Height, result.Width, result.ResultSequence[i]);
				}

				Console.WriteLine(".");
				Console.WriteLine(".");
				Console.WriteLine(".");
				Console.WriteLine("Other {0} states.", result.ResultSequence.Count - RESULT_STATES_PARTLY_DISPLAY_COUNT * 2);
				Console.WriteLine(".");
				Console.WriteLine(".");
				Console.WriteLine(".");

				for (int i = result.ResultSequence.Count - RESULT_STATES_PARTLY_DISPLAY_COUNT - 1; i < result.ResultSequence.Count; i++)
				{
					GridLogger.Log(result.Height, result.Width, result.ResultSequence[i]);
				}
			}
		}

		private static bool? processDecision(string input)
		{
			var parsedInput = input[0];

			switch (char.ToLower(parsedInput))
			{
				case 'n':
					return false;
				case 'y':
					return true;
				default:
					Console.WriteLine("Unknown input. Try again.");
					return null;
			}
		}

		private static void printAvailableTxtFiles()
		{
			Console.WriteLine();

			var dir = new DirectoryInfo(@".");
			var files = dir.GetFiles(@"*");

			Console.WriteLine("Available files in this directory:");

			foreach (var file in files)
			{
				Console.WriteLine(file.Name);
			}
		}

		private static void processGridInput(IInputMethod inputMethod, int width, int height, int[,] state)
		{
			for (int y = 0; y < height; y++)
			{
				Console.WriteLine("Line id {0}:", y);

				string line = inputMethod.ReadLine();
				var lineVals = Regex.Split(line, @",");

				if (lineVals.Length != width)
				{
					throw new Exception("Wrong number of values.");
				}

				for (int x = 0; x < width; x++)
				{
					state[y, x] = int.Parse(lineVals[x]);
				}
			}
		}

	}
}
