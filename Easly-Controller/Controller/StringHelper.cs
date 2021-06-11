namespace EaslyController
{
    using System;
    using EaslyController.Constants;

    /// <summary>
    /// Provides methods to handle strings modified by keyboard events.
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Gets the visible part of a string.
        /// </summary>
        /// <param name="text">The string to filter.</param>
        public static string VisibleSubset(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            string Result = string.Empty;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (char.IsControl(c) || char.IsLowSurrogate(c) || c == 0x2028 || c == 0x2029 || c == 0xFFFD)
                    continue;

                if (char.IsHighSurrogate(c) && i + 1 < text.Length)
                {
                    Result += c;
                    c = text[++i];
                }

                Result += c;
            }

            return Result;
        }

        /// <summary>
        /// Returns the text modified with <see cref="AutoFormatModes.FirstOnly"/>.
        /// </summary>
        /// <param name="text">The text.</param>
        public static string FirstOnlyFormattedText(string text)
        {
            string Result = string.Empty;

            bool SetToUpper = true;
            foreach (char c in text)
            {
                if (c == ' ')
                {
                    SetToUpper = true;
                    Result += c;
                }
                else
                {
                    string Letter = c.ToString();

                    if (SetToUpper)
                    {
                        SetToUpper = false;
                        Result += Letter.ToUpper();
                    }
                    else
                        Result += Letter.ToLower();
                }
            }

            return Result;
        }

        /// <summary>
        /// Returns the text modified with <see cref="AutoFormatModes.FirstOrAll"/>.
        /// </summary>
        /// <param name="text">The text.</param>
        public static string FirstOrAllFormattedText(string text)
        {
            string Result = string.Empty;

            bool SetToUpper = true;
            foreach (char c in text)
            {
                if (c == ' ')
                {
                    SetToUpper = true;
                    Result += c;
                }
                else
                {
                    string Letter = c.ToString();

                    if (SetToUpper)
                    {
                        if (Letter == Letter.ToUpper())
                            Result += Letter;
                        else
                        {
                            Result += Letter.ToUpper();
                            SetToUpper = false;
                        }
                    }
                    else
                        Result += c.ToString().ToLower();
                }
            }

            return Result;
        }

        /// <summary>
        /// Returns the text modified with <see cref="AutoFormatModes.AllLowercase"/>.
        /// </summary>
        /// <param name="text">The text.</param>
        public static string AllLowercaseFormattedText(string text)
        {
            return text.ToLower();
        }
    }
}
