using Domain.Entities;
using Application.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IStatus.Repository
{
    public interface IStatusCommand
    {
        Task InsertStatus(Status status);
        Task UpdateStatus(Status status);
        Task RemoveStatus(Status status);
    }
}
