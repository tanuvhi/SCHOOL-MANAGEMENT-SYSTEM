using LCC.Domain.Abstract;
using LCC.Domain.Entities;
using LCC.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace LCC.WebUI.Controllers
{
    public class PurchaseController : Controller
    {
        //
        // GET: /Purchase/
        private readonly IPurchaseRepository purchaseRepo ;

      
      public  PurchaseController(IPurchaseRepository PRepo)
        {
            var badCulture = new CultureInfo("");
            badCulture.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            Thread.CurrentThread.CurrentCulture = badCulture;
            purchaseRepo = PRepo;
        }

        public ViewResult PurchaseItem()
        {
            
            return View();
         }
        [HttpPost]
        public ActionResult PurchaseItem(int[] ItemId, decimal[] Quantiy, decimal[] Total_Price ,string PurchaseDate)
        {
            List<PurchaseList> prl = new List<PurchaseList>();
            for (int i = 0; i < ItemId.Length; i++)
            {

                PurchaseList purl = new PurchaseList();
                purl.ItemId = ItemId[i];
                purl.Price = Total_Price[i];
                purl.Quantity = Quantiy[i];
                purl.PurchaseDate = Convert.ToDateTime( PurchaseDate);
                purl.EntryDate = DateTime.Now.Date;
                if(purl.ItemId!=0 && purl.PurchaseDate !=null && purl.Quantity !=0 && purl.Price !=0 )
                purchaseRepo.UpdatePurchaseList(purl);
            }

                return View();
        }
        public ViewResult AddNewItem()
        {

            ViewBag.ItemLists = new SelectList(purchaseRepo.QuantityTypes, "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult AddNewItem(ItemList itemList)
        {
          var r=  purchaseRepo.ItemLists.Where(p => p.Name == itemList.Name).FirstOrDefault();
          ViewBag.ItemLists = new SelectList(purchaseRepo.QuantityTypes, "Id", "Name");
          if (r != null)
          {
              TempData["UnsuccessMessage"] = string.Format("This Item Name Already Added ");
              return View();
          }
           
            purchaseRepo.UpdateItemList(itemList);
            TempData["SuccessMessage"] = string.Format("Item Name added successful");
            return View();
        }
          public JsonResult INameAutoComplite(string term)
             {
            var result = from u in purchaseRepo.ItemLists                     
                         where u.Name.ToLower().Contains(term.ToLower())
                         select new { u.Name, u.Id };

            return Json(result, JsonRequestBehavior.AllowGet);
             }
     
        public ViewResult TodayInsertedList()
          {
              PurchaseModel model = new PurchaseModel();


            
              DateTime dat = DateTime.Now.Date;
              model.purchaseListL = purchaseRepo.PurchaseLists.Where(p => p.EntryDate == dat).ToList();
              model.itemDic = purchaseRepo.ItemLists.ToDictionary(k=>k.Id,k=>k.Name);
              return View(model);
          }
        public ActionResult EditPurchaseItemList(int id)
        {
            PurchaseModel model = new PurchaseModel();
            model.purchaseList = purchaseRepo.PurchaseLists.Where(p => p.Id == id).FirstOrDefault();
            model.itemDic = purchaseRepo.ItemLists.ToDictionary(k => k.Id, k => k.Name);
           if (model.purchaseList.EntryDate != DateTime.Now.Date && Session["MyKey"].ToString()!="1")
           {
               return RedirectToAction("TodayInsertedList"); 
           }
            return View(model);
        }

        [HttpPost]
        public ActionResult EditPurchaseItemList(PurchaseModel purchase)
        {
            purchase.purchaseList.EntryDate = purchase.purchaseList.EntryDate.Value.Date;
            purchase.purchaseList.PurchaseDate = purchase.purchaseList.PurchaseDate.Value.Date;
            purchaseRepo.UpdatePurchaseList(purchase.purchaseList);
           

            return RedirectToAction("TodayInsertedList"); 

        }

        public ActionResult DeletePurchaseItem(int id)
        {
            purchaseRepo.DeletePurchaseItem(id);
            return RedirectToAction("TodayInsertedList"); 
        }

        public ViewResult PurchaseList(string d1 , string d2)
        {
            if (d1 == null || d2 == null)
            {
                d1 = "1-2-2015";
                d2 = "1-2-2015";
            }
            PurchaseModel model = new PurchaseModel();          

            DateTime dat = DateTime.Now.Date;
            model.purchaseListL = purchaseRepo.PurchaseLists.Where(p => p.PurchaseDate  >= DateTime.Parse( d1)).Where(p => p.PurchaseDate  <= DateTime.Parse( d2)).ToList();
            model.itemDic = purchaseRepo.ItemLists.ToDictionary(k => k.Id, k => k.Name);
            ViewBag.Total = purchaseRepo.PurchaseLists.Where(p => p.PurchaseDate >= DateTime.Parse(d1)).Where(p => p.PurchaseDate <= DateTime.Parse(d2)).GroupBy(P=>P.PurchaseDate).Select(group=>group.Sum(item=>item.Price) ).ToList();
            return View(model);

        }

	}
}