using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChainRunner.UnitTests.Data.FaultyResponsibilityHandlers
{
    public class NullReferenceExceptionHandler : IResponsibilityExceptionHandler<FakeChainRequest>
    {
        private readonly string _logToAdd;
        private readonly bool _handled;

        public NullReferenceExceptionHandler(string logToAdd, bool handled)
        {
            _logToAdd = logToAdd;
            _handled = handled;
        }
        public Task HandleAsync(FakeChainRequest request, Exception exception, IChainContext chainContext, IChainExceptionResult result, CancellationToken cancellationToken = default)
        {
            request.ExecutionLogs.Add(_logToAdd);
            result.Handled = _handled;
            return Task.CompletedTask;
        }
    }
}