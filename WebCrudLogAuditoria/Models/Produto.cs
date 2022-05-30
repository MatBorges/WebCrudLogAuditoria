using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebCrudLogAuditoria.Models
{
    [Table("Produto")]
    public class Produto
    {
        [Column("Id")]
        [Display(Name = "Codigo")]

        public int Id { get; set; }

        [Column("Nome")]
        [Display(Name = "Nome")]

        public string Nome { get; set; }

    }
}
