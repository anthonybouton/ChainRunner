using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChainRunner
{
    public interface IResponsibilityHandler
    {
    }

    public interface IResponsibilityExceptionHandler
    {
        
    }

    public interface IResponsibilityInterceptorHandler
    {
        
    }
    public interface IResponsibilityHandler<in TRequest> : IResponsibilityHandler
    {
        Task HandleAsync(TRequest request,
            IChainContext chainContext,
            CancellationToken cancellationToken = default);
    }

    public interface IResponsibilityExceptionHandler<in TRequest> : IResponsibilityExceptionHandler
    {
        Task HandleAsync(TRequest request,
            Exception exception,
            IChainContext chainContext,
            IChainExceptionResult result,
            CancellationToken cancellationToken = default);
    }
    
    public interface IResponsibilityInterceptorHandler<in TRequest> : IResponsibilityExceptionHandler
    {
        Task HandleAsync(TRequest request,
            IChainContext chainContext,
            CancellationToken cancellationToken = default);
    }
}