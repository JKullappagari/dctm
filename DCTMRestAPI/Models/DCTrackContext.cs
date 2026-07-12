using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DCTMRestAPI.Models.Custom;

namespace DCTMRestAPI.Models
{
    public partial class DCTrackContext : DbContext
    {
        public virtual DbSet<TblAirFlowDirection> TblAirFlowDirection { get; set; }
        public virtual DbSet<TblApplCriticality> TblApplCriticality { get; set; }
        public virtual DbSet<TblApplication> TblApplication { get; set; }
        public virtual DbSet<TblApplicationMap> TblApplicationMap { get; set; }
        public virtual DbSet<TblApplType> TblApplType { get; set; }
        public virtual DbSet<TblAppStatus> TblAppStatus { get; set; }
        public virtual DbSet<TblAsset> TblAsset { get; set; }
        public virtual DbSet<TblAssetExcel> TblAssetExcel { get; set; }
        public virtual DbSet<TblAssetDrwdata> TblAssetDrwdata { get; set; }
        public virtual DbSet<TblAssetGroup> TblAssetGroup { get; set; }
        public virtual DbSet<TblAssetHostAssignment> TblAssetHostAssignment { get; set; }
        public virtual DbSet<TblAssetModel> TblAssetModel { get; set; }
        public virtual DbSet<TblAssetTransactionLog> TblAssetTransactionLog { get; set; }
        public virtual DbSet<TblAssetTransactionLogTagsSeen> TblAssetTransactionLogTagsSeen { get; set; }
        public virtual DbSet<TblAuditCycle> TblAuditCycle { get; set; }
        public virtual DbSet<TblAuditTrailSession> TblAuditTrailSession { get; set; }
        public virtual DbSet<TblBarredHistory> TblBarredHistory { get; set; }
        public virtual DbSet<TblBladeModelDetails> TblBladeModelDetails { get; set; }
        public virtual DbSet<TblBudivAssignment> TblBudivAssignment { get; set; }
        public virtual DbSet<TblBusinessUnit> TblBusinessUnit { get; set; }
        public virtual DbSet<TblBusiteAssignment> TblBusiteAssignment { get; set; }
        public virtual DbSet<TblCheckDelete> TblCheckDelete { get; set; }
        public virtual DbSet<TblCheckOutItems> TblCheckOutItems { get; set; }
        public virtual DbSet<TblCheckoutPurpose> TblCheckoutPurpose { get; set; }
        public virtual DbSet<TblCheckOutSession> TblCheckOutSession { get; set; }
        public virtual DbSet<TblCity> TblCity { get; set; }
        public virtual DbSet<TblCountry> TblCountry { get; set; }
        public virtual DbSet<TblDeviceData> TblDeviceData { get; set; }
        public virtual DbSet<TblDivision> TblDivision { get; set; }
        public virtual DbSet<TblEnclModelDetails> TblEnclModelDetails { get; set; }
        public virtual DbSet<TblEnclPositions> TblEnclPositions { get; set; }
        public virtual DbSet<TblEntityType> TblEntityType { get; set; }
        public virtual DbSet<TblExternalSync> TblExternalSync { get; set; }
        public virtual DbSet<TblExternalSysIntegrationLog> TblExternalSysIntegrationLog { get; set; }
        public virtual DbSet<TblExternalSysInterfaceErrors> TblExternalSysInterfaceErrors { get; set; }
        public virtual DbSet<TblExternalSysInterfaceSuccesses> TblExternalSysInterfaceSuccesses { get; set; }
        public virtual DbSet<TblGroup> TblGroup { get; set; }
        public virtual DbSet<TblGroupMember> TblGroupMember { get; set; }
        public virtual DbSet<TblGroupModuleRight> TblGroupModuleRight { get; set; }
        public virtual DbSet<TblHost> TblHost { get; set; }
        public virtual DbSet<TblHpstaging> TblHpstaging { get; set; }
        public virtual DbSet<TblImportObjects> TblImportObjects { get; set; }
        public virtual DbSet<TblInputConnectorType> TblInputConnectorType { get; set; }
        public virtual DbSet<TblLocation> TblLocation { get; set; }
        public virtual DbSet<TblLocationType> TblLocationType { get; set; }
        public virtual DbSet<TblMainModule> TblMainModule { get; set; }
        public virtual DbSet<TblManufacturer> TblManufacturer { get; set; }
        public virtual DbSet<TblMessage> TblMessage { get; set; }
        public virtual DbSet<TblMobileDevice> TblMobileDevice { get; set; }
        public virtual DbSet<TblModule> TblModule { get; set; }
        public virtual DbSet<TblModuleRight> TblModuleRight { get; set; }
        public virtual DbSet<TblMountType> TblMountType { get; set; }
        public virtual DbSet<TblMusterReason> TblMusterReason { get; set; }
        public virtual DbSet<TblOrientation> TblOrientation { get; set; }
        public virtual DbSet<TblOutputConnectorType> TblOutputConnectorType { get; set; }
        public virtual DbSet<TblOwner> TblOwner { get; set; }
        public virtual DbSet<TblOwnerDivisionAssignment> TblOwnerDivisionAssignment { get; set; }
        public virtual DbSet<TblPurpose> TblPurpose { get; set; }
        public virtual DbSet<TblRacklPositions> TblRacklPositions { get; set; }
        public virtual DbSet<TblRfidcard> TblRfidcard { get; set; }
        public virtual DbSet<TblRight> TblRight { get; set; }
        public virtual DbSet<TblSite> TblSite { get; set; }
        public virtual DbSet<TblSiteLocationAssignment> TblSiteLocationAssignment { get; set; }
        public virtual DbSet<TblSiteRestriction> TblSiteRestriction { get; set; }
        public virtual DbSet<TblSpcEqMaster> TblSpcEqMaster { get; set; }
        public virtual DbSet<TblStatusHistory> TblStatusHistory { get; set; }
        public virtual DbSet<TblStatusMaster> TblStatusMaster { get; set; }
        public virtual DbSet<TblStockTakeItems> TblStockTakeItems { get; set; }
        public virtual DbSet<TblStockTakeSession> TblStockTakeSession { get; set; }
        public virtual DbSet<TblTechCategory> TblTechCategory { get; set; }
        public virtual DbSet<TblTenant> TblTenant { get; set; }
        public virtual DbSet<TblTenantApplicationAssignment> TblTenantApplicationAssignment { get; set; }
        public virtual DbSet<TblTenantAssetAssignment> TblTenantAssetAssignment { get; set; }
        public virtual DbSet<TblTenantDivisionAssignment> TblTenantDivisionAssignment { get; set; }
        public virtual DbSet<TblTenantGroupAssignment> TblTenantGroupAssignment { get; set; }
        public virtual DbSet<TblTenantHostAssignment> TblTenantHostAssignment { get; set; }
        public virtual DbSet<TblTenantLocationAssignment> TblTenantLocationAssignment { get; set; }
        public virtual DbSet<TblTenantOwnerAssignment> TblTenantOwnerAssignment { get; set; }
        public virtual DbSet<TblTransactionTypes> TblTransactionTypes { get; set; }
        public virtual DbSet<TblUom> TblUom { get; set; }
        public virtual DbSet<TblUser> TblUser { get; set; }
        public virtual DbSet<TblUserBusinessUnit> TblUserBusinessUnit { get; set; }
        public virtual DbSet<TblUserPassword> TblUserPassword { get; set; }
        public virtual DbSet<TblWorldRegion> TblWorldRegion { get; set; }

