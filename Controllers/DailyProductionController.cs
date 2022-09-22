using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DailyProduction.Model;
using Azure.Data.Tables;
using Azure;

namespace IbasAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DailyProductionController : ControllerBase
    {

        private List<DailyProductionDTO> _productionRepo;
        private readonly ILogger<DailyProductionController> _logger;
        private TableClient _tableClient;

        public DailyProductionController(ILogger<DailyProductionController> logger)
        {


            this._tableClient = new TableClient(
new Uri("https://storageibas.table.core.windows.net/IBASProduktion2020"),
"IBASProduktion2020",
new TableSharedKeyCredential("storageibas", "hYz3pWntexoeuNZMGNpEaWUcEEZIo8bT2iyiDUj8/9/6JGuWtbB7LfB3Zumebm4psSJMynQqQW/f+ASth+mfwA=="));


        }
        
        [HttpGet]
        public IEnumerable<DailyProductionDTO> Get()
        {
            var production = new List<DailyProductionDTO>();
            Pageable<TableEntity> entities = this._tableClient.Query<TableEntity>();
            foreach (var entity in entities)
            {
                var dto = new DailyProductionDTO
                {
                    Date = DateTime.Parse(entity.RowKey),
                    Model = (BikeModel)Enum.ToObject(typeof(BikeModel), Int32.Parse(entity.PartitionKey)),
                    ItemsProduced = (int)entity.GetInt32("itemsProduced")
                };
                production.Add(dto);
            }
            return production;
        }
    }
}
