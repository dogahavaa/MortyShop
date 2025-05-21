using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using MortyShop_MVC.Models;

namespace MortyShop_MVC.Areas.AdminPanel.Services
{
    public class XMLProductService
    {
        string xmlPath = @"C:\BayilikXML\Bronz.xml";
        MortyShopDB db = new MortyShopDB();

        public class XMLProductResult
        {
            public List<Product> NewProducts = new List<Product>();
            public List<Product> UpdatedProducts = new List<Product>();
            public List<Product> DeletedProducts = new List<Product>();
        }

        public XMLProductResult ProcessXMLProduct()
        {
            XMLProductResult result = new XMLProductResult();
            XDocument xmlDoc = XDocument.Load(xmlPath);
            List<XElement> xmlProducts = xmlDoc.Descendants("Urun").ToList(); //XML Dosyasındaki tüm Urun'leri arar ve bunu bir liste haline çevirir


            foreach (XElement xmlProduct in xmlProducts)
            {
                string barcode = xmlProduct.Element("BarkodNo").Value;
                Product existingProduct = db.Products.FirstOrDefault(x => x.BarcodeNo == barcode);

                if (existingProduct == null) // Yeni ürün
                {
                    Product newProduct = new Product
                    {
                        BarcodeNo = barcode,
                        Name = xmlProduct.Element("UrunAdi").Value,
                        Description = xmlProduct.Element("Aciklama").Value,
                        CategoryID = GetOrCreateCategory(xmlProduct.Element("Kategori").Value),
                        BrandID = GetOrCreateBrand(xmlProduct.Element("Marka").Value),
                        Image = xmlProduct.Element("ResimAdi").Value,
                        Price = Convert.ToDecimal(xmlProduct.Element("Fiyat")),
                        CreationTime = Convert.ToDateTime(xmlProduct.Element("OlusturmaTarihi")),
                        UpdateTime = Convert.ToDateTime(xmlProduct.Element("GuncellemeTarihi")),
                        IsActive = true,
                        IsDeleted = false
                    };
                }

            }

        }

        public int GetOrCreateCategory(string categoryName) // Kategori yoksa oluştur
        {
            Category c = db.Categories.FirstOrDefault(x => x.Name == categoryName);
            if (c == null)
            {
                Category newCategory = new Category();
                newCategory.Name = categoryName;
                newCategory.IsActive = true;
                db.Categories.Add(newCategory);
                db.SaveChanges();
                return newCategory.ID;
            }
            return c.ID;
        }
        public int GetOrCreateBrand(string brandName) // Kategori yoksa oluştur
        {
            Brand b = db.Brands.FirstOrDefault(x => x.Name == brandName);
            if (b == null)
            {
                Brand newBrand = new Brand();
                newBrand.Name = brandName;
                newBrand.IsActive = true;
                db.Brands.Add(newBrand);
                db.SaveChanges();
                return newBrand.ID;
            }
            return b.ID;
        }
    }
}


//using System;

//namespace MortyShop_MVC.Models
//{
//    public class TempProduct
//    {
//        public int ID { get; set; }
//        public string BarcodeNo { get; set; }
//        public string Name { get; set; }
//        public string Description { get; set; }
//        public int CategoryID { get; set; }
//        public int BrandID { get; set; }
//        public string Image { get; set; }
//        public decimal Price { get; set; }
//        public DateTime CreationTime { get; set; }
//        public DateTime UpdateTime { get; set; }
//        public bool IsNew { get; set; } // Yeni ürün mü yoksa güncelleme mi
//        public bool IsDeleted { get; set; } // Silinecek ürün mü
//        public bool IsProcessed { get; set; } // İşlem yapıldı mı

//        // Navigation properties
//        public virtual Category Category { get; set; }
//        public virtual Brand Brand { get; set; }
//    }
//}


//public class XMLProductResult
//{
//    public List<TempProduct> NewProducts { get; set; } = new List<TempProduct>();
//    public List<TempProduct> UpdatedProducts { get; set; } = new List<TempProduct>();
//    public List<TempProduct> DeletedProducts { get; set; } = new List<TempProduct>();
//}

//public XMLProductResult ProcessXMLProducts()
//{
//    var result = new XMLProductResult();
//    var xmlDoc = XDocument.Load(_xmlPath);
//    var xmlProducts = xmlDoc.Descendants("Urun").ToList();

