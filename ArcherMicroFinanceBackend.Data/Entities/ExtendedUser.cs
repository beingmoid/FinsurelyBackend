using NukesLab.Core.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using PanoramaBackend.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PanoramaBackend.Services.Data.DTOs
{
    public class ExtendedUser : IdentityUser<Guid>, IBaseEntity<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public override Guid Id { get; set; }



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

        public byte[] Timestamp { get; set; }
        [NotMapped]
        [JsonIgnore]
        public bool? IsNew => (this.Id == null) ? false : this.Id.Equals(default(Guid));
        [NotMapped]
        [JsonIgnore]
        object IBaseEntity.Id => this.Id;
        private ICollection<UserDetails> _UserDetails;
        public ICollection<UserDetails> UserDetails => _UserDetails ?? (new List<UserDetails>());
    }


   
    public enum MarriageStatus
    {
        Single = 1,
        Married,
        Divorced
    }
    public enum TypeOfUsers
    {
        Agent = 1,
        Broker
    }
    public enum Gender
    {
        Male = 1,
        Female
    }

}


