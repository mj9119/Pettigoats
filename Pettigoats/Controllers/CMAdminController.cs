using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pettigoats.Models;
using System.Drawing;
using System.IO;

using System.Data;

namespace Pettigoats.Controllers
{
    public class CMAdminController : Controller
    {
        private EFDbGalleryContext db = new EFDbGalleryContext();
        //
        // GET: /CMAdmin/


        public ActionResult Index()
        {
            //var db = new EFDbGalleryContext();
            // db.Gallery.Find(116);
            // var xxxgalleryCollection = db.Gallery.Find(116);
            // var xxxgalleryCollection = db.Gallery.T;

            var galleryCollection = db.Gallery.ToArray();

            ViewBag.Message = "Please Choose to Add, Update or Delete from the options in yellow below ";
            return View(galleryCollection);
        }

        [Authorize]
        public ActionResult Delete(Int32 id)
        {                                               
            //Student student = db.Students.Find(id);
            Gallery gallery = db.Gallery.Find(id);
            string smallImageFilePath = Path.Combine(Server.MapPath("~/Images/demo/gallery/") + "ThumbSize" + (gallery.Filename));
            string largeImageFilePath = Path.Combine(Server.MapPath("~/Images/demo/gallery/") + (gallery.Filename));
            //string ThumbNailFilePath = "~/images/demo/gallery/ThumbSize" + gallery.Filename;

            db.Gallery.Remove(gallery);
            db.SaveChanges();

            //Delete small and large files from the FileSystem                            
            System.IO.File.Delete(smallImageFilePath);

            //System.IO.File.GetAccessControl(largeImageFilePath);
            try 
            {
            //System.IO.File.GetAccessControl(largeImageFilePath);
            System.IO.File.Delete(largeImageFilePath);
            }
            catch (System.IO.IOException e)
            {
                TempData["SomeData"] = " Delete exception.  The Details follow:  " + e.ToString();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["SomeData"] = " Delete exception.  The Details follow:  " + ex.ToString();
                return RedirectToAction("Index");
            }            


            TempData["SomeData"] = "Deleted File: " + gallery.Filename;
            return RedirectToAction("Index");
        }

        [Authorize]                
        public ActionResult Update(Int32 id)
        {
            ViewBag.Message = "It's Update Time";
                        
            Gallery gallery = db.Gallery.Find(id);

            if (gallery == null)
            {
                return HttpNotFound();
            }
            return View(gallery);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind(Include="PhotoNumberID,Filename,PhotoDescription")]Gallery gallery)
        {
            ViewBag.Message = "It's Update Time";

            if (ModelState.IsValid)
            {
                db.Entry(gallery).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SomeData"] = "Photo was successfully updated";
                return RedirectToAction("Index");
            }            
            return View(gallery);
        }


        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Message = "Click Browse to choose a photo to upload.  Optionally, enter a Caption below";
            return View();
        }
        
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase file, Models.Gallery gallerycm)
        {
            ViewBag.Message = "Testing Gallery File Create";

            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Images/demo/gallery"),
                                               Path.GetFileName(file.FileName));
                    
                    //System.IO.File.SetAttributes(path, System.IO.FileAttributes.Normal);
                    
                    //System.Drawing.Image MainImgPhotoVert = System.Drawing.Image.FromFile(path);
                    /*
                    System.Drawing.Image MainImgPhotoVert = System.Drawing.Image.FromStream(System.IO.Stream file);
                    Bitmap MainImgPhoto = (System.Drawing.Bitmap)ScaleByPercent(MainImgPhotoVert, 100);
                    MainImgPhoto.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                    MainImgPhoto.Dispose();
                    */

                    //if the file alread exists get out!
                    if (System.IO.File.Exists(path))
                    {
                        TempData["SomeData"] = "A Photo named: " + file.FileName + " already exists.  You may Delete it below.  Then re-Add the photo" ;
                        return RedirectToAction("Index");
                    }
                    {
                        file.SaveAs(path);
                    }   
                        
                    //file.InputStream.Flush(); //useless
                    //file.InputStream.Close(); //less than useless
                    //file.InputStream.Dispose(); //complete waste of keystrokes
                    
                    //System.IO.File.SetAttributes(path, System.IO.FileAttributes.Normal);

                    // Validating whether the following commented code releases a recently created
                    // file from IIS for file Delete.  Problem occuring in the Visual Studio test environment.
                    //file.InputStream.Dispose();
                    //GC.Collect();
                    //GC.WaitForPendingFinalizers();

                    // Create the Thumbnail image
                    string smallImageFilePath = Path.Combine(Server.MapPath("~/Images/demo/gallery/") + "ThumbSize" + (file.FileName));
                    
                    //allocate an Image object from the uploaded full sized .jpg 

                    // works System.Drawing.Image imgPhotoVert = System.Drawing.Image.FromFile(path);
                    FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                    System.Drawing.Image imgPhotoVert = System.Drawing.Image.FromStream(fs);

                    Bitmap imgPhoto = (System.Drawing.Bitmap)ScaleByPercent(imgPhotoVert, 50);

                    //imgPhoto.Save(smallImageFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    imgPhoto.Save(smallImageFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    fs.Close();

                    imgPhoto.Dispose();
                    //((IDisposable)imgPhoto).Dispose();
                    

                    var gallery = new Gallery();
                    //gallery.PhotoNumberID = 9;
                    gallery.Filename = file.FileName;
                    if (gallerycm.PhotoDescription == null)
                        gallerycm.PhotoDescription = " ";
                    gallery.PhotoDescription = gallerycm.PhotoDescription;

                    var galleryContext = new EFDbGalleryContext();
                    galleryContext.Gallery.Add(gallery);
                    galleryContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    TempData["SomeData"] = file.FileName + " Upload exception.  The Details follow:  " + ex.ToString();
                    return RedirectToAction("Index");
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            TempData["SomeData"] = "Photo was successfully Added";
            return RedirectToAction("Index");
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
