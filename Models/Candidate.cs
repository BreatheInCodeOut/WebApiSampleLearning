using LocalBodyElection.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LocalBodyElection.Models
{
    public class Candidate
    {
        public string CandidateId { get; set; }
        public string CandidateName { get; set; }
        [JsonConverter(typeof(StringEnumConverter))] //Data annotation - to return value as Enum name for CandidateParty
        public Party CandidateParty { get; set; }
        public int Votes { get; set; }
        public Boolean IsWinner { get; set; }
    }
}
