using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace Pettigoats.Models
{
    public class EFDbGalleryContext : DbContext
    {
        public DbSet<Gallery> Gallery { get; set; }
    }
}