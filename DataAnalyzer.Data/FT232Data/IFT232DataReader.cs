﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer.Data.FT232Data
{
    public interface IFT232DataReader
    {
         Task<byte[]> ReadDataAsync(uint bufferSize);
    }
}
