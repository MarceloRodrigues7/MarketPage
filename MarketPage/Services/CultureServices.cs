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
            RequestLocalizationOptions localizationOptions;
            CultureInfo cultureInfo;

            cultureInfo = string.IsNullOrEmpty(culture) ? SetCultureDefault() : SetCultureToString(culture);

            localizationOptions = GeraObj(cultureInfo);

            return localizationOptions;
        }

        private static CultureInfo SetCultureDefault() => new("pt-BR");

        private static CultureInfo SetCultureToString(string culture) => new(culture);

        private static RequestLocalizationOptions GeraObj(CultureInfo newCulture) => new()
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

        #region Testes Unitarios
        public static CultureInfo TesteSetCultureDefault() => SetCultureDefault();
        public static CultureInfo TesteSetCultureToString(string culture) => SetCultureToString(culture);
        public static RequestLocalizationOptions TesteGeraObj(CultureInfo newCulture) => GeraObj(newCulture);
        #endregion

    }
}
