using System.Collections.Generic;

namespace ChainRunner
{
    public class ChainBuilder
    {
        public static ChainBuilder<TRequest> For<TRequest>()
        {
            return new ChainBuilder<TRequest>();
        }
    }
    
    public class ChainBuilder<TRequest>
    {
        private readonly List<IResponsibilityHandler<TRequest>> _handlers = new();
        private readonly List<IResponsibilityExceptionHandler<TRequest>> _exceptionHandlers = new();
        private readonly List<IResponsibilityInterceptorHandler<TRequest>> _interceptorHandlers = new();

        public ChainBuilder<TRequest> WithHandler<THandler>(THandler instance) where THandler : IResponsibilityHandler<TRequest>
        {
            _handlers.Add(instance);
            return this;
        }
        public ChainBuilder<TRequest> WithHandler<THandler>() where THandler : IResponsibilityHandler<TRequest>, new()
        {
            _handlers.Add(new THandler());
            return this;
        }
        public ChainBuilder<TRequest> WithExceptionHandler<TExceptionHandler>() where TExceptionHandler : IResponsibilityExceptionHandler<TRequest>, new()
        {
            _exceptionHandlers.Add(new TExceptionHandler());
            return this;
        }
        public ChainBuilder<TRequest> WithExceptionHandler<TExceptionHandler>(TExceptionHandler exceptionHandler) where TExceptionHandler : IResponsibilityExceptionHandler<TRequest>
        {
            _exceptionHandlers.Add(exceptionHandler);
            return this;
        }
        public ChainBuilder<TRequest> WithInterceptor<TInterceptorHandler>() where TInterceptorHandler : IResponsibilityInterceptorHandler<TRequest>, new()
        {
            _interceptorHandlers.Add(new TInterceptorHandler());
            return this;
        }
        public ChainBuilder<TRequest> WithInterceptor<TInterceptorHandler>(TInterceptorHandler exceptionHandler) where TInterceptorHandler : IResponsibilityInterceptorHandler<TRequest>
        {
            _interceptorHandlers.Add(exceptionHandler);
            return this;
        }

        public IChain<TRequest> Build()
        {
            return new Chain<TRequest>(_handlers, _exceptionHandlers, _interceptorHandlers);
        }
    }
}