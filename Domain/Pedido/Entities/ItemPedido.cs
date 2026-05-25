using Domain.Common.Base;
using Domain.Common.Exceptions;
using Domain.Common.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Pedido.Entities
{
    public sealed class ItemPedido : Entity
    {
        public long ProdutoId { get; private set; }

        public string NomeProduto { get; private set; } = string.Empty;

        public decimal PrecoUnitario { get; private set; }

        public int Quantidade { get; private set; }

        public decimal DescontoAplicado { get; private set; }

        public decimal ValorTotal { get; private set; }

        internal ItemPedido(long produtoId, string nomeProduto, decimal precoUnitario, int quantidade)
        {
            DomainValidator.Against<DomainException>(produtoId <= 0, "O ID do produto deve ser maior que zero.");
            DomainValidator.AgainstNullOrWhiteSpace(nomeProduto, nameof(nomeProduto), "O nome do produto é obrigatório.");
            DomainValidator.Against<DomainException>(precoUnitario <= 0, "O preço unitário deve ser maior que zero.");
            DomainValidator.Against<DomainException>(quantidade <= 0, "A quantidade deve ser maior que zero.");

            ProdutoId = produtoId;
            NomeProduto = nomeProduto;
            PrecoUnitario = precoUnitario;
            Quantidade = quantidade;
            DescontoAplicado = 0;

            CalcularValorTotal();
        }

        public void AdicionarUnidades(int unidades)
        {
            DomainValidator.Against<DomainException>(unidades <= 0, "Deve-se adicionar pelo menos uma unidade.");

            Quantidade += unidades;
            SetDataAtualizacao();
            CalcularValorTotal();
        }

        private void CalcularValorTotal()
        {
            ValorTotal = PrecoUnitario * Quantidade - DescontoAplicado;
        }
    }
}
