using MarketPage.Models;
using Microsoft.AspNetCore.Http;

namespace MarketPage.Repository
{
    public interface IImagemRepository
    {
        ImgItem GetImgItem(long idItem);
        void PostImgItem(ImgItem imgItem);
        void DeleteImgItem(long idItem);
        ImgItem GeraImgItemPadrao(string nomeItem);
        ImgItem GeraImgItemPrincipal(string nomeItem);
        byte[] GeraImgByte(IFormFile formFile);
        void DeletaItemImgMain(ItemImagem produto);
        void DeletaItemImgPadrao(ItemImagem produto);
    }
}
