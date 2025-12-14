using Microsoft.EntityFrameworkCore;
using PeopleIO.Domain.Entity;

namespace PeopleIO.Infrastructure.Context;

public class PeopleIoContext : DbContext
{
    public PeopleIoContext(DbContextOptions<PeopleIoContext> options)
        : base(options) { }

    public DbSet<Colaborador> Colaborador { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<Colaborador>(entity =>
        {
            entity.ToTable("colaborador");

            entity.HasKey(c => c.Id);
            
            entity.Property(c => c.Sexo)
                .HasConversion<string>();
            
            entity.Property(c => c.CorRaca)
                .HasConversion<string>();
            
            entity.OwnsOne(c => c.Endereco, endereco =>
            {
                endereco.Property(e => e.Rua).HasColumnName("Rua");
                endereco.Property(e => e.Numero).HasColumnName("Numero");
                endereco.Property(e => e.Bairro).HasColumnName("Bairro");
                endereco.Property(e => e.Cidade).HasColumnName("Cidade");
                endereco.Property(e => e.Estado).HasColumnName("Estado");
                endereco.Property(e => e.CEP).HasColumnName("CEP");
            });

            entity.OwnsOne(c => c.ArquivoRG, doc =>
            {
                doc.Property(d => d.NomeArquivo).HasColumnName("ArquivoRGNome");
                doc.Property(d => d.Url).HasColumnName("ArquivoRGURL");
                doc.Property(d => d.DataUpload).HasColumnName("ArquivoRGDataUpload");
                doc.Property(d => d.TipoMime).HasColumnName("ArquivoRGTipoMime");
            });

            entity.OwnsOne(c => c.ArquivoCNH, doc =>
            {
                doc.Property(d => d.NomeArquivo).HasColumnName("ArquivoCNHNome");
                doc.Property(d => d.Url).HasColumnName("ArquivoCNHURL");
                doc.Property(d => d.DataUpload).HasColumnName("ArquivoCNHDataUpload");
                doc.Property(d => d.TipoMime).HasColumnName("ArquivoCNHTipoMime");
            });

            entity.OwnsOne(c => c.ArquivoCPF, doc =>
            {
                doc.Property(d => d.NomeArquivo).HasColumnName("ArquivoCPFNome");
                doc.Property(d => d.Url).HasColumnName("ArquivoCPFURL");
                doc.Property(d => d.DataUpload).HasColumnName("ArquivoCPFDataUpload");
                doc.Property(d => d.TipoMime).HasColumnName("ArquivoCPFTipoMime");
            });

            entity.OwnsOne(c => c.ArquivoComprovanteResidencia, doc =>
            {
                doc.Property(d => d.NomeArquivo).HasColumnName("ArquivoResidenciaNome");
                doc.Property(d => d.Url).HasColumnName("ArquivoResidenciaURL");
                doc.Property(d => d.DataUpload).HasColumnName("ArquivoResidenciaDataUpload");
                doc.Property(d => d.TipoMime).HasColumnName("ArquivoResidenciaTipoMime");
            });
            entity.OwnsOne(c => c.Curriculo, doc =>
            {
                doc.Property(d => d.NomeArquivo).HasColumnName("NomeArquivoCurriculo");
                doc.Property(d => d.Url).HasColumnName("ArquivoCurriculoURL");
                doc.Property(d => d.DataUpload).HasColumnName("ArquivoCurriculoDataUpload");
                doc.Property(d => d.TipoMime).HasColumnName("ArquivoCurriculoTipoMime");
            });
        });
    }
}