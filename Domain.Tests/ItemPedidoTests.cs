using Domain.Pedido.Entities;
using Domain.Pedido.ValueObjects;
using System;
using System.Linq;
using Xunit;

namespace Domain.Tests.Pedido
{
    public class ItemPedidoTests
    {
        // Método auxiliar para criar um Pedido válido e focar apenas no teste do Item
        private Domain.Pedido.Entities.Pedido CriarPedidoBase()
        {
            var endereco = EnderecoEntrega.Criar("01001-000", "Centro", "SP", "São Paulo", "Complemento");
            return Domain.Pedido.Entities.Pedido.Criar(endereco);
        }

        [Fact]
        public void AdicionarItem_Valido_DeveCalcularValorTotalCorretamente()
        {
            // Arrange
            var pedido = CriarPedidoBase();

            // Act
            pedido.AdicionarItem(1, "Produto A", 10.50m, 2);
            var item = pedido.Itens.First();

            // Assert
            Assert.Equal(1, item.ProdutoId);
            Assert.Equal("Produto A", item.NomeProduto);
            Assert.Equal(21.00m, item.ValorTotal);
            Assert.Equal(0, item.DescontoAplicado);
        }

        [Theory]
        [InlineData(0, "Produto", 10, 1)]  // ProdutoId zero
        [InlineData(-1, "Produto", 10, 1)] // ProdutoId negativo
        [InlineData(1, "", 10, 1)]         // Nome vazio
        [InlineData(1, null, 10, 1)]       // Nome nulo
        [InlineData(1, "Produto", 0, 1)]   // Preço zero
        [InlineData(1, "Produto", 10, 0)]  // Quantidade zero
        public void AdicionarItem_ComDadosInvalidos_DeveLancarExcecao(long produtoId, string nome, decimal preco, int qtd)
        {
            // Arrange
            var pedido = CriarPedidoBase();

            // Act & Assert
            Assert.ThrowsAny<Exception>(() => pedido.AdicionarItem(produtoId, nome, preco, qtd));
        }

        [Fact]
        public void AdicionarUnidades_EmItemExistente_DeveAtualizarQuantidadeEValorTotal()
        {
            // Arrange
            var pedido = CriarPedidoBase();
            pedido.AdicionarItem(1, "Produto A", 10.00m, 2); // Cria o item com total = 20

            // Act
            pedido.AdicionarItem(1, "Produto A", 10.00m, 3);
            var item = pedido.Itens.First();

            // Assert
            Assert.Equal(5, item.Quantidade);
            Assert.Equal(50.00m, item.ValorTotal);
            Assert.NotNull(item.DataAtualizacao);
        }

        [Fact]
        public void AdicionarUnidades_ZeroOuNegativoEmItemExistente_DeveLancarExcecao()
        {
            // Arrange
            var pedido = CriarPedidoBase();
            pedido.AdicionarItem(1, "Produto A", 10.00m, 2);

            // Act & Assert
            Assert.ThrowsAny<Exception>(() => pedido.AdicionarItem(1, "Produto A", 10.00m, 0));
            Assert.ThrowsAny<Exception>(() => pedido.AdicionarItem(1, "Produto A", 10.00m, -1));
        }
    }
}