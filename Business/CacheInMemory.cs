using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Caching;

namespace Business
{
    public class CacheInMemory
    {
        MemoryCache InnerCache { get { return System.Runtime.Caching.MemoryCache.Default; } }
        TimeSpan DefaultSlidingExpiration { get; set; }
        bool ThrowExceptionWhenRecached;

        public CacheInMemory(TimeSpan defaultSlidingExpiration = new TimeSpan(), bool throwExceptionWhenRecached = false)
        {
            DefaultSlidingExpiration = defaultSlidingExpiration;
            ThrowExceptionWhenRecached = throwExceptionWhenRecached;
        }

        public void Insert<TCachedItem>(string uniqueName, TCachedItem item, DateTime absoluteExpiration = new DateTime(), TimeSpan slidingExpiration = new TimeSpan(), CacheItemPriority priority = CacheItemPriority.Default,  CacheEntryRemovedCallback removedCallBack = null)
        {
            if (slidingExpiration == TimeSpan.Zero)
                slidingExpiration = DefaultSlidingExpiration;

            //if (!isAlreadyCached(uniqueName, item))
            //{
                InnerCache.Set(new CacheItem(uniqueName, new CachedItem(item)),
                    new CacheItemPolicy()
                    {
                        AbsoluteExpiration = absoluteExpiration,
                        Priority = priority,
                        RemovedCallback = removedCallBack,
                        SlidingExpiration = slidingExpiration,
                    });
            //}
            //else
            //{
            //    if (ThrowExceptionWhenRecached)
            //        throw new Exception(string.Format("The uniqueName '{0}' is already cached.", uniqueName));
            //}

        }
        public void Remove(string uniqueName)
        {
            InnerCache.Remove(uniqueName);
        }

        public CachedItem GetCachedItem(string uniqueName)
        {
            CachedItem returnedCachedItem = default(CachedItem);

            try
            {
                returnedCachedItem = (CachedItem)InnerCache.Where(c => c.Key == uniqueName).FirstOrDefault().Value;
            }
            catch(Exception ex)
            {
                if (ThrowExceptionWhenRecached)
                    throw ex;
            }

            return returnedCachedItem;           
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
        public bool isAlreadyCached<TCachedItem>(string uniqueName, TCachedItem item)
        {
            return InnerCache.Contains(
                new KeyValuePair<string, object>(uniqueName, item));
        }
    }
}
