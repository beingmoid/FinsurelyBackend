using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace NukesLab.Core.Common
{
	
	public abstract class BaseEntity<TKey> : IBaseEntity<TKey>
	{
		[NotMapped]
		object IBaseEntity.Id => this.Id;

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public virtual TKey Id { get; set; }

        [JsonIgnore]
        public Guid? CreateUserId { get;  set; }
        [JsonIgnore]
        public DateTime? CreateTime { get;  set; }
        [JsonIgnore]
        public Guid? EditUserId { get;  set; }
        [JsonIgnore]
        public DateTime? EditTime { get;  set; }

		[JsonIgnore]
		public bool? IsDeleted { get; set; }

        public byte[] Timestamp { get; set; }
		[NotMapped]
		[JsonIgnore]
		public bool? IsNew => (this.Id == null) ? false : this.Id.Equals(default(TKey));





		public virtual void Map(IMapper mapper, BaseEntity<TKey> dest)
		{
			mapper.Map(this, dest);
		}
	}

	public interface IBaseEntity<TKey> : IBaseEntity
	{
		new TKey Id { get; set; }
	}
	public interface IBaseEntity
	{
		object Id { get; }

		Guid? CreateUserId { get; }
		DateTime? CreateTime { get; }
        Guid? EditUserId { get; }
		DateTime? EditTime { get; }
		bool? IsDeleted { get; set; }
		byte[]? Timestamp { get; set; }
		bool? IsNew { get; }

	}
	public abstract class AuditEntries<T> where T : IBaseEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }


		private ICollection<T> _collection;
		public ICollection<T> CollectionsEntries => _collection ?? (_collection = new List<T>());
	}
}
