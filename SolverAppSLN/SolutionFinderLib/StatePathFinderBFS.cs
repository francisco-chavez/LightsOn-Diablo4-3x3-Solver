
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;


namespace SolutionFinderLib;

public class StatePathFinderBFS
{
	private class Node
	{
		public GridState Start;
		public GridState End;
		public int SelectedIndex;
		public int Depth;
	}

	public async Task<int[]> FindPathAsync(GridState startingState, GridState targetState)
	{
		return await Task.Run(() => FindPath(startingState, targetState));
	}

	public int[] FindPath(GridState startingState, GridState targetState)
	{

		return null;
	}

}
