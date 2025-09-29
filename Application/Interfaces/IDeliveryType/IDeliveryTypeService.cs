using Application.Models.Request;
using Application.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces.IDeliveryType
{
    public interface IDeliveryTypeService
    {
        
        Task<List<DeliveryTypeResponse>> GetAllDeliveryTypes();

        Task<DeliveryTypeResponse> CreateDeliveryType(DeliveryTypeRequest deliveryTypeRequest);

        
        Task<DeliveryTypeResponse> UpdateDeliveryType(int id, DeliveryTypeRequest deliveryTypeRequest);

 
        Task<DeliveryTypeResponse> DeleteDeliveryType(int id);

       
        Task<DeliveryTypeResponse> GetDeliveryTypeById(int id);
    }
}
