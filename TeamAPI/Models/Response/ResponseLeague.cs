using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TeamAPI.Models.Response
{
    public class ResponseLeague
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ResponseCountry Country { get; set; }
        public ResponseLeague()
        {
            Country = new ResponseCountry();
        }
    }
}