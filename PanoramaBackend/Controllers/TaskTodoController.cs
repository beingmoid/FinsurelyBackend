using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NukesLab.Core.Api;
using NukesLab.Core.Repository;
using PanoramaBackend.Controllers;
using PanoramBackend.Data.Entities;
using PanoramBackend.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PanoramaBackend.Api.Controllers
{

    public class TaskTodoController : BaseController<TaskTodo,int>
    {
        private readonly ITaskTodoService _service;

        public TaskTodoController(RequestScope requestScope,ITaskTodoService
            service)
            :base(requestScope,service)
        {
            _service = service;
        }
        public async override Task<BaseResponse> Get()
        {
            var result = await _service.Get(x => x.Include(x => x.AssignedTo)
           .Include(x => x.AssignedBy)
           .Include(x => x.Status)
                .Include(x => x.Priority)
            );
            return constructResponse(result.ToList());
        }

        [HttpGet("EmployeeTask")]
        public async Task<BaseResponse> GetTaskForEmployees(int userId)
        {
            var result = await _service.Get(x => x.Include(x => x.AssignedTo)
          .Include(x => x.AssignedBy)
          .Include(x => x.Status)
               .Include(x => x.Priority),x=> x.AssignedToId==userId 
           );
           

            return constructResponse(result);
        }
    }
}
