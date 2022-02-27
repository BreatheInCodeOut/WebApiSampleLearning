namespace LocalBodyElection.Models
{
    public class Ward
    {
        public string City { get; set; }
        public int WardNumber { get; set; }
        public string ZoneName { get; set; }
        public Candidate[] CandidateList { get; set; }
    }
}
