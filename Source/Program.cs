using System;
using System.Text.RegularExpressions;
using SequenceExtension;
using System.IO;

namespace AI_Z1
{
	class Program
	{
		static void Main(string[] args)
		{
			while (true)
			{
				using (var inputMethod = UIController.SetupInputMethod())
				{
					if (UIController.ProcessUserInput(inputMethod, out var solverInputModel))
					{
						Solver solver = new Solver(solverInputModel);
						var solutionResult = solver.Solve();

						UIController.ProcessSolverResult(solutionResult);
					}
					else
					{
						Console.WriteLine("Wrong input!");
					}
				}

				if (UIController.ProcessExit())
				{
					return;
				}

				Console.WriteLine();
			}
		}

	}
}