//    // Önceki işlenmemiş ürünleri temizle
//    var oldTempProducts = _db.TempProducts.Where(tp => !tp.IsProcessed).ToList();
//    _db.TempProducts.RemoveRange(oldTempProducts);
//    _db.SaveChanges();

//    // XML'deki ürünleri işle
//    foreach (var xmlProduct in xmlProducts)
//    {
//        var barcode = xmlProduct.Element("BarkodNo")?.Value;
//        var existingProduct = _db.Products.FirstOrDefault(p => p.BarcodeNo == barcode);

//        // Tarihleri parse et
//        var creationDate = DateTime.Parse(xmlProduct.Element("OlusturmaTarihi")?.Value ?? DateTime.Now.ToString());
//        var updateDate = DateTime.Parse(xmlProduct.Element("GuncellemeTarihi")?.Value ?? DateTime.Now.ToString());

//        if (existingProduct == null)
//        {
//            // Yeni ürün
//            var newTempProduct = new TempProduct
//            {
//                BarcodeNo = barcode,
//                Name = xmlProduct.Element("UrunAdi")?.Value,
//                Description = xmlProduct.Element("Aciklama")?.Value,
//                CategoryID = GetOrCreateCategory(xmlProduct.Element("Kategori")?.Value),
//                BrandID = GetOrCreateBrand(xmlProduct.Element("Marka")?.Value),
//                Image = xmlProduct.Element("ResimAdi")?.Value,
//                Price = decimal.Parse(xmlProduct.Element("Fiyat")?.Value ?? "0"),
//                CreationTime = creationDate,
//                UpdateTime = updateDate,
//                IsNew = true,
//                IsDeleted = false,
//                IsProcessed = false
//            };

//            _db.TempProducts.Add(newTempProduct);
//            result.NewProducts.Add(newTempProduct);
//        }
//        else
//        {
//            // Ürün güncelleme kontrolü
//            bool isUpdated = false;

//            // Güncelleme tarihi kontrolü
//            if (existingProduct.UpdateTime < updateDate)
//            {
//                if (existingProduct.Name != xmlProduct.Element("UrunAdi")?.Value ||
//                    existingProduct.Description != xmlProduct.Element("Aciklama")?.Value ||
//                    existingProduct.Price != decimal.Parse(xmlProduct.Element("Fiyat")?.Value ?? "0") ||
//                    existingProduct.CategoryID != GetOrCreateCategory(xmlProduct.Element("Kategori")?.Value) ||
//                    existingProduct.BrandID != GetOrCreateBrand(xmlProduct.Element("Marka")?.Value) ||
//                    existingProduct.Image != xmlProduct.Element("ResimAdi")?.Value)
//                {
//                    var updatedTempProduct = new TempProduct
//                    {
//                        BarcodeNo = barcode,
//                        Name = xmlProduct.Element("UrunAdi")?.Value,
//                        Description = xmlProduct.Element("Aciklama")?.Value,
//                        CategoryID = GetOrCreateCategory(xmlProduct.Element("Kategori")?.Value),
//                        BrandID = GetOrCreateBrand(xmlProduct.Element("Marka")?.Value),
//                        Image = xmlProduct.Element("ResimAdi")?.Value,
//                        Price = decimal.Parse(xmlProduct.Element("Fiyat")?.Value ?? "0"),
//                        CreationTime = existingProduct.CreationTime,
//                        UpdateTime = updateDate,
//                        IsNew = false,
//                        IsDeleted = false,
//                        IsProcessed = false
//                    };

//                    _db.TempProducts.Add(updatedTempProduct);
//                    result.UpdatedProducts.Add(updatedTempProduct);
//                }
//            }
//        }
//    }

//    // Silinen ürünleri kontrol et
//    var xmlBarcodes = xmlProducts.Select(x => x.Element("BarkodNo")?.Value).ToList();
//    var deletedProducts = _db.Products.Where(p => !xmlBarcodes.Contains(p.BarcodeNo) && !p.IsDeleted).ToList();

//    foreach (var product in deletedProducts)
//    {
//        var deletedTempProduct = new TempProduct
//        {
//            BarcodeNo = product.BarcodeNo,
//            Name = product.Name,
//            Description = product.Description,
//            CategoryID = product.CategoryID,
//            BrandID = product.BrandID,
//            Image = product.Image,
//            Price = product.Price,
//            CreationTime = product.CreationTime,
//            UpdateTime = DateTime.Now,
//            IsNew = false,
//            IsDeleted = true,
//            IsProcessed = false
//        };

