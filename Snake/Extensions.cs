using System;

namespace snek
{
    internal static class Extensions
    {
        internal static Random _R = new Random();
        internal static T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(_R.Next(v.Length));
        }

    }
}
