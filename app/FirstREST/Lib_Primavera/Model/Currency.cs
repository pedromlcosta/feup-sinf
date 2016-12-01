using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace FirstREST.Lib_Primavera.Model
{ 
    public static class CurrencyCodeMapper
    {
        private static readonly Dictionary<string, string> currencySymbol;

        public static string GetCurrenySymbol(string currencyCode) { return currencySymbol[currencyCode]; }

        static CurrencyCodeMapper()
        {
            currencySymbol = new Dictionary<string, string>();

            var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                          .Select(x => new RegionInfo(x.LCID));

            foreach (var region in regions)
                if (!currencySymbol.ContainsKey(region.ISOCurrencySymbol))
                    currencySymbol.Add(region.ISOCurrencySymbol, region.CurrencySymbol);
        }
    }
}