//        _db.TempProducts.Add(deletedTempProduct);
//        result.DeletedProducts.Add(deletedTempProduct);
//    }

//    _db.SaveChanges();
//    return result;
//}

//public void ApproveProduct(int tempProductId)
//{
//    var tempProduct = _db.TempProducts.Find(tempProductId);
//    if (tempProduct == null) return;

//    if (tempProduct.IsDeleted)
//    {
//        // Ürünü sil
//        var product = _db.Products.FirstOrDefault(p => p.BarcodeNo == tempProduct.BarcodeNo);
//        if (product != null)
//        {
//            product.IsDeleted = true;
//            product.UpdateTime = DateTime.Now;
//        }
//    }
//    else
//    {
//        var existingProduct = _db.Products.FirstOrDefault(p => p.BarcodeNo == tempProduct.BarcodeNo);
//        if (existingProduct == null)
//        {
//            // Yeni ürün ekle
//            var newProduct = new Product
//            {
//                BarcodeNo = tempProduct.BarcodeNo,
//                Name = tempProduct.Name,
//                Description = tempProduct.Description,
//                CategoryID = tempProduct.CategoryID,
//                BrandID = tempProduct.BrandID,
//                Image = tempProduct.Image,
//                Price = tempProduct.Price,
//                CreationTime = tempProduct.CreationTime,
//                UpdateTime = tempProduct.UpdateTime,
//                IsActive = true,
//                IsDeleted = false
//            };
//            _db.Products.Add(newProduct);
//        }
//        else
//        {
//            // Mevcut ürünü güncelle
//            existingProduct.Name = tempProduct.Name;
//            existingProduct.Description = tempProduct.Description;
//            existingProduct.CategoryID = tempProduct.CategoryID;
//            existingProduct.BrandID = tempProduct.BrandID;
//            existingProduct.Image = tempProduct.Image;
//            existingProduct.Price = tempProduct.Price;
//            existingProduct.UpdateTime = tempProduct.UpdateTime;
//        }
//    }

//    tempProduct.IsProcessed = true;
//    _db.SaveChanges();
//}

//public void RejectProduct(int tempProductId)
//{
//    var tempProduct = _db.TempProducts.Find(tempProductId);
//    if (tempProduct != null)
//    {
//        tempProduct.IsProcessed = true;
//        _db.SaveChanges();
//    }
//}


//[HttpPost]
//public ActionResult ApproveProduct(int id)
//{
//    _xmlService.ApproveProduct(id);
//    return RedirectToAction("Index");
//}

//[HttpPost]
//public ActionResult RejectProduct(int id)
//{
//    _xmlService.RejectProduct(id);
//    return RedirectToAction("Index");
//}
//    }
//}


//        </ div >
//        < div class= "card-content" >
 
//             < h3 class= "card-title" > Toplam Ürün </ h3 >
     
//                 < div class= "card-value" > @ViewBag.NewProducts.Count + @ViewBag.UpdatedProducts.Count </ div >
      
//                  < div class= "card-footer" >
       
//                       < span class= "trend up" >
        
//                            < i class= "fas fa-arrow-up" ></ i >
//                             Yeni ve Güncellenmiş
//                         </span>
//                     </div>
//        </div>
//    </div>

//    <div class= "admin-card" >
//        < div class= "card-icon" >
 
//             < i class= "fas fa-trash" ></ i >
  
//          </ div >
  
//          < div class= "card-content" >
   
//               < h3 class= "card-title" > Silinecek Ürün </ h3 >
       
//                   < div class= "card-value" > @ViewBag.DeletedProducts.Count </ div >
        
//                    < div class= "card-footer" >
         
//                         < span class= "trend down" >
          
//                              < i class= "fas fa-arrow-down" ></ i >
//                               Silinecek
//                           </ span >
           
//                       </ div >
           
//                   </ div >
           
//               </ div >
//           </ div >
           

//           < div class= "xml-products-section" >
            
//                < h2 class= "section-title" > XML Ürün Değişiklikleri</h2>
                

