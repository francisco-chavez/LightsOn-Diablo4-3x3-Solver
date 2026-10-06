
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

	public GridState(bool i0 = false, bool i1 = false, bool i2 = false, bool i3 = false,
		bool i4 = false, bool i5 = false, bool i6 = false, bool i7 = false,
		bool i8 = false)
	{
		_buffer[0] = i0;
		_buffer[1] = i1;
		_buffer[2] = i2;
		_buffer[3] = i3;
		_buffer[4] = i4;
		_buffer[5] = i5;
		_buffer[6] = i6;
		_buffer[7] = i7;
		_buffer[8] = i8;
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
