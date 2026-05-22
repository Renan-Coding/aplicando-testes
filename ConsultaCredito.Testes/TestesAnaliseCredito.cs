using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConsultaCredito.Testes
{
    public class TestesAnaliseCredito
    {
        private readonly Mock<IServicoConsultaCredito> mock;

        private const string CPF_INVALIDO = "123A";
        private const string CPF_ERRO_COMUNICACAO = "76217486300";
        private const string CPF_SEM_PENDENCIAS = "60487583752";
        private const string CPF_INADIMPLENTE = "82226651209";

        public TestesAnaliseCredito()
        {
            mock = new Mock<IServicoConsultaCredito>(MockBehavior.Strict);

            mock.Setup(s => s.ConsultarPendenciasPorCPF(CPF_INVALIDO))
                .Returns(() => null);

            mock.Setup(s => s.ConsultarPendenciasPorCPF(CPF_ERRO_COMUNICACAO))
                .Throws(new Exception("Testando erro de comunicacao"));

            mock.Setup(s => s.ConsultarPendenciasPorCPF(CPF_SEM_PENDENCIAS))
                .Returns(() => new List<Pendencia>());

            Pendencia pendencia = new Pendencia()
            {
                CPF = CPF_INADIMPLENTE,
                NomePessoa = "Cliente Teste",
                NomeReclamante = "Empresas ACME",
                DescricaoPendencia = "Parcela nao paga",
                VlPendencia = 900.50
            };

            List<Pendencia> pendencias = new List<Pendencia>();
            pendencias.Add(pendencia);

            mock.Setup(s => s.ConsultarPendenciasPorCPF(CPF_INADIMPLENTE))
                .Returns(() => pendencias);
        }

        private StatusConsultaCredito ObterStatusAnaliseCredito(string cpf)
        {
            AnaliseCredito analise = new AnaliseCredito(mock.Object);
            return analise.ConsultarSituacaoCPF(cpf);
        }

        [Fact]
        public void TestarCPFInvalidoMoq()
        {
            StatusConsultaCredito status =
                ObterStatusAnaliseCredito(CPF_INVALIDO);

            status.Should().Be(
                StatusConsultaCredito.ParametroEnvioInvalido,
                "resultado incorreto para um CPF invalido");
        }

        [Fact]
        public void TestarErroComunicacaoMoq()
        {
            StatusConsultaCredito status =
                ObterStatusAnaliseCredito(CPF_ERRO_COMUNICACAO);

            status.Should().Be(
                StatusConsultaCredito.ErroComunicacao,
                "resultado incorreto para um erro de comunicacao");
        }

        [Fact]
        public void TestarCPFSemPendenciasMoq()
        {
            StatusConsultaCredito status =
                ObterStatusAnaliseCredito(CPF_SEM_PENDENCIAS);

            status.Should().Be(
                StatusConsultaCredito.SemPendencias,
                "resultado incorreto para um CPF sem pendencias");
        }

        [Fact]
        public void TestarCPFInadimplenteMoq()
        {
            StatusConsultaCredito status =
                ObterStatusAnaliseCredito(CPF_INADIMPLENTE);

            status.Should().Be(
                StatusConsultaCredito.Inadimplente,
                "resultado incorreto para um CPF inadimplente");
        }
    }
}
