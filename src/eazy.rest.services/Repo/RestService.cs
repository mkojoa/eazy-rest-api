using eazy.rest.data;
using eazy.rest.data.Models;
using eazy.rest.extension.AsyncQuery;
using eazy.rest.extension.Exceptions;
using eazy.rest.services.Interface;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eazy.rest.services.Repo
{
    public class RestService : IRestService<Guid>
    {
        private readonly DataContext _context;

        public RestService(DataContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateTaskAync(CreateDto dto, Guid userId)
        {
            try
            {
                await _context.LoadStoredProc("dbo.spTask_Create")
                    .AddParam("szTitle", dto.Title)
                    .AddParam("szDescription", dto.Description)
                    .AddParam("bIsDone", dto.IsDone)
                    .AddParam("mAmountCharged", dto.AmountCharged)
                    .AddParam("uUserId", userId)
                    .AddParam("CreatedGuid", out IOutParam<Guid> limitOut)
                    .ExecNonQueryAsync();

                var mock = limitOut.Value;

                return mock;
            }
            catch (Exception e)
            {
                throw new RepoException(e.Message);
            }
        }

        public async Task<bool> DeleteTaskAync(Guid pkId)
        {
            try
            {
                await _context.LoadStoredProc("dbo.spTask_Delete")
                    .AddParam("pkId", pkId)
                    .AddParam("CreatedGuid", out IOutParam<bool> limitOut)
                    .ExecNonQueryAsync();

                var mock = limitOut.Value;

                return mock;
            }
            catch (Exception e)
            {
                throw new RepoException(e.Message);
            }
        }

        public async Task<IEnumerable<TasksDto>> GetSingleTaskAync(Guid pkId)
        {
            try
            {
                List<TasksDto> rows = null;

                await _context.LoadStoredProc("dbo.spTask_GetByPkId")
                    .AddParam("pkId", pkId)
                    .ExecAsync(
                        async r => rows
                            = await r.ToListAsync<TasksDto>()
                    );

                var mock = new AsyncQueryProvider<TasksDto>(rows).AsQueryable();

                return mock;
            }
            catch (Exception e)
            {
                throw new RepoException(e.Message);
            }
        }

        public async Task<IEnumerable<TasksDto>> GetTaskAllAsync()
        {
            try
            {
                List<TasksDto> rows = null;

                await _context.LoadStoredProc("dbo.spTask_GetAll")
                    .ExecAsync(
                        async r => rows
                            = await r.ToListAsync<TasksDto>()
                    );

                var mock = new AsyncQueryProvider<TasksDto>(rows).AsQueryable();

                return mock;
            }
            catch (Exception e)
            {
                throw new RepoException(e.Message);
            }
        }

        public async Task<bool> UpdateTaskAync(UpdateDto dto, Guid pkId, Guid userId)
        {
            try 
            {
                await _context.LoadStoredProc("dbo.spTask_Update")
                    .AddParam("pkId", pkId)
                   .AddParam("szTitle", dto.Title)
                    .AddParam("szDescription", dto.Description)
                    .AddParam("bIsDone", dto.IsDone)
                    .AddParam("mAmountCharged", dto.AmountCharged)
                    .AddParam("uUserId", userId)
                    .AddParam("CreatedGuid", out IOutParam<bool> retParam)
                    .ExecNonQueryAsync(); 

                var mock = retParam.Value;

                return mock;
            }
            catch (Exception e)
            {
                throw new RepoException(e.Message);
            }
        }
    }
}
