using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SequenceExtension;

namespace AI_Z1
{
	public class Solver
	{
		private class MeetingNode
		{
			public readonly Node CurrentDirectionNode;
			public readonly Node OtherDirectionNode;

			public MeetingNode(Node currentDirNode, Node otherDirNode)
			{
				this.CurrentDirectionNode = currentDirNode;
				this.OtherDirectionNode = otherDirNode;
			}
		}

		private class Node
		{
			public readonly int[] NodeValue;
			public readonly Node PreviousNode;

			public Node(int[] nodeValue, Node previousNode)
			{
				this.NodeValue = nodeValue;
				this.PreviousNode = previousNode;
			}
		}

		private class NodeEqualityComparer : IEqualityComparer<Node>
		{
			public bool Equals(Node x, Node y)
			{
				return x.NodeValue.SequenceEqual(y.NodeValue);
			}

			public int GetHashCode(Node obj)
			{
				unchecked
				{
					if (obj.NodeValue == null)
					{
						return 0;
					}

					int hash = 0;

					for (int i = 0; i < obj.NodeValue.Length; i++)
					{
						hash += obj.NodeValue[i] * i ^ 31;
					}

					return hash;
				}
			}
		}

		public const int EMPTY_SPACE_REPRESENTATION = 0;

		/// <summary>
		/// Arguments: height, width, currentState
		/// Return value: (bool - isMoved, int[] - newState after move)
		/// </summary>
		private readonly Func<int, int, int[], (bool isAvailable, int[] newState)>[] possibleMoves = new Func<int, int, int[], (bool, int[])>[4] { right, left, up, down };

		private int width, height;
		private int maxIterationsCount;
		private Node startState;
		private Node finalState;

		/// <summary>
		/// Value 0 means empty space.
		/// </summary>
		public Solver(SolverInput model)
		{
			possibleMoves = new Func<int, int, int[], (bool, int[])>[4] { right, left, up, down };

			this.maxIterationsCount = model.MaxIterationsCount;
			this.width = model.Width;
			this.height = model.Height;

			this.startState = new Node(model.InitialState.Convert2dArray2Sequence(), null);
			this.finalState = new Node(model.FinalState.Convert2dArray2Sequence(), null);

			if (!model.IsInputCorrect())
			{
				throw new Exception("Input not valid!");
			}
		}

		public SolverResult Solve()
		{
			HashSet<Node> visitedNodesFromStart = new HashSet<Node>(new NodeEqualityComparer());
			HashSet<Node> visitedNodesFromEnd = new HashSet<Node>(new NodeEqualityComparer());

			Queue<Node> nodesToVisitFromStart = new Queue<Node>();
			Queue<Node> nodesToVisitFromEnd = new Queue<Node>();

			if (startState.NodeValue.SequenceEqual(finalState.NodeValue))
			{
				var result = new List<int[]>(1);
				result.Add(startState.NodeValue);

				return new SolverResult(height, width, result, null);
			}

			nodesToVisitFromStart.Enqueue(startState);
			nodesToVisitFromEnd.Enqueue(finalState);

			visitedNodesFromStart.Add(startState);
			visitedNodesFromEnd.Add(finalState);

			for (int i = 0; i < maxIterationsCount; i++)
			{
				var meetingNode = VisitNextNodesLevel(nodesToVisitFromStart, visitedNodesFromStart, visitedNodesFromEnd);

				if (meetingNode != null)
				{
					var result = getResultPath(meetingNode, true);
					return new SolverResult(height, width, result, getOperatorsFromPath(result));
				}

				meetingNode = VisitNextNodesLevel(nodesToVisitFromEnd, visitedNodesFromEnd, visitedNodesFromStart);

				if (meetingNode != null)
				{
					var result = getResultPath(meetingNode, false);
					return new SolverResult(height, width, result, getOperatorsFromPath(result));
				}
			}

			return null;
		}

