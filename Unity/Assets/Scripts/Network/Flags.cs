using System;
using UnityEngine;

class Flags
{
    public static int Encode(bool[] values)
    {
        int result = 0;

        for (int i = 0; i < values.Length; i++)
        {
            if (values[i])
            {
                result = result | (1 << i);
            }
        }

        return result;
    }

    public static bool[] Decode(int flags, int count)
    {
        bool[] values = new bool[count];

        for (int i = 0; i < count; i++)
        {
            values[i] = (flags & (1 << i)) == (1 << i);
        }

        return values;
    }
}
