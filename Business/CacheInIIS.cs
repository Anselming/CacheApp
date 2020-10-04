using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Web;

namespace Business
{
    public class CacheInIIS
    {
        Cache InnerCache { get { return HttpRuntime.Cache; } }
        TimeSpan DefaultSlidingExpiration { get; set; }
        bool ThrowExceptionWhenRecached;

        public CacheInIIS(TimeSpan defaultSlidingExpiration = new TimeSpan(), bool throwExceptionWhenRecached = false)
        {
            DefaultSlidingExpiration = defaultSlidingExpiration;
            ThrowExceptionWhenRecached = throwExceptionWhenRecached;
        }

        public void Insert<TCachedItem>(string uniqueName, TCachedItem item, DateTime absoluteExpiration = new DateTime(), TimeSpan slidingExpiration = new TimeSpan(), CacheDependency dependencies = null, CacheItemPriority priority = CacheItemPriority.Normal,  CacheItemRemovedCallback removedCallBack = null)
        {
            if (slidingExpiration == TimeSpan.Zero)
                slidingExpiration = DefaultSlidingExpiration;

            if (!isAlreadyCached(uniqueName))
                InnerCache.Insert(uniqueName, new CachedItem(item), dependencies, absoluteExpiration, slidingExpiration, priority, removedCallBack);
            else
            {
                if (ThrowExceptionWhenRecached)
                    throw new Exception(string.Format("The uniqueName '{0}' is already cached.", uniqueName));
            }

        }
        public void Remove(string uniqueName)
        {
            InnerCache.Remove(uniqueName);
        }

        public CachedItem GetCachedItem(string uniqueName)
        {
            return (CachedItem)InnerCache.Get(uniqueName);
        }
        public IEnumerable<TCast> GetAllCachedItems<TCast>()
        {
            var cachedValues = InnerCache.Cast<CachedItem>();
            throw new NotImplementedException();
        }
        public IEnumerable<TCast> GetCachedItems<TCast>(Func<TCast,bool> criteria)
        {
            var cachedValues = GetAllCachedItems<TCast>().Where(criteria);
            return cachedValues;
        }
        public bool isAlreadyCached(string uniqueName)
        {
            return InnerCache.Get(uniqueName) != null;
        }

    }
}
