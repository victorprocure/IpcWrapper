using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IpcWrapper.Configurations;
using IpcWrapper.Exceptions;
using IpcWrapper.Factories;
using IpcWrapper.Security;
using Moq;
using Xunit;

namespace IpcWrapper.Library.Tests
{
    public class IpcServerFactoryTests
    {
        [Fact]
        public async Task FactoryShouldValidateConfiguration()
        {
            var mockInternalServerFactory = new Mock<IIpcServerFactory>();
            var mockServer = new Mock<IIpcServer>();

            mockInternalServerFactory
                .Setup(sf => sf.CreateServerAsync(It.IsAny<IIpcServerConfiguration>()))
                .ReturnsAsync(mockServer.Object);

            var factoryMock = new Mock<ServerFactory>(mockInternalServerFactory.Object);
            factoryMock.Setup(fm => fm.ValidateConfiguration(It.IsAny<IIpcServerConfiguration>())).Verifiable();
            var server = await factoryMock.Object.CreateServerAsync(new IpcServerConfiguration("Test"));

            factoryMock.Verify(fm => fm.ValidateConfiguration(It.IsAny<IIpcServerConfiguration>()), Times.Once);
        }

        [Fact]
        public async Task FactoryShouldValidateServerConfigurationHasName()
        {
            var mockInternalServerFactory = new Mock<IIpcServerFactory>();
            var mockServer = new Mock<IIpcServer>();

            mockInternalServerFactory
                .Setup(sf => sf.CreateServerAsync(It.IsAny<IIpcServerConfiguration>()))
                .ReturnsAsync(mockServer.Object);

            var mockServerConfiguration = new Mock<IIpcServerConfiguration>();
            mockServerConfiguration.SetupGet(sc => sc.Name).Returns("TestServer").Verifiable();

            var factoryMock = new Mock<ServerFactory>(mockInternalServerFactory.Object);
            factoryMock.Setup(fm => fm.ValidateConfiguration(It.IsAny<IIpcServerConfiguration>())).Verifiable();
            var server = await factoryMock.Object.CreateServerAsync(mockServerConfiguration.Object);

            factoryMock.VerifyAll();
        }

        [Fact]
        public async Task CreatedServerShouldHaveDefaultSecuritySetIfNotProvided()
        {
            var mockInternalServerFactory = new Mock<IIpcServerFactory>();
            var mockServer = new Mock<IIpcServer>();

            mockInternalServerFactory
                .Setup(sf => sf.CreateServerAsync(It.IsAny<IIpcServerConfiguration>()))
                .ReturnsAsync(mockServer.Object);

            var factoryMock = new Mock<ServerFactory>(mockInternalServerFactory.Object);
            factoryMock.Setup(fm => fm.ValidateConfiguration(It.IsAny<IIpcServerConfiguration>())).Verifiable();
            var config = new IpcServerConfiguration("TestServer");
            mockServer.SetupGet(ms => ms.Configuration).Returns(config);

            var server = await factoryMock.Object.CreateServerAsync(config);

            factoryMock.VerifyAll();

            Assert.IsType<IpcDefaultServerSecurity>(server.Configuration.Security);
        }

        [Fact]
        public async Task FactoryShouldThrowIfNameValidatorFails()
        {
            var mockInternalServerFactory = new Mock<IIpcServerFactory>();
            var mockServer = new Mock<IIpcServer>();

            mockInternalServerFactory
                .Setup(sf => sf.CreateServerAsync(It.IsAny<IIpcServerConfiguration>()))
                .ReturnsAsync(mockServer.Object);

            var mockServerNameValidator = new Mock<IIpcServerNameValidator>();
            mockServerNameValidator.Setup(sn => sn.Validate(It.IsAny<string>()))
                .Throws(new IpcServerNameValidatorException("Test Exception"));

            var factory = new TestFactory(mockInternalServerFactory.Object, new[] { mockServerNameValidator.Object });

            await Assert
                .ThrowsAsync<IpcServerNameInvalidException>(() => factory.CreateServerAsync(new IpcServerConfiguration("Test Server")));
        }

        [Fact]
        public async Task FactoryShouldThrowOnConfigurationException()
        {
            var mockInternalServerFactory = new Mock<IIpcServerFactory>();
            var mockServer = new Mock<IIpcServer>();

            mockInternalServerFactory
                .Setup(sf => sf.CreateServerAsync(It.IsAny<IIpcServerConfiguration>()))
                .ReturnsAsync(mockServer.Object);

            var mockConfiguration = new Mock<IIpcServerConfiguration>();
            mockConfiguration.SetupGet(c => c.Name).Returns("Test");

            var factory = new TestFactory(mockInternalServerFactory.Object);

            await Assert.ThrowsAsync<IpcServerConfigurationException>(() => factory.CreateServerAsync(mockConfiguration.Object));
        }

        private class TestFactory : ServerFactory
        {
            private IIpcServerFactory internalFactory;
            public TestFactory(IIpcServerFactory internalFactory) : this(internalFactory, null)
            {

            }

            public TestFactory(IIpcServerFactory internalFactory, IEnumerable<IIpcServerNameValidator> nameValidators) : base(nameValidators)
            {
                this.internalFactory = internalFactory;
            }

            public override void ValidateConfiguration(IIpcServerConfiguration configuration)
            {
                base.ValidateConfiguration(configuration);
            }

            protected override async Task<IIpcServer> CreateTypedServerAsync(IIpcServerConfiguration configuration)
            {
                return await internalFactory.CreateServerAsync(configuration);
            }
        }
    }
}
