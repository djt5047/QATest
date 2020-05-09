using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PlexureAPITest
{
    public interface IEntity
    {
        
    }

    public class UserEntity : IEntity
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string AccessToken { get; set; }
    }

    public class PurchaseEntity : IEntity
    {
     

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "points")]
        public int Points { get; set; }
    }

    public class PointsEntity : IEntity
    {
        public int UserId { get; set; }
        
        [JsonProperty(PropertyName = "Points", Required = Required.Always)]
        public int Value { get; set; }
    }
}
