using ExternalLogins.Models;
using System.Collections.Generic;
using System.Linq;

namespace ExternalLogins.Repository
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly ProfileContext _context;
        public ProfileRepository(ProfileContext context)
        {
            _context = context;
        }

        public List<Individual> GetIndividualList(string id)
        {
            return _context.Individuals.Where(x => x.AspNetUserId == id).ToList();
        }

        public async void CreateIndividual(Individual model)
        {
            _context.Add(model);
            await _context.SaveChangesAsync();
        }
    }
}
