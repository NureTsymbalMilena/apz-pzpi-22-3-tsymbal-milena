using InRoom.API.Contracts.Hospital;
using InRoom.API.Contracts.User;
using InRoom.BLL.Interfaces;
using InRoom.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InRoom.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HospitalController : ControllerBase
{
    private readonly IHospitalService _hospitalService;

    public HospitalController(IHospitalService hospitalService)
    {
        _hospitalService = hospitalService;
    }
    
    // Endpoint for creating a new hospital with the provided name and address
    [HttpPost]
    [SwaggerOperation("Create hospital")]
    public async Task<IActionResult> AddHospital(CreateHospitalRequest request)
    {
        var hospital = await _hospitalService.Add(request.Name, request.Address);
        return Ok(hospital);
    }
    
    // Endpoint for retrieving a hospital by its ID
    [HttpGet("{hospitalId:Guid}")]
    [SwaggerOperation("Get hospital by id")]
    public async Task<IActionResult> GetHospital([FromRoute] Guid hospitalId)
    {
        var hospital = await _hospitalService.GetById(hospitalId);
        return Ok(hospital);
    }
    
    // Endpoint for retrieving all hospitals
    [HttpGet]
    [SwaggerOperation("Get all hospitals")]
    public async Task<IActionResult> GetAllHospitals()
    {
        var hospitals = await _hospitalService.GetAll();
        return Ok(hospitals);
    }
    
    // Endpoint for updating hospital information (name and address) by its ID
    [HttpPatch("{hospitalId:Guid}")]
    [SwaggerOperation("Edit hospital by id")]
    public async Task<IActionResult> EditHospital([FromRoute] Guid hospitalId, UpdateHospitalRequest request)
    {
        var hospital = await _hospitalService.Update(hospitalId, request.Name, request.Address);
        return Ok(hospital);
    }
    
    // Endpoint for deleting a hospital by its ID
    [HttpDelete("{hospitalId:Guid}")]
    [SwaggerOperation("Delete hospital by id")]
    public async Task<IActionResult> DeleteHospital([FromRoute] Guid hospitalId)
    {
        var hospital = await _hospitalService.Delete(hospitalId);
        return Ok(hospital);
    }
}