        //custom
        public virtual DbSet<TblServerProperties> TblServerProperties { get; set; }

        public virtual DbSet<TblDeletedRows> TblDeletedRows { get; set; }


        public DCTrackContext(DbContextOptions<DCTrackContext> options)
    : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblAirFlowDirection>(entity =>
            {
                entity.ToTable("tblAirFlowDirection");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AirFlowDirection)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblApplCriticality>(entity =>
            {
                entity.HasKey(e => e.ApplCriticalityId);

                entity.ToTable("tblApplCriticality");

                entity.Property(e => e.ApplCriticalityId).HasColumnName("ApplCriticalityID");

                entity.Property(e => e.ApplCriticality)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ApplCriticalityDesc)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.BackColorCode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('((FFFFFF))')");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ForeColorCode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblApplication>(entity =>
            {
                entity.HasKey(e => e.ApplId);

                entity.ToTable("tblApplication");

                entity.Property(e => e.ApplId).HasColumnName("ApplID");

                entity.Property(e => e.AppStatusId)
                    .HasColumnName("AppStatusID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ApplCriticalityId).HasColumnName("ApplCriticalityID");

                entity.Property(e => e.ApplDesc)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ApplManageId).HasColumnName("ApplManageID");

                entity.Property(e => e.ApplName)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ApplTypeId).HasColumnName("ApplTypeID");

                entity.Property(e => e.Buid).HasColumnName("BUID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedBy).HasColumnName("lastModifiedBy");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.OwnerId).HasColumnName("OwnerID");

            });

            modelBuilder.Entity<TblApplicationMap>(entity =>
            {
                entity.HasKey(e => e.ApplMapId);

                entity.ToTable("tblApplicationMap");

                entity.Property(e => e.ApplMapId)
                    .HasColumnName("ApplMApID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.ApplId).HasColumnName("ApplID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

            });

            modelBuilder.Entity<TblApplType>(entity =>
            {
                entity.HasKey(e => e.ApplTypeId);

                entity.ToTable("tblApplType");

                entity.Property(e => e.ApplTypeId).HasColumnName("ApplTypeID");

                entity.Property(e => e.ApplType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ApplTypeDesc)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblAppStatus>(entity =>
            {
                entity.HasKey(e => e.AppStatusId);

                entity.ToTable("tblAppStatus");

                entity.Property(e => e.AppStatusId).HasColumnName("AppStatusID");

                entity.Property(e => e.AppStatus)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblAsset>(entity =>
            {
                entity.HasKey(e => e.AssetId);

                entity.ToTable("tblAsset");

                entity.Property(e => e.AssetId).HasColumnName("AssetID");

                entity.Property(e => e.AssetCreatedDate).HasColumnType("datetime");

                entity.Property(e => e.AssetGroupId).HasColumnName("AssetGroupID");

                entity.Property(e => e.AssetName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.BarredEndDate).HasColumnType("datetime");

                entity.Property(e => e.BarredStartDate).HasColumnType("datetime");

                entity.Property(e => e.BusinessUnitId).HasColumnName("BusinessUnitID");

                entity.Property(e => e.Cpu)
                    .HasColumnName("CPU")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Cpucore)
                    .HasColumnName("CPUCore")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Cpucount).HasColumnName("CPUCount");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CurrentOwnerId)
                    .HasColumnName("CurrentOwnerID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CurrentOwnerRfidbadge)
                    .HasColumnName("CurrentOwnerRFIDBadge")
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.CurrentRfidcardNumber)
                    .HasColumnName("CurrentRFIDCardNumber")
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.CurrentStatusId).HasColumnName("CurrentStatusID");

                entity.Property(e => e.DefaultLocationId).HasColumnName("DefaultLocationID");

                entity.Property(e => e.DeratedPower).HasDefaultValueSql("((0))");

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.ExternalId)
                    .HasColumnName("ExternalID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InternalId)
                    .HasColumnName("InternalID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsApproved).HasDefaultValueSql("((1))");

                entity.Property(e => e.IssuedDate).HasColumnType("datetime");

                entity.Property(e => e.LastGantryUpdatedTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LastSeenLocationId).HasColumnName("LastSeenLocationID");

                entity.Property(e => e.LastSeenLocationTime).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.ModelId).HasColumnName("ModelID");

                entity.Property(e => e.MusterReasonId).HasColumnName("MusterReasonID");

                entity.Property(e => e.NoOfRus).HasColumnName("NoOfRUs");

                entity.Property(e => e.Orientation)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Os)
                    .HasColumnName("OS")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ParentAssetId).HasColumnName("ParentAssetID");

                entity.Property(e => e.PrimarySiteId).HasColumnName("PrimarySiteID");

                entity.Property(e => e.RackorStand)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.ReceivedDate).HasColumnType("datetime");

                entity.Property(e => e.RefNumber)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RfidassignDate)
                    .HasColumnName("RFIDAssignDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.RfidupdatedDateTime)
                    .HasColumnName("RFIDUpdatedDateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.TechId).HasColumnName("TechID");

                entity.Property(e => e.WriteOffReasonId).HasColumnName("WriteOffReasonID");

            });

