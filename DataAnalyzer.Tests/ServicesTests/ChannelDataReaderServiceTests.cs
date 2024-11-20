using DataAnalyzer.Data.FT232Data;
using DataAnalyzer.Services.FT232ReaderServices;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer.Tests.ServicesTests
{
    public class ChannelDataReaderServiceTests
    {
        private readonly Mock<IFT232DataReader> _ft232DataReaderMock;
        private readonly ChannelDataReaderService _channelDataReaderService;

        public ChannelDataReaderServiceTests()
        {
            _ft232DataReaderMock = new Mock<IFT232DataReader>();
            _channelDataReaderService = new ChannelDataReaderService(_ft232DataReaderMock.Object);
        }

        [Fact]
        public async Task ReadDataAsync_ShouldReturnDataFromFT232()
        {
            // Arrange
            uint bufferSize = 8;
            var mockData = new byte[] { 0b10101010, 0b11001111, 0b11011100, 0b11101100, 0b01001100, 0b11001110, 0b11000100, 0b11001000 };
            _ft232DataReaderMock.Setup(reader => reader.ReadDataFromFT232(bufferSize))
                                .Returns(mockData);

            // Act
            var result = await _channelDataReaderService.ReadDataAsync(bufferSize);

            // Assert
            Assert.Equal(mockData, result);
            _ft232DataReaderMock.Verify(reader => reader.ReadDataFromFT232(bufferSize), Times.Once);
        }

        [Fact]
        public async Task ReadDataAsync_WithInsufficientData_ShouldThrowException()
        {
            // Arrange
            uint bufferSize = 8;
            var mockData = new byte[] { 0b1010101, 0b1100110 }; // Less than bufferSize

            // Arrange for the method to return insufficient data
            _ft232DataReaderMock.Setup(reader => reader.ReadDataFromFT232(bufferSize))
                                .Returns(mockData);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _channelDataReaderService.ReadDataAsync(bufferSize));
        }

        [Fact]
        public async Task ReadDataAsync_WithExcessData_ShouldThrowException()
        {
            // Arrange
            uint bufferSize = 8;
            var mockData = new byte[] { 0b10101010, 0b11001111, 0b11011100, 0b11101100, 0b01001100, 0b11001110, 0b11000100, 0b11001000, 0b11111000 }; // More than bufferSize
            _ft232DataReaderMock.Setup(reader => reader.ReadDataFromFT232(bufferSize))
                                .Returns(mockData);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _channelDataReaderService.ReadDataAsync(bufferSize));
        }

        [Fact]
        public async Task ReadGeneratedMockDataAsync_ShouldReturnMockData()
        {
            // Arrange
            uint bufferSize = 8;
            var mockData = new byte[] { 0b10101010, 0b11001111, 0b11011100, 0b11101100, 0b01001100, 0b11001110, 0b11000100, 0b11001000 };
            _ft232DataReaderMock.Setup(reader => reader.GenerateMockDataFromFT232(bufferSize))
                                .Returns(mockData);

            // Act
            var result = await _channelDataReaderService.ReadGeneratedMockDataAsync(bufferSize);

            // Assert
            Assert.Equal(mockData, result);
            _ft232DataReaderMock.Verify(reader => reader.GenerateMockDataFromFT232(bufferSize), Times.Once);
        }

    }
}
