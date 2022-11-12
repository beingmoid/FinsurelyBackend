
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;
using NukesLab.Core.Api;
using NukesLab.Core.Common;
using NukesLab.Core.Repository;
using PanoramBackend.Services.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static NukesLab.Core.Common.Constants;

namespace PanoramaBackend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public abstract class BaseController : ControllerBase
	{
		public BaseController(RequestScope scopeContext, IBaseService service)
		{

		}

		protected IBaseService Service { get; }

		protected internal BaseResponse constructResponse(object response)
		{
			return new BaseResponse()
			{

				dynamicResult = response,
				isSuccessfull = OtherConstants.isSuccessful,
				statusCode = response != null ? 200 : 500,
				messageType = OtherConstants.messageType,
				message = OtherConstants.responseMsg,
				errorMessage = OtherConstants.responseMsg,
				NewAccessToken = Utils.NewAccessToken ?? null
			};
		}
	}

	public class PaginatedData<TEntity,TKey> where	TEntity : class,IBaseEntity<TKey>
    {

        public int totalRows { get; set; }
        public dynamic data{ get; set; }
    }


	public abstract class BaseController<TEntity, TKey> : BaseController
		where TEntity : class, IBaseEntity<TKey>, new()
	{
		public BaseController(RequestScope scopeContext, IBaseService<TEntity, TKey> service)
			: base(scopeContext, service)
		{
			this.Service = service;
		}

		protected new IBaseService<TEntity, TKey> Service { get; }
		// GET api/values
		[HttpGet]

		public virtual async Task<BaseResponse> Get()
		{
			var result = await this.Service.Get();

			OtherConstants.isSuccessful = true;
			OtherConstants.messageType = MessageType.Success;
			return (constructResponse(result));
		}
	//	[AllowAnonymous]
	//	[HttpGet("GenericSearch")]

	//	public virtual async Task<BaseResponse> GenericSearch(string query)
	//	{
	//		var stringProperties = typeof(TEntity).GetProperties().Where(prop =>
	//prop.PropertyType == query.GetType()).ToList();
	//		var data = await this.Service.Get(customer =>
	// stringProperties.Any(prop => ((prop.GetValue(customer, null) == null) ? "" : prop.GetValue(customer, null).ToString().ToLower()) == query));
	//		OtherConstants.messageType = MessageType.Success;
	//		return (constructResponse(data));
	//	}
		[HttpGet]
		public virtual async Task<BaseResponse> GetPaginated<TEntity>(int page, int pageSize) where TEntity : class,IBaseEntity<TKey>
		{
			var items = await this.Service.GetPaginated(page, pageSize);
			var response = new PaginatedData<TEntity,TKey>();
			if (items != null)
			{
				
				response.totalRows = items.Count;
				response.data = items;
				OtherConstants.isSuccessful = true;
				return constructResponse(response);
			}
			else
			{
				OtherConstants.isSuccessful = false;
				return constructResponse(response);
			}
		}

		// GET api/values/5
		[HttpGet("{id}")]

		public virtual async Task<BaseResponse> Get(TKey id) => constructResponse(await this.Service.GetOne(id));

		// POST api/values
		[HttpPost]

		public virtual async Task<BaseResponse> Post([FromBody] TEntity entity)
		{
			var result = await Service.Insert(new[] { entity });
		
			if (result.Success)
			{
				OtherConstants.isSuccessful = true;
				OtherConstants.messageType = MessageType.Success;
				return constructResponse(result.Entities.Single());
			}
			else
			{
				OtherConstants.isSuccessful = false;
				OtherConstants.messageType = MessageType.Error;
				return constructResponse(BadRequest());
			}
		}

		// PUT api/values/5
		[HttpPut("{id}")]
		public virtual async Task<BaseResponse> Put(TKey id, [FromBody] TEntity entity)
		{
			var result = await Service.Update(id, entity);
			if (result.Success)
			{
				OtherConstants.isSuccessful = true;
				OtherConstants.messageType = MessageType.Success;
				return constructResponse(result.Entity);
			}
			else
			{
				OtherConstants.isSuccessful = false;
				OtherConstants.messageType = MessageType.Error;
				return constructResponse(BadRequest());
			}
		}

		// DELETE api/values/5
		[HttpDelete("{id}")]
		public virtual async Task<BaseResponse> Delete(TKey id)
		{
			if (await this.Service.Delete(id))
			{
				OtherConstants.isSuccessful = true;
				OtherConstants.messageType = MessageType.Success;
				return constructResponse("Entity deleted succesfully");
			}
			else
			{
				OtherConstants.isSuccessful = false;
				OtherConstants.messageType = MessageType.Error;
				return constructResponse(BadRequest());
			}
		}

	
	}
}
