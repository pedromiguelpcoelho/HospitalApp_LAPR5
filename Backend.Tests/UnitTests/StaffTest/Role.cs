using System;
using Xunit;

namespace DomainTests
{
    public class RoleTest
    {
        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenRoleIsInvalid()
        {
            // Testa se ArgumentException é lançado quando a role é nula, vazia, ou contém valor inválido
            Assert.Throws<ArgumentException>(() => new Role(null));
            Assert.Throws<ArgumentException>(() => new Role(""));
            Assert.Throws<ArgumentException>(() => new Role("InvalidRole")); // Valor fora dos permitidos
        }

        [Fact]
        public void Constructor_ShouldSetRoleValue_WhenRoleIsValid()
        {
            // Verifica se o valor da role é corretamente setado quando o valor é válido
            var role = new Role("Doctor");
            Assert.Equal("Doctor", role.Value);
        }

        [Fact]
        public void ImplicitConversion_ShouldConvertToString()
        {
            // Verifica se a conversão implícita para string funciona corretamente
            var role = new Role("Nurse");
            string roleString = role;
            Assert.Equal("Nurse", roleString);
        }

        [Fact]
        public void ImplicitConversion_ShouldConvertFromString()
        {
            // Verifica se a conversão implícita de string para Role funciona corretamente
            Role role = "Other";
            Assert.Equal("Other", role.Value);
        }

        [Fact]
        public void ToString_ShouldReturnRoleValue()
        {
            // Verifica se o método ToString retorna o valor correto
            var role = new Role("Doctor");
            Assert.Equal("Doctor", role.ToString());
        }
    }
}