		/// <summary>
		/// Returns the MeetingNode if there is a Node that meets the other search direction.
		/// </summary>
		private MeetingNode VisitNextNodesLevel(Queue<Node> nodesToVisit, HashSet<Node> currentDirSet, HashSet<Node> otherDirSet)
		{
			int limit = nodesToVisit.Count;

			for (int i = 0; i < limit; i++)
			{
				var node = nodesToVisit.Dequeue();

				foreach (var move in possibleMoves)
				{
					var moveResult = move(height, width, node.NodeValue);

					if (moveResult.isAvailable)
					{
						var newNode = new Node(moveResult.newState, node);

						if (otherDirSet.TryGetValue(newNode, out Node otherDirNode))
						{
							return new MeetingNode(newNode, otherDirNode);
						}

						if (!currentDirSet.Contains(newNode))
						{
							currentDirSet.Add(newNode);
							nodesToVisit.Enqueue(newNode);
						}
					}
				}
			}

			return null;
		}

		private List<string> getOperatorsFromPath(List<int[]> path)
		{
			List<string> appliedOperators = new List<string>();

			for (int i = 0; i < path.Count - 1; i++)
			{
				foreach (var move in possibleMoves)
				{
					var result = move(height, width, path[i]);
					if (result.isAvailable && result.newState.SequenceEqual(path[i + 1]))
					{
						appliedOperators.Add(move.Method.Name);
						break;
					}
				}
			}

			return appliedOperators;
		}

		private List<int[]> getResultPath(MeetingNode meetingNode, bool fromStart)
		{
			List<int[]> result = new List<int[]>();

			if (fromStart)
			{
				tracePathBackwards(meetingNode.CurrentDirectionNode, result);
				tracePathForwards(meetingNode.OtherDirectionNode.PreviousNode, result);
			}
			else
			{
				tracePathBackwards(meetingNode.OtherDirectionNode, result);
				tracePathForwards(meetingNode.CurrentDirectionNode.PreviousNode, result);
			}

			return result;
		}

		private void tracePathBackwards(Node finalNode, List<int[]> result)
		{
			if (finalNode.PreviousNode != null)
			{
				tracePathBackwards(finalNode.PreviousNode, result);
			}

			result.Add(finalNode.NodeValue);
		}

		private void tracePathForwards(Node startNode, List<int[]> result)
		{
			if (startNode == null)
			{
				return;
			}

			result.Add(startNode.NodeValue);

			tracePathForwards(startNode.PreviousNode, result);
		}

		private static (bool, int[]) left(int height, int width, int[] currentState)
		{
			int emptyID = currentState.ToList().IndexOf(EMPTY_SPACE_REPRESENTATION);

			if (emptyID % width == 0)
			{
				return (false, null);
			}

			var res = currentState.SwapByCopy(emptyID, emptyID - 1);
			return (true, res);
		}

		private static (bool, int[]) right(int height, int width, int[] currentState)
		{
			int emptyID = currentState.ToList().IndexOf(EMPTY_SPACE_REPRESENTATION);

			if (emptyID % width == width - 1)
			{
				return (false, null);
			}

			var res = currentState.SwapByCopy(emptyID, emptyID + 1);
			return (true, res);
		}

		private static (bool, int[]) down(int height, int width, int[] currentState)
		{
			int emptyID = currentState.ToList().IndexOf(EMPTY_SPACE_REPRESENTATION);

			if (emptyID / width == height - 1)
			{
				return (false, null);
			}

			var res = currentState.SwapByCopy(emptyID, emptyID + width);
			return (true, res);
		}

		private static (bool, int[]) up(int height, int width, int[] currentState)
		{
			int emptyID = currentState.ToList().IndexOf(EMPTY_SPACE_REPRESENTATION);

			if (emptyID / width == 0)
			{
				return (false, null);
			}

			var res = currentState.SwapByCopy(emptyID, emptyID - width);
			return (true, res);
		}

	}
}
