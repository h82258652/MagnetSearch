using System;

namespace MagnetSearch.Utils
{
    public static class SizeHelper
    {
        public static long GetSize(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            long size = 0;
            if (input.EndsWith("GB", StringComparison.OrdinalIgnoreCase))
            {
                if (double.TryParse(input.Substring(0, input.Length - "GB".Length).Trim(), out var gb))
                {
                    size = (long)(gb * 1024 * 1024 * 1024);
                }
            }
            else if (input.EndsWith("MB", StringComparison.OrdinalIgnoreCase))
            {
                if (double.TryParse(input.Substring(0, input.Length - "MB".Length).Trim(), out var mb))
                {
                    size = (long)(mb * 1024 * 1024);
                }
            }
            else if (input.EndsWith("KB", StringComparison.OrdinalIgnoreCase))
            {
                if (double.TryParse(input.Substring(0, input.Length - "KB".Length).Trim(), out var kb))
                {
                    size = (long)(kb * 1024);
                }
            }
            else if (input.EndsWith("B", StringComparison.OrdinalIgnoreCase))
            {
                long.TryParse(input.Substring(0, input.Length - "B".Length).Trim(), out size);
            }

            return size;
        }
    }
}
