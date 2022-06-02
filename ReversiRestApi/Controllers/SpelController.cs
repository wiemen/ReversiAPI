using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReversiRestApi.DAL.Interfaces;
using ReversiRestApi.Models;
using ReversiRestApi.Models.DTO;
using ReversiRestApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReversiRestApi.Controllers
{
    [Route("api/Spel")]
    [ApiController]
    public class SpelController : ControllerBase
    {
        private readonly ISpelRepository _spelRepository;

        public SpelController(ISpelRepository spelRepository)
        {
            _spelRepository = spelRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> GetSpelOmschrijvingenVanSpellenMetWachtendeSpeler()
        {
            var result = _spelRepository.GetSpellen().Where(x => string.IsNullOrEmpty(x.Speler2Token));
            return Ok(result.Select(x => new { x.ID, x.Omschrijving, x.Speler1Token }));
        }

        [Route("{token}")]
        [HttpGet]
        public ActionResult GetByToken(string token)
        {
            var spel = _spelRepository.Get(token);
            if (spel == null)
            {
                return BadRequest();
            }

            return Ok(GetJson(spel));
        }

        [Route("[action]")]
        [HttpPut]
        public ActionResult<bool> Pas([FromBody] SpelPutDTO request)
        {
            var spel = _spelRepository.Get(request.Token);
            if (spel == null)
            {
                return BadRequest();
            }

            try
            {
                spel.Pas();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [Route("[action]/{token}")]
        [HttpGet]
        public ActionResult<List<(int, int, int)>> GetStats(string token)
        {
            var spel = _spelRepository.Get(token);
            if (spel == null)
            {
                return BadRequest();
            }

            return Ok(GetJson(spel.Beurten));
        }

        [Route("[action]/{token}")]
        [HttpGet]
        public ActionResult SpelSpeler(string token)
        {
            var spel = _spelRepository.GetSpellen().FirstOrDefault(x => (x.Speler1Token == token || x.Speler2Token == token) &&
            (x.Status == Status.Bezig || x.Status == Status.Wachtende));
            if (spel == null)
            {
                return BadRequest();
            }
            return Ok(GetJson(spel));
        }

        [Route("[action]/{token}")]
        [HttpGet]
        public ActionResult<string> Beurt(string token)
        {
            var spel = _spelRepository.Get(token);
            if (spel == null)
            {
                return BadRequest();
            }

            if (spel.AandeBeurt == Kleur.Wit)
            {
                return spel.Speler1Token;
            }
            else if (spel.AandeBeurt == Kleur.Zwart)
            {
                return spel.Speler2Token;
            }
            else
            {
                return string.Empty;
            }
        }

        [Route("[action]/{token}")]
        [HttpGet]
        public ActionResult<Kleur> BeurtKleur(string token)
        {
            var spel = _spelRepository.Get(token);
            if (spel == null)
            {
                return BadRequest();
            }

            return spel.AandeBeurt;
        }

        [HttpPost]
        public ActionResult Create([FromBody] SpelCreateDTO request)
        {
            var id = Guid.NewGuid();
            var spel = new Spel
            {
                ID = id,
                Token = id.ToString(),
                Speler1Token = request.Speler1Token,
                Omschrijving = request.Omschrijving
            };

            _spelRepository.Add(spel);
            return new OkResult();
        }

        [Route("[action]")]
        [HttpPut]
        public ActionResult<bool> Zet([FromBody] SpelPutDTO request)
        {
            var spel = _spelRepository.Get(request.Token);
            if (spel == null)
            {
                return BadRequest();
            }

            spel.DoeZet(request.X.Value, request.Y.Value);

            if (spel.Afgelopen())
                spel.Status = Status.Afgelopen;
            return Ok(_spelRepository.Update(spel));
        }

        [Route("[action]")]
        [HttpPut]
        public ActionResult Join([FromBody] SpelPutDTO request)
        {
            if (string.IsNullOrEmpty(request.Speler2Token))
            {
                return BadRequest();
            }

            var spel = _spelRepository.GetSpellen().SingleOrDefault(x => x.Token == request.Token);
            if (spel != null && !string.IsNullOrEmpty(spel.Speler2Token))
            {
                // already playing
                return BadRequest();
            }

            spel.Speler2Token = request.Speler2Token;
            spel.AandeBeurt = Kleur.Wit;
            spel.Status = Status.Bezig;
            var json = JsonConvert.SerializeObject(spel, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return _spelRepository.Update(spel) == true ? Ok(json) : StatusCode(500);
        }

        private string GetJson(object spel)
        {
            return JsonConvert.SerializeObject(spel, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
    }
}
