using System;

namespace Hebilife
{
    struct Tuple<T1, T2>
    {
        T1 Value1;
        T2 Value2;
        public Tuple(T1 a, T2 b)
        {
            Value1 = a;
            Value2 = b;
        }
    }
}
