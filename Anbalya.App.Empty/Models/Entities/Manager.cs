using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Models.Entities;

namespace Models.Entities
{
    public class Manager : BaseEntity
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }

        public Manager() { }
    }
}