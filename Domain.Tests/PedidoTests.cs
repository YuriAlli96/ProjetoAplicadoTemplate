using Domain.Pedido.Entities;
using Domain.Pedido.ValueObjects;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Domain.Tests.Pedido
{

    public class PedidoTests
    {
        private EnderecoEntrega ObterEnderecoValido() => (EnderecoEntrega)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(EnderecoEntrega));

        [Fact]
        public void CriarPedido_Valido_DeveInicializarComZeroEGerarNumero()
        {
            // Arrange
            var endereco = ObterEnderecoValido();

            // Act
            var pedido = Domain.Pedido.Entities.Pedido.Criar(endereco);

            // Assert
            Assert.NotNull(pedido);
            Assert.Equal(endereco, pedido.EnderecoEntrega);
            Assert.Equal(0m, pedido.ValorTotal);
            Assert.StartsWith("PED-", pedido.NumeroPedido);
            Assert.Empty(pedido.Itens);
        }

        [Fact]
        public void CriarPedido_EnderecoNulo_DeveLancarExcecao()
        {
            // Act & Assert
            Assert.ThrowsAny<Exception>(() => Domain.Pedido.Entities.Pedido.Criar(null!));
        }

        [Fact]
        public void AdicionarItem_NovoProduto_DeveAdicionarAListaERecalcularTotal()
        {
            // Arrange
            var pedido = Domain.Pedido.Entities.Pedido.Criar(ObterEnderecoValido());

            // Act
            pedido.AdicionarItem(1, "Monitor", 1000m, 1);
            pedido.AdicionarItem(2, "Mouse", 100m, 2);

            // Assert
            Assert.Equal(2, pedido.Itens.Count);
            Assert.Equal(1200m, pedido.ValorTotal);
            Assert.NotNull(pedido.DataAtualizacao);
        }

        [Fact]
        public void AdicionarItem_ProdutoExistente_DeveSomarQuantidadeERecalcularTotal()
        {
            // Arrange
            var pedido = Domain.Pedido.Entities.Pedido.Criar(ObterEnderecoValido());
            pedido.AdicionarItem(1, "Monitor", 1000m, 1); // Adiciona 1

            // Act
            pedido.AdicionarItem(1, "Monitor", 1000m, 2); // Adiciona mais 2

            // Assert
            Assert.Single(pedido.Itens);
            Assert.Equal(3, pedido.Itens.First().Quantidade);
            Assert.Equal(3000m, pedido.ValorTotal);
        }

        [Fact]
        public void RemoverItem_Valido_DeveRemoverERecalcularTotal()
        {
            // Arrange
            var pedido = Domain.Pedido.Entities.Pedido.Criar(ObterEnderecoValido());
            pedido.AdicionarItem(1, "Teclado", 200m, 1);
            pedido.AdicionarItem(2, "Mouse", 100m, 1);

            // Usando Reflection para injetar um ID no item para conseguir testar a remoção
            // (Veja a observação abaixo sobre esse comportamento)
            var item = pedido.Itens.First(i => i.ProdutoId == 1);
            var propertyInfo = typeof(Domain.Common.Base.Entity).GetProperty("Id");
            propertyInfo?.SetValue(item, 10L);

            // Act
            pedido.RemoverItem(10L);

            // Assert
            Assert.Single(pedido.Itens);
            Assert.Equal(100m, pedido.ValorTotal);
        }

        [Fact]
        public void RemoverItem_DeixandoListaVazia_DeveLancarExcecao()
        {
            // Arrange
            var pedido = Domain.Pedido.Entities.Pedido.Criar(ObterEnderecoValido());
            pedido.AdicionarItem(1, "Teclado", 200m, 1);

            var item = pedido.Itens.First();
            var propertyInfo = typeof(Domain.Common.Base.Entity).GetProperty("Id");
            propertyInfo?.SetValue(item, 10L);

            // Act & Assert
            var exception = Assert.ThrowsAny<Exception>(() => pedido.RemoverItem(10L));
            Assert.Contains("O pedido deve conter pelo menos um item", exception.Message);
        }

        [Fact]
        public void AtualizarEnderecoEntrega_Valido_DeveAtualizar()
        {
            // Arrange
            var pedido = Domain.Pedido.Entities.Pedido.Criar(ObterEnderecoValido());
            var novoEndereco = ObterEnderecoValido(); // Simulando um endereço diferente

            // Act
            pedido.AtualizarEnderecoEntrega(novoEndereco);

            // Assert
            Assert.Equal(novoEndereco, pedido.EnderecoEntrega);
            Assert.NotNull(pedido.DataAtualizacao);
        }
    }
}