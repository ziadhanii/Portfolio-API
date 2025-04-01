using Microsoft.AspNetCore.Mvc;
using Portfolio.DTOs;
using Portfolio.Services;

namespace Portfolio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CertificatesController : ControllerBase
    {
        private readonly ICertificateService _certificateService;
        private readonly ILogger<CertificatesController> _logger;

        public CertificatesController(
            ICertificateService certificateService,
            ILogger<CertificatesController> logger)
        {
            _certificateService = certificateService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<CertificateDto>>> GetAllCertificates()
        {
            try
            {
                var certificates = await _certificateService.GetAllCertificatesAsync();
                return Ok(certificates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all certificates");
                return StatusCode(500, "Error getting certificates");
            }
        }

        [HttpGet("active")]
        public async Task<ActionResult<List<CertificateDto>>> GetActiveCertificates()
        {
            try
            {
                var certificates = await _certificateService.GetActiveCertificatesAsync();
                return Ok(certificates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active certificates");
                return StatusCode(500, "Error getting active certificates");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CertificateDto>> GetCertificateById(int id)
        {
            try
            {
                var certificate = await _certificateService.GetCertificateByIdAsync(id);
                if (certificate == null)
                    return NotFound();

                return Ok(certificate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting certificate with ID {Id}", id);
                return StatusCode(500, "Error getting certificate");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CertificateDto>> CreateCertificate([FromBody] CreateCertificateDto createDto)
        {
            try
            {
                var certificate = await _certificateService.CreateCertificateAsync(createDto);
                return CreatedAtAction(nameof(GetCertificateById), new { id = certificate.Id }, certificate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating certificate");
                return StatusCode(500, "Error creating certificate");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CertificateDto>> UpdateCertificate(int id, [FromBody] UpdateCertificateDto updateDto)
        {
            try
            {
                var certificate = await _certificateService.UpdateCertificateAsync(id, updateDto);
                if (certificate == null)
                    return NotFound();

                return Ok(certificate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating certificate with ID {Id}", id);
                return StatusCode(500, "Error updating certificate");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCertificate(int id)
        {
            try
            {
                var result = await _certificateService.DeleteCertificateAsync(id);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting certificate with ID {Id}", id);
                return StatusCode(500, "Error deleting certificate");
            }
        }
    }
} 