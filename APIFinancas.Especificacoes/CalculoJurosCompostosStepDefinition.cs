using System.Globalization;
using TechTalk.SpecFlow;
using Xunit;

namespace APIFinancas.Especificacoes
{
    [Binding]
    public class CalculoJurosCompostosStepDefinition
    {
        private double _valorEmprestimo;
        private int _numMeses;
        private double _percTaxa;
        private double _valorCalculado;

        [Given(@"que o valor o valor do empréstimo é de R\$ (.*)")]
        public void PreencherValorEmprestimo(string valorEmprestimo)
        {
            _valorEmprestimo = ConverterDecimalBrasileiro(valorEmprestimo);
        }

        [Given(@"que este empréstimo será por (.*) meses")]
        public void PreencherNumeroMeses(int numMeses)
        {
            _numMeses = numMeses;
        }

        [Given(@"que a taxa de juros é de (.*)% ao mês")]
        public void PreencherPercentualTaxa(string percTaxa)
        {
            _percTaxa = ConverterDecimalBrasileiro(percTaxa);
        }

        [When(@"eu solicitar o cálculo do valor total a ser pago ao final do período")]
        public void ProcessarCalculoJurosCompostos()
        {
            _valorCalculado = CalculoFinanceiro
                .CalcularValorComJurosCompostos(
                    _valorEmprestimo, _numMeses, _percTaxa);
        }

        [Then(@"o resultado será (.*)")]
        public void ValidarResultado(string valorFinalEmprestimo)
        {
            Assert.Equal(
                ConverterDecimalBrasileiro(valorFinalEmprestimo),
                _valorCalculado);
        }

        private static double ConverterDecimalBrasileiro(string valor)
        {
            return double.Parse(
                valor,
                new CultureInfo("pt-BR"));
        }
    }
}
