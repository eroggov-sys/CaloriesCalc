using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;

namespace api.Interfaces
{
    public interface IExternalFoodProvider
    {
        Task<IReadOnlyList<ExternalFoodDto>> SearchAsync(string query, CancellationToken cancellationToken = default);
    }
}