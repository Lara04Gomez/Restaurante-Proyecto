using Application.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Request;

namespace Application.Interfaces.IStatus
{
    public interface IStatusService
    {
        
        Task<List<StatusResponse>> GetAllStatuses();

      
        Task<StatusResponse> CreateStatus(string status);

       
        Task<StatusResponse> UpdateStatus(int id, string status);

       
        Task<StatusResponse> DeleteStatus(int id);

       
        Task<StatusResponse> GetStatusById(int id);
    }
}
