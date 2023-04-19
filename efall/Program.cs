using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Configuration;

namespace efall
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<BloggingContext, Configuration>);
            using (var db = new BloggingContext())
            {
                Console.WriteLine("Lütfen yeni bir blog ismi giriniz: ");
                // veri tabanına kayıt
                var name = Console.ReadLine();
                var blog = new Blog { Name = name}; 
                db.Blogs.Add(blog);
                db.SaveChanges();
                // veri tabanındakileri gösterme
                var query = from b in db.Blogs orderby b.Name select b;
                Console.WriteLine("Veritabanındaki tüm bloglar");
                foreach(var item in query)
                {
                    Console.WriteLine(item.Name);
                }
                Console.WriteLine("Çıkmak için bir tuşa basınız...");
                Console.ReadKey();
            }
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public virtual List<Post> Posts{ get; set; }

        public virtual BlogImage BlogImageId{ get; set; }
    }
    // post - tag arasında çoka çok ilişki
    public class Post
    {
        public int PostId { get; set;}
        public string Title { get; set; }
        public string Content { get; set; }
        public int BlogId { get; set; }
        public virtual List<Tag> PostTags { get; set; }
    }
    
    public class BlogImage
    {
        public int BlogImageId { get; set; }
        public byte[] Image { get; set; }
        public string Caption { get; set; }
        public virtual Blog Blog { get; set; }
    }

    public class Tag
    {
        public int TagId { get; set; }
        public virtual List<Post> PostTags { get; set; }
    }

    public class BloggingContext: DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>().HasOptional(e => e.BlogImageId).WithRequired(e => e.Blog);
        }

        public DbSet<Blog> Blogs { get; set;}
        public DbSet<Tag> Tags { get;set;}
        public DbSet<BlogImage> BlogImages { get; set;}
    }
}




