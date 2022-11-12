using System;
using System.Net;
using System.Runtime.Serialization;

namespace PanoramBackend.Services.Services
{
	public class ServiceException : Exception
	{
		public ServiceException(params string[] errors)
		{
			this.Errros = errors;
		}
		public ServiceException(HttpStatusCode httpStatusCode, params string[] errors)
			: this(errors)
		{

			this.HttpStatusCode = httpStatusCode;
		}

		public HttpStatusCode HttpStatusCode { get; } = HttpStatusCode.BadRequest;
		public string[] Errros { get; }
	}
}