            modelBuilder.Entity<TblAssetExcel>(entity =>
            {
                entity.HasKey(e => e.AssetID);

                entity.Property(e => e.AssetID).HasColumnName("AssetID");

                entity.Property(e => e.SerialNo)
                   .IsRequired()
                   .HasMaxLength(100)
                   .IsUnicode(false);
                entity.Property(e => e.AssetType)
                   .IsRequired()
                   .HasMaxLength(25)
                   .IsUnicode(false);

                entity.Property(e => e.Manufacturer)
                     .HasMaxLength(100)
                     .IsUnicode(false);

                entity.Property(e => e.AssetModel)
                                    .HasMaxLength(100)
                                    .HasColumnName("Model")
                                    .IsUnicode(false);

                entity.Property(e => e.Assetname)
                   .HasMaxLength(250)
                   .IsUnicode(false);
                entity.Property(e => e.Mounttype)
                   .HasMaxLength(25)
                   .IsUnicode(false);
                entity.Property(e => e.StartRU)
                .HasColumnName("StartRU");

                entity.Property(e => e.EndRU).HasColumnName("EndRU");

                entity.Property(e => e.Orientation)
                   .HasMaxLength(25)
                   .IsUnicode(false);

                entity.Property(e => e.Site)
                                   .IsRequired()
                                   .HasMaxLength(25)
                                   .IsUnicode(false);

                entity.Property(e => e.Room)
                                  .IsRequired()
                                  .HasMaxLength(50)
                                  .IsUnicode(false);
                entity.Property(e => e.Row)
                                  .IsRequired()
                                  .HasMaxLength(50)
                                  .IsUnicode(false);
                entity.Property(e => e.Rack)
                                  .IsRequired()
                                  .HasMaxLength(50)
                                  .IsUnicode(false);
                entity.Property(e => e.RackTag)
                                   .HasColumnName("RackTag")
                                   .HasMaxLength(24)
                                   .IsUnicode(false);
                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                                    .IsRequired()
                                    .HasMaxLength(255)
                                    .IsUnicode(false);

                entity.Property(e => e.Region)
                                  .HasMaxLength(10)
                                  .IsUnicode(false);


                entity.Property(e => e.Tagno)
                                   .HasColumnName("TagNo")
                                   .HasMaxLength(24)
                                   .IsUnicode(false);

                entity.Property(e => e.Hostname)
                                    .IsRequired()
                                    .HasMaxLength(100)
                                    .IsUnicode(false);
                entity.Property(e => e.OS)
                    .HasColumnName("OS")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CPU)
                                   .HasColumnName("CPU")
                                   .HasMaxLength(50)
                                   .IsUnicode(false);

                entity.Property(e => e.CPUCore)
                    .HasColumnName("CPUCore")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CPUCount).HasColumnName("CPUCount");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");


                entity.Property(e => e.Creator)
                                    .HasColumnName("Creator")
                                   .IsRequired()
                                   .HasMaxLength(100)
                                   .IsUnicode(false);

