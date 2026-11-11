
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;


namespace SolutionFinderLib;

[StructLayout(LayoutKind.Explicit)]
public unsafe struct GridState
{

	#region Constants

	public const int	GRID_CELL_COUNT = 9;

	private const int	CELL_BIT_MASK_0 =   1;
	private const int	CELL_BIT_MASK_1 =   2;
	private const int	CELL_BIT_MASK_2 =   4;
	private const int	CELL_BIT_MASK_3 =   8;
	private const int	CELL_BIT_MASK_4 =  16;
	private const int	CELL_BIT_MASK_5 =  32;
	private const int	CELL_BIT_MASK_6 =  64;
	private const int	CELL_BIT_MASK_7 = 128;
	private const int	CELL_BIT_MASK_8 = 256;

	private const int   ALL_FALSE		= 0;
	private const int	ALL_TRUE		= CELL_BIT_MASK_0 + CELL_BIT_MASK_1 + CELL_BIT_MASK_2 
										+ CELL_BIT_MASK_3 + CELL_BIT_MASK_4 + CELL_BIT_MASK_5 
										+ CELL_BIT_MASK_6 + CELL_BIT_MASK_7 + CELL_BIT_MASK_8;

	#endregion


	#region Attributes && Properties

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

	public static GridState AllOnes
	{
		get { return new GridState(GridState.ALL_TRUE); }
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
		_bitBuffer = (ushort)  (cell0 ? CELL_BIT_MASK_0 : 0);
		_bitBuffer += (ushort) (cell1 ? CELL_BIT_MASK_1 : 0);
		_bitBuffer += (ushort) (cell2 ? CELL_BIT_MASK_2 : 0);
		_bitBuffer += (ushort) (cell3 ? CELL_BIT_MASK_3 : 0);
		_bitBuffer += (ushort) (cell4 ? CELL_BIT_MASK_4 : 0);
		_bitBuffer += (ushort) (cell5 ? CELL_BIT_MASK_5 : 0);
		_bitBuffer += (ushort) (cell6 ? CELL_BIT_MASK_6 : 0);
		_bitBuffer += (ushort) (cell7 ? CELL_BIT_MASK_7 : 0);
		_bitBuffer += (ushort) (cell8 ? CELL_BIT_MASK_8 : 0);
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

	public static GridState operator~(GridState gridState)
	{
		ushort newState = (ushort) (~gridState._bitBuffer & GridState.ALL_TRUE);
		return new GridState(newState);
	}


	private static int BitMask(int index)
	{
		switch (index)
		{
			case 0:
				return GridState.CELL_BIT_MASK_0;
			case 1:	    
				return GridState.CELL_BIT_MASK_1;
			case 2:	    
				return GridState.CELL_BIT_MASK_2;
			case 3:	    
				return GridState.CELL_BIT_MASK_3;
			case 4:	    
				return GridState.CELL_BIT_MASK_4;
			case 5:	    
				return GridState.CELL_BIT_MASK_5;
			case 6:	    
				return GridState.CELL_BIT_MASK_6;
			case 7:
				return GridState.CELL_BIT_MASK_7;
			case 8:
				return GridState.CELL_BIT_MASK_8;
			default:
				throw new IndexOutOfRangeException("Cell index must range within [0, 8].");
		}
	}

	#endregion

	public static GridState InvertCells(GridState originState, int centerCellIndex)
	{
		/********************************************************************************************
		 * We create a mask showing which bits will be inverted.
		 * 
		 * The rule is that we invert the state of the cell along with the cells that touch it 
		 * (diagonal doesn't count as a touch).
		 * 
		 * The diagram below is to help select the correct overlapping bitmasks for each selected 
		 * cell index.
		 * 
		 * 0 | 1 | 2
		 * - + - + -
		 * 3 | 4 | 5
		 * - + - + -
		 * 6 | 7 | 8
		 * 
		 ********************************************************************************************/
		int mask = 0;

		switch (centerCellIndex)
		{
			case 0:
				mask = CELL_BIT_MASK_0 + CELL_BIT_MASK_1 + CELL_BIT_MASK_3;
				break;
			case 1:
				mask = CELL_BIT_MASK_0 + CELL_BIT_MASK_1 + CELL_BIT_MASK_2 + CELL_BIT_MASK_4;
				break;
			case 2:
				mask = CELL_BIT_MASK_1 + CELL_BIT_MASK_2 + CELL_BIT_MASK_5;
				break;
			case 3:
				mask = CELL_BIT_MASK_0 + CELL_BIT_MASK_3 + CELL_BIT_MASK_4 + CELL_BIT_MASK_6;
				break;
			case 4:
				mask = CELL_BIT_MASK_1 + CELL_BIT_MASK_3 + CELL_BIT_MASK_4 + CELL_BIT_MASK_5 + CELL_BIT_MASK_7;
				break;
			case 5:
				mask = CELL_BIT_MASK_2 + CELL_BIT_MASK_4 + CELL_BIT_MASK_5 + CELL_BIT_MASK_8;
				break;
			case 6:
				mask = CELL_BIT_MASK_3 + CELL_BIT_MASK_6 + CELL_BIT_MASK_7;
				break;
			case 7:
				mask = CELL_BIT_MASK_4 + CELL_BIT_MASK_6 + CELL_BIT_MASK_7 + CELL_BIT_MASK_8;
				break;
			case 8:
				mask = CELL_BIT_MASK_5 + CELL_BIT_MASK_7 + CELL_BIT_MASK_8;
				break;

			default:
				throw new IndexOutOfRangeException();
		}

		int rawValue = originState._bitBuffer;		// rawValue is easier to type out than originalState._bitBuffer;
		var t = (~mask | ~rawValue) & mask;			// We invert the bits within the mask and place them in 't'.
		var newValue = (rawValue & ~mask) | t;		// We take the starting value, and overwrite the mask bits with what's in 't'.

		return new GridState((ushort) newValue);
	}

}
