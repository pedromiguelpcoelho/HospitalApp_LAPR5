using System;
using Xunit;

namespace DefaultNamespace
{
    public class PhoneNumberTest
    {
        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenPhoneNumberIsInvalid()
        {
            // Testa se ArgumentException é lançado para números inválidos
            Assert.Throws<ArgumentException>(() => new PhoneNumber("12345678"));   // Menos de 9 dígitos
            Assert.Throws<ArgumentException>(() => new PhoneNumber("1234567890")); // Mais de 9 dígitos
            Assert.Throws<ArgumentException>(() => new PhoneNumber("12345abc9"));  // Contém letras
            Assert.Throws<ArgumentException>(() => new PhoneNumber("123 456789")); // Contém espaços
        }

        [Fact]
        public void Constructor_ShouldSetPhoneNumberValue_WhenPhoneNumberIsValid()
        {
            // Verifica se o número de telefone é corretamente setado quando o valor é válido
            var phoneNumber = new PhoneNumber("912345678");
            Assert.Equal("912345678", phoneNumber.Value);
        }

        [Fact]
        public void ImplicitConversion_ShouldConvertToString()
        {
            // Verifica se a conversão implícita para string funciona corretamente
            var phoneNumber = new PhoneNumber("912345678");
            string phoneNumberString = phoneNumber;
            Assert.Equal("912345678", phoneNumberString);
        }

        [Fact]
        public void ImplicitConversion_ShouldConvertFromString()
        {
            // Verifica se a conversão implícita de string para PhoneNumber funciona corretamente
            PhoneNumber phoneNumber = "912345678";
            Assert.Equal("912345678", phoneNumber.Value);
        }

        [Fact]
        public void ToString_ShouldReturnPhoneNumberValue()
        {
            // Verifica se o método ToString retorna o valor correto
            var phoneNumber = new PhoneNumber("912345678");
            Assert.Equal("912345678", phoneNumber.ToString());
        }
    }
}
