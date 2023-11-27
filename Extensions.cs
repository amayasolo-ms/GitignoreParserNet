using System.Text;

namespace GitignoreParserNet
{
    internal static class Extensions
    {
        //
        // Summary:
        //     Preappends a specified number of copies of the string representation of a Unicode
        //     character to this instance.
        //
        // Parameters:
        //   value:
        //     The character to append.
        //
        //   repeatCount:
        //     The number of times to append value.
        //
        // Returns:
        //     A reference to this instance after the append operation has completed.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     repeatCount is less than zero. -or- Enlarging the value of this instance would
        //     exceed System.Text.StringBuilder.MaxCapacity.
        //
        //   T:System.OutOfMemoryException:
        //     Out of memory.
        internal static StringBuilder Preappend(this StringBuilder builder, char value, int repeatCount)
        {
            return builder.Insert(0, new string(value, repeatCount));
        }

        //
        // Summary:
        //     Preappends the string representation of a specified Boolean value to this instance.
        //
        // Parameters:
        //   value:
        //     The Boolean value to append.
        //
        // Returns:
        //     A reference to this instance after the append operation has completed.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     Enlarging the value of this instance would exceed System.Text.StringBuilder.MaxCapacity.
        internal static StringBuilder Preappend(this StringBuilder builder, bool value)
        {
            return builder.Insert(0, value);
        }

        //
        // Summary:
        //     Preappends the string representation of a specified System.Char object to this instance.
        //
        // Parameters:
        //   value:
        //     The UTF-16-encoded code unit to append.
        //
        // Returns:
        //     A reference to this instance after the append operation has completed.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     Enlarging the value of this instance would exceed System.Text.StringBuilder.MaxCapacity.
        internal static StringBuilder Preappend(this StringBuilder builder, char value)
        {
            return builder.Insert(0, value);
        }

        //
        // Summary:
        //     Preappends the string representation of a specified 64-bit unsigned integer to this
        //     instance.
        //
        // Parameters:
        //   value:
        //     The value to append.
        //
        // Returns:
        //     A reference to this instance after the append operation has completed.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     Enlarging the value of this instance would exceed System.Text.StringBuilder.MaxCapacity.
        internal static StringBuilder Preappend(this StringBuilder builder, ulong value)
        {
            return builder.Insert(0, value);
        }

        //
        // Summary:
        //     Preappends the string representation of a specified 32-bit unsigned integer to this
        //     instance.
        //
        // Parameters:
        //   value:
        //     The value to append.
        //
        // Returns:
        //     A reference to this instance after the append operation has completed.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     Enlarging the value of this instance would exceed System.Text.StringBuilder.MaxCapacity.
        internal static StringBuilder Preappend(this StringBuilder builder, uint value)
        {
            return builder.Insert(0, value);
        }

        //
        // Summary:
        //     Preappends the string representation of a specified 8-bit unsigned integer to this
        //     instance.
        //
        // Parameters:
        //   value:
        //     The value to append.
        //
        // Returns:
        //     A reference to this instance after the append operation has completed.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     Enlarging the value of this instance would exceed System.Text.StringBuilder.MaxCapacity.
        internal static StringBuilder Preappend(this StringBuilder builder, byte value)
        {
            return builder.Insert(0, value);
        }

        //
        // Summary:
        //     Appends a copy of the specified string to this instance.
        //
        // Parameters:
        //   value:
        //     The string to append.
        //
        // Returns:
        //     A reference to this instance after the append operation has completed.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     Enlarging the value of this instance would exceed System.Text.StringBuilder.MaxCapacity.
        internal static StringBuilder Preappend(this StringBuilder builder, string value)
        {
            return builder.Insert(0, value);
        }

        //
        // Summary:
        //     Preappends the string representation of a specified single-precision floating-point
        //     number to this instance.
        //
        // Parameters:
        //   value:
        //     The value to append.
        //
        // Returns:
        //     A reference to this instance after the append operation has completed.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     Enlarging the value of this instance would exceed System.Text.StringBuilder.MaxCapacity.
        internal static StringBuilder Preappend(this StringBuilder builder, float value)
        {
            return builder.Insert(0, value);
        }

        //
        // Summary:
        //     Preappends the string representation of a specified 16-bit unsigned integer to this
        //     instance.
        //
        // Parameters:
        //   value:
        //     The value to append.
        //
        // Returns:
        //     A reference to this instance after the append operation has completed.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     Enlarging the value of this instance would exceed System.Text.StringBuilder.MaxCapacity.
        internal static StringBuilder Preappend(this StringBuilder builder, ushort value)
        {
            return builder.Insert(0, value);
        }

