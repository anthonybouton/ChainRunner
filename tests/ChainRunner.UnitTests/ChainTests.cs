using System;
using System.Threading.Tasks;
using ChainRunner.UnitTests.Data;
using ChainRunner.UnitTests.Data.FaultyResponsibilityHandlers;
using ChainRunner.UnitTests.Data.InterceptorHandlers;
using FluentAssertions;
using Xunit;

namespace ChainRunner.UnitTests
{
    public class ChainTests
    {
        [Fact]
        public async Task chain_should_call_handlers_in_orders()
        {
            // Arrange
            var chain = new ChainBuilder<FakeChainRequest>()
                .WithHandler<FirstFakeResponsibilityHandler>()
                .WithHandler<SecondFakeResponsibilityHandler>()
                .WithHandler<ThirdFakeResponsibilityHandler>()
                .Build();

            var request = new FakeChainRequest();

            // Act
            await chain.RunAsync(request);

            // Assert
            request.ExecutionLogs.Should().HaveElementAt(0, "1");
            request.ExecutionLogs.Should().HaveElementAt(1, "2");
            request.ExecutionLogs.Should().HaveElementAt(2, "3");
        }
        [Fact]
        public async Task chain_should_call_execution_interceptors()
        {
            // Arrange
            var chain = new ChainBuilder<FakeChainRequest>()
                .WithHandler<FirstFakeResponsibilityHandler>()
                .WithInterceptor(new LogInterceptorHandler("Intercept 0"))
                .Build();

            var request = new FakeChainRequest();

            // Act
            await chain.RunAsync(request);

            // Assert
            request.ExecutionLogs.Should().HaveElementAt(0, "Intercept 0");
        }
        
        [Fact]
        public async Task chain_should_call_execution_interceptors_in_order()
        {
            // Arrange
            var chain = new ChainBuilder<FakeChainRequest>()
                .WithHandler<FirstFakeResponsibilityHandler>()
                .WithInterceptor(new LogInterceptorHandler("Intercept 0"))
                .WithInterceptor(new LogInterceptorHandler("Intercept 1"))
                .Build();

            var request = new FakeChainRequest();

            // Act
            await chain.RunAsync(request);

            // Assert
            request.ExecutionLogs.Should().HaveElementAt(0, "Intercept 0");
            request.ExecutionLogs.Should().HaveElementAt(1, "Intercept 1");
        }
        
        [Fact]
        public async Task chain_should_call_execution_interceptors_in_order_multiple_handlers()
        {
            // Arrange
            var chain = new ChainBuilder<FakeChainRequest>()
                .WithHandler<FirstFakeResponsibilityHandler>()
                .WithHandler<SecondFakeResponsibilityHandler>()
                .WithInterceptor(new LogInterceptorHandler("Intercept 0"))
                .WithInterceptor(new LogInterceptorHandler("Intercept 1"))
                .Build();

            var request = new FakeChainRequest();

            // Act
            await chain.RunAsync(request);

            // Assert
            request.ExecutionLogs.Should().HaveElementAt(0, "Intercept 0");
            request.ExecutionLogs.Should().HaveElementAt(1, "Intercept 1");
            request.ExecutionLogs.Should().HaveElementAt(3, "Intercept 0");
            request.ExecutionLogs.Should().HaveElementAt(4, "Intercept 1");
        }
        
        [Fact]
        public async Task chain_should_call_execution_handlers()
        {
            // Arrange
            var chain = new ChainBuilder<FakeChainRequest>()
                .WithHandler(new FirstFaultyFakeResponsibilityHandler(new NullReferenceException()))
                .WithExceptionHandler(new NullReferenceExceptionHandler("Exception 0", true))
                .Build();

            var request = new FakeChainRequest();

            // Act
            await chain.RunAsync(request);

            // Assert
            request.ExecutionLogs.Should().HaveElementAt(0, "Exception 0");
        }

        [Fact]
        public async Task chain_should_call_execution_handlers_in_order()
        {
            // Arrange
            var chain = new ChainBuilder<FakeChainRequest>()
                .WithHandler(new FirstFaultyFakeResponsibilityHandler(new NullReferenceException()))
                .WithExceptionHandler(new NullReferenceExceptionHandler("Exception 0", false))
                .WithExceptionHandler(new NullReferenceExceptionHandler("Exception 1", false))
                .WithExceptionHandler(new NullReferenceExceptionHandler("Exception 2", false))
                .Build();

            var request = new FakeChainRequest();

            // Act
            await chain.RunAsync(request);

            // Assert
            request.ExecutionLogs.Should().HaveElementAt(0, "Exception 0");
            request.ExecutionLogs.Should().HaveElementAt(1, "Exception 1");
            request.ExecutionLogs.Should().HaveElementAt(2, "Exception 2");
        }

        [Fact]
        public async Task chain_should_run_execution_handlers_untill_handled()
        {
            // Arrange
            var chain = new ChainBuilder<FakeChainRequest>()
                .WithHandler(new FirstFaultyFakeResponsibilityHandler(new NullReferenceException()))
                .WithExceptionHandler(new NullReferenceExceptionHandler("Exception 0", false))
                .WithExceptionHandler(new NullReferenceExceptionHandler("Exception 1", false))
                .WithExceptionHandler(new NullReferenceExceptionHandler("Exception 2", true))
                .WithExceptionHandler(new NullReferenceExceptionHandler("Exception 3", false))
                .Build();

            var request = new FakeChainRequest();

            // Act
            await chain.RunAsync(request);

            // Assert
            request.ExecutionLogs.Should().HaveElementAt(0, "Exception 0");
            request.ExecutionLogs.Should().HaveElementAt(1, "Exception 1");
            request.ExecutionLogs.Should().HaveElementAt(2, "Exception 2");
            request.ExecutionLogs.Should().NotHaveCount(4, "Was handled after exception 3.");
        }
        
        
    }
}