//                    @if (ViewBag.NewProducts.Count == 0 && ViewBag.UpdatedProducts.Count == 0 && ViewBag.DeletedProducts.Count == 0)
//{
//        < div class= "no-changes" >
 
//             < i class= "fas fa-info-circle" ></ i >
  
//              < p > Yeni, güncellenmiş veya silinecek ürün bulunmamaktadır.</p>
//        </div>
//    }
//    else
//{
//        < div class= "products-table" >
 
//             < table >
 
//                 < thead >
 
//                     < tr >
 
//                         < th > Barkod </ th >
 
//                         < th > Ürün Adı </ th >
    
//                            < th > Kategori </ th >
    
//                            < th > Marka </ th >
    
//                            < th > Fiyat </ th >
    
//                            < th > Durum </ th >
    
//                            < th > İşlemler </ th >
    
//                        </ tr >
    
//                    </ thead >
    
//                    < tbody >
//                        @foreach(var product in ViewBag.NewProducts)
//                    {
//                        < tr class= "new-product" >
 
//                             < td > @product.BarcodeNo </ td >
 
//                             < td > @product.Name </ td >
 
//                             < td > @product.Category.Name </ td >
 
//                             < td > @product.Brand.Name </ td >
 
//                             < td > @product.Price.ToString("C") </ td >
 
//                             < td >< span class= "status new" > Yeni Ürün </ span ></ td >
       
//                                   < td class= "actions" >
//                                        @using(Html.BeginForm("ApproveProduct", "Home", FormMethod.Post))
//                                {
//    @Html.Hidden("id", product.ID)
//    < button type = "submit" class= "btn-approve" >

//           < i class= "fas fa-check" ></ i > Onayla
//        </ button >
//                                }
//                                @using(Html.BeginForm("RejectProduct", "Home", FormMethod.Post))
//                                {
//    @Html.Hidden("id", product.ID)
//    < button type = "submit" class= "btn-reject" >

//           < i class= "fas fa-times" ></ i > Reddet
//        </ button >
//                                }
//                            </ td >
//                        </ tr >
//                    }
//                    @foreach(var product in ViewBag.UpdatedProducts)
//                    {
//                        < tr class= "updated-product" >
 
//                             < td > @product.BarcodeNo </ td >
 
//                             < td > @product.Name </ td >
 
//                             < td > @product.Category.Name </ td >
 
//                             < td > @product.Brand.Name </ td >
 
//                             < td > @product.Price.ToString("C") </ td >
 
//                             < td >< span class= "status updated" > Güncellendi </ span ></ td >
    
//                                < td class= "actions" >
//                                     @using(Html.BeginForm("ApproveProduct", "Home", FormMethod.Post))
//                                {
//    @Html.Hidden("id", product.ID)
//    < button type = "submit" class= "btn-approve" >

//           < i class= "fas fa-check" ></ i > Onayla
//        </ button >
//                                }
//                                @using(Html.BeginForm("RejectProduct", "Home", FormMethod.Post))
//                                {
//    @Html.Hidden("id", product.ID)
//    < button type = "submit" class= "btn-reject" >

//           < i class= "fas fa-times" ></ i > Reddet
//        </ button >
//                                }
//                            </ td >
//                        </ tr >
//                    }
//                    @foreach(var product in ViewBag.DeletedProducts)
//                    {
//                        < tr class= "deleted-product" >
 
//                             < td > @product.BarcodeNo </ td >
 
//                             < td > @product.Name </ td >
 
//                             < td > @product.Category.Name </ td >
 
//                             < td > @product.Brand.Name </ td >
 
//                             < td > @product.Price.ToString("C") </ td >
 
//                             < td >< span class= "status deleted" > Silinecek </ span ></ td >
    
//                                < td class= "actions" >
//                                     @using(Html.BeginForm("ApproveProduct", "Home", FormMethod.Post))
//                                {
//    @Html.Hidden("id", product.ID)
//    < button type = "submit" class= "btn-approve" >

//           < i class= "fas fa-check" ></ i > Onayla
//        </ button >
//                                }
//                                @using(Html.BeginForm("RejectProduct", "Home", FormMethod.Post))
//                                {
//    @Html.Hidden("id", product.ID)
//    < button type = "submit" class= "btn-reject" >

//           < i class= "fas fa-times" ></ i > Reddet
//        </ button >
//                                }
//                            </ td >


