using System;
using System.Collections.Generic;

namespace Chess
{
	public class Grid<T>
	{
		T[,] array;
		public int Size
        {
			get
			{
				return array.GetLength(0);
			}
        }

		public Grid(int size)
		{
			array = new T[size, size];
		}

		public Grid (T[,] initial)
		{
			array = initial;
		}

		public T Get(int x, int y)
		{
			return array[y, x];
		}

		public void Set(int x, int y, T value)
		{
			array[y, x] = value;
		}

		public bool CheckBounds (int x, int y)
        {
			if (0 <= x && x < Size && 0 <= y && y < Size)
            {
				return true;
            }
            else
            {
				return false;
            }
        }

		public T Find (Predicate<T> predicate)
        {
			for (int y = 0; y < Size; y++)
			{
				for (int x = 0; x < Size; x++)
				{
					T value = Get(x, y);
					if (predicate(value))
					{
						return value;
					}
				}
			}
			return default(T);
		}

		public List<T> FindAll(Predicate<T> predicate)
		{
			List<T> matches = new List<T>();
			for (int y = 0; y < Size; y++)
			{
				for (int x = 0; x < Size; x++)
				{
					T value = Get(x, y);
					if (predicate(value))
					{
						matches.Add(value);
					}
				}
			}
			return matches;
		}

		public Tuple<int, int> CoordinatesOf (T value)
        {
			for (int y = 0; y < Size; y++)
            {
				for (int x = 0; x < Size; x++)
                {
					if (value.Equals(Get(x, y)))
                    {
						return new Tuple<int, int>(x, y);
                    }
                }
            }
			return null;
        }

        public override string ToString()
        {
			string build_string = "";
			for (int y = 0; y < array.GetLength(0); y++)
            {
				for (int x = 0; x < array.GetLength(1); x++)
                {
					if (this.array[y, x] != null)
                    {
						build_string += array[y, x].ToString().Substring(0,1);
					} else
                    {
						build_string += ".";
                    }
					build_string += " ";
				}
				build_string += "\n";
            }
			return build_string;
        }
    }

}