using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Interfaces;
using ShittyApi.Models;

namespace ShittyApi.Controllers;

[ApiController]
[Route("[controller]")]
public class BaseApiController : ControllerBase
{
    IScrapper otodomScrapper;
    public BaseApiController(IOtodomScrapper otodomScrapper) { 
        this.otodomScrapper = otodomScrapper;
    }
    [HttpGet("GetServiceData")]
    public async Task<JsonResult> GetServiceData(BaseGetRequestModel model){
        foreach(var param in model.Params)
        {
            otodomScrapper.AddParameter(param.Name, param.Value);
        }
        otodomScrapper.Scrap();
        return new JsonResult(otodomScrapper.GetFormatedData());
    }
}
