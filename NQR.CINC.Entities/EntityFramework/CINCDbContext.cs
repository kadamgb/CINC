using NQR.CINC.Entities.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NQR.CINC.EntityFramework.Entities
{
    public class CINCEntities : DbContext
    {
        public CINCEntities() : base("CINCEntities") {
            Database.SetInitializer<CINCEntities>(new CreateDatabaseIfNotExists<CINCEntities>());
        }
        public DbSet<User> User { get; set; }
        //public override void Dispose()
        //{
        //    base.Dispose();
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //{
        //    var currentAppPath = Environment.CurrentDirectory;
        //    var applicationSettings = new ApplicationSettings(currentAppPath);
        //    var connectionString = applicationSettings.GetAppSettingVal("AutoGearDbContextConnection");
        //    options.UseSqlServer(connectionString).UseRowNumberForPaging();
        //}

        //public virtual DbSet<DeviceVendor> DeviceVendor { get; set; }
    }
}
