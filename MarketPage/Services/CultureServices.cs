using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using System.Collections.Generic;
using System.Globalization;

namespace MarketPage.Services
{
    public class CultureServices
    {
        public static RequestLocalizationOptions SetLocazationOptions(string culture)
        {
            var newCulture = new CultureInfo("pt-BR");
            var localizationOptions = new RequestLocalizationOptions()
            {
                SupportedCultures = new List<CultureInfo>()
                {
                    newCulture
                },
                SupportedUICultures = new List<CultureInfo>()
                {
                    newCulture
                },
                DefaultRequestCulture = new RequestCulture(newCulture),
                FallBackToParentCultures = false,
                FallBackToParentUICultures = false,
                RequestCultureProviders = null
            };
            return localizationOptions;
        }
    }
}
