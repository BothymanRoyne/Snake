using System;
using System.Drawing;

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

        internal static double GetDistance(this Point p, Point q)
        {
            return Math.Sqrt(Math.Pow((q.X - p.X), 2) + Math.Pow((q.Y - p.Y), 2));
        }
    }
}
