using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Interfaces
{
    public interface IAttendanceService
    {
        Task<IEnumerable<Device>> GetOfflineDevicesAsync();
        Task<IEnumerable<OfflineAttendanceRecord>> GetPendingRecordsAsync(string deviceId);
        Task ProcessOfflineAttendanceAsync(OfflineAttendanceRecord record);
    }
}
