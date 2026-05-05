using System;
using System.Linq;

namespace Restaurant
{
    public static class DataFormatter
    {
        public static string ConvertToInitials(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
                return string.Empty;

            string[] parts = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length >= 3)
            {
                return $"{parts[0]} {parts[1][0]}.{parts[2][0]}.";
            }
            else if (parts.Length == 2)
            {
                return $"{parts[0]} {parts[1][0]}.";
            }
            else
            {
                return fullName;
            }
        }

        public static string MaskPhoneNumber(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return string.Empty;

            string digitsOnly = new string(phone.Where(char.IsDigit).ToArray());

            if (digitsOnly.Length == 11 && digitsOnly.StartsWith("7"))
            {
                string visiblePrefix = "+7";
                string firstHidden = "***";
                string secondHidden = "***";
                string lastFourDigits = digitsOnly.Substring(digitsOnly.Length - 4);
                string formattedLastDigits = $"{lastFourDigits.Substring(0, 2)}-{lastFourDigits.Substring(2)}";

                return $"{visiblePrefix}({firstHidden}) {secondHidden}-{formattedLastDigits}";
            }
            else if (digitsOnly.Length == 11 && digitsOnly.StartsWith("8"))
            {
                string visiblePrefix = "8";
                string firstHidden = "***";
                string secondHidden = "***";
                string lastFourDigits = digitsOnly.Substring(digitsOnly.Length - 4);
                string formattedLastDigits = $"{lastFourDigits.Substring(0, 2)}-{lastFourDigits.Substring(2)}";

                return $"{visiblePrefix}({firstHidden}) {secondHidden}-{formattedLastDigits}";
            }
            else if (digitsOnly.Length >= 6)
            {
                int visibleStartCount = Math.Min(2, digitsOnly.Length - 4);
                string visibleStart = digitsOnly.Substring(0, visibleStartCount);
                string lastFourDigits = digitsOnly.Length >= 4
                    ? digitsOnly.Substring(digitsOnly.Length - 4)
                    : digitsOnly;

                string formattedLastDigits = lastFourDigits.Length == 4
                    ? $"{lastFourDigits.Substring(0, 2)}-{lastFourDigits.Substring(2)}"
                    : lastFourDigits;

                int hiddenCount = digitsOnly.Length - visibleStartCount - 4;
                if (hiddenCount > 0)
                {
                    string hiddenPart = new string('*', hiddenCount);
                    return $"{visibleStart}{hiddenPart}-{formattedLastDigits}";
                }
                else
                {
                    return $"{visibleStart}-{formattedLastDigits}";
                }
            }
            else
            {
                return phone;
            }
        }

        public static string MaskPassport(string passport)
        {
            if (string.IsNullOrEmpty(passport))
                return string.Empty;

            string digitsOnly = new string(passport.Where(char.IsDigit).ToArray());

            if (digitsOnly.Length >= 10)
            {
                string firstTwoSeries = digitsOnly.Substring(0, 2);
                string lastTwoSeries = "**";
                string firstTwoNumber = "**";
                string lastFourNumber = digitsOnly.Substring(digitsOnly.Length - 4);

                return $"{firstTwoSeries}{lastTwoSeries} {firstTwoNumber}{lastFourNumber}";
            }
            else
            {
                return new string('*', passport.Length);
            }
        }

        public static string ValidateAndFormatName(string input, ref int cursorPos)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            int spaceCount = input.Count(c => c == ' ');
            if (spaceCount > 2)
            {
                int lastSpace = input.LastIndexOf(' ');
                input = input.Remove(lastSpace, 1);
            }

            int dashCount = input.Count(c => c == '-');
            if (dashCount > 1)
            {
                int lastDash = input.LastIndexOf('-');
                input = input.Remove(lastDash, 1);
            }

            string[] parts = input
                .Split(new char[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => char.ToUpper(p[0]) + p.Substring(1).ToLower())
                .ToArray();

            string formatted = input;
            int index = 0;
            foreach (string part in parts)
            {
                int pos = formatted.IndexOf(part, index, StringComparison.OrdinalIgnoreCase);
                if (pos >= 0)
                {
                    formatted = formatted.Remove(pos, part.Length).Insert(pos, part);
                    index = pos + part.Length;
                }
            }

            cursorPos = Math.Min(cursorPos, formatted.Length);
            return formatted;
        }
    }
}