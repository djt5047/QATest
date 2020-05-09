using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace PlexureAPITest
{
    public class Response<T> where T : IEntity
    {
        public Response(HttpStatusCode statusCode, string error)
        {
            StatusCode = statusCode;
            Error = error;
        }

        public Response(HttpStatusCode statusCode, T entity)
        {
            StatusCode = statusCode;
            Entity = entity;
        }

        public string Error { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public T Entity { get; set; }

        // MARK - TestCase convenience

        public void Expect(HttpStatusCode statusCode)
        {
            Assert.AreEqual(StatusCode, statusCode);
        }
    }
}
