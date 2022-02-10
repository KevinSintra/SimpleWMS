namespace WmsAllModels.WmsModels
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.ComponentModel.DataAnnotations;

    public partial class WebWmsModel : DbContext
    {
        public WebWmsModel()
            : base("name=WebWmsModel")
        {
        }

        public DbSet<FangGoodsMast> FangGoodsMast { get; set; }

        public DbSet<GoodMastBarcode> GoodMastBarcode { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // �o�̥i�H��m�۩w�q�s�W��EF����SQL�ҫإ߸�ƪ��{��
            // �Y���Q�n��Model�W�����@��S�ʥi�Ѧ��B�s�W
            // ��z�Ѧ� "http://xpower2888.pixnet.net/blog/post/222817108-ef%E9%87%8Cguid%E9%A1%9E%E5%9E%8B%E6%95%B8%E6%93%9A%E7%9A%84%E8%87%AA%E5%A2%9E%E9%95%B7%E3%80%81%E6%99%82%E9%96%93%E6%88%B3%E5%92%8C%E5%BE%A9%E9%9B%9C%E9%A1%9E%E5%9E%8B"

            // ���� 1 to n �� n to n �ӨM�w��ƪ�W��
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            
            //modelBuilder.Entity<GoodMastBarcode>().HasRequired(x => x.GoodsMast).
            //    WithMany(x => x.GoodMastBarcode).HasForeignKey(x => x.PdCode);            
        }
    }
}
