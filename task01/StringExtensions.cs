using System;
using System.Linq;


namespace task01
{
    public static class StringExtensions
    {
        public static bool IsPalindrome(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            string lower = input.ToLower();

            string res = new string(
                lower.Where(c => !char.IsWhiteSpace(c) && !char.IsPunctuation(c)).ToArray());

            if (string.IsNullOrEmpty(res))
                return false;

            string revers = new string(res.Reverse().ToArray());
    
            return res == revers;
        }
    }
}
