using LocalBodyElection.Enums;
using LocalBodyElection.Models;

namespace LocalBodyElection.Interfaces
{
    public interface IWardRepository
    {
        public Ward GetWardByCityAndWardNo(string cityName, int wardNo);
        public Candidate[] GetWinnersByCity(string cityName);
        public Candidate[] GetPartyCandidates(Party partyName);

        public Boolean AddWard(Ward ward);
        public Boolean AddCandidate(Candidate candidate);
        public Boolean UpdateWard(Ward ward);
        public Boolean UpdateCandidate(Candidate candidate);
        public Boolean DeleteWard(string city, int wardNo);
        public Boolean DeleteCandidate(string candidateId);
    }
}
