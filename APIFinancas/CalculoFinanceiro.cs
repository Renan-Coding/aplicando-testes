using System;

namespace APIFinancas
{
    public static class CalculoFinanceiro
    {
        public static double CalcularValorComJurosCompostos(
            double valorEmprestimo, int numMeses, double percTaxa)
        {
            return Math.Round(
                valorEmprestimo * Math.Pow(1 + (percTaxa / 100), numMeses),
                2);
        }
    }
}
