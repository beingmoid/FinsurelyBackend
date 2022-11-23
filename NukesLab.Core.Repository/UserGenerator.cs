using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Text;

namespace NukesLab.Core.Repository
{
    public class UserGenerator : ValueGenerator
    {
        private static bool? _isScopeAvailable;

        public override bool GeneratesTemporaryValues => true;

        protected override object NextValue(EntityEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            var user = entry.Context.GetService<IUserProvider>();
            return user.UserId;
        }
    }
}
