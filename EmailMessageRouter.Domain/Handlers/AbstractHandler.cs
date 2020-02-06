namespace EmailMessageRouter.Domain.Handlers
{
    public abstract class AbstractHandler<T> : IHandler<T>
    {
        protected IHandler<T> _nextHandler;
        
        public void SetNextHandler(IHandler<T> handler)
        {
            _nextHandler = handler;
        }

        public abstract void Process(T request);
    }
}