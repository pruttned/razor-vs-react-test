using System.Collections.Generic;
using System.Linq;

namespace RazorApp.Services
{
    public interface INewsService
    {
        List<NewsListViewModel> GetAll(int cnt);
        NewsViewModel Get(string alias);
    }
    public class NewsService : INewsService
    {
        private List<NewsViewModel> Db;

        public NewsService()
        {
            Db = Enumerable.Range(1, 20).Select(i =>
            new NewsViewModel()
            {
                Alias = $"news{i}",
                Title = $"News {i}",
                Text = @"<b>Lorem ipsum dolor sit amet</b>, consectetur adipiscing elit. Proin mollis, purus sit <i>amet</i> ultricies sollicitudin, odio dolor malesuada odio, eget imperdiet enim enim quis purus. Curabitur luctus tortor at arcu vestibulum ultrices. Mauris ante arcu, ultricies ut metus fringilla, scelerisque pharetra enim. Suspendisse ipsum purus, vestibulum in accumsan in, bibendum sed nisi. Curabitur egestas diam sit amet blandit tincidunt. Etiam gravida leo et turpis scelerisque, in imperdiet leo porttitor. Cras tincidunt ligula non enim scelerisque, eu rutrum neque convallis.

Praesent ac semper est. Donec egestas non lectus non vestibulum. Donec non eros in tortor venenatis aliquet. Donec sed neque ut quam euismod vulputate. Aenean congue lorem eget felis tincidunt, sed dictum ante ultrices. Vivamus nisi nibh, maximus pretium commodo bibendum, aliquet sit amet dolor. Cras facilisis metus vel nisl varius, nec elementum odio placerat. Aliquam sit amet elementum arcu. Proin ullamcorper vehicula nulla non efficitur. Aliquam cursus ipsum lacus, id imperdiet eros fringilla in. Phasellus hendrerit mauris enim, semper pretium sapien semper nec. Quisque a ante iaculis, egestas turpis eget, blandit turpis.

Etiam eget lorem vitae diam posuere maximus non et sem. Aenean tempor porta metus sed lacinia. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus pharetra leo nibh, a semper turpis ultrices vel. Donec at cursus elit, sit amet egestas orci. Morbi bibendum, elit et tempor accumsan, est nunc viverra libero, sed semper libero erat sed elit. Nullam viverra, sem ac maximus interdum, lacus odio dignissim diam, eu condimentum mi metus at risus. Fusce vitae nibh ligula. Morbi iaculis fringilla lectus. Nulla facilisi.

Cras fermentum, nulla sed malesuada blandit, turpis nibh maximus risus, vitae aliquet mi est sed ligula. Curabitur odio metus, egestas quis placerat at, placerat ut ex. Sed massa justo, volutpat nec dui ut, elementum consectetur nibh. In pretium sapien eu tincidunt interdum. Interdum et malesuada fames ac ante ipsum primis in faucibus. Nam laoreet, elit ac pharetra sodales, mauris erat accumsan sapien, cursus tristique mauris nisl at nisi. Praesent aliquet posuere justo quis fringilla.

Maecenas vulputate enim a sem congue, et vulputate nisl condimentum. Donec feugiat eleifend rhoncus. Cras eu aliquam augue. Suspendisse sit amet libero eget libero fringilla ultrices. Mauris vel arcu quis sem rutrum semper nec quis tortor. Proin nisi massa, rutrum at risus ut, lobortis tincidunt metus. Nulla ut urna id nibh vulputate sodales. Morbi semper, urna et consectetur suscipit, felis tellus efficitur est, nec volutpat velit risus id nibh. Cras suscipit velit in diam vulputate gravida. Donec eget nunc accumsan, sodales nisi a, faucibus nunc. Nunc gravida luctus purus, in lacinia risus rutrum sed.
",
                Excerpt = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin mollis, purus sit amet ultricies sollicitudin, odio dolor malesuada odio, eget imperdiet enim enim quis purus. Curabitur luctus tortor at arcu vestibulum ultrices. Mauris ante arcu, ultricies ut metus fringilla, scelerisque pharetra enim. Suspendisse ipsum purus, vestibulum in accumsan in, bibendum sed nisi. Curabitur egestas diam sit amet blandit tincidunt. Etiam gravida leo et turpis scelerisque, in imperdiet leo porttitor. Cras tincidunt ligula non enim scelerisque, eu rutrum neque convallis."
            }).ToList();
        }

        public List<NewsListViewModel> GetAll(int cnt)
        {
            return Db.Take(cnt).Select(n => new NewsListViewModel
            {
                Alias = n.Alias,
                Excerpt = n.Excerpt,
                Title = n.Title,
            }).ToList();
        }
        public NewsViewModel Get(string alias)
        {
            return Db.Single(n => n.Alias == alias);
        }
    }

    public class NewsViewModel
    {
        public string Alias { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Excerpt { get; set; }
    }

    public class NewsListViewModel
    {
        public string Alias { get; set; }
        public string Title { get; set; }
        public string Excerpt { get; set; }
    }
}