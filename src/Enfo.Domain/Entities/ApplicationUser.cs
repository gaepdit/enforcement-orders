using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Enfo.Domain.Entities
{
    // Add profile data for application users by adding properties to the 
    // ApplicationUser class

    public class ApplicationUser : IdentityUser<Guid>
    {
        [ProtectedPersonalData]
        [StringLength(150)]
        public string FirstName { get; set; }

        [ProtectedPersonalData]
        [StringLength(150)]
        public string LastName { get; set; }

        public bool Active { get; set; } = true;

        // Generated fields
        public string FullName =>
            string.Join(" ", new[] {FirstName, LastName}.Where(s => !string.IsNullOrEmpty(s)));

        public string SortableFullName =>
            string.Join(", ", new[] {LastName, FirstName}.Where(s => !string.IsNullOrEmpty(s)));
    }
}
