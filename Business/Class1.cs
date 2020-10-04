using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Caching;
using System.Web;

namespace Business
{
    public class BusinessLayer
    {
        public CacheInMemory _applicationCache;
        public CacheInMemory ApplicationCache
        {
            get
            {
                if (_applicationCache == null)
                    _applicationCache = new CacheInMemory();
                return _applicationCache;
            }
        }

        public DateTime ObterValor()
        {
            CachedItem r = ApplicationCache.GetCachedItem("UltimoAcesso");
            if (r != null)
            {
                return (DateTime)r.Value;
            }
            else
            {
                ApplicationCache.Insert<DateTime>("UltimoAcesso", DateTime.Now, DateTime.Now.AddSeconds(10));
                r = ApplicationCache.GetCachedItem("UltimoAcesso");
                if (r == null)
                    throw new Exception("ERRO!");
                return (DateTime)r.Value;
            }
        }
    }
}
