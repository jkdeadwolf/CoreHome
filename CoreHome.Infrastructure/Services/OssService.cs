using Aliyun.OSS;
using Aliyun.OSS.Model;
using CoreHome.Infrastructure.Models;
using System.Runtime.InteropServices;
using System.Web;

namespace CoreHome.Infrastructure.Services
{
    internal interface IOssClient
    {
        PutObjectResult PutObject(string bucketName, string key, Stream content);

        IObjectListing ListObjects(string bucketName, string prefix);
    }

    internal interface IObjectListing
    {
        IEnumerable<OssObjectSummary> ObjectSummaries { get; }
    }

    internal class OssObjectListing : IObjectListing
    {
        private IEnumerable<OssObjectSummary> objectSummaries;
        internal OssObjectListing(IEnumerable<OssObjectSummary> objectSummaries)
        {
            this.objectSummaries = objectSummaries;
        }
        public IEnumerable<OssObjectSummary> ObjectSummaries
        {
            get
            {
                return this.objectSummaries;
            }
        }
    }

    internal class AliyunOssClient : OssClient, IOssClient
    {
        internal AliyunOssClient(OssConfig config)
            :base(config.EndPoint, config.AccessKeyId, config.AccessKeySecret)
        { 
        }

        IObjectListing IOssClient.ListObjects(string bucketName, string prefix)
        {
            ObjectListing objectListing = this.ListObjects(bucketName, prefix);
            OssObjectListing ossObjectListing = new OssObjectListing(objectListing.ObjectSummaries);
            return ossObjectListing;
        }
    }

    internal class LocalFileOssClient : IOssClient
    {
        private readonly string ossPath = @".\oos";
        public IObjectListing ListObjects(string bucketName, string prefix)
        {
            OssObjectListing ossObjectListing = new OssObjectListing(new List<OssObjectSummary>());
            return ossObjectListing;
        }

        public PutObjectResult PutObject(string bucketName, string key, Stream content)
        {
            return null;
        }
    }

    public class OssService
    {
        private readonly IOssClient client;
        private readonly OssConfig config;

        public OssService(OssConfig config)
        {
            if (string.IsNullOrEmpty(config.EndPoint) == false)
                client = new AliyunOssClient(config);
            else                
                client = new LocalFileOssClient();
            this.config = config;
        }

        public string UploadProjCover(Stream stream)
        {
            string fileName = Guid.NewGuid().ToString() + ".jpg";
            string path = "images/projects/";
            _ = client.PutObject(config.BucketName, Path.Combine(path, fileName), stream);
            return Path.Combine(config.BucketDomainName, path, fileName);
        }

        public string GetAvatar()
        {
            return Path.Combine(config.BucketDomainName, "images/avatar.jpg");
        }

        public void UploadAvatar(Stream stream)
        {
            _ = client.PutObject(config.BucketName, "images/avatar.jpg", stream);
        }

        public string GetBackground()
        {
            return Path.Combine(config.BucketDomainName, "images/background.jpg");
        }

        public void UploadBackground(Stream stream)
        {
            _ = client.PutObject(config.BucketName, "images/background.jpg", stream);
        }

        public string UploadBlogPic(Stream stream)
        {
            string fileName = Guid.NewGuid().ToString() + ".jpg";
            string path = "blogs/";
            _ = client.PutObject(config.BucketName, Path.Combine(path, fileName), stream);
            return Path.Combine(config.BucketDomainName, path, fileName);
        }

        public List<string> GetMusics()
        {
            string path = "musics/";
            IObjectListing listing = client.ListObjects(config.BucketName, path);

            List<string> musics = [];
            foreach (OssObjectSummary item in listing.ObjectSummaries)
            {
                if (item.Key != path)
                {
                    musics.Add(Path.Combine(config.BucketDomainName, HttpUtility.UrlEncode(item.Key)));
                }
            }
            return musics;
        }

    }
}
