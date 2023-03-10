using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UcomGridView.Shared.Dtos
{
    public class CreateUserDto 
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
        public IFormFile Avatar { get; set; }
        public int StatusId { get; set; }

        public string? IsValid()
        {
            var message = new StringBuilder();

            var email = new EmailAddressAttribute();
            if (!email.IsValid(Email)) message.Append(Messages.EmailError + "<br>");

            return message.Length != 0 ? message.ToString() : null;
        }
    }
}
