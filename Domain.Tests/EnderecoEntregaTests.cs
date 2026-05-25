using Domain.Pedido.ValueObjects;
using System;
using Xunit;

namespace Domain.Tests.Pedido.ValueObjects
{
    public class EnderecoEntregaTests
    {
        [Fact]
        public void Criar_ComParametrosValidos_DeveInstanciarCorretamente()
        {
            // Arrange
            var cep = "01001-000";
            var bairro = "Sé";
            var estado = "SP";
            var cidade = "São Paulo";
            var complemento = "Apto 101";

            // Act
            var endereco = EnderecoEntrega.Criar(cep, bairro, estado, cidade, complemento);

            // Assert
            Assert.Equal(cep, endereco.Cep);
            Assert.Equal(bairro, endereco.Bairro);
            Assert.Equal(estado, endereco.Estado);
            Assert.Equal(cidade, endereco.Cidade);
            Assert.Equal(complemento, endereco.Complemento);
        }

        [Fact]
        public void Criar_ComComplementoNulo_DeveAtribuirStringVazia()
        {
            // Arrange & Act
            var endereco = EnderecoEntrega.Criar("01001-000", "Sé", "SP", "São Paulo", null!);

            // Assert
            Assert.Equal(string.Empty, endereco.Complemento);
        }

        [Theory]
        [InlineData("", "Sé", "SP", "São Paulo")] // Cep vazio
        [InlineData(null, "Sé", "SP", "São Paulo")] // Cep nulo
        [InlineData("01001-000", "", "SP", "São Paulo")] // Bairro vazio
        [InlineData("01001-000", "Sé", "", "São Paulo")] // Estado vazio
        [InlineData("01001-000", "Sé", "SP", "")] // Cidade vazia
        public void Criar_ComCamposObrigatoriosNulosOuVazios_DeveLancarExcecao(string cep, string bairro, string estado, string cidade)
        {
            // Arrange, Act & Assert
            Assert.ThrowsAny<Exception>(() =>
                EnderecoEntrega.Criar(cep, bairro, estado, cidade, "Complemento"));
        }

        [Fact]
        public void ValueObjects_ComMesmosDados_DevemSerConsideradosIguais()
        {
            // Arrange
            var endereco1 = EnderecoEntrega.Criar("01001-000", "Sé", "SP", "São Paulo", "Apto 1");
            var endereco2 = EnderecoEntrega.Criar("01001-000", "Sé", "SP", "São Paulo", "Apto 1");

            // Act & Assert
            Assert.True(endereco1.Equals(endereco2));
            Assert.True(endereco1 == endereco2);
            Assert.Equal(endereco1.GetHashCode(), endereco2.GetHashCode());
        }

        [Fact]
        public void ValueObjects_ComDadosDiferentes_NaoDevemSerConsideradosIguais()
        {
            // Arrange
            var endereco1 = EnderecoEntrega.Criar("01001-000", "Sé", "SP", "São Paulo", "Apto 1");
            var endereco2 = EnderecoEntrega.Criar("01001-000", "Sé", "SP", "São Paulo", "Apto 2"); // Complemento diferente

            // Act & Assert
            Assert.False(endereco1.Equals(endereco2));
            Assert.True(endereco1 != endereco2);
            Assert.NotEqual(endereco1.GetHashCode(), endereco2.GetHashCode());
        }
    }
}