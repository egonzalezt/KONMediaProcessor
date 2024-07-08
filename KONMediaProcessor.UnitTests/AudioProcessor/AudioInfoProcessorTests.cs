namespace KONMediaProcessor.UnitTests.AudioProcessor;

using KONMediaProcessor.FileValidator;
using KONMediaProcessor.FFmpegExecutor;
using KONMediaProcessor.AudioProcessor.AudioInfo;
using Moq;

public class AudioInfoProcessorTests
{
    private readonly Mock<IFFmpegExecutor> executorMock = new();
    private readonly Mock<IFileValidator> fileValidatorMock = new ();
    
    [Fact]
    public void GetAudioInfo_FileNotFound_ThrowsException()
    {
        // Arrange
        const string inputFile = "nonexistentfile.mp3";
        fileValidatorMock.Setup(f => f.FileExists(inputFile)).Returns(false);
        var audioInfoProcessor = new AudioInfoProcessor(executorMock.Object, fileValidatorMock.Object);

        // Act - Assert
        var exception = Assert.Throws<FileNotFoundException>(() => audioInfoProcessor.GetAudioInfo(inputFile));
        Assert.Equal(inputFile, exception.Message);
    }
}
