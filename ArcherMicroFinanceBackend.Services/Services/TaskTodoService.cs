
using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Services.Services
{
    public class TaskTodoService : BaseService<TaskTodo, int>, ITaskTodoService
    {
        public TaskTodoService(RequestScope scopeContext, ITaskTodoRepository repo) : base(scopeContext, repo)

        {

        }
    }
    public interface ITaskTodoService : IBaseService<TaskTodo, int>
    {

    }

    public class StatusService : BaseService<Status, int>, IStatusService
    {
        public StatusService(RequestScope scopeContext, IStatusRepository repo) : base(scopeContext, repo)

        {

        }
    }
    public interface IStatusService : IBaseService<Status, int>
    {

    }

    public class PriorityService : BaseService<Priority, int>, IPriorityService
    {
        public PriorityService(RequestScope scopeContext, IPriorityRepository repo) : base(scopeContext, repo)

        {

        }
    }
    public interface IPriorityService : IBaseService<Priority, int>
    {

    }
}
