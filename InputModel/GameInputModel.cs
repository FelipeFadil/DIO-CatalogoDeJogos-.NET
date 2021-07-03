using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.InputModel
{
    public class GameInputModel
    {
        // DataAnnotations does the validations inside the middleware.
        // if something is wrong, the request won't send to controller

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The game's name must contain between 3 and 100 characters")]
        public string Name { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "The producer's name must contain between 1 and 100 characters")]
        public string Producer { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "The price must be at least 1 real and at most 1000 reais")]
        public double Price { get; set; }
    }
}
