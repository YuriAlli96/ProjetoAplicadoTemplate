using Domain.Common.Base;
using Domain.Common.Exceptions;
using Domain.Common.Validations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Domain.Pedido.ValueObjects
{
    public class EnderecoEntrega : ValueObject
    {
        public string Cep { get; private set; }
        public string Bairro { get; private set; }
        public string Estado { get; private set; }
        public string Cidade { get; private set; }
        public string Complemento { get; private set; }

        private EnderecoEntrega(string cep, string bairro, string estado, string cidade, string complemento)
        {
            DomainValidator.AgainstNullOrWhiteSpace(cep, nameof(Cep));
            DomainValidator.AgainstNullOrWhiteSpace(bairro, nameof(Bairro));
            DomainValidator.AgainstNullOrWhiteSpace(estado, nameof(Estado));
            DomainValidator.AgainstNullOrWhiteSpace(cidade, nameof(Cidade));

            Cep = cep!;
            Bairro = bairro;
            Estado = estado;
            Cidade = cidade;
            Complemento = complemento ?? string.Empty;
        }
        public static EnderecoEntrega Criar(string cep, string bairro, string estado, string cidade, string complemento)
        {
            return new EnderecoEntrega(cep, bairro, estado, cidade, complemento);
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Cep;
            yield return Complemento ?? string.Empty;
            yield return Bairro;
            yield return Estado;
            yield return Cidade;
        }
    }
}
