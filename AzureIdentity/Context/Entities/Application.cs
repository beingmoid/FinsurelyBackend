using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AzureIdentity.Context.Entities
{
    public class Application
    {
        public Guid? TenantId{ get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        public string ApplicationName { get; set; }




    }
}
