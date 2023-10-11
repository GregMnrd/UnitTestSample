using FluentAssertions;
using Moq;
using Moq.Protected;
using MyProject.Entities;
using MyProject.Repositories;
using MyProject.Services.Product;
using System.ComponentModel;
using System.Diagnostics;

namespace MyProject.Tests.Services.Product
{
    public class ProductServiceTests
    {
        [Fact]
        [Description("See why mock is usefull")]
        public void GetProductById_WithoutMock_ThrowNotImplementedException()
        {
            // ARRANGE
            //--------------------------------------
            int id = 1;
            ProductEntity? productEntity = null;
            IStockRepository stockRepo = new StockRepository();
            IInfoProductRepository infoProductRepo = new InfoProductRepository();
            IProductService productService = new ProductService(infoProductRepo, stockRepo);

            // ACT
            //--------------------------------------
            Action act = () => productEntity = productService.GetProductById(id);

            // ASSERT
            //--------------------------------------
            act.Should().Throw<NotImplementedException>();
        }

        [Fact]
        [Description("Throw ArgumentNullException if id is negative")]
        public void GetProductById_NegativeId_ThrowArgumentNullException()
        {
            // ARRANGE
            //--------------------------------------
            int negativeId = -1;
            Mock<IStockRepository> stockRepoMock = new();
            Mock<IInfoProductRepository> infoProductRepoMock = new();
            IProductService productService = new ProductService(infoProductRepoMock.Object, stockRepoMock.Object);

            // ACT
            //--------------------------------------
            Action act = () => productService.GetProductById(negativeId);

            // ASSERT
            //--------------------------------------
            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [Description("Test GetProductById availability when stock is available")]
        public void GetProductById_WithStock_Availability(int quantity, bool expectedAvailability)
        {
            // ARRANGE
            //--------------------------------------
            int id = 1;
            string name = "name";
            // stock
            StockEntity stockEntity = new(id, quantity);
            Mock<IStockRepository> stockRepoMock = new();
            stockRepoMock.Setup(x => x.GetStockById(id)).Returns(stockEntity);
            // product
            InfoProductEntity infoProductEntity = new(id, name);
            Mock<IInfoProductRepository> infoProductRepoMock = new();
            infoProductRepoMock.Setup(x => x.GetInfoProductById(id)).Returns(infoProductEntity);
            // service
            IProductService productService = new ProductService(infoProductRepoMock.Object, stockRepoMock.Object);

            // ACT
            //--------------------------------------
            ProductEntity? expectedProduct = productService.GetProductById(id);

            // ASSERT
            //--------------------------------------
            expectedProduct.Should().NotBeNull();
            expectedProduct!.Id.Should().Be(id);
            expectedProduct!.StockEntity.Should().Be(stockEntity);
            expectedProduct!.InfoProductEntity.Should().Be(infoProductEntity);
            expectedProduct!.IsAvailable.Should().Be(expectedAvailability);
            infoProductRepoMock.Verify(x => x.GetInfoProductById(id), Times.Once(), "GetInfoProductById is not call once");
            stockRepoMock.Verify(x => x.GetStockById(id), Times.Once(), "GetStockById is not call once");
        }

        [Fact]
        [Description("Test GetProductById without info product and no stockservice call")]
        public void GetProductById_WithoutInfoProduct_GetStockNotCalled()
        {
            // ARRANGE
            //--------------------------------------
            int id = 1;
            // stock
            Mock<IStockRepository> stockRepoMock = new();
            // product
            Mock<IInfoProductRepository> infoProductRepoMock = new();
            infoProductRepoMock.Setup(x => x.GetInfoProductById(id))
                               .Returns((InfoProductEntity?)null)
                               .Callback((int id) => { Debug.WriteLine($"CALLBACK: {id}"); });
            // service
            IProductService productService = new ProductService(infoProductRepoMock.Object, stockRepoMock.Object);

            // ACT
            //--------------------------------------
            ProductEntity? expectedProduct = productService.GetProductById(id);

            // ASSERT
            //--------------------------------------
            expectedProduct.Should().NotBeNull();
            expectedProduct!.Id.Should().Be(id);
            expectedProduct!.StockEntity.Should().BeNull();
            expectedProduct!.InfoProductEntity.Should().BeNull();
            expectedProduct!.IsAvailable.Should().BeFalse();
            infoProductRepoMock.Verify(x => x.GetInfoProductById(id), Times.Once(), "GetInfoProductById is call more than once");
            stockRepoMock.Verify(x => x.GetStockById(id), Times.Never(), "GetStockById is call");
        }

        [Fact]
        [Description("Test GetTwoProductsById for test moq callbase, sequence, virtual, callback and protected")]
        public void GetTwoProductsById_TestMock_CallbaseAndSequenceAndVirtualAndCallbackAndProtected()
        {
            // ARRANGE
            //--------------------------------------
            ProductEntity productEntity1 = Mock.Of<ProductEntity>(x => x.StockEntity == Mock.Of<StockEntity>(s => s.Quantity == 2));
            ProductEntity productEntity2 = Mock.Of<ProductEntity>(x => x.StockEntity == Mock.Of<StockEntity>(s => s.Quantity == 0));
            Mock<ProductService> productServiceMock = new Mock<ProductService>(null, null);
            productServiceMock.SetupSequence(x => x.GetProductById(It.IsAny<int>()))
                              .Returns(productEntity1)
                              .Returns(productEntity2);
            productServiceMock.Protected()
                              .Setup<bool>("IsProductEnabled", ItExpr.IsAny<int>())
                              .Returns(true);
            productServiceMock.CallBase = true;

            // ACT
            //--------------------------------------
            List<ProductEntity?> expectedResult = productServiceMock.Object.GetAvailabilityOfTwoProductsById(0, 0);

            // ASSERT
            //--------------------------------------
            expectedResult.Should().BeEquivalentTo(new[] { productEntity1, productEntity2 });
        }
    }
}
