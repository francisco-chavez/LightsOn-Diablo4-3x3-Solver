using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SolutionFinderLib;

[StructLayout(LayoutKind.Explicit)]
public unsafe struct GridState
{

	public static int GRID_CELL_COUNT = 9;


	[FieldOffset(0)] private fixed bool _buffer[9];


	public bool this[int index]
	{
		get { return _buffer[index]; }
		set { _buffer[index] = value; }
	}


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

}
