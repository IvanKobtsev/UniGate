using Microsoft.AspNetCore.Mvc;
using UniGateAPI.Interfaces;

namespace UniGateAPI.Controllers;

[ApiController]
[Route("api/v1")]
public class ApplicantsController(IApplicantService applicantService) : ControllerBase
{
}