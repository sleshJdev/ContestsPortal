using ContestsPortal.Domain.DataAccess.Providers.Interfaces;
using ContestsPortal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContestsPortal.Domain.DataAccess.Providers
{
    public class PostProvider : IPostProvider
    {
         #region Fields

        private readonly Func<PortalContext> _getContext;

        #endregion

        #region Constructors

        public PostProvider()
        {
        }

        public PostProvider(Func<PortalContext> context)
        {
            if (context == null) throw new ArgumentNullException("context");
            _getContext = context;
        }

        #endregion

        static Random gen = new Random();
        Func<DateTime> generator = () =>
        {
            DateTime start = new DateTime(1995, 1, 1);
            int range = ((TimeSpan)(DateTime.Today - start)).Days;
            return start.AddDays(gen.Next(range));
        };

        public IList<Post> GetAllPosts()
        {
                using (PortalContext context = _getContext())
                {
                    //return context.Posts.ToList();
                    return new[]
                    {
                      new Post(){PostContent = "Post 1", PublicationDate = generator()},
                      new Post(){PostContent = "Post 2", PublicationDate = generator()},
                      new Post(){PostContent = "Post 3", PublicationDate = generator()},
                      new Post(){PostContent = "Post 4", PublicationDate = generator()},
                      new Post(){PostContent = "Post 4", PublicationDate = generator()}
                    };
                }
        }

        public Post GetPost(int postId)
        {
                using (PortalContext context = _getContext())
                {
                    return context.Posts.Where(x => x.PostId.Equals(postId)).FirstOrDefault();
                }
        }
    }
}
