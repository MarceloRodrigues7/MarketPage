using ADO;
using MarketPage.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace MarketPage.Repository
{
    public interface IImagemRepository
    {
        IEnumerable<ImgItem> GetImgPrincipalItens();
        ImgItem GetImgItem(long idItem);
        ImgItem GetImgPrincipalPorId(long idItem);
        List<byte[]> GetDemaisImagensPorId(long idItem);
        void PostImgItem(ImgItem imgItem);
        void DeleteImgItem(long idItem);
        ImgItem GeraImgItemPadrao(long idItem);
        ImgItem GeraImgItemPrincipal(long idItem);
        byte[] GeraImgByte(IFormFile formFile);
        void DeletaItemImgMain(ViewItemAdmAddEEdit produto);
        void DeletaItemImgPadrao(ViewItemAdmAddEEdit produto);
    }
}
