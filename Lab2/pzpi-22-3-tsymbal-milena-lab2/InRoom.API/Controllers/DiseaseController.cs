using InRoom.API.Contracts.Disease;
using InRoom.API.Contracts.Hospital;
using InRoom.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InRoom.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiseaseController : ControllerBase
{
    private readonly IDiseaseService _diseaseService;

    public DiseaseController(IDiseaseService diseaseService)
    {
        _diseaseService = diseaseService;
    }
    
    // Endpoint for creating a new disease 
    [HttpPost]
    [SwaggerOperation("Create disease")]
    public async Task<IActionResult> AddDisease(CreateDiseaseRequest request)
    {
        var disease = await _diseaseService.Add(
            request.Name, 
            request.SeverityLevel,
            request.Contagious,
            request.ContagionRate,
            request.IncubationPeriod,
            request.MortalityRate,
            request.TransmissionMode);
        return Ok(disease);
    }
    
    // Endpoint for retrieving a disease by its ID
    [HttpGet("{diseaseId:Guid}")]
    [SwaggerOperation("Get disease by id")]
    public async Task<IActionResult> GetDisease([FromRoute] Guid diseaseId)
    {
        var disease = await _diseaseService.GetById(diseaseId);
        return Ok(disease);
    }
    
    // Endpoint for retrieving all diseases
    [HttpGet]
    [SwaggerOperation("Get all diseases")]
    public async Task<IActionResult> GetAllDiseases()
    {
        var diseases = await _diseaseService.GetAll();
        return Ok(diseases);
    }
    
    // Endpoint for updating disease information by its ID
    [HttpPatch("{diseaseId:Guid}")]
    [SwaggerOperation("Edit disease by id")]
    public async Task<IActionResult> EditDisease([FromRoute] Guid diseaseId, UpdateDiseaseRequest request)
    {
        var disease = await _diseaseService.Update(
            diseaseId, 
            request.Name, 
            request.SeverityLevel,
            request.Contagious,
            request.ContagionRate,
            request.IncubationPeriod,
            request.MortalityRate,
            request.TransmissionMode);
        return Ok(disease);
    }
    
    // Endpoint for deleting a disease by its ID
    [HttpDelete("{diseaseId:Guid}")]
    [SwaggerOperation("Delete disease by id")]
    public async Task<IActionResult> DeleteDisease([FromRoute] Guid diseaseId)
    {
        var disease = await _diseaseService.Delete(diseaseId);
        return Ok(disease);
    }
}