using System;
namespace yc_psql_demo.Models
{
    public class PostCreateRequestClass
    {
        public string Heading { get; set; }
        public string Text { get; set; }
    }

    public class PostUpdateRequestClass
    {
        public string Heading { get; set; }
        public string Text { get; set; }
    }
}
