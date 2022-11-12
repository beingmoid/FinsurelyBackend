//using Microsoft.EntityFrameworkCore.ChangeTracking;
//using Microsoft.EntityFrameworkCore.Infrastructure;
//using Microsoft.EntityFrameworkCore.ValueGeneration;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using NukesLab.Core.Repository;

//namespace OASCore.Repositories
//{
//	public class UserIdGenerator : HRZValueGenerator
//	{
//		private static bool? _isScopeAvailable;
//		private RequestScope _request;

//		public UserIdGenerator(RequestScope request)
//		{
//			_request = request;
//		}
//		protected override object NextValue(EntityEntry entry)
//		{
//			if (entry == null)
//			{
//				throw new ArgumentNullException(nameof(entry));
//			}

//			// Temporary fix, as migration can not resolve it.
//			RequestScope scope = null;
//			if (!_isScopeAvailable.HasValue)
//			{
//				try
//				{
//					scope = entry.Context.GetService<RequestScope>();
//					_isScopeAvailable = true;
//				}
//				catch
//				{
//					_isScopeAvailable = false;
//				}
//			}
//			else if (_isScopeAvailable.Value)
//			{
//				scope = entry.Context.GetService<RequestScope>();
//			}


//			return scope?.UserId;
//		}
//	}

//}
