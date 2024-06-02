using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FileProcessingController : ControllerBase
{
    private readonly FileProcessingService _fileProcessingService;

    public FileProcessingController(FileProcessingService fileProcessingService)
    {
        _fileProcessingService = fileProcessingService;
    }

    [HttpPost("compress")]
    public async Task<IActionResult> CompressFile([FromForm] FileData fileData)
    {
        var compressedData = await _fileProcessingService.CompressFileAsync(fileData.File);
        var compressedFileName = $"{fileData.File.FileName}.gz";
        return File(compressedData, "application/gzip", compressedFileName);
    }

    [HttpPost("decompress")]
    public async Task<IActionResult> DecompressFile([FromForm] FileData fileData)
    {
        var decompressedData = await _fileProcessingService.DecompressFileAsync(fileData.File);
        var originalFileName = fileData.File.FileName;
        if (originalFileName.EndsWith(".gz"))
        {
            originalFileName = originalFileName.Substring(0, originalFileName.Length - 3);
        }
        return File(decompressedData, "application/octet-stream", originalFileName);
    }
}
