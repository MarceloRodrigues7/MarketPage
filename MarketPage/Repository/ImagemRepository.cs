using MarketPage.Context;
using MarketPage.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPage.Repository
{
    public class ImagemRepository : IImagemRepository
    {
        public ImgItem GetImgItem(long idItem)
        {
            using (var context = new ContextEF())
            {
                return context.ImagensItem.Where(i => i.IdItem == idItem && i.Principal == true).FirstOrDefault();
            };
        }
        public void PostImgItem(ImgItem imgItem)
        {
            using (var context = new ContextEF())
            {
                context.ImagensItem.Add(imgItem);
                context.SaveChanges();
            };
        }
        public void DeleteImgItem(long idItem)
        {
            using (var context = new ContextEF())
            {
                context.ImagensItem.RemoveRange(context.ImagensItem.Where(i => i.IdItem == idItem));
                context.SaveChanges();
            };
        }
        public ImgItem GeraImgItemPrincipal(string nomeItem)
        {
            using (var context = new ContextEF())
            {
                return new ImgItem
                {
                    IdItem = context.Itens.Where(i => i.Nome == nomeItem).Select(i => i.Id).FirstOrDefault(),
                    Principal = true,
                    DataAdicao = DateTime.Now
                };
            };
        }
        public ImgItem GeraImgItemPadrao(string nomeItem)
        {
            using (var context = new ContextEF())
            {
                return new ImgItem
                {
                    IdItem = context.Itens.Where(i => i.Nome == nomeItem).Select(i => i.Id).FirstOrDefault(),
                    Principal = false,
                    DataAdicao = DateTime.Now
                };
            };

        }
        public byte[] GeraImgByte(IFormFile formFile)
        {
            var res = new BinaryReader(formFile.OpenReadStream());
            return res.ReadBytes((int)formFile.Length);
        }

        public void DeletaItemImgMain(ItemImagem produto)
        {
            using (var context = new ContextEF())
            {
                if (produto.ImageUpload != null)
                {
                    var img = context.ImagensItem.Where(i => i.IdItem == produto.Id && i.Principal == true);
                    context.ImagensItem.RemoveRange(img);
                }
                context.SaveChanges();
            };
        }

        public void DeletaItemImgPadrao(ItemImagem produto)
        {
            using (var context = new ContextEF())
            {
                if (produto.ImageUpload != null)
                {
                    var img = context.ImagensItem.Where(i => i.IdItem == produto.Id && i.Principal == false);
                    context.ImagensItem.RemoveRange(img);
                }
                context.SaveChanges();
            };
        }
    }
}
