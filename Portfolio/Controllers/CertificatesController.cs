using Portfolio.Services;

namespace Portfolio.Controllers;

public class CertificatesController(
    ICertificateService certificateService)
    : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<List<CertificateDto>>> GetAllCertificates()
    {
        var certificates = await certificateService.GetAllCertificatesAsync();
        return Ok(certificates);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CertificateDto>> GetCertificateById(int id)
    {
        var certificate = await certificateService.GetCertificateByIdAsync(id);

        if (certificate == null)
            return NotFound();

        return Ok(certificate);
    }

    [HttpPost]
    public async Task<ActionResult<CertificateDto>> CreateCertificate([FromForm] CreateCertificateDto createDto)
    {
        var certificate = await certificateService.CreateCertificateAsync(createDto, Request);
        return CreatedAtAction(nameof(GetCertificateById), new { id = certificate.Id }, certificate);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CertificateDto>> UpdateCertificate(int id, [FromForm] UpdateCertificateDto updateDto)
    {
        var certificate = await certificateService.UpdateCertificateAsync(id, updateDto, Request);
        if (certificate == null)
            return NotFound();

        return Ok(certificate);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCertificate(int id)
    {
        var result = await certificateService.DeleteCertificateAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}