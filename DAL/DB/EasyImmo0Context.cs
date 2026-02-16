using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL.DB;

public partial class EasyImmo0Context : DbContext
{
    public EasyImmo0Context()
    {
    }

    public EasyImmo0Context(DbContextOptions<EasyImmo0Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Acheteur> Acheteurs { get; set; }

    public virtual DbSet<Activite> Activites { get; set; }

    public virtual DbSet<Adresse> Adresses { get; set; }

    public virtual DbSet<Bien> Biens { get; set; }

    public virtual DbSet<DocumentBien> DocumentBiens { get; set; }

    public virtual DbSet<EffectuerOperationPersonne> EffectuerOperationPersonnes { get; set; }

    public virtual DbSet<Employe> Employes { get; set; }

    public virtual DbSet<HistoriqueStatusBien> HistoriqueStatusBiens { get; set; }

    public virtual DbSet<Locataire> Locataires { get; set; }

    public virtual DbSet<MoyenContact> MoyenContacts { get; set; }

    public virtual DbSet<OperationImmobiliere> OperationImmobilieres { get; set; }

    public virtual DbSet<ParticiperActivitePersonne> ParticiperActivitePersonnes { get; set; }

    public virtual DbSet<Personne> Personnes { get; set; }

    public virtual DbSet<Planning> Plannings { get; set; }

    public virtual DbSet<PossederBienProprietaire> PossederBienProprietaires { get; set; }

    public virtual DbSet<Proprietaire> Proprietaires { get; set; }

    public virtual DbSet<StatutBien> StatutBiens { get; set; }

    public virtual DbSet<TypeActivite> TypeActivites { get; set; }

    public virtual DbSet<TypeBien> TypeBiens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=EasyImmo0;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Acheteur>(entity =>
        {
            entity.HasKey(e => e.IdAch);

            entity.ToTable("Acheteur");

            entity.Property(e => e.IdAch)
                .ValueGeneratedOnAdd()
                .HasColumnName("id_ach");
            entity.Property(e => e.BudgetMaxAch)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("budget_max_ach");
            entity.Property(e => e.IdPersonne).HasColumnName("id_personne");
            entity.Property(e => e.ZoneSouhaiteAch)
                .HasMaxLength(50)
                .HasColumnName("zone_souhaite_ach");

            entity.HasOne(d => d.IdAchNavigation).WithOne(p => p.Acheteur)
                .HasForeignKey<Acheteur>(d => d.IdAch)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Acheteur_Personnes");
        });

        modelBuilder.Entity<Activite>(entity =>
        {
            entity.HasKey(e => e.IdActivite).HasName("PK_activite");

            entity.ToTable("Activite");

            entity.Property(e => e.IdActivite).HasColumnName("id_activite");
            entity.Property(e => e.DateActivite).HasColumnName("date_activite");
            entity.Property(e => e.DureeActivite).HasColumnName("duree_activite");
            entity.Property(e => e.IdBien).HasColumnName("id_bien");
            entity.Property(e => e.IdPlanning).HasColumnName("id_planning");
            entity.Property(e => e.IdType).HasColumnName("id_type");
            entity.Property(e => e.LibelleActivite)
                .HasMaxLength(50)
                .HasColumnName("libelle_activite");
        });

        modelBuilder.Entity<Adresse>(entity =>
        {
            entity.HasKey(e => e.IdAdresse).HasName("PK__Adresse__05B3E6DC19617CE5");

            entity.ToTable("Adresse");

            entity.Property(e => e.IdAdresse).HasColumnName("id_adresse");
            entity.Property(e => e.CodePostal)
                .HasMaxLength(20)
                .HasColumnName("code_postal");
            entity.Property(e => e.Numero)
                .HasMaxLength(10)
                .HasColumnName("numero");
            entity.Property(e => e.Pays)
                .HasMaxLength(100)
                .HasColumnName("pays");
            entity.Property(e => e.Rue)
                .HasMaxLength(255)
                .HasColumnName("rue");
            entity.Property(e => e.Ville)
                .HasMaxLength(100)
                .HasColumnName("ville");
        });

        modelBuilder.Entity<Bien>(entity =>
        {
            entity.HasKey(e => e.IdBien);

            entity.ToTable("Bien");

            entity.Property(e => e.IdBien).HasColumnName("id_bien");
            entity.Property(e => e.DatePublicationBien).HasColumnName("date_publication_bien");
            entity.Property(e => e.DescriptionBien)
                .HasMaxLength(50)
                .HasColumnName("description_bien");
            entity.Property(e => e.IdAdresse).HasColumnName("id_adresse");
            entity.Property(e => e.IdOperation).HasColumnName("id_operation");
            entity.Property(e => e.IdTy).HasColumnName("id_ty");
            entity.Property(e => e.PrixBien)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("prix_bien");
            entity.Property(e => e.SurfacesBien)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("surfaces_bien");

            entity.HasOne(d => d.IdAdresseNavigation).WithMany(p => p.Biens)
                .HasForeignKey(d => d.IdAdresse)
                .HasConstraintName("FK_Bien_Adresse");

            entity.HasOne(d => d.IdOperationNavigation).WithMany(p => p.Biens)
                .HasForeignKey(d => d.IdOperation)
                .HasConstraintName("FK_Bien_OperationImmobiliere");

            entity.HasOne(d => d.IdTyNavigation).WithMany(p => p.Biens)
                .HasForeignKey(d => d.IdTy)
                .HasConstraintName("FK_Bien_TypeBien");
        });

        modelBuilder.Entity<DocumentBien>(entity =>
        {
            entity.HasKey(e => e.IdDoc);

            entity.ToTable("DocumentBien");

            entity.Property(e => e.IdDoc).HasColumnName("id_doc");
            entity.Property(e => e.DescriptionDoc).HasColumnName("description_doc");
            entity.Property(e => e.IdBien).HasColumnName("id_bien");

            entity.HasOne(d => d.IdBienNavigation).WithMany(p => p.DocumentBiens)
                .HasForeignKey(d => d.IdBien)
                .HasConstraintName("FK_DocumentBien_Bien");
        });

        modelBuilder.Entity<EffectuerOperationPersonne>(entity =>
        {
            entity.ToTable("EffectuerOperationPersonne");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.IdOperation).HasColumnName("id_operation");
            entity.Property(e => e.IdPersonne).HasColumnName("id_personne");
            entity.Property(e => e.TypePersonne)
                .HasMaxLength(50)
                .HasColumnName("type_personne");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.EffectuerOperationPersonne)
                .HasForeignKey<EffectuerOperationPersonne>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EffectuerOperationPersonne_OperationImmobiliere");

            entity.HasOne(d => d.Id1).WithOne(p => p.EffectuerOperationPersonne)
                .HasForeignKey<EffectuerOperationPersonne>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EffectuerOperationPersonne_Personnes");
        });

        modelBuilder.Entity<Employe>(entity =>
        {
            entity.HasKey(e => e.IdEmp).HasName("PK_employe");

            entity.ToTable("Employe");

            entity.Property(e => e.IdEmp)
                .ValueGeneratedOnAdd()
                .HasColumnName("id_emp");
            entity.Property(e => e.IdPersonne).HasColumnName("id_personne");
            entity.Property(e => e.RoleEmp)
                .HasMaxLength(50)
                .HasColumnName("role_emp");

            entity.HasOne(d => d.IdEmpNavigation).WithOne(p => p.Employe)
                .HasForeignKey<Employe>(d => d.IdEmp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employe_Personnes");
        });

        modelBuilder.Entity<HistoriqueStatusBien>(entity =>
        {
            entity.ToTable("HistoriqueStatusBien");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.IdBien).HasColumnName("id_bien");
            entity.Property(e => e.IdSta).HasColumnName("id_sta");
            entity.Property(e => e.NombreBienVendu).HasColumnName("nombre_bien_vendu");
            entity.Property(e => e.NombreBienVisite).HasColumnName("nombre_bien_visite");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.HistoriqueStatusBien)
                .HasForeignKey<HistoriqueStatusBien>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistoriqueStatusBien_Bien");

            entity.HasOne(d => d.Id1).WithOne(p => p.HistoriqueStatusBien)
                .HasForeignKey<HistoriqueStatusBien>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistoriqueStatusBien_StatutBien");
        });

        modelBuilder.Entity<Locataire>(entity =>
        {
            entity.HasKey(e => e.IdLocataire);

            entity.ToTable("Locataire");

            entity.Property(e => e.IdLocataire)
                .ValueGeneratedOnAdd()
                .HasColumnName("id_locataire");
            entity.Property(e => e.IdPersonne).HasColumnName("id_personne");
            entity.Property(e => e.MontantMax)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("montant_max");

            entity.HasOne(d => d.IdLocataireNavigation).WithOne(p => p.Locataire)
                .HasForeignKey<Locataire>(d => d.IdLocataire)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Locataire_Personnes");
        });

        modelBuilder.Entity<MoyenContact>(entity =>
        {
            entity.HasKey(e => e.IdContact).HasName("PK__MoyenCon__3D4F725E6A5F795A");

            entity.ToTable("MoyenContact");

            entity.Property(e => e.IdContact).HasColumnName("id_contact");
            entity.Property(e => e.IdPersonne).HasColumnName("id_personne");
            entity.Property(e => e.TypeContact)
                .HasMaxLength(50)
                .HasColumnName("type_contact");
            entity.Property(e => e.Valeur)
                .HasMaxLength(255)
                .HasColumnName("valeur");
        });

        modelBuilder.Entity<OperationImmobiliere>(entity =>
        {
            entity.HasKey(e => e.IdOperation);

            entity.ToTable("OperationImmobiliere");

            entity.Property(e => e.IdOperation).HasColumnName("id_operation");
            entity.Property(e => e.DateOperation).HasColumnName("date_operation");
            entity.Property(e => e.DescriptionOperation)
                .HasMaxLength(50)
                .HasColumnName("description_operation");
            entity.Property(e => e.IdEmp).HasColumnName("id_emp");

            entity.HasOne(d => d.IdEmpNavigation).WithMany(p => p.OperationImmobilieres)
                .HasForeignKey(d => d.IdEmp)
                .HasConstraintName("FK_OperationImmobiliere_Employe");
        });

        modelBuilder.Entity<ParticiperActivitePersonne>(entity =>
        {
            entity.ToTable("ParticiperActivitePersonne");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.But)
                .HasMaxLength(50)
                .HasColumnName("but");
            entity.Property(e => e.IdActivite).HasColumnName("id_activite");
            entity.Property(e => e.IdPersonne).HasColumnName("id_personne");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.ParticiperActivitePersonne)
                .HasForeignKey<ParticiperActivitePersonne>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ParticiperActivitePersonne_Activite");

            entity.HasOne(d => d.Id1).WithOne(p => p.ParticiperActivitePersonne)
                .HasForeignKey<ParticiperActivitePersonne>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ParticiperActivitePersonne_Personnes");
        });

        modelBuilder.Entity<Personne>(entity =>
        {
            entity.HasKey(e => e.IdPersonne);

            entity.Property(e => e.IdPersonne).HasColumnName("id_personne");
            entity.Property(e => e.NomPersonne)
                .HasMaxLength(50)
                .HasColumnName("nom_personne");
            entity.Property(e => e.PrenomPersonne)
                .HasMaxLength(50)
                .HasColumnName("prenom_personne");
        });

        modelBuilder.Entity<Planning>(entity =>
        {
            entity.HasKey(e => e.IdPlanning);

            entity.ToTable("Planning");

            entity.Property(e => e.IdPlanning).HasColumnName("id_planning");
            entity.Property(e => e.DescriptionPlanning)
                .HasMaxLength(50)
                .HasColumnName("description_planning");
            entity.Property(e => e.IdActivite).HasColumnName("id_activite");

            entity.HasOne(d => d.IdActiviteNavigation).WithMany(p => p.Plannings)
                .HasForeignKey(d => d.IdActivite)
                .HasConstraintName("FK_Planning_Activite");
        });

        modelBuilder.Entity<PossederBienProprietaire>(entity =>
        {
            entity.ToTable("PossederBienProprietaire");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AnneeAcquisition).HasColumnName("annee_acquisition");
            entity.Property(e => e.IdBien).HasColumnName("id_bien");
            entity.Property(e => e.IdPro).HasColumnName("id_pro");

            entity.HasOne(d => d.IdBienNavigation).WithMany(p => p.PossederBienProprietaires)
                .HasForeignKey(d => d.IdBien)
                .HasConstraintName("FK_PossederBienProprietaire_Bien");

            entity.HasOne(d => d.IdProNavigation).WithMany(p => p.PossederBienProprietaires)
                .HasForeignKey(d => d.IdPro)
                .HasConstraintName("FK_PossederBienProprietaire_Proprietaire");
        });

        modelBuilder.Entity<Proprietaire>(entity =>
        {
            entity.HasKey(e => e.IdPro);

            entity.ToTable("Proprietaire");

            entity.Property(e => e.IdPro)
                .ValueGeneratedOnAdd()
                .HasColumnName("id_pro");
            entity.Property(e => e.IdPersonnes).HasColumnName("id_personnes");
            entity.Property(e => e.TypePro)
                .HasMaxLength(50)
                .HasColumnName("type_pro");

            entity.HasOne(d => d.IdProNavigation).WithOne(p => p.Proprietaire)
                .HasForeignKey<Proprietaire>(d => d.IdPro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Proprietaire_Personnes");
        });

        modelBuilder.Entity<StatutBien>(entity =>
        {
            entity.HasKey(e => e.IdSta);

            entity.ToTable("StatutBien");

            entity.Property(e => e.IdSta).HasColumnName("id_sta");
            entity.Property(e => e.DateMiseJourSta).HasColumnName("date_mise_jour_sta");
            entity.Property(e => e.LibelleSta)
                .HasMaxLength(50)
                .HasColumnName("libelle_sta");
        });

        modelBuilder.Entity<TypeActivite>(entity =>
        {
            entity.HasKey(e => e.IdType);

            entity.ToTable("TypeActivite");

            entity.Property(e => e.IdType).HasColumnName("id_type");
            entity.Property(e => e.DescriptionType)
                .HasMaxLength(50)
                .HasColumnName("description_type");
        });

        modelBuilder.Entity<TypeBien>(entity =>
        {
            entity.HasKey(e => e.IdTy);

            entity.ToTable("TypeBien");

            entity.Property(e => e.IdTy).HasColumnName("id_ty");
            entity.Property(e => e.DescriptionType).HasColumnName("description_type");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
