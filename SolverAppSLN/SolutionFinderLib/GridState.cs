
using System;
using System.Collections;
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

	public const int GRID_CELL_COUNT = 9;
	private const int ALL_TRUE = 1 + 2 + 4 + 8 + 16 + 32 + 64 + 256;


	[FieldOffset(0)] private UInt16 _bitBuffer;


	public bool this[int index]
	{
		get 
		{
			var bitMask = GridState.BitMask(index);
			return (bitMask & _bitBuffer) == bitMask;
		}
		set
		{
			var bitMask = GridState.BitMask(index);
			if (value)
				_bitBuffer = (ushort) (_bitBuffer | bitMask);
			else
				// The &ALL_TRUE is used to zero out the extra bits
				_bitBuffer = (ushort) (_bitBuffer & (~bitMask & GridState.ALL_TRUE));
		}
	}

	#endregion


	#region Constructors

	public GridState()
	{
		_bitBuffer = 0;
	}

	public GridState(bool cell0 = false, bool cell1 = false, bool cell2 = false, bool cell3 = false,
		bool cell4 = false, bool cell5 = false, bool cell6 = false, bool cell7 = false,
		bool cell8 = false)
	{
		_bitBuffer = (ushort) (cell0 ? 1 : 0);
		_bitBuffer += (ushort) (cell1 ?   2 : 0);
		_bitBuffer += (ushort) (cell2 ?   4 : 0);
		_bitBuffer += (ushort) (cell3 ?   8 : 0);
		_bitBuffer += (ushort) (cell4 ?  16 : 0);
		_bitBuffer += (ushort) (cell5 ?  32 : 0);
		_bitBuffer += (ushort) (cell6 ?  64 : 0);
		_bitBuffer += (ushort) (cell7 ? 128 : 0);
		_bitBuffer += (ushort) (cell8 ? 256 : 0);
	}

	public GridState(bool[] cellStates)
	{
		var j = 1;
		for (int i = 0; i < GRID_CELL_COUNT; i++)
		{
			_bitBuffer += (ushort) (cellStates[i] ? j : 0);
			j *= 2;
		}
	}

	private GridState(UInt16 numValue)
	{
		_bitBuffer = numValue;
	}

	#endregion


	#region Hashing && Equality

	public override int GetHashCode()
	{
		return _bitBuffer;
	}

	public override bool Equals([NotNullWhen(true)] object obj)
	{
		var other = obj as GridState?;
		return other == null
			? false
			: (this == other.Value);
	}

	public static bool operator==(GridState lhs, GridState rhs)
	{
		return lhs._bitBuffer == rhs._bitBuffer;
	}

	public static bool operator!=(GridState lhs, GridState rhs)
	{
		return lhs._bitBuffer != rhs._bitBuffer;
	}

	#endregion


	#region Bitwise Operations

	public static GridState operator&(GridState lhs, GridState rhs)
	{
		// C# doesn't have a builtin bitwise operators for shorts and ushorts. So, they get cast to
		// ints of all things. This means we need to cast the result back into a ushort.
		ushort newState = (ushort) (lhs._bitBuffer & rhs._bitBuffer);
		return new GridState(newState);
	}

	public static GridState operator|(GridState lhs, GridState rhs)
	{
		ushort newState = (ushort) (lhs._bitBuffer | rhs._bitBuffer);
		return new GridState(newState);
	}

	#endregion

	private static int BitMask(int index)
	{
		switch (index)
		{
			case 0:
				return 1;
			case 1:
				return 2;
			case 2:
				return 4;
			case 3:
				return 8;
			case 4:
				return 16;
			case 5:
				return 32;
			case 6:
				return 64;
			case 7:
				return 128;
			case 8:
				return 256;
			default:
				throw new IndexOutOfRangeException("Cell index musht range [0, 8].");
		}
	}

}
