using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DDDSample1.Domain.StaffProfile;

namespace DDDSample1.Infrastructure.StaffProfile
{
    internal class StaffEntityTypeConfiguration : IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> builder)
        {

            string role = "";

            // Define a tabela correspondente e a chave primária
            builder.ToTable("Staff")
                .HasKey(s => s.Id);

            // Mapeia o ID personalizado
            builder.Property(s => s.Id)
                .HasConversion(
                    id => id.Value, // Converte de StaffId para Guid
                    value => new StaffId(value)); // Converte de Guid para StaffId


            // Mapeia a propriedade Name (FirstName)
            builder.Property(s => s.FirstName)
                .IsRequired() // Propriedade obrigatória
                .HasConversion(
                    name => name.Value, // Converte de FullName para string
                    value => new FirstName(value)) // Converte de string para FirstName
                .HasMaxLength(100); // Define o tamanho máximo


            // Mapeia a propriedade Name (LastName)
            builder.Property(s => s.LastName)
                .IsRequired() // Propriedade obrigatória
                .HasConversion(
                    name => name.Value, // Converte de FullName para string
                    value => new LastName(value)) // Converte de string para lastName
                .HasMaxLength(100); // Define o tamanho máximo    


            // Mapeia a propriedade Name (FullName)
            builder.Property(s => s.Name)
                .IsRequired() // Propriedade obrigatória
                .HasConversion(
                    name => name.Value, // Converte de FullName para string
                    value => new FullName(value)) // Converte de string para FullName
                .HasMaxLength(100); // Define o tamanho máximo


            // Mapeia a propriedade Role
            builder.Property(s => s.Role)
                .IsRequired() // Propriedade obrigatória
                .HasConversion(
                    role => role.Value, // Converte de Role para string
                    value => new Role(value)) // Converte de string para Role
                .HasMaxLength(50); // Define o tamanho máximo

                    

            // Mapeia a propriedade Email
            builder.Property(s => s.Email)
                .IsRequired() // Propriedade obrigatória
                .HasConversion(
                    email => email.Value, // Converte de Email para string
                    value => new Email(value)) // Converte de string para Email
                .HasMaxLength(100); // Define o tamanho máximo

            // Mapeia a propriedade PhoneNumber
            builder.Property(s => s.PhoneNumber)
                .IsRequired() // Propriedade obrigatória
                .HasConversion(
                    phoneNumber => phoneNumber.Value, // Converte de PhoneNumber para string
                    value => new PhoneNumber(value)) // Converte de string para PhoneNumber
                .HasMaxLength(20); // Define o tamanho máximo

            // Mapeia a propriedade IsActive
            builder.Property(s => s.IsActive)
                .IsRequired(); // Propriedade obrigatória


                  builder.Property(s => s.LicenseNumber)
                .IsRequired() // Propriedade obrigatória
                .HasConversion(
                    licenseNumber => licenseNumber.Value, // Converte de LicenseNumber para string
                     value => CreateLicenseNumber(value,role)) // Chama o método para criar LicenseNumber
                .HasMaxLength(50); // Define o tamanho máximo

                   // Mapeia a propriedade Specialization
            builder.Property(s => s.Specialization)
                .IsRequired() // Propriedade obrigatória
                .HasConversion(
                    Specialization => Specialization.Value, // Converte de Specialization para string
                    value => new Specialization(value,role)) // Converte de string para specialization
                .HasMaxLength(100); // Define o tamanho máximo

        }


         private Specialization CreateSpecialization(string value)
        {
     
     
                string role = "Other"; 
                
                char roleChar = value[0];
                if (roleChar == 'D')
                {
                 role = "Doctor";
                }
                else if (roleChar == 'N')
                {
                    role = "Nurse";
                }

                
            return new Specialization(value, role);
        }

        private LicenseNumber CreateLicenseNumber(string value,string role)
        {
            var sequentialNumber = int.TryParse(value.Substring(value.Length - 5), out var number) ? number : 0;

             role = "Other"; 
    
            char roleChar = value[0];
            if (roleChar == 'D')
            {
                role = "Doctor";
            }
                else if (roleChar == 'N')
            {
                role = "Nurse";
            }
    
            return new LicenseNumber(role, sequentialNumber);
        }

       
    }
}

