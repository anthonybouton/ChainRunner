using System.Threading;
using System.Threading.Tasks;

namespace ChainRunner.UnitTests.Data.InterceptorHandlers
{
    public class LogInterceptorHandler : IResponsibilityInterceptorHandler<FakeChainRequest>
    {
        private readonly string _messageToLog;

        public LogInterceptorHandler(string messageToLog)
        {
            _messageToLog = messageToLog;
        }

        public Task HandleAsync(FakeChainRequest request, IChainContext chainContext, CancellationToken cancellationToken = default)
        {
            request.ExecutionLogs.Add(_messageToLog);
            return Task.CompletedTask;
        }
    }
}