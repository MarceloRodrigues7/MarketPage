using MarketPage.Models;
using Microsoft.AspNetCore.Http;

namespace MarketPage.Repository
{
    public interface IImagemRepository
    {
        ImgItem GetImgItem(long idItem);
        void PostImgItem(ImgItem imgItem);
        void DeleteImgItem(long idItem);
        ImgItem GeraImgItemPadrao(long idItem);
        ImgItem GeraImgItemPrincipal(long idItem);
        byte[] GeraImgByte(IFormFile formFile);
        void DeletaItemImgMain(ViewItemAdmAddEEdit produto);
        void DeletaItemImgPadrao(ViewItemAdmAddEEdit produto);
    }
}
