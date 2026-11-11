
using System;
using System.Collections.Generic;
using System.Text;

using SolutionFinderLib;


namespace TestSolutionFinderLib;

[TestClass]
public class TestStatePathFinderBFS
{

	private static readonly StatePathFinderBFS FinderInstance = new StatePathFinderBFS();

	#region Same start as finish

	[TestMethod]
	public void TestSameStartAsEnd0()
	{
		var startingState = new GridState();
		var targetState = startingState;    // This is a struct, so it copies the value over to a new (idendical) GridState.

		var path = FinderInstance.FindPath(startingState, targetState);
		Assert.IsEmpty(path);	// Pass in 'null' to see what happens. Test failed, which is what we would want it to do.
	}

	[TestMethod]
	public void TestSameStartAsEnd1()
	{
		var startingState = new GridState(cell4: true);
		var targetState = startingState;	// This is a struct, so it copies the value over to a new (idendical) GridState.

		var path = FinderInstance.FindPath(startingState, targetState);
		Assert.IsEmpty(path);	// Pass in 'null' to see what happens. Test failed, which is what we would want it to do.
	}

	[TestMethod]
	public void TestSameStartAsEnd3()
	{
		var startingState = new GridState(cell5: true, cell7: true);
		var targetState = startingState;    // This is a struct, so it copies the value over to a new (idendical) GridState.

		var path = FinderInstance.FindPath(startingState, targetState);
		Assert.IsEmpty(path);   // Pass in 'null' to see what happens. Test failed, which is what we would want it to do.
	}

	[TestMethod]
	public void TestSameStartAsEnd4()
	{
		var startingState = new GridState(cell1: true, cell3: true, cell8: true);
		var targetState = startingState;    // This is a struct, so it copies the value over to a new (idendical) GridState.

		var path = FinderInstance.FindPath(startingState, targetState);
		Assert.IsEmpty(path);   // Pass in 'null' to see what happens. Test failed, which is what we would want it to do.
	}

	#endregion


	#region Path Length 1

	[TestMethod]
	public void TestPathLengthOfOne0()
	{
		// 000
		// 000
		// 000
		var startingState = new GridState();

		// Path [ 4 ]
		// 010
		// 111
		// 010
		var targetState = new GridState(cell1: true, cell3: true, cell4: true, cell5: true, cell7: true);

		var path = FinderInstance.FindPath(startingState, targetState);

		var expectedPath = new[] { 4 };
		CollectionAssert.AreEqual(expectedPath, path);
	}

	[TestMethod]
	public void TestPathLengthOfOne1()
	{
		// 000
		// 000
		// 000
		var startingState = new GridState();

		// Path [ 2 ]
		// 011
		// 001
		// 000
		var targetState = new GridState(cell1: true, cell2: true, cell5: true);

		var path = FinderInstance.FindPath(startingState, targetState);

		var expectedPath = new[] { 2 };
		CollectionAssert.AreEqual(expectedPath, path);
	}

	[TestMethod]
	public void TestPathLengthOfOne2()
	{
		// 000
		// 001
		// 011
		var startingState = new GridState(cell5: true, cell7: true, cell8: true);

		// Path [ 2 ]
		// 011
		// 000
		// 011
		var targetState = new GridState(cell1: true, cell2: true, cell7: true, cell8: true);

		var path = FinderInstance.FindPath(startingState, targetState);

		var expectedPath = new[] { 2 };
		CollectionAssert.AreEqual(expectedPath, path);
	}

	[TestMethod]
	public void TestPathLengthOfOne3()
	{
		// 000
		// 101
		// 001
		var startingState = new GridState(cell3: true, cell5: true, cell8: true);

		// Path [ 2 ]
		// 000
		// 001
		// 111
		var targetState = new GridState(cell5: true, cell6: true, cell7: true, cell8: true);

		var path = FinderInstance.FindPath(startingState, targetState);

		var expectedPath = new[] { 6 };
		CollectionAssert.AreEqual(expectedPath, path);
	}

	#endregion

}
