using Domain.Common.Base;
using Domain.Common.Exceptions;
using Domain.Common.Validations;
using Domain.Pedido.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Pedido.Entities
{
    public sealed class Pedido : AggregateRoot
    {
        public EnderecoEntrega EnderecoEntrega { get; private set; }

        public decimal ValorTotal { get; private set; }
        public string NumeroPedido { get; private set; } = string.Empty;


        private readonly List<ItemPedido> _itens = new();
        public IReadOnlyCollection<ItemPedido> Itens => _itens.AsReadOnly();

        private Pedido(EnderecoEntrega enderecoEntrega)
        {
            DomainValidator.AgainstNull(enderecoEntrega, nameof(enderecoEntrega), "O endereço de entrega é obrigatório.");

            EnderecoEntrega = enderecoEntrega;
            ValorTotal = 0m;

            GerarNumeroPedido();
        }

        public static Pedido Criar(EnderecoEntrega enderecoEntrega)
         => new(enderecoEntrega);


        public void AdicionarItem(long produtoId, string nomeProduto,
                                  decimal precoUnitario, int quantidade)
        {

            var existente = _itens.FirstOrDefault(i => i.ProdutoId == produtoId);
            if (existente is not null)
                existente.AdicionarUnidades(quantidade);
            else
                _itens.Add(new ItemPedido(produtoId, nomeProduto, precoUnitario, quantidade));

            RecalcularValorTotal();
            SetDataAtualizacao();
        }
        public void RemoverItem(long itemId)
        {
            DomainValidator.Against<DomainException>(itemId <= 0, "ItemId inválido.");

            var item = _itens.FirstOrDefault(i => i.Id == itemId);
            DomainValidator.AgainstNull(item, nameof(item), "Item não encontrado no pedido.");

            _itens.Remove(item!);//sei que item não pode ser nulo aqui

            DomainValidator.Against<DomainException>(_itens.Count == 0,
                "O pedido deve conter pelo menos um item.");

            RecalcularValorTotal();
            SetDataAtualizacao();
        }

        public void AtualizarEnderecoEntrega(EnderecoEntrega novoEndereco)
        {
            DomainValidator.AgainstNull(novoEndereco, nameof(novoEndereco));

            EnderecoEntrega = novoEndereco;
            SetDataAtualizacao();
        }
        
        private void RecalcularValorTotal()
          => ValorTotal = _itens.Sum(i => i.ValorTotal);

        private void GerarNumeroPedido()
           => NumeroPedido = $"PED-{Id.ToString("D8")[..8].ToUpper()}";

    }
}
