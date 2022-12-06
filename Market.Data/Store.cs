using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Market.Data.Users;

namespace Market.Data
{
    public class Store
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}

