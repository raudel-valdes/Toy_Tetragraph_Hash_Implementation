using System;
using System.Collections.Generic;

namespace Toy_Tetragraph_Hash_Implementation
{
    class Program
    {
        static void Main(string[] args)
        {
            string message = "";
            List<char> cleanMessage = new List<char>();
            List<char> charBlock = new List<char>();
            List<int> intBlock = new List<int>();
            List<int> runningTotal = new List<int> {0,0,0,0};
            int numberOfBlocks = 0;
            int numberOfPadding = 0;

            if (args.Length == 0)
            {
                message = "I leave twenty million dollars to my friendly cousin Bill.".ToLower().Trim();
            } else
            {
                foreach(string arg in args)
                {
                    message += arg + " ";
                }

                message = message.ToLower().Trim();
            }

            foreach(char character in message)
            {
                if (char.IsLetter(character) && character != ' ')
                {
                    cleanMessage.Add(character);
                }
            }

            if (cleanMessage.Count % 16 != 0)
            {
                numberOfPadding = cleanMessage.Count % 16;
            }

            numberOfBlocks = cleanMessage.Count / 16;

            for (int j = 0; j < numberOfBlocks; j++)
            {
                CompressionFunctionRoundOne(ref intBlock, ref charBlock, ref cleanMessage, j, numberOfPadding, numberOfBlocks);
                CompressionFunctionRoundTwo(ref runningTotal, ref intBlock, ref charBlock);
            }
        }

        public static void CompressionFunctionRoundOne(
            ref List<int> intBlock, 
            ref List<char> charBlock,
            ref List<char> cleanMessage,
            int onBlock,
            int numberOfPadding,
            int numberOfBlocks)
        {
            for (int i = onBlock * 16; i < ((onBlock * 16) + 16) && i < cleanMessage.Count ; i++)
            {
                char character = cleanMessage[i];
                charBlock.Add(character);
                intBlock.Add(((int)character) - 97);
            }

            if (onBlock * 16 == numberOfBlocks)
            {
                for (int j = 0;  j < numberOfPadding; j++)
                {
                    charBlock.Add(' ');
                    intBlock.Add(-1);
                }
            }
        }

        public static void CompressionFunctionRoundTwo(ref List<int> runningTotal,
            ref List<int> intBlock, 
            ref List<char> charBlock)
        {
            charBlock.Clear();

        }
    }
}
