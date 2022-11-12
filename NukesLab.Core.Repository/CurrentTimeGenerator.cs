using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace NukesLab.Core.Repository
{
	public class CurrentTimeGenerator : HRZValueGenerator
	{
		private static bool? _isScopeAvailable;
		protected override object NextValue(EntityEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException(nameof(entry));
			}

			// Its workaround so it should not generate any value on migration.
			if (!_isScopeAvailable.HasValue)
			{
				try
				{
					entry.Context.GetService<RequestScope>();
					_isScopeAvailable = true;
				}
				catch
				{
					_isScopeAvailable = false;
				}
			}

			if (_isScopeAvailable.Value)
			{
				return DateTime.UtcNow;
			}
			else
			{
				return null;
			}
		}
	}
}
