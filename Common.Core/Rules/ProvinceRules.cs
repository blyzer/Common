using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Common360.Core.Repositories;
using Common360.Entities.Models;

namespace Common360.Core.Rules
{
    public class ProvinceRules : RulesBase<Province>
    {
        readonly ProvinceRepository _provinceRepository;

        public ProvinceRules (ProvinceRepository provinceRepository) : base (provinceRepository)
        {
            _provinceRepository = provinceRepository;
        }

        public Province GetByName (string provinceName)
        {
            return _provinceRepository.GetAll ().FirstOrDefault (p => p.Name == provinceName);
        }

        public ICollection<Municipality> GetMunicipalities (string provinceName)
        {
            return _provinceRepository.GetAll ()
                .Include (p => p.Municipalities)
                .Where (p => p.Name == provinceName)
                .SelectMany (p => p.Municipalities)
                .ToList ();
        }

        public ICollection<District> GetDistricts (string provinceName, string municipalityName)
        {
            return _provinceRepository.GetDistrictsByMunicipalityName (provinceName, municipalityName).ToList ();
        }

        public async Task<ISet<Province>> GetPredefinedProvincesAsync ()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceStream = assembly.GetManifestResourceStream ("Common360.Core.Resources.PoliticalDivision.json");

            using (var reader = new StreamReader (resourceStream, Encoding.UTF8))
            {
                string json = await reader.ReadToEndAsync ();
                return JsonConvert.DeserializeObject<ISet<Province>>(json);
            }
        }
    }
}