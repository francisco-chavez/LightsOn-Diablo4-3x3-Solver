
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;


namespace SolutionFinderLib;

[StructLayout(LayoutKind.Explicit)]
public unsafe struct GridState
{

	#region Attributes && Properties

	public static int GRID_CELL_COUNT = 9;


	[FieldOffset(0)] private fixed bool _buffer[9];


	public bool this[int index]
	{
		get { return _buffer[index]; }
		set { _buffer[index] = value; }
	}

	#endregion


	#region Constructors

	public GridState()
	{
		for (int i = 0; i < GRID_CELL_COUNT; i++)
			_buffer[i] = false;
	}

	public GridState(bool cell0 = false, bool cell1 = false, bool cell2 = false, bool cell3 = false,
		bool cell4 = false, bool cell5 = false, bool cell6 = false, bool cell7 = false,
		bool cell8 = false)
	{
		_buffer[0] = cell0;
		_buffer[1] = cell1;
		_buffer[2] = cell2;
		_buffer[3] = cell3;
		_buffer[4] = cell4;
		_buffer[5] = cell5;
		_buffer[6] = cell6;
		_buffer[7] = cell7;
		_buffer[8] = cell8;
	}

	public GridState(bool[] cellStates)
	{
		for (int i = 0; i < GRID_CELL_COUNT; i++)
			_buffer[i] = cellStates[i];
	}

	#endregion


	#region Hashing && Equality

	public override int GetHashCode()
	{
		int value = 0;

		for (int i = 0; i < GRID_CELL_COUNT; i++)
		{
			value += _buffer[i] ? 1 : 0;
			value = value << 1;
		}
		return value;
	}

	public override bool Equals([NotNullWhen(true)] object obj)
	{
		var other = (GridState) obj;
		return this == other;
	}

	public static bool operator==(GridState lhs, GridState rhs)
	{
		var same = true;
		int i = 0;
		do
		{
			same = lhs._buffer[i] == rhs._buffer[i];
			i++;
		} while (i < GRID_CELL_COUNT && same);
		return same;
	}

	public static bool operator!=(GridState lhs, GridState rhs)
	{
		return !(lhs == rhs);
	}

	#endregion

}
