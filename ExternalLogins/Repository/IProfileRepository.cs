using ExternalLogins.Models;
using System.Collections.Generic;

namespace ExternalLogins.Repository
{
    public interface IProfileRepository
    {
        List<Individual> GetIndividualList(string id);
        void CreateIndividual(Individual model);
    }
}
