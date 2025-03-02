using Crazy_Musicians.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml;

namespace Crazy_Musicians.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusiciansController : ControllerBase
    {
        private static List<Musicians> _musicians = new List<Musicians>()
        {
            new Musicians{Id=1, Name=" Ahmet Çalgı", Job="Ünlü Çalgı Çalar", FunFeature="Her zaman yanlış nota çalar ama çok eğlenceli"},
            new Musicians{Id=2, Name="Zeynep Melodi", Job="Popüler Melodi Yazarı",FunFeature="Şarkıları yanlış anlaşılır ama çok popüler"},
            new Musicians{Id=3, Name="Cemil Akor", Job="Çılgın Akorist",FunFeature="Akorları sık değiştirir, ama şaşırtıcı derecede yetenekli"},
            new Musicians{Id=4, Name="Fatma Nota", Job="Sürpriz Nota Üreticisi",FunFeature="Nota üretirken sürekli sürprizler hazırlar"},
            new Musicians{Id=5, Name="Hasan Ritim", Job="Ritim Canavarı",FunFeature="Her ritmi kendi tarzında yapar, hiç uymaz ama komiktir"},
            new Musicians{Id=6, Name="Elif Armoni", Job="Armoni Ustası",FunFeature="Armonilerini bazen yanlış çalar, ama çok yaratıcıdır"},
            new Musicians{Id=7, Name="Ali Perde", Job="Perde Uygulayıcı", FunFeature="Her perdeyi farklı şekilde çalar, her zaman sürprizlidir"},
            new Musicians{Id=8, Name="Ayşe Rezonans", Job="Rezonans Uzmanı",FunFeature="Rezonans kanununda uzman, ama bazen çok gürültü çıkarır"},
            new Musicians{Id=9, Name="Murat Ton", Job="Tonlama Meraklısı",FunFeature="Tonlamalarındaki farklılıklar bazen komik, ama oldukça ilginç"},
            new Musicians{Id=10, Name="Selin Akor", Job="Akor Sihirbazı",FunFeature="Akorları değiştirdiğinde bazen sihirli bir hava yaratır"},
        };

        [HttpGet]
        public IEnumerable<Musicians> GetAll()
        {
            return _musicians;
        }

        [HttpGet("{id:int:min(1)}")]
        public ActionResult<Musicians> GetMusician(int id)
        {
            var musician = _musicians.FirstOrDefault(x=>x.Id==id);
            if(musician == null)
            {
                return NotFound($"Musician Id {id} not found.");
            }
            return Ok(musician);
        }

        [HttpGet("Name/{musicianName}")]
        public ActionResult<IEnumerable<Musicians>> GetMusicianByName( string musicianName)
        {
            var musicianPerson = _musicians.Where(x => x.Name.Equals(musicianName, StringComparison.OrdinalIgnoreCase)).ToList();
            if(!musicianPerson.Any())
            {
                return NotFound($"{musicianName} no name found for.");
            }
            return Ok(musicianPerson);
        }
        [HttpPost]
        public ActionResult<Musicians> Create([FromBody] Musicians musician)
        {
            var id = _musicians.Max(x => x.Id) + 1;
            musician.Id = id;
            _musicians.Add(musician);
            return CreatedAtAction(nameof(GetAll), new { id = musician.Id }, musician);
        }
        [HttpPut("update/{id:int:min(1)}/{newMusicianName}")]
        public IActionResult UpdateMusicians(int id, string newMusicianName)
        {
            var musicianPerson = _musicians.FirstOrDefault(x=>x.Id==id);
            if(musicianPerson == null)
            {
                return NotFound($"Musician Id {id} not found.");
            }
            musicianPerson.Name = newMusicianName;
            return Ok(); //Güncellenmiş müzisyeni döndürür.
        }
        [HttpDelete("{id:int:min(1)}")]
        public IActionResult CencelMusician(int id)
        {
            var musicianRemove = _musicians.FirstOrDefault(x=>x.Id == id);
            if(musicianRemove == null)
            {
                return NotFound($"specified Id {id} not found.");
            }
            _musicians.Remove(musicianRemove);
            return Ok();
        }

        [HttpDelete("cancel/{musicianName}")]
        public IActionResult CencelMusician(string musicianName)
        {
            var musicianRemove = _musicians.FirstOrDefault(x=>x.Name.Equals(musicianName,StringComparison.OrdinalIgnoreCase));
            if (musicianRemove == null)
            {
                return NotFound($"Specified name {musicianName} not found.");
            }
            _musicians.Remove(musicianRemove);
            return NoContent();
        }

        [HttpPatch("change/{id:int:min(1)}/{newFunFeature}")]
        public IActionResult ChangeMusician(int id, string newFunFeature)
        {
            var musician = _musicians.FirstOrDefault(x => x.Id == id);
            if (musician == null)
            {
                return NotFound($"Musician Id {id} not found.");
            }
            musician.FunFeature= newFunFeature;
            
            return Ok();
        }
        [HttpGet("search")]
        public ActionResult<List<Musicians>> Search([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return BadRequest("Name parameter is required!");

            var musicians = _musicians.Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

            if (musicians.Count == 0) return NotFound($"Musician with the name {name} could not be found!");

            return Ok(musicians);
        }

    }
}
