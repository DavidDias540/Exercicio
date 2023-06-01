using Microsoft.AspNetCore.Mvc;
using Exercise;
using WebApplication1.Exercise.Exercise.Controllers;

namespace WebApplication1.Exercise
{

    public class LetterController : ODataController
    {
        private readonly IDataService _dataService;

        public LetterController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [EnableQuery]
        public IActionResult Get()
        {
            try
            {
                // Obter os dados da fonte de dados
                var letterData = _dataService.GetLetterData();

                // Retornar os dados como resposta JSON
                return Ok(letterData);
            }
            catch (Exception ex)
            {
                // Lidar com erros e retornar uma resposta de erro adequada
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }

}
