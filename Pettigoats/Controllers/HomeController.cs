using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using Pettigoats.Models;
using System.IO;
using System.Drawing;

namespace Pettigoats.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            ViewBag.Slide1Image = "/images/demo/homepage/GoatEve.jpg";
            ViewBag.Slide1Title = "Sunset at Pettigoat Acres";
            ViewBag.Slide1Text = "";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "on hold";

            //var gallery = new Gallery();
            //gallery.PhotoNumberID = 9;
            //gallery.Filename = "LARGEGoatInGrass.jpg";
            //gallery.PhotoDescription = "Joe Insert Testing";

            //var galleryContext = new EFDbGalleryContext();
            //galleryContext.Gallery.Add(gallery);
            //galleryContext.SaveChanges();


            return View();
        }

        //[HttpGet]
        //[Authorize]
        //public ActionResult CMGallery()
        //{
        //    ViewBag.Message = "Click Browse to choose a photo to upload.  Optionally, enter a Caption below";

        //    return View();
        //}

        //[HttpPost]
        //[Authorize]
        //public ActionResult CMGallery(HttpPostedFileBase file, Models.Gallery gallerycm)
        //{
        //    ViewBag.Message = "Testing CMGallery";

        //    if (file != null && file.ContentLength > 0)
        //        try
        //        {
        //            string path = Path.Combine(Server.MapPath("~/Images/demo/gallery"),
        //                                       Path.GetFileName(file.FileName));
        //            file.SaveAs(path);
        //            ViewBag.Message = file.FileName + " File was uploaded successfully";
                    
        //            //Now, create the Thumbnail image
        //            string smallImageFilePath = Path.Combine(Server.MapPath("~/Images/demo/gallery/") + "ThumbSize" + (file.FileName));
        //            //allocate an Image object from the uploaded full sized .jpg 
        //            System.Drawing.Image imgPhotoVert = System.Drawing.Image.FromFile(path);
        //            Bitmap imgPhoto = (System.Drawing.Bitmap)ScaleByPercent(imgPhotoVert, 50);
        //            imgPhoto.Save(smallImageFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
        //            imgPhoto.Dispose();

        //            var gallery = new Gallery();
        //            gallery.Filename = file.FileName;
        //            gallery.PhotoDescription = gallerycm.PhotoDescription;
        //            var galleryContext = new EFDbGalleryContext();
        //            galleryContext.Gallery.Add(gallery);
        //            galleryContext.SaveChanges();
        //        }
        //        catch (Exception ex)
        //        {
        //            TempData["SomeData"] = file.FileName + " Upload exception.  The Details follow:  " + ex.ToString() ;
        //            return RedirectToAction("CMGallery");
        //        }
        //    else
        //    {
        //        ViewBag.Message = "You have not specified a file.";
        //    } 
        //    return View();
        //}


        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Page";
            //@ViewBag.ContactPageMessage = " ";
            return View();
        }

        [HttpPost]
        public ActionResult Contact(Models.ContactMe contact)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Message = "Contact Page";
                MailMessage msg = new MailMessage();

                try
                {
                    msg.From = new MailAddress(contact.EmailAddress);
                }
                catch (System.FormatException sysforex)
                {
                    Console.WriteLine("Exception caught in Contact(): {0}", sysforex.ToString());
                    @ViewBag.ContactPageMessage = "Invalid email address";
                    return View("Contact");
                }
                string fromPlusBody = "From: " + contact.Name + " email address: " + msg.From + " Message: " + contact.Comments;
                msg.Body = fromPlusBody;
                msg.From = new MailAddress("mroth1101@pettigoats.com");

                //string toEmailAddress = "maryroth1101@yahoo.com";
                //msg.To.Add(new MailAddress("mroth@pettigoats.com"));
                msg.To.Add(new MailAddress("maryroth1101@pettigoats.com"));

                msg.Subject = "from Pettigoats.com Contact page";
                

                //MailMessage msg = new MailMessage("me@mymail.com", "maryroth1101@yahoo.com", "Test with Exception from pettigoats", "email body");
                SmtpClient client = new SmtpClient("relay-hosting.secureserver.net", 25);

                try
                {
                    client.Send(msg);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught in CreateTestMessage(): {0}", ex.ToString());
                    @ViewBag.ContactPageMessage = "The system is temporarily unable to transmit your email.  Please contact Pettigoats owner Mary Roth at maryroth1101@yahoo.com";

                    //return RenderAction("ADifferentResult");
                    //return RedirectToAction("Index");

                    //return Redirect("Contact");

                    //return RedirectToRoute(Index);
                    return View("Contact");
                    //return View("Index", "The system is temporarily unable to transmit your email.  Please contact Pettigoats owner Mary Roth at maryroth1101@yahoo.com");
                }
                @ViewBag.ContactPageMessage = "Your message was successfully emailed to maryroth1101@yahoo.com ";
                return View("Contact");
            }       
            else
                return View();
        }

        public ActionResult Gallery()
        {
            ViewBag.Message = "Gallery Page";

            //var gallery = new Gallery();
            //gallery.PhotoNumberID = 7;
            //gallery.Filename = "LARGEGoatInGrass.jpg";
            //gallery.PhotoDescription = "Joe Insert Testing";

            //var galleryContext = new EFDbGalleryContext();
            //galleryContext.Gallery.Add(gallery);
            //galleryContext.SaveChanges();

            //Gallery galleryRet = galleryContext.Gallery.Single(emp => emp.Id == id);

            var db = new EFDbGalleryContext();
            // db.Gallery.Find(116);
            // var xxxgalleryCollection = db.Gallery.Find(116);
            // var xxxgalleryCollection = db.Gallery.T;

            var galleryCollection = db.Gallery.ToArray();

            
            //anotherint = galleryContext.Gallery.Count();
            //Gallery galleryRet = galleryContext.Gallery.Single(gal => gal.Id == id);

            return View(galleryCollection);
        }
        
        public ActionResult Bucks()
        {
            ViewBag.Message = "Bucks Page";

            return View();
        }
        public ActionResult Does()
        {
            ViewBag.Message = "Does Page";

            return View();
        }

        [HttpGet]
        public ViewResult ContactMe()
        {
            ViewBag.Message = "ContactMe Page Under Construction";            
            return View();          
        }

        [HttpPost]
        public ViewResult ContactMe(Models.ContactMe contact)
        {
            ViewBag.Message = "ContactMe Page Under Construction";

            string test1 = contact.Comments;

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(contact.EmailAddress);
            msg.To.Add(new MailAddress("maryroth1101@yahoo.com"));
            msg.Subject = "from Pettigoats.com";
            msg.Body = "pettigoats Body of email";
            
            SmtpClient client = new SmtpClient("relay-hosting.secureserver.net", 25);

            //client.Send(msg);

            try
            {
                client.Send(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage(): {0}", ex.ToString());
                return View("ContactMe");
            }

            return View("Contact");            
        }


        static System.Drawing.Image ScaleByPercent(System.Drawing.Image imgPhoto, int Percent)
        {
            float nPercent = ((float)Percent / 100);

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;

            int destX = 0;
            int destY = 0;
            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(destWidth, destHeight,
                                     System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                                    imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode =
                System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }



    }
}
