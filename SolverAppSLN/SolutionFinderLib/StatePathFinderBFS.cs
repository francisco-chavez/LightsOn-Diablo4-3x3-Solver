
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;


namespace SolutionFinderLib;

public class StatePathFinderBFS
{

	public async Task<int[]> FindPathAsync(GridState startingState, GridState targetState)
	{
		return await Task.Run(() => FindPath(startingState, targetState));
	}

	public int[] FindPath(GridState startingState, GridState targetState)
	{
		// We're already there.
		if (startingState == targetState)
			return [];  // This returns Array.Empty<int>(), which is a shared empty array to cut down on memory use
						// of creating multpile empty arrays. I'm guessing that it just looks at the method's return
						// type to figure out the array type.

		var queue = new LinkedList<GridState>();
		var childToParentPath = new Dictionary<GridState, int>();

		childToParentPath.Add(startingState, -1);
		queue.AddLast(startingState);

		while (queue.Count > 0)
		{
			var current = queue.First.Value;
			queue.RemoveFirst();

			// Target found
			if (current == targetState)
			{
				// Build path from target state to starting state.
				var path = new List<int>();
				var t = current;
				while (childToParentPath.ContainsKey(t) && childToParentPath[t] != -1)
				{
					var i = childToParentPath[t];
					path.Add(i);
					t = GridState.InvertCells(t, i);
				}

				// Reverse path so that it runs from starting state to target state and return it.
				path.Reverse();
				return path.ToArray();
			}

			// More work to do, So, keep at it.
			for (int i = 0; i < GridState.GRID_CELL_COUNT; i++)
			{
				var child = GridState.InvertCells(current, i);
				if (childToParentPath.ContainsKey(child))
					continue;

				queue.AddLast(child);
				childToParentPath.Add(child, i);
			}
		}

		// There is no path.
		return null;
	}

}
