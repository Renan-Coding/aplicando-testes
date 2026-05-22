using System.Net;
using APIFinancas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace APIFinancas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculoFinanceiroController : ControllerBase
    {
        [HttpGet("juroscompostos")]
        [ProducesResponseType(typeof(Emprestimo), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(FalhaCalculo), (int)HttpStatusCode.BadRequest)]
        public ActionResult<Emprestimo> Get(
            [FromServices] ILogger<CalculoFinanceiroController> logger,
            double valorEmprestimo,
            int numMeses,
            double percTaxa)
        {
            if (valorEmprestimo <= 0)
                return new BadRequestObjectResult(new FalhaCalculo()
                {
                    Mensagem = "O valor do emprestimo deve ser maior do que zero."
                });

            if (numMeses <= 0)
                return new BadRequestObjectResult(new FalhaCalculo()
                {
                    Mensagem = "O numero de meses deve ser maior do que zero."
                });

            if (percTaxa <= 0)
                return new BadRequestObjectResult(new FalhaCalculo()
                {
                    Mensagem = "O percentual da taxa de juros deve ser maior do que zero."
                });

            logger.LogInformation(
                "Recebida nova requisicao|" +
                $"Valor do emprestimo: {valorEmprestimo}|" +
                $"Numero de meses: {numMeses}|" +
                $"% Taxa de Juros: {percTaxa}");

            double valorFinalJuros =
                CalculoFinanceiro.CalcularValorComJurosCompostos(
                    valorEmprestimo, numMeses, percTaxa);

            logger.LogInformation($"Valor final com juros: {valorFinalJuros}");

            return new Emprestimo()
            {
                ValorEmprestimo = valorEmprestimo,
                NumMeses = numMeses,
                TaxaPercentual = percTaxa,
                ValorFinalComJuros = valorFinalJuros
            };
        }
    }
}
