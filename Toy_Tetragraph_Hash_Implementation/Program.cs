using System;
using System.Collections.Generic;

namespace Toy_Tetragraph_Hash_Implementation
{
    class Program
    {
        static void Main(string[] args)
        {

            //THINGS LEFT TO DO:
            //1)Take care of the cases where the character count is not divisible by 16
            //2)Make sure that it works when there are more than one block
            //3)Make sure that it works with differnt convinations of 1 and 2 above.
            //4)Make sure it works through command line string
            //5)

            string message = "";
            List<char> cleanMessage = new List<char>();
            List<char> charBlock = new List<char>();
            List<int> intBlock = new List<int>();
            List<int> runningTotal = new List<int> {0,0,0,0};
            int numberOfBlocks = 0;
            int numberOfPadding = 0;
            string hash = "";

            if (args.Length == 0)
            {
                message = "I leave twenty million dollars to my friendly cousin Bill.".ToLower().Trim();
                //message = "ABCDEFGHIJKLMNOP".ToLower().Trim();
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
                CompressionFunctionRoundOne(ref runningTotal, ref intBlock, ref charBlock, ref cleanMessage, j, numberOfPadding, numberOfBlocks);
                CompressionFunctionRoundTwo(ref runningTotal, ref intBlock, ref charBlock);
            }


            hash += (char)(runningTotal[0] + 97);
            hash += (char)(runningTotal[1] + 97);
            hash += (char)(runningTotal[2] + 97);
            hash += (char)(runningTotal[3] + 97);
            hash = hash.ToUpper();

            Console.WriteLine("Your four character hash is: " + hash);
      
        }

        public static void CompressionFunctionRoundOne(
            ref List<int> runningTotal,
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

            CalcRunningTotal(ref runningTotal, ref intBlock);

            if (onBlock + 1 == numberOfBlocks)
            {
                for (int j = 0;  j < numberOfPadding; j++)
                {
                    charBlock.Add(' ');
                }
            }
        }

        public static void CompressionFunctionRoundTwo(
            ref List<int> runningTotal,
            ref List<int> intBlock, 
            ref List<char> charBlock)
        {
            int[] tempIntList = new int[16];
            char[] tempCharList = new char[16];

            charBlock.CopyTo(tempCharList);
            intBlock.CopyTo(tempIntList);

            for (int y = 0; y < 4; y++)
            {
                int count = 0;

                for (int x = 0; x < 4; x++)
                {
                    if (x + y < 3)
                    {
                        charBlock[(4 * y) + x] = tempCharList[(4 * y) + (x + y + 1)];

                    } else if (y == 3)
                    {
                        charBlock[(4 * y) + x] = tempCharList[(4 * y) + 3 - x];
                    } else
                    {
                        charBlock[(4 * y) + x] = tempCharList[(4 * y) + count++];
                    }

                }
            }

            CalcRunningTotal(ref runningTotal, ref charBlock);

            charBlock.Clear();
            intBlock.Clear();

        }

        public static void CalcRunningTotal(ref List<int> runningTotal,ref List<char> charBlock)
        {
            for (int x = 0; x < 4; x++)
            {
                runningTotal[x] = (
                    runningTotal[x] +
                    ((int)charBlock[x] - 97) +
                    ((int)charBlock[4 + x] - 97) +
                    ((int)charBlock[8 + x] - 97) +
                    ((int)charBlock[12 + x] - 97)) % 26;
            }
        }

        public static void CalcRunningTotal(ref List<int> runningTotal, ref List<int> intBlock)
        {
            for (int x = 0; x < 4; x++)
            {
                runningTotal[x] = (runningTotal[x] + intBlock[x] + intBlock[4 + x] + intBlock[8 + x] + intBlock[12 + x]) % 26;
            }
        }
    }
}
