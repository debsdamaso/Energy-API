using Energy_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Energy_API.Services.Interfaces
{
    public interface IEfficiencyService
    {
        string ClassifyDeviceEfficiency(string deviceType, double monthlyUsage);
    }
}
