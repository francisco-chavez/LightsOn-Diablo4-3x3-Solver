
using System;
using System.Collections.Generic;
using System.Text;

using SolutionFinderLib;


namespace TestSolutionFinderLib;

[TestClass]
public class TestGridState
{

	[TestMethod]
	public void ConstructorTest01()
	{
		GridState gridState = new GridState();
		for (int i = 0; i < GridState.GRID_CELL_COUNT; i++)
			Assert.IsFalse(gridState[i]);
	}

	[TestMethod]
	public void ConstructorTest02()
	{
		GridState gridState = new GridState(cell3: true, cell4: false);

		for (int i = 0; i < GridState.GRID_CELL_COUNT; i++)
		{
			if (i == 3)
				continue;

			Assert.IsFalse(gridState[i]);
		}
		Assert.IsTrue(gridState[3]);
	}

}
