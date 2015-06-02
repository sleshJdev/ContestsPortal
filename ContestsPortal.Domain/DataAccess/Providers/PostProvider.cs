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
        static Func<DateTime> generator = () =>
        {
            DateTime start = new DateTime(1995, 1, 1);
            int range = ((TimeSpan)(DateTime.Today - start)).Days;
            return start.AddDays(gen.Next(range));
        };

        static Post[] posts = new[]
        {
            new Post(){PostId = 1, PostTitle = "Title post 1", PostContent = "Very very very big post 1", PublicationDate = generator()},
            new Post(){PostId = 2, PostTitle = "Title post 2", PostContent = "Very very very big post 2", PublicationDate = generator()},
            new Post(){PostId = 3, PostTitle = "Title post 3", PostContent = "Very very very big post 3", PublicationDate = generator()},
            new Post(){PostId = 4, PostTitle = "Title post 4", PostContent = "Very very very big post 4", PublicationDate = generator()},
            new Post(){PostId = 5, PostTitle = "Title post 5", PostContent = "Very very very big post 4", PublicationDate = generator()}
        };

        public IList<Post> GetAllPosts()
        {
                using (PortalContext context = _getContext())
                {
                    //return context.Posts.ToList();
                    return posts;
                }
        }

        public Post GetPost(int postId)
        {
                using (PortalContext context = _getContext())
                {
                    //return context.Posts.Where(x => x.PostId.Equals(postId)).FirstOrDefault();
                    return GetAllPosts().Where(x => x.PostId.Equals(postId)).FirstOrDefault();
                }
        }
    }
}
