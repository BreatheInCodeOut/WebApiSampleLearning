using LocalBodyElection.Enums;
using LocalBodyElection.Interfaces;
using LocalBodyElection.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LocalBodyElection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalBodyElectionController : Controller
    {
        private IWardRepository _wardRepository;

        public LocalBodyElectionController(IWardRepository wardRepository)
        {
            _wardRepository = wardRepository;
        }

        [HttpGet("{city}/{wardNo}", Name ="GetWard")]
        public string GetWardDetailsByCityWardNo(string city, int wardNo)
        {
            return getJsonOutput<Ward>(_wardRepository.GetWardByCityAndWardNo(city, wardNo));
        }

        [HttpGet("winner/{city}", Name = "GetWinners")]
        public string GetWinnerListByCity(string city)
        {
            return getJsonOutput<Candidate[]>(_wardRepository.GetWinnersByCity(city));
        }

        [HttpGet("candidates/{partyName}", Name = "GetPartyCandidates")]
        public string GetPartyCandidates(Party partyName)
        {
            return getJsonOutput<Candidate[]>(_wardRepository.GetPartyCandidates(partyName));
        }

        [HttpPost("add/ward")]
        public HttpResponseMessage AddWard(Ward ward)
        {
            if (_wardRepository.AddWard(ward))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
        }

       [HttpPost("add/candidate")]
       public HttpResponseMessage AddCandidate(Candidate candidate)
        {
            if(_wardRepository.AddCandidate(candidate))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
        }
       
       [HttpPost("update/ward")]
       public IActionResult UpdateWard(Ward ward)
        {
            if (_wardRepository.UpdateWard(ward))
            {
                return Ok();
            }

            return NotFound();
        }
       
       [HttpPost("update/candidate")]
       public IActionResult UpdateCandidate(Candidate candidate)
        {
            if (_wardRepository.UpdateCandidate(candidate))
            {
                return Ok();
            }

            return NotFound();
        }
       
       [HttpGet("delete/ward/{city}/{wardNo}")]
       public string DeleteWard(string city, int wardNo)
        {
            if (_wardRepository.DeleteWard(city, wardNo))
            {
                return "Deleted Ward Successfully";
            }

            return "Not Found";
        }
       
       [HttpGet("delete/candidate/{candidateId}")]
       public string DeleteCandidate(string candidateId)
        {
            if (_wardRepository.DeleteCandidate(candidateId))
            {
                return "Deleted Candidate Successfully";
            }

            return "Not Found";
        }
       
       private string getJsonOutput<T>(T objToSerialize)
        {
            if (objToSerialize != null)
            {
                string json = JsonConvert.SerializeObject(objToSerialize);
                if (json != "[]")
                    return json;
            }
            return "Not Found";
        }
    }
}