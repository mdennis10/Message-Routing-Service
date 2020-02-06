using System;

namespace EmailMessageRouter.Domain.Handlers
{
    public interface IHandler<T>
    {
        public void SetNextHandler(IHandler<T> handler);
        
        public void Process(T request);
    }
}