        //
        // Summary:
        //     Preappends the string representation of a specified object to this instance.
        //
        // Parameters:
        //   value:
        //     The object to append.
        //
        // Returns:
        //     A reference to this instance after the append operation has completed.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     Enlarging the value of this instance would exceed System.Text.StringBuilder.MaxCapacity.
        internal static StringBuilder Preappend(this StringBuilder builder, object value)
        {
            return builder.Insert(0, value);
        }

        //
        // Summary:
        //     Preappends the string representation of the Unicode characters in a specified array
        //     to this instance.
        //
        // Parameters:
        //   value:
        //     The array of characters to append.
        //
        // Returns:
        //     A reference to this instance after the append operation has completed.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     Enlarging the value of this instance would exceed System.Text.StringBuilder.MaxCapacity.
        internal static StringBuilder Preappend(this StringBuilder builder, char[] value)
        {
            return builder.Insert(0, value);
        }

        //
        // Summary:
        //     Preappends the string representation of a specified 8-bit signed integer to this
        //     instance.
        //
        // Parameters:
        //   value:
        //     The value to append.
        //
        // Returns:
        //     A reference to this instance after the append operation has completed.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     Enlarging the value of this instance would exceed System.Text.StringBuilder.MaxCapacity.
        internal static StringBuilder Preappend(this StringBuilder builder, sbyte value)
        {
            return builder.Insert(0, value);
        }

        //
        // Summary:
        //     Preappends the string representation of a specified decimal number to this instance.
        //
        // Parameters:
        //   value:
        //     The value to append.
        //
        // Returns:
        //     A reference to this instance after the append operation has completed.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     Enlarging the value of this instance would exceed System.Text.StringBuilder.MaxCapacity.
        internal static StringBuilder Preappend(this StringBuilder builder, decimal value)
        {
            return builder.Insert(0, value);
        }

        //
        // Summary:
        //     Preappends the string representation of a specified 16-bit signed integer to this
        //     instance.
        //
        // Parameters:
        //   value:
        //     The value to append.
        //
        // Returns:
        //     A reference to this instance after the append operation has completed.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     Enlarging the value of this instance would exceed System.Text.StringBuilder.MaxCapacity.
        internal static StringBuilder Preappend(this StringBuilder builder, short value)
        {
            return builder.Insert(0, value);
        }

        //
        // Summary:
        //     Preappends the string representation of a specified 32-bit signed integer to this
        //     instance.
        //
        // Parameters:
        //   value:
        //     The value to append.
        //
        // Returns:
        //     A reference to this instance after the append operation has completed.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     Enlarging the value of this instance would exceed System.Text.StringBuilder.MaxCapacity.
        internal static StringBuilder Preappend(this StringBuilder builder, int value)
        {
            return builder.Insert(0, value);
        }

        //
        // Summary:
        //     Preappends the string representation of a specified 64-bit signed integer to this
        //     instance.
        //
        // Parameters:
        //   value:
        //     The value to append.
        //
        // Returns:
        //     A reference to this instance after the append operation has completed.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     Enlarging the value of this instance would exceed System.Text.StringBuilder.MaxCapacity.
        internal static StringBuilder Preappend(this StringBuilder builder, long value)
        {
            return builder.Insert(0, value);
        }

        //
        // Summary:
        //     Preappends the string representation of a specified double-precision floating-point
        //     number to this instance.
        //
        // Parameters:
        //   value:
        //     The value to append.
        //
        // Returns:
        //     A reference to this instance after the append operation has completed.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     Enlarging the value of this instance would exceed System.Text.StringBuilder.MaxCapacity.
        internal static StringBuilder Preappend(this StringBuilder builder, double value)
        {
            return builder.Insert(0, value);
        }

#if !NETSTANDARD2_1_OR_GREATER && !NETCOREAPP2_0_OR_GREATER
        //
        // Summary:
        //     Determines whether the beginning of this string instance matches the specified
        //     character.
        //
        // Parameters:
        //   value:
        //     The character to compare.
        //
        // Returns:
        //     true if value matches the beginning of this string; otherwise, false.
        internal static bool StartsWith(this string @string, char value) => @string.Length >= 1 && @string[0] == value;

        //
        // Summary:
        //     Determines whether the end of this string instance matches the specified character.
        //
        //
        // Parameters:
        //   value:
        //     The character to compare to the substring at the end of this instance.
        //
        // Returns:
        //     true if value matches the end of this instance; otherwise, false.
        internal static bool EndsWith(this string @string, char value) => @string.Length >= 1 && @string[@string.Length - 1] == value;
#endif
    }
}
