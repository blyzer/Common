using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Common360.Api.Configurations;
using Common360.Api.Models;
using Common360.Core.Rules;
using Common360.Entities.Models;
using System.Collections.Generic;

namespace Common360.Api.Controllers
{
    [Route("api/political-divisions")]
    public class PoliticalDivisionsController : BaseController
    {
        readonly ProvinceRules _rules;
        readonly ILogger<PoliticalDivisionsController> _logger;

        public PoliticalDivisionsController(ProvinceRules provinceRules, ILoggerFactory loggerFactory)
        {
            _rules = provinceRules;
            _logger = loggerFactory.CreateLogger<PoliticalDivisionsController>();
        }

        [HttpGet]
        [Route("provinces")]
        public ICollection<Province> GetProvinces()
        {
            ICollection<Province> provinces = _rules.GetAll();

            _logger.LogInformation(LoggingEvents.GetItem, "Getting Provinces.");

            return provinces;
            // return procinces.Map<ICollection<Province>, ICollection<ProvinceViewModel>>();
        }

        [HttpGet]
        [Route("provinces/{provinceName}/municipalities")]
        public ICollection<MunicipalityViewModel> GetMunicipalities(string provinceName)
        {
            ICollection<Municipality> municipalities = _rules.GetMunicipalities(provinceName);

            _logger.LogInformation(LoggingEvents.GetItem, "Getting Municipalities.");

            return municipalities.Map<ICollection<Municipality>, ICollection<MunicipalityViewModel>>();
        }

        [HttpGet]
        [Route("provinces/{provinceName}/municipalities/{municipalityName}/districts")]
        public ICollection<DistrictViewModel> GetDistricts(string provinceName, string municipalityName)
        {
            ICollection<District> municipalities = _rules.GetDistricts(provinceName, municipalityName);

            _logger.LogInformation(LoggingEvents.GetItem, "Getting Districts.");

            return municipalities.Map<ICollection<District>, ICollection<DistrictViewModel>>();
        }
    }
}
