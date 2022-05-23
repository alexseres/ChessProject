using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public class PopulationCount : IPopulationCount
    {
        public  byte[] InitPopulationCount()
        {
            byte[] populationCountOfByte256 = new byte[256];
            for (int i = 1; i < 256; i++)
            {
                populationCountOfByte256[i] = (byte)(populationCountOfByte256[i / 2] + (i & 1));
            }
            return populationCountOfByte256;
        }

        public int GetPopulation(ulong x)
        {
            byte[] popCountOfByte256 = InitPopulationCount();
            int result = popCountOfByte256[x & 0xff] +
            popCountOfByte256[(x >> 8) & 0xff] +
            popCountOfByte256[(x >> 16) & 0xff] +
            popCountOfByte256[(x >> 24) & 0xff] +
            popCountOfByte256[(x >> 32) & 0xff] +
            popCountOfByte256[(x >> 40) & 0xff] +
            popCountOfByte256[(x >> 48) & 0xff] +
            popCountOfByte256[x >> 56];
            return result;
        }
    }
}
