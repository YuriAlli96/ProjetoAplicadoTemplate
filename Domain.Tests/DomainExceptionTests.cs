using Domain.Common.Exceptions;
using Xunit;

namespace Domain.Tests.Common.Exceptions
{
    public class DomainExceptionTests
    {
        [Fact]
        public void Construtor_DeveDefinirMensagemCorretamente()
        {
            // Arrange
            var mensagem = "Regra de negócio violada.";

            // Act
            var exception = new DomainException(mensagem);

            // Assert
            Assert.Equal(mensagem, exception.Message);
        }

        [Fact]
        public void When_CondicaoForVerdadeira_DeveLancarDomainException()
        {
            // Arrange
            var mensagemErro = "Ocorreu um erro validado pelo When.";

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() =>
                DomainException.When(true, mensagemErro));

            Assert.Equal(mensagemErro, exception.Message);
        }

        [Fact]
        public void When_CondicaoForFalsa_NaoDeveLancarExcecao()
        {
            // Arrange, Act & Assert
            var exception = Record.Exception(() =>
                DomainException.When(false, "Este erro não deve ocorrer."));

            Assert.Null(exception);
        }
    }
}