using eazy.rest.data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eazy.rest.services.Interface
{
    public interface IRestService<T>
    {
        Task<IEnumerable<TasksDto>> GetTaskAllAsync();
        Task<IEnumerable<TasksDto>> GetSingleTaskAync(T pkId);
        Task<T> CreateTaskAync(CreateDto codes, T guid);
        Task<bool> UpdateTaskAync(UpdateDto dto, T pkId, T guid);
        Task<bool> DeleteTaskAync(T pkId);
    }
}
