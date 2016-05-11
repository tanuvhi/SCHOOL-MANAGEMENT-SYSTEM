using LCC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCC.WebUI.Models
{
    public class PurchaseModel
    {
        public PurchaseList purchaseList { get; set; }
        public ItemList itemList { get; set; }

        public QuantityType quantityType { get; set; }

       public  Dictionary<int, string> itemDic { get; set; }
        public IEnumerable<PurchaseList> purchaseListL { get;set;}
        public IEnumerable<ItemList> itemListl { get; set; }
    }
}