                entity.Property(e => e.Custodian)
                    .HasColumnName("Custodian")
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);



            });

            modelBuilder.Entity<TblAssetDrwdata>(entity =>
            {
                entity.ToTable("tblAssetDRWData");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AssetId).HasColumnName("AssetID");

                entity.Property(e => e.Comments)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.ReasonId).HasColumnName("ReasonID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");
            });

            modelBuilder.Entity<TblAssetGroup>(entity =>
            {
                entity.HasKey(e => e.AssetGroupId);

                entity.ToTable("tblAssetGroup");

                entity.HasIndex(e => e.AssetGroup)
                    .HasDatabaseName("IX_tblDocumentPrimaryGroup")
                    .IsUnique();

                entity.Property(e => e.AssetGroupId).HasColumnName("AssetGroupID");

                entity.Property(e => e.AssetGroup)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblAssetHostAssignment>(entity =>
            {
                entity.ToTable("tblAssetHostAssignment");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.AssetId).HasColumnName("AssetID");

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.HostId).HasColumnName("HostID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("date");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblAssetModel>(entity =>
            {
                entity.HasKey(e => e.ModelId);

                entity.ToTable("tblAssetModel");

                entity.Property(e => e.ModelId).HasColumnName("ModelID");

                entity.Property(e => e.AirFlowDirectionId).HasColumnName("AirFlowDirectionID");

                entity.Property(e => e.AssetTypeId)
                    .HasColumnName("AssetTypeID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.BusinessUnitId).HasColumnName("BusinessUnitID");

                entity.Property(e => e.Comment)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ConnectorTypeDeviceSide)
                    .HasColumnName("ConnectorType_DeviceSide")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.ConnectorTypePduside)
                    .HasColumnName("ConnectorType_PDUSide")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DepthMm)
                    .HasColumnName("Depth_mm")
                    .HasDefaultValueSql("((0.0))");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.HeightMm)
                    .HasColumnName("Height_mm")
                    .HasDefaultValueSql("((0.0))");

                entity.Property(e => e.InternalDepthRack)
                    .HasColumnName("InternalDepth_Rack")
                    .HasDefaultValueSql("((0.0))");

                entity.Property(e => e.InternalHeightRack)
                    .HasColumnName("InternalHeight_Rack")
                    .HasDefaultValueSql("((0.0))");

                entity.Property(e => e.InternalWidthRack)
                    .HasColumnName("InternalWidth_Rack")
                    .HasDefaultValueSql("((0.0))");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.MaxPowerWatts)
                    .HasColumnName("MaxPower_Watts")
                    .HasDefaultValueSql("((0.0))");

                entity.Property(e => e.MfgId).HasColumnName("MfgID");

                entity.Property(e => e.ModelName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.MountTypeId).HasColumnName("MountTypeID");

                entity.Property(e => e.RequiredPsucount)
                    .HasColumnName("RequiredPSUCount")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Spcid).HasColumnName("SPCID");

                entity.Property(e => e.SteadyStateWatts)
                    .HasColumnName("SteadyState_Watts")
                    .HasDefaultValueSql("((0.0))");

                entity.Property(e => e.TechId).HasColumnName("TechID");

                entity.Property(e => e.TotalPsucount)
                    .HasColumnName("TotalPSUCount")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Uheight)
                    .HasColumnName("UHeight")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.WeightKg)
                    .HasColumnName("Weight_kg")
                    .HasDefaultValueSql("((0.0))");

                entity.Property(e => e.WidthMm)
                    .HasColumnName("Width_mm")
                    .HasDefaultValueSql("((0.0))");

                //entity.HasOne(d => d.BusinessUnit)
                //    .WithMany(p => p.TblAssetModel)
                //    .HasForeignKey(d => d.BusinessUnitId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblAssetModel_tblBusinessUnit");

                //entity.HasOne(d => d.Mfg)
                //    .WithMany(p => p.TblAssetModel)
                //    .HasForeignKey(d => d.MfgId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblAssetModel_tblManufacturer");
            });

            modelBuilder.Entity<TblAssetTransactionLog>(entity =>
            {
                entity.ToTable("tblAssetTransactionLog");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AssetGroupId).HasColumnName("AssetGroupID");

                entity.Property(e => e.AssetId).HasColumnName("AssetID");

                entity.Property(e => e.AssetName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.BarredEndDate).HasColumnType("datetime");

                entity.Property(e => e.BarredStartDate).HasColumnType("datetime");

                entity.Property(e => e.BusinessUnitId).HasColumnName("BusinessUnitID");

                entity.Property(e => e.CurrentOwnerId).HasColumnName("CurrentOwnerID");

                entity.Property(e => e.DeviceId).HasColumnName("DeviceID");

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.GantryLocation)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IssuedDate).HasColumnType("datetime");

                entity.Property(e => e.LastCheckOutDestinationId).HasColumnName("LastCheckOutDestinationID");

                entity.Property(e => e.LastMovedPurposeId).HasColumnName("LastMovedPurposeID");

                entity.Property(e => e.LastSeenLocationTime).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.ReceivedDate).HasColumnType("datetime");

                entity.Property(e => e.RefNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RfidcardNumber)
                    .HasColumnName("RFIDCardNumber")
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.SiteId).HasColumnName("SiteID");

                entity.Property(e => e.TransactionDate).HasColumnType("datetime");

                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            });

            modelBuilder.Entity<TblAssetTransactionLogTagsSeen>(entity =>
            {
                entity.HasKey(e => new { e.TransactionId, e.TagId });

                entity.ToTable("tblAssetTransactionLogTagsSeen");

                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");

                entity.Property(e => e.TagId)
                    .HasColumnName("TagID")
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EntityId).HasColumnName("EntityID");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblAuditCycle>(entity =>
            {
                entity.ToTable("tblAuditCycle");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblAuditTrailSession>(entity =>
            {
                entity.HasKey(e => e.AuditTrailSessionId);

                entity.ToTable("tblAuditTrailSession");

                entity.Property(e => e.AuditTrailSessionId).HasColumnName("AuditTrailSessionID");

                entity.Property(e => e.Ipaddress)
                    .HasColumnName("IPAddress")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.LogoffDate).HasColumnType("datetime");

                entity.Property(e => e.LogonDate).HasColumnType("datetime");

                entity.Property(e => e.SessionId)
                    .HasColumnName("SessionID")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<TblBarredHistory>(entity =>
            {
                entity.HasKey(e => e.BarredHistoryId);

                entity.ToTable("tblBarredHistory");

                entity.Property(e => e.BarredHistoryId).HasColumnName("BarredHistoryID");

                entity.Property(e => e.BarredPeriodEnd).HasColumnType("datetime");

                entity.Property(e => e.BarredPeriodStart).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.StatusHistoryRefId).HasColumnName("StatusHistoryRefID");
            });

            modelBuilder.Entity<TblBladeModelDetails>(entity =>
            {
                entity.ToTable("tblBladeModelDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BladeColumnCount).HasDefaultValueSql("((0))");

                entity.Property(e => e.BladeModelId).HasColumnName("BladeModelID");

                entity.Property(e => e.BladeRowCount).HasDefaultValueSql("((0))");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblBudivAssignment>(entity =>
            {
                entity.HasKey(e => e.BudivAssignmentId);

                entity.ToTable("tblBUDivAssignment");

                entity.Property(e => e.BudivAssignmentId).HasColumnName("BUDivAssignmentID");

                entity.Property(e => e.BusinessUnitId).HasColumnName("BusinessUnitID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DivisionId).HasColumnName("DivisionID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                //entity.HasOne(d => d.BusinessUnit)
                //    .WithMany(p => p.TblBudivAssignment)
                //    .HasForeignKey(d => d.BusinessUnitId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblBUDivAssignment_tblBusinessUnit");

                //entity.HasOne(d => d.Division)
                //    .WithMany(p => p.TblBudivAssignment)
                //    .HasForeignKey(d => d.DivisionId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblBUDivAssignment_tblDivision");
            });

            modelBuilder.Entity<TblBusinessUnit>(entity =>
            {
                entity.HasKey(e => e.BusinessUnitId);

                entity.ToTable("tblBusinessUnit");

                entity.HasIndex(e => e.BusinessUnit)
                    .HasDatabaseName("IX_tblBusinessUnit")
                    .IsUnique();

                entity.Property(e => e.BusinessUnitId).HasColumnName("BusinessUnitID");

                entity.Property(e => e.BusinessUnit)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.CoPrefix)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<TblBusiteAssignment>(entity =>
            {
                entity.HasKey(e => e.BusiteAssignmentId);

                entity.ToTable("tblBUSiteAssignment");

                entity.HasIndex(e => new { e.BusinessUnitId, e.SiteId })
                    .HasDatabaseName("IX_tblBUSiteAssignment")
                    .IsUnique();

                entity.Property(e => e.BusiteAssignmentId).HasColumnName("BUSiteAssignmentID");

                entity.Property(e => e.BusinessUnitId).HasColumnName("BusinessUnitID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.SiteId).HasColumnName("SiteID");

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                //entity.HasOne(d => d.BusinessUnit)
                //    .WithMany(p => p.TblBusiteAssignment)
                //    .HasForeignKey(d => d.BusinessUnitId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblSiteBUAssignment_tblBusinessUnit");

                //entity.HasOne(d => d.Site)
                //    .WithMany(p => p.TblBusiteAssignment)
                //    .HasForeignKey(d => d.SiteId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblBUSiteAssignment_tblSite");
            });

            modelBuilder.Entity<TblCheckDelete>(entity =>
            {
                entity.HasKey(e => e.ColumnTableId);

                entity.ToTable("tblCheckDelete");

                entity.Property(e => e.ColumnTableId).HasColumnName("ColumnTableID");

                entity.Property(e => e.ColumnName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ColumnNameAlias)
                    .HasColumnName("ColumnName_Alias")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblCheckOutItems>(entity =>
            {
                entity.HasKey(e => new { e.CheckOutSessionId, e.CheckOutAssetId });

                entity.ToTable("tblCheckOutItems");

                entity.Property(e => e.DestinationLocationId).HasColumnName("DestinationLocationID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IsRfidCheckOut).HasDefaultValueSql("((1))");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.PurposeId).HasColumnName("PurposeID");
            });

            modelBuilder.Entity<TblCheckoutPurpose>(entity =>
            {
                entity.ToTable("tblCheckoutPurpose");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CheckoutPurposeId).HasColumnName("CheckoutPurposeID");

                entity.Property(e => e.CheckoutSessionId).HasColumnName("CheckoutSessionID");

                entity.Property(e => e.CinItems).HasColumnName("CInItems");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.TotalCoutItems).HasColumnName("TotalCOutItems");
            });

            modelBuilder.Entity<TblCheckOutSession>(entity =>
            {
                entity.ToTable("tblCheckOutSession");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CheckOutDateTime).HasColumnType("datetime");

                entity.Property(e => e.CheckOutSessionId).ValueGeneratedOnAdd();

                entity.Property(e => e.CheckOutWorkflowId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblCity>(entity =>
            {
                entity.HasKey(e => e.CityId);

                entity.ToTable("tblCity");

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.CityName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblCountry>(entity =>
            {
                entity.HasKey(e => e.CountryId);

                entity.ToTable("tblCountry");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.CountryCode)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.Region)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Uomid).HasColumnName("UOMID");

                entity.Property(e => e.WorldRegionId).HasColumnName("WorldRegionID");
            });

            modelBuilder.Entity<TblDeviceData>(entity =>
            {
                entity.HasKey(e => e.Sno);

                entity.ToTable("tblDeviceData");

                entity.Property(e => e.DeviceId)
                    .IsRequired()
                    .HasColumnName("DeviceID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstInstallDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.LatestInstallDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblDivision>(entity =>
            {
                entity.HasKey(e => e.DivisionId);

                entity.ToTable("tblDivision");

                entity.Property(e => e.DivisionId).HasColumnName("DivisionID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Division)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DivisionDesc)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblEnclModelDetails>(entity =>
            {
                entity.ToTable("tblEnclModelDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EnclFrontColumnCount).HasDefaultValueSql("((0))");

                entity.Property(e => e.EnclFrontRowCount).HasDefaultValueSql("((0))");

                entity.Property(e => e.EnclModelId).HasColumnName("EnclModelID");

                entity.Property(e => e.EnclRearColumnCount).HasDefaultValueSql("((0))");

                entity.Property(e => e.EnclRearRowCount).HasDefaultValueSql("((0))");

                entity.Property(e => e.FrontChildCount).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.RearChildCount).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblEnclPositions>(entity =>
            {
                entity.ToTable("tblEnclPositions");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EnclId).HasColumnName("EnclID");

                entity.Property(e => e.FrontPositions)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.RearPositions)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblEntityType>(entity =>
            {
                entity.HasKey(e => e.EntityTypeId);

                entity.ToTable("tblEntityType");

                entity.HasIndex(e => e.EntityType)
                    .HasDatabaseName("IX_tblEntityType")
                    .IsUnique();

                entity.Property(e => e.EntityTypeId)
                    .HasColumnName("EntityTypeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.EntityType)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblExternalSync>(entity =>
            {
                entity.HasKey(e => e.ExternalSyncId);

                entity.ToTable("tblExternalSync");

                entity.Property(e => e.ExternalSyncId).HasColumnName("ExternalSyncID");

                entity.Property(e => e.ExternalSystemName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastSyncDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblExternalSysIntegrationLog>(entity =>
            {
                entity.ToTable("tblExternalSysIntegrationLog");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ExternalSystem)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.ProcessedDate).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionType)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblExternalSysInterfaceErrors>(entity =>
            {
                entity.HasKey(e => e.ErrorId);

                entity.ToTable("tblExternalSysInterfaceErrors");

                entity.Property(e => e.ErrorId).HasColumnName("ErrorID");

                entity.Property(e => e.FailureReason)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ImportObjectId).HasColumnName("ImportObjectID");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.ProcessedDate).HasColumnType("datetime");

                entity.Property(e => e.RowData)
                    .HasMaxLength(600)
                    .IsUnicode(false);

                //entity.HasOne(d => d.IdNavigation)
                //    .WithMany(p => p.TblExternalSysInterfaceErrors)
                //    .HasForeignKey(d => d.Id)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblExternalSysInterfaceErrors_tblExternalSysIntegrationLog");

                //entity.HasOne(d => d.ImportObject)
                //    .WithMany(p => p.TblExternalSysInterfaceErrors)
                //    .HasForeignKey(d => d.ImportObjectId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblExternalSysInterfaceErrors_tblImportObjects");
            });

            modelBuilder.Entity<TblExternalSysInterfaceSuccesses>(entity =>
            {
                entity.HasKey(e => e.SuccessId);

                entity.ToTable("tblExternalSysInterfaceSuccesses");

                entity.Property(e => e.SuccessId).HasColumnName("SuccessID");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ImportObjectId).HasColumnName("ImportObjectID");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.ProcessedDate).HasColumnType("datetime");

                entity.Property(e => e.RowData)
                    .HasMaxLength(600)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                //entity.HasOne(d => d.IdNavigation)
                //    .WithMany(p => p.TblExternalSysInterfaceSuccesses)
                //    .HasForeignKey(d => d.Id)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblExternalSysInterfaceSuccesses_tblExternalSysIntegrationLog");

                //entity.HasOne(d => d.ImportObject)
                //    .WithMany(p => p.TblExternalSysInterfaceSuccesses)
                //    .HasForeignKey(d => d.ImportObjectId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblExternalSysInterfaceSuccesses_tblExternalSysInterfaceSuccesses");
            });

            modelBuilder.Entity<TblGroup>(entity =>
            {
                entity.HasKey(e => e.GroupId);

                entity.ToTable("tblGroup");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Group)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<TblGroupMember>(entity =>
            {
                entity.HasKey(e => e.GroupMemberId);

                entity.ToTable("tblGroupMember");

                entity.Property(e => e.GroupMemberId).HasColumnName("GroupMemberID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                //entity.HasOne(d => d.Group)
                //    .WithMany(p => p.TblGroupMember)
                //    .HasForeignKey(d => d.GroupId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblSecurityGroupMember_tblSecurityGroup");

                //entity.HasOne(d => d.User)
                //    .WithMany(p => p.TblGroupMember)
                //    .HasForeignKey(d => d.UserId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblSecurityGroupMember_tblSecurityUser");
            });

            modelBuilder.Entity<TblGroupModuleRight>(entity =>
            {
                entity.HasKey(e => e.GroupModuleRightId);

                entity.ToTable("tblGroupModuleRight");

                entity.Property(e => e.GroupModuleRightId).HasColumnName("GroupModuleRightID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.RightModuleId).HasColumnName("RightModuleID");

                //entity.HasOne(d => d.Group)
                //    .WithMany(p => p.TblGroupModuleRight)
                //    .HasForeignKey(d => d.GroupId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblGroupModuleRight_tblGroup");

                //entity.HasOne(d => d.RightModule)
                //    .WithMany(p => p.TblGroupModuleRight)
                //    .HasForeignKey(d => d.RightModuleId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblGroupModuleRight_tblModuleRight");
            });

            modelBuilder.Entity<TblHost>(entity =>
            {
                entity.HasKey(e => e.HostId);

                entity.ToTable("tblHost");

                entity.HasIndex(e => e.HostName)
                    .HasDatabaseName("IX_tblHost")
                    .IsUnique();

                entity.Property(e => e.HostId)
                    .HasColumnName("HostID")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.HostName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedDate).HasColumnType("date");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblHpstaging>(entity =>
            {
                entity.HasKey(e => e.AssetId);

                entity.ToTable("tblHPStaging");

                entity.Property(e => e.AssetId)
                    .HasColumnName("AssetID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AssetGroup)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AssetName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CityName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CountryName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CurrentRfidcardNumber)
                    .HasColumnName("CurrentRFIDCardNumber")
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.FloorNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.MfgName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModelName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Os)
                    .HasColumnName("OS")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ParentAssetId).HasColumnName("ParentAssetID");

                entity.Property(e => e.Rack)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RefNumber)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Region)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Room)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Row)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SiteName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblImportObjects>(entity =>
            {
                entity.HasKey(e => e.ImportObjectId);

                entity.ToTable("tblImportObjects");

                entity.Property(e => e.ImportObjectId).HasColumnName("ImportObjectID");

                entity.Property(e => e.ImportObject)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblInputConnectorType>(entity =>
            {
                entity.HasKey(e => e.InputConnectorTypeId);

                entity.ToTable("tblInputConnectorType");

                entity.Property(e => e.InputConnectorTypeId).HasColumnName("InputConnectorTypeID");

                entity.Property(e => e.InputConnectorType)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblLocation>(entity =>
            {
                entity.HasKey(e => e.LocationId);

                entity.ToTable("tblLocation");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ExternalId)
                    .HasColumnName("ExternalID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FloorNo)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.IpAddress)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LocationTypeId).HasColumnName("LocationTypeID");

                entity.Property(e => e.Manufacturer)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Model)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ModelId).HasColumnName("ModelID");

                entity.Property(e => e.ParentLocationId).HasColumnName("ParentLocationID");

                entity.Property(e => e.SerialNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.TagId)
                    .HasColumnName("TagID")
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.Uheight)
                    .HasColumnName("UHeight")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblLocationType>(entity =>
            {
                entity.HasKey(e => e.LocationTypeId);

                entity.ToTable("tblLocationType");

                entity.HasIndex(e => e.LocationType)
                    .HasDatabaseName("IX_tblLocationType")
                    .IsUnique();

                entity.Property(e => e.LocationTypeId).HasColumnName("LocationTypeID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IsRfidlocation).HasColumnName("IsRFIDLocation");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.LocationType)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<TblMainModule>(entity =>
            {
                entity.HasKey(e => e.MainModuleId);

                entity.ToTable("tblMainModule");

                entity.Property(e => e.MainModuleId)
                    .HasColumnName("MainModuleID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.MainModule)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ModuleType).HasDefaultValueSql("((1))");

                entity.Property(e => e.PageUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblManufacturer>(entity =>
            {
                entity.HasKey(e => e.MfgId);

                entity.ToTable("tblManufacturer");

                entity.Property(e => e.MfgId).HasColumnName("MfgID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.MfgName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblMessage>(entity =>
            {
                entity.HasKey(e => e.MessageCodeId);

                entity.ToTable("tblMessage");

                entity.Property(e => e.MessageCodeId).HasColumnName("MessageCodeID");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MessageCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblMobileDevice>(entity =>
            {
                entity.ToTable("tblMobileDevice");

                entity.HasIndex(e => e.DeviceId)
                    .HasDatabaseName("IX_tblMobileDevice")
                    .IsUnique();

                entity.HasIndex(e => e.DeviceName)
                    .HasDatabaseName("IX_DEVICENAME")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeviceId)
                    .IsRequired()
                    .HasColumnName("DeviceID")
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.DeviceName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.ShortCode)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.SiteId).HasColumnName("SiteID");

                //entity.HasOne(d => d.Site)
                //    .WithMany(p => p.TblMobileDevice)
                //    .HasForeignKey(d => d.SiteId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblMobileDevice_tblSite");
            });

            modelBuilder.Entity<TblModule>(entity =>
            {
                entity.HasKey(e => e.ModuleId);

                entity.ToTable("tblModule");

                entity.Property(e => e.ModuleId).HasColumnName("ModuleID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnType("nchar(250)");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.MainModuleId).HasColumnName("MainModuleID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Module)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.PageUrl)
                    .HasColumnName("PageURL")
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblModuleRight>(entity =>
            {
                entity.HasKey(e => e.RightModuleId);

                entity.ToTable("tblModuleRight");

                entity.Property(e => e.RightModuleId).HasColumnName("RightModuleID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.KeyValue)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ModuleId).HasColumnName("ModuleID");

                entity.Property(e => e.RightId).HasColumnName("RightID");

                entity.Property(e => e.RightType)
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.TabDisplay)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                //entity.HasOne(d => d.Module)
                //    .WithMany(p => p.TblModuleRight)
                //    .HasForeignKey(d => d.ModuleId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblModuleRight_tblModule");

                //entity.HasOne(d => d.Right)
                //    .WithMany(p => p.TblModuleRight)
                //    .HasForeignKey(d => d.RightId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblModuleRight_tblRight");
            });

            modelBuilder.Entity<TblMountType>(entity =>
            {
                entity.HasKey(e => e.Mounttypeid);

                entity.ToTable("tblMountType");

                entity.Property(e => e.Mounttypeid).HasColumnName("MOUNTTYPEID");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.Mounttype)
                    .IsRequired()
                    .HasColumnName("MOUNTTYPE")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblMusterReason>(entity =>
            {
                entity.HasKey(e => e.MusterReasonId);

                entity.ToTable("tblMusterReason");

                entity.HasIndex(e => e.MusterReason)
                    .HasDatabaseName("IX_tblMusterReason")
                    .IsUnique();

                entity.Property(e => e.MusterReasonId).HasColumnName("MusterReasonID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.MusterReason)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblOrientation>(entity =>
            {
                entity.HasKey(e => e.OrientationId);

                entity.ToTable("tblOrientation");

                entity.Property(e => e.OrientationId).HasColumnName("OrientationID");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.OrientationName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblOutputConnectorType>(entity =>
            {
                entity.HasKey(e => e.OutputConnectorTypeId);

                entity.ToTable("tblOutputConnectorType");

                entity.Property(e => e.OutputConnectorTypeId).HasColumnName("OutputConnectorTypeID");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.OutputConnectorType)
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblOwner>(entity =>
            {
                entity.HasKey(e => e.OwnerId);

                entity.ToTable("tblOwner");

                entity.Property(e => e.OwnerId).HasColumnName("OwnerID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.OwnerFirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OwnerLastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblOwnerDivisionAssignment>(entity =>
            {
                entity.HasKey(e => e.OwnerDivId);

                entity.ToTable("tblOwnerDivisionAssignment");

                entity.Property(e => e.OwnerDivId).HasColumnName("OwnerDivID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DivisionId).HasColumnName("DivisionID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.OwnerId).HasColumnName("OwnerID");
            });

            modelBuilder.Entity<TblPurpose>(entity =>
            {
                entity.HasKey(e => e.PurposeId);

                entity.ToTable("tblPurpose");

                entity.HasIndex(e => e.Purpose)
                    .HasDatabaseName("IX_tblPurpose")
                    .IsUnique();

                entity.Property(e => e.PurposeId).HasColumnName("PurposeID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.Purpose)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<TblRacklPositions>(entity =>
            {
                entity.ToTable("tblRacklPositions");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.FrontPositions)
                    .HasMaxLength(68)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.RackId).HasColumnName("RackID");

                entity.Property(e => e.RearPositions)
                    .HasMaxLength(68)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblRfidcard>(entity =>
            {
                entity.HasKey(e => e.AutoId);

                entity.ToTable("tblRFIDCard");

                entity.Property(e => e.AutoId).HasColumnName("AutoID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedBy).HasDefaultValueSql("((1))");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.RfidcardNumber)
                    .HasColumnName("RFIDCardNumber")
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.RfidcardType)
                    .HasColumnName("RFIDCardType")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");
            });

            modelBuilder.Entity<TblRight>(entity =>
            {
                entity.HasKey(e => e.RightsId);

                entity.ToTable("tblRight");

                entity.Property(e => e.RightsId).HasColumnName("RightsID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.Rights)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<TblSite>(entity =>
            {
                entity.HasKey(e => e.SiteId);

                entity.ToTable("tblSite");

                entity.HasIndex(e => e.Site)
                    .HasDatabaseName("IX_tblSite")
                    .IsUnique();

                entity.Property(e => e.SiteId).HasColumnName("SiteID");

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.Site)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.Url)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblSiteLocationAssignment>(entity =>
            {
                entity.HasKey(e => e.SiteLocationAssignmentId);

                entity.ToTable("tblSiteLocationAssignment");

                entity.HasIndex(e => new { e.SiteId, e.LocationId })
                    .HasDatabaseName("IX_tblSiteLocAssignment")
                    .IsUnique();

                entity.Property(e => e.SiteLocationAssignmentId).HasColumnName("SiteLocationAssignmentID");

                entity.Property(e => e.BusinessUnitId).HasColumnName("BusinessUnitID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.SiteId).HasColumnName("SiteID");

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                //entity.HasOne(d => d.BusinessUnit)
                //    .WithMany(p => p.TblSiteLocationAssignment)
                //    .HasForeignKey(d => d.BusinessUnitId)
                //    .HasConstraintName("FK_tblSiteLocationAssignment_tblBusinessUnit");

                //entity.HasOne(d => d.Location)
                //    .WithMany(p => p.TblSiteLocationAssignment)
                //    .HasForeignKey(d => d.LocationId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblSiteLocationAssignment_tblLocation");
            });

            modelBuilder.Entity<TblSiteRestriction>(entity =>
            {
                entity.HasKey(e => e.SiteRestrictionsId);

                entity.ToTable("tblSiteRestriction");

                entity.Property(e => e.SiteRestrictionsId).HasColumnName("SiteRestrictionsID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.SiteId).HasColumnName("SiteID");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<TblSpcEqMaster>(entity =>
            {
                entity.HasKey(e => e.SpcId);

                entity.ToTable("tblSpcEqMaster");

                entity.Property(e => e.SpcId).HasColumnName("SpcID");

                entity.Property(e => e.DepthInches).HasColumnName("Depth_Inches");

                entity.Property(e => e.DepthMm).HasColumnName("Depth_MM");

                entity.Property(e => e.Empty2).HasMaxLength(255);

                entity.Property(e => e.HeightInches).HasColumnName("Height_Inches");

                entity.Property(e => e.HeightMm).HasColumnName("Height_MM");

                entity.Property(e => e.ItemType).HasMaxLength(255);

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.MakeModel).HasMaxLength(255);

                entity.Property(e => e.NewRowRefId).HasColumnName("NewRowRefID");

                entity.Property(e => e.Notes1)
                    .HasColumnName("Notes_1")
                    .HasMaxLength(500);

                entity.Property(e => e.Notes2)
                    .HasColumnName("Notes_2")
                    .HasMaxLength(500);

                entity.Property(e => e.Path).HasMaxLength(255);

                entity.Property(e => e.ProductNumber).HasMaxLength(255);

                entity.Property(e => e.SourceFile).HasMaxLength(255);

                entity.Property(e => e.SqftStandalone).HasColumnName("SQFT_Standalone");

                entity.Property(e => e.SqmetreStandalone).HasColumnName("SQMetre_Standalone");

                entity.Property(e => e.WeightKg).HasColumnName("Weight_KG");

                entity.Property(e => e.WeightLb).HasColumnName("Weight_LB");

                entity.Property(e => e.WidthInches).HasColumnName("Width_Inches");

                entity.Property(e => e.WidthMm).HasColumnName("Width_MM");
            });

            modelBuilder.Entity<TblStatusHistory>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.StatusHistoryId });

                entity.ToTable("tblStatusHistory");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.StatusHistoryId)
                    .HasColumnName("StatusHistoryID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Comments)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeviceId).HasColumnName("DeviceID");

                entity.Property(e => e.EntityId).HasColumnName("EntityID");

                entity.Property(e => e.EntityTypeId).HasColumnName("EntityTypeID");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.RefId)
                    .HasColumnName("RefID")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StatusDate).HasColumnType("datetime");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.LocationID).HasColumnName("LocationID");

                //entity.HasOne(d => d.EntityType)
                //    .WithMany(p => p.TblStatusHistory)
                //    .HasForeignKey(d => d.EntityTypeId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblStatusHistory_tblEntityType");

                //entity.HasOne(d => d.Status)
                //    .WithMany(p => p.TblStatusHistory)
                //    .HasForeignKey(d => d.StatusId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblStatusHistory_tblStatusMaster");
            });

            modelBuilder.Entity<TblStatusMaster>(entity =>
            {
                entity.HasKey(e => e.StatusId);

                entity.ToTable("tblStatusMaster");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.Action)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.EntityTypeId).HasColumnName("EntityTypeID");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.RefIdremarks)
                    .HasColumnName("RefIDRemarks")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sqlcondition)
                    .HasColumnName("SQLCOndition")
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                //entity.HasOne(d => d.EntityType)
                //    .WithMany(p => p.TblStatusMaster)
                //    .HasForeignKey(d => d.EntityTypeId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblStatusMaster_tblEntityType");
            });

            modelBuilder.Entity<TblStockTakeItems>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.StockTakeSessionId, e.AssetId });

                entity.ToTable("tblStockTakeItems");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblStockTakeSession>(entity =>
            {
                entity.ToTable("tblStockTakeSession");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.DeviceId).HasColumnName("DeviceID");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.StockTakeDateTime).HasColumnType("datetime");

                entity.Property(e => e.StockTakeSessionId).ValueGeneratedOnAdd();

                entity.Property(e => e.WorkflowInstanceId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblTechCategory>(entity =>
            {
                entity.HasKey(e => e.TechId);

                entity.ToTable("tblTechCategory");

                entity.Property(e => e.TechId).HasColumnName("TechID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.TechName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblTenant>(entity =>
            {
                entity.HasKey(e => e.TenantId);

                entity.ToTable("tblTenant");

                entity.Property(e => e.ContactEmail)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.ContactFirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ContactLastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.TenantFullName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.TenantShortName)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.TenantType)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblTenantApplicationAssignment>(entity =>
            {
                entity.HasKey(e => e.TenantApplicationId);

                entity.ToTable("tblTenantApplicationAssignment");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                //entity.HasOne(d => d.Application)
                //    .WithMany(p => p.TblTenantApplicationAssignment)
                //    .HasForeignKey(d => d.ApplicationId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblTenantApplicationAssignment_tblApplication");
            });

            modelBuilder.Entity<TblTenantAssetAssignment>(entity =>
            {
                entity.HasKey(e => e.TenantAssetId);

                entity.ToTable("tblTenantAssetAssignment");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                //entity.HasOne(d => d.Asset)
                //    .WithMany(p => p.TblTenantAssetAssignment)
                //    .HasForeignKey(d => d.AssetId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblTenantAssetAssignment_tblAsset");
            });

            modelBuilder.Entity<TblTenantDivisionAssignment>(entity =>
            {
                entity.HasKey(e => e.TenantDivisionId);

                entity.ToTable("tblTenantDivisionAssignment");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                //entity.HasOne(d => d.Division)
                //    .WithMany(p => p.TblTenantDivisionAssignment)
                //    .HasForeignKey(d => d.DivisionId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblTenantDivisionAssignment_tblDivision");
            });

            modelBuilder.Entity<TblTenantGroupAssignment>(entity =>
            {
                entity.HasKey(e => e.TenantGroupId);

                entity.ToTable("tblTenantGroupAssignment");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                //entity.HasOne(d => d.Group)
                //    .WithMany(p => p.TblTenantGroupAssignment)
                //    .HasForeignKey(d => d.GroupId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblTenantGroupAssignment_tblGroup");
            });

            modelBuilder.Entity<TblTenantHostAssignment>(entity =>
            {
                entity.HasKey(e => e.TenantHostId);

                entity.ToTable("tblTenantHostAssignment");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                //entity.HasOne(d => d.Host)
                //    .WithMany(p => p.TblTenantHostAssignment)
                //    .HasForeignKey(d => d.HostId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblTenantHostAssignment_tblHost");
            });

            modelBuilder.Entity<TblTenantLocationAssignment>(entity =>
            {
                entity.HasKey(e => e.TenantLocationId);

                entity.ToTable("tblTenantLocationAssignment");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                //entity.HasOne(d => d.Location)
                //    .WithMany(p => p.TblTenantLocationAssignment)
                //    .HasForeignKey(d => d.LocationId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblTenantLocationAssignment_tblLocation");
            });

            modelBuilder.Entity<TblTenantOwnerAssignment>(entity =>
            {
                entity.HasKey(e => e.TenantOwnerId);

                entity.ToTable("tblTenantOwnerAssignment");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                //entity.HasOne(d => d.Owner)
                //    .WithMany(p => p.TblTenantOwnerAssignment)
                //    .HasForeignKey(d => d.OwnerId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblTenantOwnerAssignment_tblOwner");
            });


            modelBuilder.Entity<TblTransactionTypes>(entity =>
            {
                entity.HasKey(e => e.TransTypeId);

                entity.ToTable("tblTransactionTypes");

                entity.Property(e => e.TransTypeId).HasColumnName("TransTypeID");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.RefIdremarks)
                    .HasColumnName("RefIDRemarks")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransTypeCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblUom>(entity =>
            {
                entity.ToTable("tblUOM");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.Uomfrom)
                    .IsRequired()
                    .HasColumnName("UOMFrom")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Uomto)
                    .IsRequired()
                    .HasColumnName("UOMTo")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblUser>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("tblUser");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CurrentRfidbadge)
                    .HasColumnName("CurrentRFIDBadge")
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.DefaultBu).HasColumnName("DefaultBU");

                entity.Property(e => e.DisplayName)
                    .HasMaxLength(102)
                    .IsUnicode(false)
                    .HasComputedColumnSql("(([FirstName]+', ')+[LastName])");

                entity.Property(e => e.Email)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.LoginName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password).IsRequired();

                entity.Property(e => e.RfidassignDate)
                    .HasColumnName("RFIDAssignDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<TblUserBusinessUnit>(entity =>
            {
                entity.HasKey(e => e.UserBusinessUnitId);

                entity.ToTable("tblUserBusinessUnit");

                entity.Property(e => e.UserBusinessUnitId).HasColumnName("UserBusinessUnitID");

                entity.Property(e => e.BusinessUnitId).HasColumnName("BusinessUnitID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                //entity.HasOne(d => d.BusinessUnit)
                //    .WithMany(p => p.TblUserBusinessUnit)
                //    .HasForeignKey(d => d.BusinessUnitId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblUserBusinessUnit_tblBusinessUnit");
            });

            modelBuilder.Entity<TblUserPassword>(entity =>
            {
                entity.HasKey(e => e.UserPasswordId);

                entity.ToTable("tblUserPassword");

                entity.Property(e => e.UserPasswordId).HasColumnName("UserPasswordID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                //entity.HasOne(d => d.User)
                //    .WithMany(p => p.TblUserPassword)
                //    .HasForeignKey(d => d.UserId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_tblUserPassword_tblSecurityUser");
            });

            modelBuilder.Entity<TblWorldRegion>(entity =>
            {
                entity.HasKey(e => e.WorldRegionId);

                entity.ToTable("tblWorldRegion");

                entity.Property(e => e.WorldRegionId)
                    .HasColumnName("WorldRegionID")
                    .ValueGeneratedNever();

                entity.Property(e => e.LastUpdatedTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.WorldRegion)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblServerProperties>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ServerDateTimeUtc)
                    .HasColumnName("serverdatetimeutc")
                    .HasColumnType("datetime");

                entity.Property(e => e.ServerDateTimeLocal)
                    .HasColumnName("serverdatetimelocal")
                    .HasColumnType("datetime");

            });

            modelBuilder.Entity<TblDeletedRows>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TableName)
                    .HasColumnName("tableName");

                entity.Property(e => e.PrimaryId)
                    .HasColumnName("primaryId");

                entity.Property(e => e.SecondaryId)
                    .HasColumnName("secondaryId");

                entity.Property(e => e.DeletedTime)
                    .HasColumnName("deletedTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedUserId)
                    .HasColumnName("updatedUserId");

                entity.Property(e => e.LastUpdatedTime)
                    .HasColumnName("LastUpdatedTime")
                    .HasColumnType("long");



            });
        }
    }
}
