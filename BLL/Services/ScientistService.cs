using BLL.AdditionalModels;
using BLL.Services.Interfaces;
using DAL.AdditionalModels;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class ScientistService : IScientistService
    {
        private readonly IScientistRepository _scientistRepository;
        public ScientistService(IScientistRepository scientistRepository)
        {
            _scientistRepository = scientistRepository;
        }

        public async Task<Scientist> GetAsync(int id)
        {
            return await _scientistRepository.GetAsync(id);
        }

        public async Task<List<Scientist>> GetScientistsAsync(ScientistFilter filter)
        {
            return await _scientistRepository.GetAll().Where(scientist => filter == null ||
                (string.IsNullOrEmpty(filter.Name) || scientist.Name.Equals(filter.Name)) &&

                (string.IsNullOrEmpty(filter.ScholarUrl) || scientist.ScientistSocialNetworks
                .Any(scientistSocialNetwork => scientistSocialNetwork.Type == SocialNetworkType.Google && scientistSocialNetwork.Url.Equals(filter.ScholarUrl))) &&

                (string.IsNullOrEmpty(filter.ScopusUrl) || scientist.ScientistSocialNetworks
                .Any(scientistSocialNetwork => scientistSocialNetwork.Type == SocialNetworkType.Scopus && scientistSocialNetwork.Url.Equals(filter.ScopusUrl))) &&

                (string.IsNullOrEmpty(filter.WosUrl) || scientist.ScientistSocialNetworks
                .Any(scientistSocialNetwork => scientistSocialNetwork.Type == SocialNetworkType.WOS && scientistSocialNetwork.Url.Equals(filter.WosUrl))) &&

                (string.IsNullOrEmpty(filter.OrcidUrl) || scientist.ScientistSocialNetworks
                .Any(scientistSocialNetwork => scientistSocialNetwork.Type == SocialNetworkType.ORCID && scientistSocialNetwork.Url.Equals(filter.OrcidUrl))) &&

                (!filter.HRatingLessThan.HasValue || scientist.Rating < filter.HRatingLessThan.Value) &&

                (!filter.HRatingMoreThan.HasValue || scientist.Rating > filter.HRatingMoreThan.Value) &&

                (!filter.FieldOfResearchId.HasValue || scientist.ScientistFieldsOfResearch
                .Any(scientistFieldOfResearch => scientistFieldOfResearch.FieldOfResearchId == filter.FieldOfResearchId.Value)) &&

                (!filter.WorkId.HasValue || scientist.ScientistsWorks.Any(scientistWork => scientistWork.WorkId == filter.WorkId.Value)))
                .AsNoTracking().ToListAsync();
        }
    }
}
