using System;
using System.Collections.Generic;
using System.Text;

namespace macman.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);
        public static bool IsNullOrWhiteSpace(this string str) => string.IsNullOrWhiteSpace(str);

    }
}
