
using System;
using System.Collections.Generic;
using System.Text;

using SolutionFinderLib;


namespace TestSolutionFinderLib;

[TestClass]
public class TestGridState
{

	/// <summary>
	/// Creates a series of sets of cell indices to set to true within a GridState to allow 
	/// running the same validation test on multiple states of the Grid State.
	/// </summary>
	private static IEnumerable<int[]> TrueCellIndices
	{
		get
		{
			// I could write an array generator method that creates every true/false grid state
			// combination, but that might be overkill.
			yield return new int[] { 0, 2, 4, 6, 8 };
			yield return new int[] { 1, 3, 5, 7 };

			for (int i = 0; i < GridState.GRID_CELL_COUNT; i++)
			{
				yield return new int[] { i };

				for (int j = 0; j < GridState.GRID_CELL_COUNT; j++)
				{
					if (j == i)
						continue;

					yield return new int[] { i, j };

					for (int k = 0; k < GridState.GRID_CELL_COUNT; k++)
					{
						if (k == j || k == i)
							continue;

						yield return new int[] { i, j, k };
					}
				}
			}

		}
	}


	[TestMethod]
	public void DefaultConstructorTest01()
	{
		GridState gridState = new GridState();
		for (int i = 0; i < GridState.GRID_CELL_COUNT; i++)
			Assert.IsFalse(gridState[i]);
	}

	[TestMethod]
	public void NamedCellConstructorTest01()
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

	[TestMethod]
	public void NamedCellConstructorTest02()
	{
		var trueCells = new int[] { 4, 7 };

		GridState gridState = new GridState(cell4: true, cell7: true);

		for (int i = 0; i < GridState.GRID_CELL_COUNT; i++)
		{
			if (trueCells.Contains(i))
				continue;

			Assert.IsFalse(gridState[i]);
		}

		foreach (var i in trueCells)
			Assert.IsTrue(gridState[i]);
	}

	[TestMethod]
	[DynamicData(nameof(TrueCellIndices))]
	public void CellArrayConstructorTest01(int[] trueCellIndices)
	{
		var initializationArray = new bool[GridState.GRID_CELL_COUNT];
		for (int i = 0; i < GridState.GRID_CELL_COUNT; i++)
			initializationArray[0] = false;

		foreach (var i in trueCellIndices)
			initializationArray[i] = true;

		// Copy the initialization array to validation array before creating the GridState in the off
		// chance that the creation of the GridState makes changes to the initialization array.
		var validationArray = new bool[GridState.GRID_CELL_COUNT];
		initializationArray.CopyTo(validationArray, 0);

		GridState gridState = new GridState(initializationArray);

		for (int i = 0; i < GridState.GRID_CELL_COUNT; i++)
			Assert.AreEqual(validationArray[i], gridState[i]);
	}

}
