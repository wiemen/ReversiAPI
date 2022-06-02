using Microsoft.AspNetCore.Mvc;
using ReversiRestApi.DAL.Interfaces;
using ReversiRestApi.Models;
using ReversiRestApi.Models.DTO;
using ReversiRestApi.Models.Interfaces;
using System.Linq;

namespace ReversiRestApi.Controllers
{
    [Route("api/Uitslag")]
    [ApiController]
    public class UitslagController : ControllerBase
    {
        private readonly IUitslagRepository _uitslagRepository;
        private readonly ISpelRepository _spelRepository;

        public UitslagController(IUitslagRepository uitslagRepository, ISpelRepository spelRepository)
        {
            _uitslagRepository = uitslagRepository;
            _spelRepository = spelRepository;
        }

        [HttpPost]
        public ActionResult<Uitslag> Post([FromBody] UitslagDTO request)
        {
            var spel = _spelRepository.GetSpellen().FirstOrDefault(x => x.Token == request.Token);
            if (spel == null)
            {
                return BadRequest();
            }
            else if(_uitslagRepository.GetAll().FirstOrDefault(x=>x.SpelID.ToString() == request.Token) != null)
            {
                return BadRequest();
            }

            var uitslag = CreateUitslag(spel, request);
            spel.Status = Status.Afgelopen;
            _spelRepository.Update(spel);

            _uitslagRepository.Add(uitslag);
            return Ok(uitslag);
        }

        private Uitslag CreateUitslag(Spel spel, UitslagDTO request)
        {
            int aantalWit = 0;
            int aantalZwart = 0;
            for (int rijZet = 0; rijZet < 8; rijZet++)
            {
                for (int kolomZet = 0; kolomZet < 8; kolomZet++)
                {
                    if (spel.Bord[rijZet, kolomZet] == Kleur.Wit)
                        aantalWit++;
                    else if (spel.Bord[rijZet, kolomZet] == Kleur.Zwart)
                        aantalZwart++;
                }
            }

            var uitslag = new Uitslag(aantalWit, aantalZwart, spel, aantalWit > aantalZwart ? Kleur.Wit : Kleur.Zwart);
            if (request.Opgegeven)
            {
                if (spel.Speler1Token.Equals(request.Opgever))
                    uitslag.Winnaar = Kleur.Zwart;
                else
                    uitslag.Winnaar = Kleur.Wit;
            }
            else if (aantalWit == aantalZwart)
            {
                uitslag.Winnaar = Kleur.Geen;
            }

            return uitslag;
        }
    }
}
