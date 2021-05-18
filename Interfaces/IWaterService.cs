using iotweb.Models;
using iotweb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iotweb.Interfaces
{
    public interface IWaterService
    {
        public event WaterDelegate OnWaterChanged;
        IList<WaterData> GetCurrentWater();
    }
}
