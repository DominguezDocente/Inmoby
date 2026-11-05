using Microsoft.AspNetCore.Identity;

namespace Identity.App.Data.Entitites
{
    public class AppRole : IdentityRole<Guid>
    {
        public AppRole()
        {
            
        }

        public AppRole(string name) : base(name)
        {
            
        }
    }
}
