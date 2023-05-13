using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NukesLab.Core.Common;
using PanoramaBackend.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace PanoramaBackend.Services.Data.DTOs
{
    public class ExtendedRole : IdentityRole<Guid>, IBaseEntity<Guid>
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public override Guid Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public override string Name { get; set; }
        public UserDetails UserDetails { get; set; }
        [JsonIgnore]
        public Guid? CreateUserId { get; set; }
        [JsonIgnore]
        public DateTime? CreateTime { get; set; }
        [JsonIgnore]
        public Guid? EditUserId { get; set; }
        [JsonIgnore]
        public DateTime? EditTime { get; set; }

        [JsonIgnore]
        public bool? IsDeleted { get; set; }

        public byte[]? Timestamp { get; set; }
        [NotMapped]
        [JsonIgnore]
        public bool? IsNew => (this.Id == null) ? false : this.Id.Equals(default(Guid));
        [NotMapped]
        object IBaseEntity.Id => this.Id;
    }

    //public class ApplicationUserToken :IBaseEntity<Guid>
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    //    public  Guid Id { get; set; }
    //    [Column(TypeName = "nvarchar(100)")]
    //    [JsonIgnore]
    //    public Guid CreateUserId { get; set; }
    //    [JsonIgnore]
    //    public DateTime? CreateTime { get; set; }
    //    [JsonIgnore]
    //    public Guid EditUserId { get; set; }
    //    [JsonIgnore]
    //    public DateTime? EditTime { get; set; }

    //    [JsonIgnore]
    //    public bool IsDeleted { get; set; }

    //    public byte[] Timestamp { get; set; }
    //    [NotMapped]
    //    [JsonIgnore]
    //    public bool IsNew => (this.UserId == null) ? false : this.UserId.Equals(default(Guid));
    //    [NotMapped]
    //    object IBaseEntity.Id => this.Id;
    //}
}

