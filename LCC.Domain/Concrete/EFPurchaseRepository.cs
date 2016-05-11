using LCC.Domain.Abstract;
using LCC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCC.Domain.Concrete
{
  public  class EFPurchaseRepository : IPurchaseRepository
    {
      private readonly EFDbContext contex = new EFDbContext();

    public  IEnumerable<ItemList> ItemLists
      {
          get { return contex.ItemLists; }
      }

    public  IEnumerable<PurchaseList>PurchaseLists
      {
          get { return contex.PurchaseLists; }
      }

    public IEnumerable<QuantityType> QuantityTypes
      {
          get { return contex.QuantityTypes; }
      }
      public int UpdateItemList(ItemList AnItem)
      {
          if(AnItem.Id==0)
          {

              contex.ItemLists.Add(AnItem);
              contex.SaveChanges();
              return AnItem.Id;
          }
          else{

             ItemList itm= contex.ItemLists.Find(AnItem.Id);
              
              itm.Name= AnItem.Name;
              itm.QuantityTypeId = AnItem.QuantityTypeId;
              contex.SaveChanges();
              return itm.Id;

          }
          
          
      }

      public int UpdatePurchaseList(PurchaseList pur)
      {
          if (pur.Id == 0)
          {

              contex.PurchaseLists.Add(pur);
              contex.SaveChanges();
              return pur.Id;
          }
          else
          {

              PurchaseList list = contex.PurchaseLists.Find(pur.Id);
              list.EntryDate = pur.EntryDate;
              list.PurchaseDate = pur.PurchaseDate;
              list.Quantity = pur.Quantity;
              list.ItemId = pur.ItemId;
              list.Quantity = pur.Quantity;

             
              contex.SaveChanges();
              return list.Id;

          }


      }
      public void DeletePurchaseItem(int id)
      {
         PurchaseList d= contex.PurchaseLists.Find(id);
          if(d!=null)
          {
              contex.PurchaseLists.Remove(d);
              contex.SaveChanges();
          }

      }
    }
}
