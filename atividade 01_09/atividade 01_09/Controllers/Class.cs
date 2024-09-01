using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace PessoaAPI.Controllers
{
    // Modelo Pessoa
    public class Pessoa
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public double Peso { get; set; }
        public double Altura { get; set; }

        public double IMC()
        {
            return Peso / (Altura * Altura);
        }
    }

    // Controller PessoaController
    [Route("api/[controller]")]
    [ApiController]
    public class PessoaController : ControllerBase
    {
        private static List<Pessoa> pessoas = new List<Pessoa>();

        // A- Adicionar uma pessoa
        [HttpPost]
        public IActionResult AdicionarPessoa([FromBody] Pessoa novaPessoa)
        {
            if (pessoas.Any(p => p.Cpf == novaPessoa.Cpf))
            {
                return BadRequest("Pessoa com este CPF já existe.");
            }
            pessoas.Add(novaPessoa);
            return Ok(novaPessoa);
        }

        // B-  Atualizar os dados de uma pessoa
        [HttpPut("{cpf}")]
        public IActionResult AtualizarPessoa(string cpf, [FromBody] Pessoa pessoaAtualizada)
        {
            var pessoa = pessoas.FirstOrDefault(p => p.Cpf == cpf);
            if (pessoa == null)
            {
                return NotFound("Pessoa não encontrada.");
            }
            pessoa.Nome = pessoaAtualizada.Nome;
            pessoa.Peso = pessoaAtualizada.Peso;
            pessoa.Altura = pessoaAtualizada.Altura;
            return Ok(pessoa);
        }

        // C- Remover uma pessoa
        [HttpDelete("{cpf}")]
        public IActionResult RemoverPessoa(string cpf)
        {
            var pessoa = pessoas.FirstOrDefault(p => p.Cpf == cpf);
            if (pessoa == null)
            {
                return NotFound("Pessoa não encontrada.");
            }
            pessoas.Remove(pessoa);
            return Ok(pessoa);
        }

        // D- Buscar todas as pessoas
        [HttpGet]
        public IActionResult BuscarTodas()
        {
            return Ok(pessoas);
        }

        // E- Buscar uma pessoa específica informando o CPF
        [HttpGet("{cpf}")]
        public IActionResult BuscarPorCpf(string cpf)
        {
            var pessoa = pessoas.FirstOrDefault(p => p.Cpf == cpf);
            if (pessoa == null)
            {
                return NotFound("Pessoa não encontrada.");
            }
            return Ok(pessoa);
        }

        // F- Buscar todas as pessoas com IMC entre 18 e 24
        [HttpGet("imc-bom")]
        public IActionResult BuscarPorIMCBom()
        {
            var pessoasComIMCBom = pessoas.Where(p => p.IMC() >= 18 && p.IMC() <= 24).ToList();
            return Ok(pessoasComIMCBom);
        }

        // G- Fazer uma busca por nome
        [HttpGet("nome/{nome}")]
        public IActionResult BuscarPorNome(string nome)
        {
            var pessoasComNome = pessoas.Where(p => p.Nome.Contains(nome)).ToList();
            return Ok(pessoasComNome);
        }
    }
}
