using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pettigoats.Models
{
    [Table("tblGallery")]
    public class Gallery
    {
        [Key]
        public int PhotoNumberID { get; set; }
        public string Filename { get; set; }
        public string PhotoDescription { get; set; }
    }
}