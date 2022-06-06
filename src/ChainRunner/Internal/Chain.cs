using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChainRunner
{
    internal class Chain<TRequest> : IChain<TRequest>
    {
        private readonly IEnumerable<IResponsibilityHandler<TRequest>> _handlers;
        private readonly IEnumerable<IResponsibilityExceptionHandler<TRequest>> _exceptionHandlers;
        private readonly IEnumerable<IResponsibilityInterceptorHandler<TRequest>> _interceptorHandlers;

        public Chain(IEnumerable<IResponsibilityHandler<TRequest>> handlers, IEnumerable<IResponsibilityExceptionHandler<TRequest>> exceptionHandlers,
            IEnumerable<IResponsibilityInterceptorHandler<TRequest>> interceptorHandlers)
        {
            _handlers = handlers;
            _exceptionHandlers = exceptionHandlers;
            _interceptorHandlers = interceptorHandlers;
        }

        public async Task RunAsync(TRequest request, CancellationToken cancellationToken = default)
        {
            var chainContext = new ChainContext();

            foreach (var handler in _handlers)
            {
                try
                {
                    await HandleInterception(request, chainContext, cancellationToken);
                    await handler.HandleAsync(request, chainContext, cancellationToken);
                }
                catch (Exception e)
                {
                    await HandleException(request, e, chainContext, cancellationToken);
                }
            }
        }

        private async Task HandleInterception(TRequest request, IChainContext context, CancellationToken cancellationToken)
        {
            foreach (var interceptorHandler in _interceptorHandlers)
            {
                await interceptorHandler.HandleAsync(request, context, cancellationToken);
            }
        }

        private async Task HandleException(TRequest request, Exception exception, IChainContext context, CancellationToken cancellationToken)
        {
            var exceptionResult = new ChainExceptionResult();

            foreach (var handler in _exceptionHandlers)
            {
                await handler.HandleAsync(request, exception, context, exceptionResult, cancellationToken);
                if (exceptionResult.Handled)
                    break;
            }
        }
    }
}