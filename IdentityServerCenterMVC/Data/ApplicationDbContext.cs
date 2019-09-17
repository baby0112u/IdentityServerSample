using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServerCenterMVC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace IdentityServerCenterMVC.Data {
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser> {
    }
}
