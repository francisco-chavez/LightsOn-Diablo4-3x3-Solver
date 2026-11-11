
using System;
using System.Collections.Generic;
using System.Text;

using SolutionFinderLib;


namespace TestSolutionFinderLib;

[TestClass]
public class TestStatePathFinderBFS
{

	#region Same start as finish

	[TestMethod]
	public void TestSameStartAsEnd0()
	{
		var startingState = new GridState();
		var targetState = startingState;	// This is a struct, so it copies the value over to a new (idendical) GridState.

		var pathFinder = new StatePathFinderBFS();

		var path = pathFinder.FindPath(startingState, targetState);
		Assert.IsEmpty(path);	// Pass in 'null' to see what happens. Test failed, which is what we would want it to do.
	}

	[TestMethod]
	public void TestSameStartAsEnd1()
	{
		var startingState = new GridState(cell4: true);
		var targetState = startingState;	// This is a struct, so it copies the value over to a new (idendical) GridState.

		var pathFinder = new StatePathFinderBFS();

		var path = pathFinder.FindPath(startingState, targetState);
		Assert.IsEmpty(path);	// Pass in 'null' to see what happens. Test failed, which is what we would want it to do.
	}

	[TestMethod]
	public void TestSameStartAsEnd3()
	{
		var startingState = new GridState(cell5: true, cell7: true);
		var targetState = startingState;    // This is a struct, so it copies the value over to a new (idendical) GridState.

		var pathFinder = new StatePathFinderBFS();

		var path = pathFinder.FindPath(startingState, targetState);
		Assert.IsEmpty(path);   // Pass in 'null' to see what happens. Test failed, which is what we would want it to do.
	}

	[TestMethod]
	public void TestSameStartAsEnd4()
	{
		var startingState = new GridState(cell1: true, cell3: true, cell8: true);
		var targetState = startingState;    // This is a struct, so it copies the value over to a new (idendical) GridState.

		var pathFinder = new StatePathFinderBFS();

		var path = pathFinder.FindPath(startingState, targetState);
		Assert.IsEmpty(path);   // Pass in 'null' to see what happens. Test failed, which is what we would want it to do.
	}

	#endregion

}
