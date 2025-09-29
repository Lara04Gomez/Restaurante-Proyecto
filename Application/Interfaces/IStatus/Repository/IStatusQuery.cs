using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IStatus.Repository
{
    public interface IStatusQuery
    {
        Task<string> GetStatusById(int id);
        Task<List<Status>> GetAllStatuses();
    }
}
