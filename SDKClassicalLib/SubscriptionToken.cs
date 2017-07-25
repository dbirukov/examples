using System;

namespace SDKClassicalLib
{
    public class SubscriptionToken
    {
        public SubscriptionToken(Type eventItemType)
        {
            Token = Guid.NewGuid();
            EventItemType = eventItemType;
        }
        
        public Guid Token { get; }
        public Type EventItemType { get; }
    }
}
