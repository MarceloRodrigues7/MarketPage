using MarketPage.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using Xunit;

namespace TestMarketPage
{
    public class ServicesTest
    {
        private AuthService _authService;

        [Fact]
        public void TestGeraAuthProperties()
        {
            _authService = new AuthService();

            var result = _authService.TestGeraAuthProperties();

            var expected = DateTime.UtcNow.AddHours(2);

            Assert.Equal(expected.Hour, result.ExpiresUtc.Value.Hour);
        }

        [Theory]
        [MemberData(nameof(DataAuthService))]
        public void TesteGeraListClaim(string idUsuario, string roleAcessUsuario)
        {
            _authService = new AuthService();

            var result = _authService.TesteGeraListClaim(idUsuario, roleAcessUsuario);

            Assert.NotEmpty(result);
        }

        [Fact]
        public void TesteSetCultureDefault()
        {
            var result = CultureServices.TesteSetCultureDefault();
            var expected = new CultureInfo("pt-br");

            Assert.Equal(expected.Name, result.Name);
        }

        [Theory]
        [MemberData(nameof(DataCultureService))]
        public void TesteSetCultureToString(string culture)
        {
            var result = CultureServices.TesteSetCultureToString(culture);
            var expected = new CultureInfo(culture);

            Assert.Equal(expected.Name, result.Name);
        }

        public static IEnumerable<object[]> DataAuthService =>
            new List<object[]>
            {
                new object[]{ "1","teste1"},
                new object[]{ "2","teste2"},
                new object[]{ "3","teste3"},
                new object[]{ "4","teste4"},
                new object[]{ "5","teste5"}
            };

        public static IEnumerable<object[]> DataCultureService =>
            new List<object[]>
            {
                new object[]{"en-US"},
                new object[]{"es-ES"},
                new object[]{"pt-BR"}
            };
    }
}
