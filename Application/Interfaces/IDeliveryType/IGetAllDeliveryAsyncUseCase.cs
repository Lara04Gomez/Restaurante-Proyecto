using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Response;

namespace Application.Interfaces.IDeliveryType
{
    public interface IGetAllDeliveryAsyncUseCase
    {
        Task<List<DeliveryTypeResponse>> GetAllAsync();
    }
}
