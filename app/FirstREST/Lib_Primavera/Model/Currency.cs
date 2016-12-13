using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Globalization;

namespace FirstREST.Lib_Primavera.Model
{
    public static class CurrencyTools
    {
        private static IDictionary<string, string> map;
        static CurrencyTools()
        {
            map = CultureInfo
                .GetCultures(CultureTypes.AllCultures)
                .Where(c => !c.IsNeutralCulture)
                .Select(culture =>
                {
                    try
                    {
                        return new RegionInfo(culture.LCID);
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Where(ri => ri != null)
                .GroupBy(ri => ri.ISOCurrencySymbol)
                .ToDictionary(x => x.Key, x => x.First().CurrencySymbol);
        }
        public static bool TryGetCurrencySymbol(
                              string ISOCurrencySymbol,
                              out string symbol)
        {
            return map.TryGetValue(ISOCurrencySymbol, out symbol);
        }
    }
}