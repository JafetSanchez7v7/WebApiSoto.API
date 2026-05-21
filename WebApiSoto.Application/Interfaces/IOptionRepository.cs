using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Common.Models;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.Interfaces
{
    public  interface IOptionRepository
    {
        Task<IEnumerable<Option>> GetOptions(FIlterOptionsDto dto, CancellationToken ct);
        //    Task<Option> GetOptionById(int id, CancellationToken ct);
        //    Task<Option>GetByName(string name, CancellationToken ct);
        //Task<Option> CreateOption(Option option, CancellationToken ct);
        //    Task<Option> UpdateOption(CancellationToken ct);
        //    Task DeActivateAsync(int id, CancellationToken ct);
    }
}
