using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using NZParadiseAPI.Library;
using NZParadiseAPI.Model.Entities;

namespace NZParadiseAPI.Model.Abstract
{
    public interface IPropertiesRepository
    {
        Task<ResponseObject> RegisterNewProperty(Properties property);
        Task<ResponseObject> GetPropertyDetails(int propertyID);
        Task<ResponseObject> UpdateProperty(Properties property);
    }
}
