using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XtremeMarbleBot
{

    public class Datum
    {
        public string id { get; set; }
        public string user_id { get; set; }
        public string game_id { get; set; }
        public List<string> community_ids { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public int viewer_count { get; set; }
        public DateTime started_at { get; set; }
        public string language { get; set; }
        public string thumbnail_url { get; set; }
    }

    public class Pagination
    {
        public string cursor { get; set; }
    }

    public class RootObject
    {
        public List<Datum> data { get; set; }
        public Pagination pagination { get; set; }
    }



    public class Datum2
    {
        public string id { get; set; }
        public string login { get; set; }
        public string display_name { get; set; }
        public string type { get; set; }
        public string broadcaster_type { get; set; }
        public string description { get; set; }
        public string profile_image_url { get; set; }
        public string offline_image_url { get; set; }
        public int view_count { get; set; }
        public string email { get; set; }
    }

    public class RootObject2
    {
        public List<Datum2> data { get; set; }
    }

}
