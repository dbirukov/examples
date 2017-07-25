using System;

namespace SKDClassicalExample
{
    public class SubscriptionToken
    {
        internal SubscriptionToken(Type eventItemType)
        {
            Token = Guid.NewGuid();
            EventItemType = eventItemType;
        }
        
        public Guid Token { get; }
        public Type EventItemType { get; }
    }
}
