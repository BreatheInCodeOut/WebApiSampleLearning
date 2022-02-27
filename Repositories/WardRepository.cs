using LocalBodyElection.Enums;
using LocalBodyElection.Interfaces;
using LocalBodyElection.Models;
using Newtonsoft.Json;

namespace LocalBodyElection.Repositories
{
    public class WardRepository : IWardRepository
    {
        #region "Fields"
        Ward[] Wards = GetWards();

        #endregion
        #region "Add"
        public bool AddCandidate(Candidate candidate)
        {
            string[] filterBy = candidate.CandidateId.Split('_');
            if (filterBy.Length == 3)
            {
                Ward ward = GetWardByCityAndWardNo(filterBy[0], Convert.ToInt32(filterBy[1]));
                if (ward != null && !ward.CandidateList.Any(c => c.CandidateId == candidate.CandidateId))
                {
                    List<Candidate> candidates = new List<Candidate>();
                    candidates = ward.CandidateList.ToList();
                    candidates.Add(candidate);
                    ward.CandidateList = candidates.ToArray();
                    UpdateFile();
                    return true;
                }
            }
            return false;
        }

        public bool AddWard(Ward ward)
        {
            if (GetWardByCityAndWardNo(ward.City, ward.WardNumber) == null)
            {
                Wards = Wards.Append(ward).ToArray();
                UpdateFile();
                return true;
            }
            return false;
        }
        #endregion

        #region "Update"
        public bool UpdateCandidate(Candidate candidate)
        {
            string[] filterBy = candidate.CandidateId.Split('_');
            Ward ward = GetWardByCityAndWardNo(filterBy[0], Convert.ToInt32(filterBy[1]));
            
            if (ward != null && ward.CandidateList.Any(c => c.CandidateId == candidate.CandidateId))
            {
                Candidate existingCandidate = ward.CandidateList.First(c => c.CandidateId == candidate.CandidateId);
                int index = Array.FindIndex(ward.CandidateList, c => c == existingCandidate);
                ward.CandidateList[index] = candidate;
                UpdateFile();
                return true;
            }
            return false;
         }
        public bool UpdateWard(Ward ward)
        {
            Ward existingWard = GetWardByCityAndWardNo(ward.City, ward.WardNumber);

            if (existingWard != null)
            {
                int index = Array.FindIndex(Wards, w => w == existingWard);
                Wards[index] = ward;
                UpdateFile();
                return true;
            }
            return false;
        }

        private void UpdateFile()
        {
            using (StreamWriter sw = new StreamWriter(@"..\LocalBodyElection\Resources\LocalBodyElectionData.json", false))
            {
                sw.WriteLine(JsonConvert.SerializeObject(Wards));
            }
        }
        #endregion

        #region "Delete"
        public bool DeleteCandidate(string candidateId)
        {
            string[] filterBy = candidateId.Split('_');
            Ward ward = GetWardByCityAndWardNo(filterBy[0], Convert.ToInt32(filterBy[1]));

            if (ward != null && ward.CandidateList.Any(c => c.CandidateId == candidateId))
            {
                ward.CandidateList = ward.CandidateList.Where(c => c.CandidateId != candidateId).ToArray();
                UpdateFile();
                return true;
            }

            return false;
        }

        public bool DeleteWard(string city, int wardNo)
        {
            Ward ward = GetWardByCityAndWardNo(city, wardNo);

            if (ward != null)
            {
                Wards = Wards.Where(w => !(w.City == city && w.WardNumber == wardNo)).ToArray();
                UpdateFile();
                return true;
            }

            return false;
        }
        #endregion

        #region "Get"
        public Candidate[] GetPartyCandidates(Party partyName)
        {
            return (from w in Wards
                    from c in w.CandidateList
                    where c.CandidateParty == partyName
                    select c).OrderBy(s => s.CandidateId).ToArray();
        }

        public Ward GetWardByCityAndWardNo(string cityName, int wardNo)
        {
            return (from w in Wards
                    where w.City == cityName && w.WardNumber == wardNo
                    select w).FirstOrDefault();

            //return wards.Where(w => w.City == cityName && w.WardNumber == wardNo).Select(s => s).FirstOrDefault();

            //Ward result = new Ward();
            //foreach (Ward w in wards)
            //{
            //    if(w.City == cityName && w.WardNumber == wardNo)
            //    {
            //        result.WardNumber = w.WardNumber;
            //        //same for others
            //        //or
            //        result = w;
            //    }
            //}
            //return result;
        }

        public Candidate[] GetWinnersByCity(string cityName)
        {
            return (from w in Wards
                    from c in w.CandidateList
                    where w.City == cityName && c.IsWinner == true
                    select c).OrderBy(s => s.CandidateId).ToArray();
        }

        private static Ward[] GetWards()
        {
            string json;
 
            using(StreamReader wardData = new StreamReader(@"..\LocalBodyElection\Resources\LocalBodyElectionData.json"))
            {
                json = wardData.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<Ward[]>(json);
        }

        #endregion
    }
}
