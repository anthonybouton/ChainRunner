using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChainRunner.UnitTests.Data.FaultyResponsibilityHandlers
{
    public class FirstFaultyFakeResponsibilityHandler : IResponsibilityHandler<FakeChainRequest>
    {
        private readonly Exception _exceptionToThrow;

        public FirstFaultyFakeResponsibilityHandler(Exception exceptionToThrow)
        {
            _exceptionToThrow = exceptionToThrow;
        }

        public Task HandleAsync(FakeChainRequest request, IChainContext chainContext, CancellationToken cancellationToken = default)
        {
            throw _exceptionToThrow;
        }
    }
}