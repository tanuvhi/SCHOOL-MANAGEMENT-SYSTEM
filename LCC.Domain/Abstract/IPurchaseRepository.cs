using LCC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCC.Domain.Abstract
{
   public interface IPurchaseRepository
    {
       IEnumerable<ItemList> ItemLists { get;}
       IEnumerable<PurchaseList> PurchaseLists { get; }
       IEnumerable<QuantityType> QuantityTypes { get; }

    int  UpdateItemList(ItemList AnItem);

    int UpdatePurchaseList(PurchaseList PurchaseLIst);
    void DeletePurchaseItem(int id);
    }
}
