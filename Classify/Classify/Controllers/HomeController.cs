using Classify.App_Start;
using Classify.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Classify.Controllers
{
    [ExceptionFilter]    
    [Authentication]
    public class HomeController : Controller
    {
        //Main Index page
        public ActionResult Index()
        {
            using (var db = new ClassifyEntities())
            {                
                ViewBag.Category = db.Categorymasters.ToList();
                return View(db.Products.ToList());
            }            
        }
        
        //Product Add Page
        [HttpGet]
        public ActionResult Create()
        {
            
            using (var db = new ClassifyEntities())
            {                
                Product product = new Product();                
                ViewBag.Category2 = new SelectList( db.Categorymasters.ToList(),"Id","Categoryname");                  
                return View(product);
            }
                
        }

        /// <summary>
        /// HEre i made one method for make file name from httppostedfile base also saved in folder
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(Product product)
        {
            using (var db = new ClassifyEntities())
            {
                if(product.ProductImg != null)
                {
                    product.Image = getimage(product);
                    string path1 = "~/images/Productimages/";
                    var filename1 = Path.Combine(Server.MapPath(path1), product.Image);
                    product.ProductImg.SaveAs(filename1);
                }               
                product.Createddate = DateTime.Now;
                product.UserId = Convert.ToInt32(Session["UserId"]);
                db.Products.Add(product);            
                if (db.SaveChanges() > 0)
                {
                    TempData["Successmessage"] = "Product added successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Category2 = new SelectList(db.Categorymasters.ToList(), "Id", "Categoryname");
                    return View(product);
                }               
            }
        }

        //Edit Product page
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            using (var db = new ClassifyEntities())
            {
                if(db.Products.Any(x => x.Id == Id))
                {
                    ViewBag.Category2 = new SelectList(db.Categorymasters.ToList(), "Id", "Categoryname");
                    return View(db.Products.Where(x => x.Id == Id).FirstOrDefault());
                }
                else
                {
                    return HttpNotFound();
                }
                
            }            
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Name,Category,Code,Price,Description,Status,Discount")] Product product)
        {
            using (var db = new ClassifyEntities())
            {
                if (product.ProductImg != null)
                {
                    product.Image = getimage(product);
                    string path1 = "~/images/Productimages/";
                    var filename1 = Path.Combine(Server.MapPath(path1), product.Image);
                    product.ProductImg.SaveAs(filename1);
                }
                else
                {
                    product.Image = db.Products.Where(x => x.Id == product.Id).First().Image;
                }
                var Productdata = db.Products.Where(x => x.Id == product.Id).First();
                Productdata.Image = product.Image;
                Productdata.Name = product.Name;
                Productdata.Category = product.Category;
                Productdata.Price = product.Price;
                Productdata.Code = product.Code;
                Productdata.Description = product.Description;
                Productdata.Status = product.Status;
                Productdata.Discount = product.Discount;                
                Productdata.ModifiedDate = DateTime.Now;                
                db.Products.Attach(Productdata);
                db.Entry(Productdata).State = System.Data.Entity.EntityState.Modified;                
                if (db.SaveChanges() > 0)
                {
                    TempData["Successmessage"] = "Product Edited successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Category2 = new SelectList(db.Categorymasters.ToList(), "Id", "Categoryname");
                    return View(product);
                }
            }
        }

        //Here is I make file names
        [NonAction]
        private string getimage(Product product)
        {
            string filename1 = DateTime.Now.ToString("yymmssfff");
            string extension1 = Path.GetExtension(product.ProductImg.FileName);
            filename1 = filename1 + extension1;
            return filename1;
        }

        //This is Post delete method
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            using (var db = new ClassifyEntities())
            {
                if(db.Products.Any(x => x.Id == Id))
                {
                    db.Products.Remove(db.Products.Where(x => x.Id == Id).First());
                    db.SaveChanges();
                    TempData["Successmessage"] = "Product Deleted successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        //This is Post delete method
        [HttpPost]
        public ActionResult DeleteMultiple(int[] MultipleDelId)
        {
            if (MultipleDelId != null && MultipleDelId.Length > 0)
            {
                using (var db = new ClassifyEntities())
                {
                    var List = new List<Product>();
                    
                    for(int i=0;i<MultipleDelId.Length;i++)
                    {
                        var Id = MultipleDelId[i];
                        db.Products.Remove(db.Products.Where(x=>x.Id==Id).First());
                        
                    }                    
                    db.SaveChanges();
                    TempData["Successmessage"] = "Products successfully Deleted";
                    return RedirectToAction("Index");
                }
            }
            TempData["Error"] = "Can't Delete Multiple item";
            return RedirectToAction("Index");
        }
    }
}