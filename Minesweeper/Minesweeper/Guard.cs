using System;

namespace Minesweeper
{
    public class Guard
    {
        public static T AgainstLessThan<T>(T lowerLimit, T value, string message)
            where T : IComparable<T>
                {
                    if (value.CompareTo(lowerLimit) < 0)
                        throw new ArgumentOutOfRangeException(
                            $"ERROR : {message} (parameter error {lowerLimit})");
            return value;
                }

        public static T AgainstGreaterThan<T>(T upperLimit, T value, string message)
             where T : IComparable<T>
        {
            if (value.CompareTo(upperLimit) > 0)
                throw new ArgumentOutOfRangeException(
                    $"ERROR : {message} (parameter error {upperLimit})");
            return value;
        }

        public static T AgainstNull<T>(T value, string message)
           where T : class
        {
            if (value == null)
                throw new ArgumentNullException($"ERROR : {message} (parameter is null)");
            return value;
        }
    }
}
