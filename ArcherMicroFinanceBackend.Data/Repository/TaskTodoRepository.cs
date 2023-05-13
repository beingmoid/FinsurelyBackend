using PanoramaBackend.Data;
using PanoramaBackend.Data.Entities;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PanoramaBackend.Data.Repository
{
    public class TaskTodoRepository : EFRepository<TaskTodo, int>, ITaskTodoRepository
    {
        public TaskTodoRepository(AMFContext requestScope) : base(requestScope)
        {

        }

    }
    public interface ITaskTodoRepository : IEFRepository<TaskTodo, int>
    {

    }

    public class StatusRepository : EFRepository<Status, int>, IStatusRepository
    {
        public StatusRepository(AMFContext requestScope) : base(requestScope)
        {

        }

    }
    public interface IStatusRepository : IEFRepository<Status, int>
    {

    }

    public class PriorityRepository : EFRepository<Priority, int>, IPriorityRepository
    {
        public PriorityRepository(AMFContext requestScope) : base(requestScope)
        {

        }

    }
    public interface IPriorityRepository : IEFRepository<Priority, int>
    {

    }

}
