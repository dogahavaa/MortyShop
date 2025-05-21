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
            public List<TempProduct> NewProducts = new List<TempProduct>();
            public List<TempProduct> UpdatedProducts = new List<TempProduct>();
            public List<TempProduct> DeletedProducts = new List<TempProduct>();
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
                    TempProduct newTempProduct = new TempProduct
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
                        IsNew = true,
                        IsDeleted = false,
                        IsProcessed = false
                    };
                    db.TempProducts.Add(newTempProduct);
                    result.NewProducts.Add(newTempProduct);
                }
                else
                {
                    // Bizdeki ürünün güncelleme tarihi daha eski ise Güncellenen Geçiçi Ürün oluştur. 
                    if (existingProduct.UpdateTime < Convert.ToDateTime(xmlProduct.Element("GuncellemeTarihi")))
                    {
                        TempProduct updatedTempProduct = new TempProduct();
                        updatedTempProduct.BarcodeNo = barcode;
                        updatedTempProduct.Name = xmlProduct.Element("UrunAdi").Value;
                        updatedTempProduct.Description = xmlProduct.Element("Aciklama").Value;
                        updatedTempProduct.CategoryID = GetOrCreateCategory(xmlProduct.Element("Kategori").Value);
                        updatedTempProduct.BrandID = GetOrCreateBrand(xmlProduct.Element("Marka").Value);
                        updatedTempProduct.Image = xmlProduct.Element("ResimAdi").Value;
                        updatedTempProduct.Price = Convert.ToDecimal(xmlProduct.Element("Fiyat").Value);
                        updatedTempProduct.CreationTime = existingProduct.CreationTime;
                        updatedTempProduct.UpdateTime = Convert.ToDateTime(xmlProduct.Element("GuncellemeTarihi"));
                        updatedTempProduct.IsDeleted = false;
                        updatedTempProduct.IsProcessed = false;
                        updatedTempProduct.IsNew = false;
                        db.TempProducts.Add(updatedTempProduct);
                        result.UpdatedProducts.Add(updatedTempProduct); // Güncellenenlere de at 
                    }
                }
            }

            //Xml dosyasındaki ürünlerin barkod numaraları
            List<string> xmlBarcodes = xmlProducts.Select(x => x.Element("BarkodNo").Value).ToList();
            //Contains => içermek
            //xmlBarcodes listesindeki x.BarcodeNo'yu içermeyen ürünleri al ve silinmemiş olsun
            List<Product> deletedProducts = db.Products.Where(x => !xmlBarcodes.Contains(x.BarcodeNo) && !x.IsDeleted).ToList();

            foreach (Product product in deletedProducts)
            {
                //dtp => DeletedTempProduct
                TempProduct dtp = new TempProduct
                {
                    BarcodeNo = product.BarcodeNo,
                    Name = product.Name,
                    Description = product.Description,
                    CategoryID = product.CategoryID,
                    BrandID = product.BrandID,
                    Image = product.Image,
                    Price = product.Price,
                    CreationTime = product.CreationTime,
                    UpdateTime = DateTime.Now,
                    IsNew = false,
                    IsDeleted = true,
                    IsProcessed = false
                };
                db.TempProducts.Add(dtp);
                result.DeletedProducts.Add(dtp);
            }
            db.SaveChanges();
            return result; 

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

        public void ApproveProduct(int tempProductID)
        {
            TempProduct tempProduct = db.TempProducts.Find(tempProductID);
            if (tempProduct != null)
            {
                if (tempProduct.IsDeleted) //Silinmiş ürün ise
                {
                    Product deleteProduct = db.Products.FirstOrDefault(x => x.BarcodeNo == tempProduct.BarcodeNo);
                    if (deleteProduct != null)
                    {
                        deleteProduct.IsDeleted = true; //Soft delete
                        deleteProduct.UpdateTime = DateTime.Now;
                    }
                }
                else
                {
                    Product existingProduct = db.Products.FirstOrDefault(x => x.BarcodeNo == tempProduct.BarcodeNo);
                    if (existingProduct == null) // Ürün yok ise yeni ürün olarak ekle
                    {
                        Product newProduct = new Product
                        {
                            BarcodeNo = tempProduct.BarcodeNo,
                            Name = tempProduct.Name,
                            Description = tempProduct.Description,
                            CategoryID = tempProduct.CategoryID,
                            BrandID = tempProduct.BrandID,
                            Image = tempProduct.Image,
                            Price = tempProduct.Price,
                            CreationTime = tempProduct.CreationTime,
                            UpdateTime = tempProduct.UpdateTime,
                            IsActive = true,
                            IsDeleted = false
                        };
                        db.Products.Add(newProduct);
                    }
                    else // Eğer ürün var ise mevcuttaki ürünü güncelle 
                    {
                        existingProduct.Name = tempProduct.Name;
                        existingProduct.Description = tempProduct.Description;
                        existingProduct.CategoryID = tempProduct.CategoryID;
                        existingProduct.BrandID = tempProduct.BrandID;
                        existingProduct.Image = tempProduct.Image;
                        existingProduct.Price = tempProduct.Price;
                        existingProduct.UpdateTime = tempProduct.UpdateTime;
                    }
                }
            }
            tempProduct.IsProcessed = true; //Geçici ürün işlendi
            db.SaveChanges();
        }

        public void RejectProduct(int tempProductID)
        {
            TempProduct tempProduct = db.TempProducts.Find(tempProductID);
            if (tempProduct != null)
            {
                tempProduct.IsProcessed = true; 
                db.SaveChanges();
            }
        }
    }
}



