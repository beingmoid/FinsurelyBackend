using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static NukesLab.Core.Common.Constants;

namespace NukesLab.Core.Common
{
	public static class ExtensionMethods
	{
		public static string ToMd5Hash(this string input)
		{
			using (MD5 md5Hash = MD5.Create())
			{
				byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
				StringBuilder sBuilder = new StringBuilder();
				for (int i = 0; i < data.Length; i++)
				{
					sBuilder.Append(data[i].ToString("x2"));
				}
				return sBuilder.ToString();
			}
		}

        public static string GetRole(this ClaimsPrincipal principal)
        {
            return principal.Claims.ToList().FirstOrDefault(x => x.Type.Trim() == CustomClaims.Role).Value;
        }

        public static string GetUserId(this ClaimsPrincipal principal)
        {
            return principal.Claims.ToList().FirstOrDefault(x => x.Type.Trim() == CustomClaims.UserId).Value;
        }
		public static byte[] GetTenantId(this ClaimsPrincipal principal)
		{
			return Encoding.ASCII.GetBytes( principal.Claims.ToList().FirstOrDefault(x => x.Type.Trim() == CustomClaims.TenantId).Value);
		}
	}

    
}
