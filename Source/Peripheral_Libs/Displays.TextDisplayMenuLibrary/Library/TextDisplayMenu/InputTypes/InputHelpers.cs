using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.Displays.TextDisplayMenu.InputTypes
{
    public static class InputHelpers
    {
        public static string PadLeft(string text, char filler, int size)
        {
            string padded = string.Empty;
            for (int i = text.Length; i < size; i++)
            {
                padded += filler;
            }
            return padded + text;
        }

        public static int Exp(int baseNumber, int power)
        {
            if (power == 0) return 1;
            if (power == 1) return baseNumber;

            for (int i = 0; i < power - 1; i++)
            {
                baseNumber *= baseNumber;
            }
            return baseNumber;
        }
    }
}
