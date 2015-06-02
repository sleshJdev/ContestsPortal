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

        public IList<Post> GetAllPosts()
        {
                using (PortalContext context = _getContext())
                {
                    return new[]
                    {
                      new Post(){PostContent = "Олимпиада bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb", PublicationDate = DateTime.Now},
                      new Post(){PostContent = "Олимпиада bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb", PublicationDate = DateTime.Now},
                      new Post(){PostContent = "Олимпиада bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb", PublicationDate = DateTime.Now},
                      new Post(){PostContent = "Олимпиада bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb", PublicationDate = DateTime.Now}
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
