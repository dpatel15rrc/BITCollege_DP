namespace BITCollege_DP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class A4_model_changes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Courses", "CourseNumber", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Courses", "CourseNumber", c => c.String(nullable: false));
        }
    }
}
